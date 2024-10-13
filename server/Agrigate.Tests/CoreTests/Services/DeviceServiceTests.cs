using Agrigate.Core.Services.DeviceService;
using Agrigate.Core.Services.DeviceService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
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
    public void InsertDevice_FailsWhenNameExists()
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

    [Test, Order(4)]
    public async Task GetDeviceDetails_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions!);

        var deviceService = new DeviceService(context);

        var result = await deviceService.GetDeviceDetails(1);

        Assert.Multiple(() => 
        {
            Assert.That(result.Name, Is.EqualTo("TestDevice"));
            Assert.That(result.Location, Is.EqualTo("TestLocation"));
        });
    }

    [Test, Order(5)]
    public async Task GetDeviceTelemetry_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions!);
        context.Telemetry.AddRange(new List<Telemetry>
        {
            new Telemetry { DeviceId = 1, IsDeleted = false, Timestamp = DateTimeOffset.UtcNow },
            new Telemetry { DeviceId = 1, IsDeleted = false, Timestamp = DateTimeOffset.UtcNow },
        });
        await context.SaveChangesAsync();

        var deviceService = new DeviceService(context);

        var result = await deviceService.GetDeviceTelemetry(1);

        Assert.That(result, Has.Count.EqualTo(2));
    }
}