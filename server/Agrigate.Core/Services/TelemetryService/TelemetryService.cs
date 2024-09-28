using Agrigate.Core.Services.TelemetryService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Core.Services.TelemetryService;

/// <inheritdoc />
public class TelemetryService : ITelemetryService
{
    private readonly AgrigateContext _db;

    public TelemetryService(AgrigateContext dbContext)
    {
        _db = dbContext 
            ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task<Telemetry> InsertDeviceTelemetry(TelemetryBase reading)
    {
        var existingDevice = await _db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(d => 
                d.Id == reading.DeviceId
                && !d.IsDeleted
            ) ?? throw new ApplicationException("Device not found");

        var now = DateTimeOffset.UtcNow;
        var newTelemetry = new Telemetry
        {
            DeviceId = existingDevice.Id,
            Timestamp = reading.Timestamp ?? now,
            Key = reading.Key,
            Value = reading.Value,
            Unit = reading.Unit,
            Created = now,
            Modified = now,
            IsDeleted = false
        };

        _db.Add(newTelemetry);
        await _db.SaveChangesAsync();

        return newTelemetry;
    }
}