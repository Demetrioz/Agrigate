using Agrigate.Core;
using Agrigate.Core.Configuration;
using Agrigate.Core.Extensions;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Agrigate.Web;
using Agrigate.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAgrigateLogging(builder.Configuration);

var settings = new AgrigateConfiguration();
builder.Configuration.Bind(Constants.Agrigate.Configuration, settings);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Identity
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Database
builder.Services.AddDbContext<AgrigateContext>(options =>
    options.UseNpgsql(settings.DbConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<AgrigateUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AgrigateContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Mudblazor
builder.Services.AddMudServices();

builder.Services.AddSingleton<IEmailSender<AgrigateUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Apply migrations
    var db = scope.ServiceProvider.GetRequiredService<AgrigateContext>();
    db.Database.Migrate();
    
    // Create the first user
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AgrigateUser>>();
    var defaultUser = userManager.FindByEmailAsync(settings.DefaultEmail ?? "").GetAwaiter().GetResult();
    if (defaultUser == null)
    {
        var adminUser = new AgrigateUser
        {
            UserName = settings.DefaultEmail,
            Email = settings.DefaultEmail,
            EmailConfirmed = true
        };
        
        userManager.CreateAsync(adminUser, settings.DefaultPassword ?? "").Wait();
    }
}

app.UseAgrigateLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
