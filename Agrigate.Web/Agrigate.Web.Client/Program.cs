using Agrigate.Web.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();

builder.Services.AddTransient<AntiforgeryHandler>();
builder.Services.AddHttpClient<BffClient>()
    .AddHttpMessageHandler<AntiforgeryHandler>();
builder.Services.AddScoped<BffClient>();
// builder.Services.AddHttpClient("backend")
//     .AddHttpMessageHandler<AntiforgeryHandler>();
// builder.Services//.AddHttpClient();
//     .AddHttpClient(
//         "backend", 
//         client =>
//         {
//             // client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
//             client.BaseAddress = new Uri("http://localhost:5003");
//         })
//     .AddHttpMessageHandler<AntiforgeryHandler>();
// builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("backend"));

await builder.Build().RunAsync();