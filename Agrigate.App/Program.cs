using Agrigate.Domain.Extensions;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MudBlazor.Services;
using Serilog;

using App = Agrigate.App.Components.App;
using Log = Serilog.Log;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("startup.txt")
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Agrigate");
    
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.UseElectron(args);

    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());
    
    // Use Electron.NET API-classes directly with DI 
    // builder.Services.AddElectron();
    builder.Services.AddMudServices();
    builder.Services.AddAggrigateDomain();

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    //////////////////////////////////////////
    //      Configure Request Pipeline      //
    //////////////////////////////////////////

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Log request information
    // app.UseSerilogRequestLogging();
    
    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    await app.StartAsync();

    if (HybridSupport.IsElectronActive)
    {
        var window = await Electron.WindowManager
            .CreateWindowAsync(new BrowserWindowOptions
            {
                // Required for interactive server to work
                WebPreferences = new WebPreferences
                {
                    NodeIntegration = false,
                    ContextIsolation = false,
                }
            });
    
        await window.WebContents.Session.ClearCacheAsync();
        window.OnReadyToShow += () => window.Show();
        window.OnClosed += () => Electron.App.Quit();
        window.SetTitle("Agrigate");
    }

    app.WaitForShutdown();
    Log.Information("Shutdown Complete");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Agrigate terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}