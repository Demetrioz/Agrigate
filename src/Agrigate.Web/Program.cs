using System.Globalization;
using Agrigate.Core;
using Agrigate.Core.Extensions;
using Agrigate.Web;
using Agrigate.Web.Extensions;

// SEE https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAppOidcServer For additional auth config

// Display $ properly on Linux
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAgrigateLogging(builder.Configuration);
builder.Services.AddOidcAuthentication(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGroup(Constants.Authentication.Routes.Auth)
    .MapAuthenticationRoutes();

app.Run();
