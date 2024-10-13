using System.Text.Json.Serialization;
using Agrigate.Api;
using Agrigate.Api.Actors;
using Agrigate.Api.Authorization;
using Agrigate.Api.Configuration;
using Agrigate.Api.Services;
using Agrigate.Core.Configuration;
using Agrigate.Core.Services.DeviceService;
using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Services.NotificationService;
using Agrigate.Core.Services.RuleService;
using Agrigate.Domain.Configuration;
using Agrigate.Domain.Contexts;
using Akka.Hosting;
using Akka.Remote.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//////////////////////////////////////////
//          Configure Settings          //
//////////////////////////////////////////

var apiOptions = new ApiOptions();
builder.Configuration.Bind("Api", apiOptions);

builder.Services.Configure<ApiOptions>(
    builder.Configuration.GetSection("Api"));

builder.Services.Configure<NotificationOptions>(
    builder.Configuration.GetSection("Notifications"));

var dbOptions = new DatabaseOptions();
builder.Configuration.Bind("Database", dbOptions);

//////////////////////////////////////////
//            Database Setup            //
//////////////////////////////////////////

var connectionString = $"Host={dbOptions.Host};Port={dbOptions.Port};Database={dbOptions.Database};User Id={dbOptions.Username};Password={dbOptions.Password};";
builder.Services.AddDbContext<AgrigateContext>(options =>
    options.UseNpgsql(connectionString));

//////////////////////////////////////////
//       Configure Authentication       //
//////////////////////////////////////////

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(
            Constants.Policies.ApiKeyPolicy, 
            policy =>
            {
                policy.AddAuthenticationSchemes([JwtBearerDefaults.AuthenticationScheme]);
                policy.Requirements.Add(new ApiKeyRequirement());
            });
    });

//////////////////////////////////////////
//          Configure Services          //
//////////////////////////////////////////

builder.Services
    .AddHttpContextAccessor()
    .AddSingleton<IMqttService, MqttService>()
    .AddScoped<IAuthorizationHandler, ApiKeyHandler>()
    .AddScoped<IAuthenticationService, AuthenticationService>()
    .AddTransient<IDeviceService, DeviceService>()
    .AddTransient<INotificationService, NotificationService>()
    .AddTransient<IRuleService, RuleService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var apiKeySecurityScheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = Constants.Authentication.ApiKeyHeader,
        Type = SecuritySchemeType.ApiKey,
        Description = Constants.Authentication.SchemeDescription,
        Scheme = Constants.Authentication.SchemeName,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = Constants.Authentication.SchemeDefinition
        }
    };

    c.AddSecurityDefinition(
        Constants.Authentication.SchemeDefinition,
        apiKeySecurityScheme
    );

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiKeySecurityScheme, [] }
    });
});

//////////////////////////////////////////
//               Akka.Net               //
//////////////////////////////////////////

builder.Services.AddAkka(nameof(Agrigate.Api), builder =>
{
    builder
        .WithRemoting(
            hostname: "0.0.0.0",
            publicHostname: apiOptions.ApiService.Hostname,
            port: apiOptions.ApiService.Port
        )
        .WithActors((system, registry, resolver) =>
        {
            var supervisorProps = resolver.Props<ApiSupervisor>();
            var supervisor = system.ActorOf(
                supervisorProps, 
                nameof(ApiSupervisor)
            );

            registry.Register<ApiSupervisor>(supervisor);
        });
});

//////////////////////////////////////////
//            Run Migrations            //
//////////////////////////////////////////

// Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
#pragma warning disable ASP0000 
using (var provider = builder.Services.BuildServiceProvider())
{
    var database = provider.GetRequiredService<AgrigateContext>();
    database.Database.Migrate();
}
#pragma warning restore ASP0000

//////////////////////////////////////////
//      Configure Request Pipeline      //
//////////////////////////////////////////

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
