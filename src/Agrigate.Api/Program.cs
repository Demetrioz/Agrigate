using Agrigate.Api.Extensions;
using Agrigate.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAgrigateLogging(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwaggerWithAuth(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();

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