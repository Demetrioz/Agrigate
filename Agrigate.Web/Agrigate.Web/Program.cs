using Agrigate.Web.Client.Authentication;
using Agrigate.Web.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    // https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-identity-server?view=aspnetcore-7.0&tabs=visual-studio

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveWebAssemblyComponents();

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();
    
    builder.Services.AddBff();
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "cookie";
            options.DefaultChallengeScheme = "oidc";
            options.DefaultSignOutScheme = "oidc";
        })
        .AddCookie("cookie", options =>
        {
            options.Cookie.Name = "__Host-blazor";
            options.Cookie.SameSite = SameSiteMode.Strict;
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://demo.duendesoftware.com";
    
            options.ClientId = "interactive.confidential";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.ResponseMode = "query";
    
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("api");
            options.Scope.Add("offline_access");
    
            options.MapInboundClaims = false;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;
            options.DisableTelemetry = true;
        });
    // builder.Services.AddAuthentication(options =>
    //     {
    //         options.DefaultScheme = "cookie";
    //         options.DefaultChallengeScheme = "oidc";
    //         options.DefaultSignOutScheme = "oidc";
    //     })
    //     .AddCookie("cookie", options =>
    //     {
    //         options.Cookie.Name = "__Host-blazor";
    //         options.Cookie.SameSite = SameSiteMode.Strict;
    //     })
    //     .AddOpenIdConnect("oidc", options =>
    //     {
    //         options.Authority = "http://localhost:5001";
    //
    //         options.ClientId = "Agrigate.Web";
    //         options.ClientSecret = "secret";
    //         options.ResponseType = "code";
    //         options.ResponseMode = "query";
    //
    //         options.Scope.Clear();
    //         options.Scope.Add("openid");
    //         options.Scope.Add("profile");
    //         // options.Scope.Add("api");
    //         // options.Scope.Add("offline_access");
    //
    //         options.MapInboundClaims = false;
    //         options.GetClaimsFromUserInfoEndpoint = true;
    //         options.SaveTokens = true;
    //
    //         #if DEBUG
    //         options.RequireHttpsMetadata = false;
    //         #endif
    //     });
    builder.Services.AddAuthorization();
    
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.UseAuthentication();
    app.UseBff();
    app.UseAuthorization();
    app.MapBffManagementEndpoints();
    
    app.MapRazorComponents<App>()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(Agrigate.Web.Client._Imports).Assembly);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}