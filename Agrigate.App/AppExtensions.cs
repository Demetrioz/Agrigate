using System.Reflection;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace Agrigate.App;

public static class AppExtensions
{
    public static async Task PrepareElectronWindow(this IServiceProvider serviceProvider)
    {
        using var provider = serviceProvider.CreateScope();
        
        var appManager = provider.ServiceProvider.GetRequiredService<ElectronNET.API.App>();
        var windowManager = provider.ServiceProvider.GetRequiredService<WindowManager>();
        var menuManager = provider.ServiceProvider.GetRequiredService<Menu>();
        
        var menu = CreateMenu();
        menuManager.SetApplicationMenu(menu);

        var window = await CreateWindow(windowManager);
        await window.WebContents.Session.ClearCacheAsync();
        window.SetTitle("Agrigate");
        window.OnReadyToShow += window.Show;
        window.OnClose += appManager.Quit;
    }

    //////////////////////////////////////////
    //            Window Helpers            //
    //////////////////////////////////////////
    
    private static async Task<BrowserWindow> CreateWindow(WindowManager windowManager)
    {
        return await windowManager.CreateWindowAsync(new BrowserWindowOptions
        {
            // Required for interactive server to function properly
            WebPreferences = new WebPreferences
            {
                NodeIntegration = false,
                ContextIsolation = false
            }
        });
    }

    //////////////////////////////////////////
    //             Menu Helpers             //
    //////////////////////////////////////////
    
    private static MenuItem[] CreateMenu()
    {
        return
        [
            FileMenu,
            HelpMenu
        ];
    }

    private static readonly MenuItem FileMenu = new MenuItem
    {
        Label = "File",
        Submenu =
        [
            new MenuItem
            {
                Label = "Exit",
                Accelerator = "CmdOrCtrl+E",
                Role = MenuRole.close
            }
        ]
    };

    private static readonly MenuItem HelpMenu = new MenuItem
    {
        Label = "Help",
        Submenu =
        [
            new MenuItem
            {
                Label = "About",
                Accelerator = "CmdOrCtrl+A",
                Click = DisplayHelpMenu
            }
        ]
    };

    private static async void DisplayHelpMenu()
    {
        var version = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0, 0);
        var options = new MessageBoxOptions($"Version: {version.Major}.{version.Minor}.{version.Build}")
        {
            Title = "About Agrigate",
            Type = MessageBoxType.none,
            NoLink = true,
            Buttons = ["Ok", "View Documentation"]
        };
        
        var response = await Electron.Dialog.ShowMessageBoxAsync(options);
        if (response?.Response == 1)
            await Electron.Shell.OpenExternalAsync("https://demetrioz.github.io/Agrigate/");
    }
}