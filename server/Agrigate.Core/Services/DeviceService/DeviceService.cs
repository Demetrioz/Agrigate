using Agrigate.Core.Services.DeviceService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Core.Services.DeviceService;

/// <inheritdoc />
public class DeviceService : IDeviceService
{
    public readonly AgrigateContext _db;

    private readonly DateTimeOffset _activeTelemetryCutoff;

    public DeviceService(AgrigateContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));

        _activeTelemetryCutoff = DateTimeOffset.UtcNow.AddMinutes(-10);
    }

    /// <inheritdoc />
    public async Task<Device> InsertDevice(
        DeviceBase device,
        CancellationToken cancellationToken = default
    )
    {
        var existingDervice = await _db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(
                d => d.Name == device.Name && !d.IsDeleted, 
                cancellationToken
            );
            
        if (existingDervice != null)
            throw new ApplicationException("Device already exists");

        var now = DateTimeOffset.UtcNow;
        var newDevice = new Device
        {
            Name = device.Name,
            Location = device.Location,
            Created = now,
            Modified = now,
            IsDeleted = false
        };

        _db.Add(newDevice);
        await _db.SaveChangesAsync(cancellationToken);

        return newDevice;
    }

    /// <inheritdoc />
    public async Task<List<DeviceBase>> GetDevices(
        CancellationToken cancellationToken = default
    )
    {
        var allDevices = await _db.Devices
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .Select(d => new DeviceBase
            {
                Id = d.Id,
                Name = d.Name,
                Location = d.Location,
                IsActive = d.Telemetry!.Any(t => t.Timestamp >= _activeTelemetryCutoff)
            })
            .ToListAsync(cancellationToken);

        return allDevices;
    }

    /// <inheritdoc />
    public async Task<DeviceDetails> GetDeviceDetails(
        long deviceId,
        CancellationToken cancellationToken = default
    )
    {
        var details = await _db.Devices
            .AsNoTracking()
            .Where(d => 
                d.Id == deviceId
                && !d.IsDeleted
            )
            .Select(d => new DeviceDetails
            {
                Id = d.Id,
                Name = d.Name,
                Location = d.Location,
                IsActive = d.Telemetry!.Any(t => t.Timestamp >= _activeTelemetryCutoff),
                Rules = d.Rules!
                    .Where(r => !r.IsDeleted)
                    .Select(r => new RuleBase
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Summary = $"{string.Join(", ", r.Conditions!.Select(c => c.Type.ToString()))} - {string.Join(", ", r.Actions!.Select(a => a.Type.ToString()))}",
                        IsActive = r.IsActive
                    })
                    .ToList(),
                DistinctTelemetry = d.Telemetry!
                    .Where(t => !t.IsDeleted)
                    .GroupBy(t => t.Key)
                    .Select(group => new TelemetryBase
                    {
                        Key = group.Key,
                        Timestamp = group.Max(t => t.Timestamp),
                        Value = group
                            .Where(t => t.Timestamp == group.Max(t => t.Timestamp))
                            .First()
                            .Value
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ApplicationException("Device not found");

        return details;
    }

    /// <inheritdoc />
    public async Task<List<IGrouping<string, TelemetryBase>>> GetDeviceTelemetry(
        long deviceId,
        CancellationToken cancellationToken = default
    )
    {
        var dateCutoff = DateTimeOffset.UtcNow.AddDays(-1);
        var historicTelemetry = await _db.Telemetry
            .AsNoTracking()
            .Where(t =>
                t.DeviceId == deviceId
                && !t.IsDeleted
                && t.Timestamp >= dateCutoff
            )
            .OrderBy(t => t.Timestamp)
            .Select(t => new TelemetryBase
            {
                Id = t.Id,
                Key = t.Key,
                Value = t.Value,
                Timestamp = t.Timestamp
            })
            .GroupBy(t => t.Key)
            .ToListAsync(cancellationToken);

        return historicTelemetry;
    }
}