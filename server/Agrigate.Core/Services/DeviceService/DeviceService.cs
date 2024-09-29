using Agrigate.Core.Services.DeviceService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Core.Services.DeviceService;

/// <inheritdoc />
public class DeviceService : IDeviceService
{
    public readonly AgrigateContext _db;

    public DeviceService(AgrigateContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    /// <inheritdoc />
    public async Task<Device> InsertDevice(DeviceBase device)
    {
        var existingDervice = await _db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(d =>
                d.Name == device.Name
                && !d.IsDeleted
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
        await _db.SaveChangesAsync();

        return newDevice;
    }
}