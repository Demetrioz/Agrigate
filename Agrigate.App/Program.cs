using Microsoft.AspNetCore.Identity;
using Agrigate.App.Components.Account;
using Agrigate.App.Extensions;
using Agrigate.Domain.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazor();
builder.Services.AddAgrigateAuthentication(builder.Configuration);

builder.Services.AddSingleton<IEmailSender<AgrigateUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseMigrationsEndPoint();
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

app.UseBlazor();
app.UseAgrigateAuthentication();

app.Run();