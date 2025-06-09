using System.Reflection;
using System.Text;
using Agrigate.Api.Crops.Actors;
using Agrigate.Api.System.Actors;
using Agrigate.Core;
using Agrigate.Core.Configuration;
using Agrigate.Core.Extensions;
using Agrigate.Domain.Contexts;
using Akka.Hosting;
using Akka.Logger.Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopifySharp;
using ShopifySharp.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAgrigateLogging();

var settings = new AgrigateConfiguration();
builder.Configuration.Bind(Constants.Agrigate.Configuration, settings);

var authSettings = new AuthenticationConfiguration();
builder.Configuration.Bind(Constants.Authentication.Configuration, authSettings);

builder.Services.AddDbContext<AgrigateContext>(options =>
    options.UseNpgsql(settings.DbConnectionString));

builder.Services.AddAkka("Agrigate", (akkaBuilder, provider) =>
{
    akkaBuilder.ConfigureLoggers(config =>
    {
        config.ClearLoggers();
        config.AddLogger<SerilogLogger>();
    });

    akkaBuilder.WithActors((system, registry, resolver) =>
    {
        var systemProps = resolver.Props<SystemManager>();
        var systemActor = system.ActorOf(systemProps, nameof(SystemManager));
        registry.Register<SystemManager>(systemActor);

        var cropProps = resolver.Props<CropManager>();
        var cropActor = system.ActorOf(cropProps, nameof(CropManager));
        registry.Register<CropManager>(cropActor);
    });
});

builder.Services.AddShopifySharp<LeakyBucketExecutionPolicy>(ServiceLifetime.Transient);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            
            ValidIssuer = authSettings.Issuer,
            ValidAudience = authSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authSettings.SecretKey))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize using PascalCase instead of camelCase  
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put ONLY your JWT token in the textbox below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    options.AddSecurityDefinition(
        jwtSecurityScheme.Reference.Id,
        jwtSecurityScheme
    );
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, [] }
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
app.UseAgrigateLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();