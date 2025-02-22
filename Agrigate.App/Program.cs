using Agrigate.Domain.Extensions;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MudBlazor.Services;

using App = Agrigate.App.Components.App;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);

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