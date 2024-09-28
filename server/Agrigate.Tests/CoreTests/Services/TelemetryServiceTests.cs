using Agrigate.Core.Services.TelemetryService;
using Agrigate.Core.Services.TelemetryService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Agrigate.Tests.CoreTests.Services;

[TestFixture]
public class TelemetryServiceTests
{
    private DbContextOptions<AgrigateContext>? _contextOptions;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _contextOptions = new DbContextOptionsBuilder<AgrigateContext>()
            .UseInMemoryDatabase(nameof(TelemetryServiceTests))
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    [Test]
    public async Task InsertDeviceTelemetry_Succeeds()
    {
        using var context = new AgrigateContext(_contextOptions!);

        var dbDevice = context.Add(new Device
        {
            Name = "TestDevice"
        });

        await context.SaveChangesAsync();

        var now = DateTimeOffset.UtcNow;
        var telemetry = new TelemetryBase
        {
            DeviceId = dbDevice.Entity.Id,
            Key = "testKey",
            Value = 123,
            Timestamp = now
        };
        
        var telemetryService = new TelemetryService(context);

        var result = await telemetryService.InsertDeviceTelemetry(telemetry);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.DeviceId, Is.EqualTo(telemetry.DeviceId));
            Assert.That(result.Key, Is.EqualTo(telemetry.Key));
            Assert.That(result.Value, Is.EqualTo(telemetry.Value));
            Assert.That(result.Timestamp, Is.EqualTo(telemetry.Timestamp));
        });
    }

    [Test]
    public void InsertDeviceTelemetry_ThrowsWhenDeviceNotFound()
    {
        using var context = new AgrigateContext(_contextOptions!);
        var telemetryService = new TelemetryService(context);

        var telemetry = new TelemetryBase
        {
            DeviceId = 999,
            Key = "testKey",
            Value = 123,
            Timestamp = DateTimeOffset.UtcNow
        };

        Assert.ThrowsAsync<ApplicationException>(async () => 
            await telemetryService.InsertDeviceTelemetry(telemetry));
    }
}