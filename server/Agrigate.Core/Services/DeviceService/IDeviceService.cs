using Agrigate.Core.Services.DeviceService.Models;
using Agrigate.Domain.Entities;

namespace Agrigate.Core.Services.DeviceService;

/// <summary>
/// Service for interacting wwith devices
/// </summary>
public interface IDeviceService
{
    /// <summary>
    /// Create a new device
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public Task<Device> InsertDevice(DeviceBase device);
}