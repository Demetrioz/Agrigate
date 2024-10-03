using Agrigate.Core.Services.DeviceService;
using Agrigate.Core.Services.DeviceService.Models;
using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Agrigate.Tests.CoreTests.Services;

[TestFixture]
public class DeviceServiceTests
{
    private DbContextOptions<AgrigateContext>? _contextOptions;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _contextOptions = new DbContextOptionsBuilder<AgrigateContext>()
            .UseInMemoryDatabase(nameof(TelemetryServiceTests))
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    [Test, Order(1)]
    public async Task InsertDevice_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions!);

        var device = new DeviceBase
        {
            Name = "TestDevice",
            Location = "TestLocation"
        };

        var deviceService = new DeviceService(context);

        var result = await deviceService.InsertDevice(device);

        Assert.Multiple(() => 
        {
            Assert.That(result.Name, Is.EqualTo(device.Name));
            Assert.That(result.Location, Is.EqualTo(device.Location));
        });
    }

    [Test, Order(2)]
    public void InsertDevice_FailesWhenNameExists()
    {
        using var context = new AgrigateContext(_contextOptions!);

        var device = new DeviceBase
        {
            Name = "TestDevice",
            Location = "TestLocation"
        };

        var deviceService = new DeviceService(context);

        Assert.ThrowsAsync<ApplicationException>(async () => 
            await deviceService.InsertDevice(device));
    }

    [Test, Order(3)]
    public async Task GetDevices_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions!);

        var deviceService = new DeviceService(context);

        var result = await deviceService.GetDevices();

        Assert.Multiple(() => 
        {
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("TestDevice"));
            Assert.That(result.First().Location, Is.EqualTo("TestLocation"));
        });
    }
}