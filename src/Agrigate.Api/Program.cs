using Agrigate.Api.Configuration;
using Agrigate.Core.Extensions;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAgrigateLogging(builder.Configuration);

var authSettings = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", authSettings);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = authSettings.Authority;
        options.Audience = authSettings.Audience;
        options.RequireHttpsMetadata = authSettings.RequireHttpsMetadata;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = authSettings.Audience,
            
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAgrigateLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();