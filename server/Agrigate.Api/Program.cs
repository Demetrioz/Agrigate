using Agrigate.Core.Services.DeviceService;
using Agrigate.Domain.Configuration;
using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//////////////////////////////////////////
//          Configure Settings          //
//////////////////////////////////////////

var dbOptions = new DatabaseOptions();
builder.Configuration.Bind("Database", dbOptions);

//////////////////////////////////////////
//            Database Setup            //
//////////////////////////////////////////

var connectionString = $"Host={dbOptions.Host};Port={dbOptions.Port};Database={dbOptions.Database};User Id={dbOptions.Username};Password={dbOptions.Password};";
builder.Services.AddDbContext<AgrigateContext>(options =>
    options.UseNpgsql(connectionString));

//////////////////////////////////////////
//          Configure Services          //
//////////////////////////////////////////

builder.Services
    .AddTransient<IDeviceService, DeviceService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
