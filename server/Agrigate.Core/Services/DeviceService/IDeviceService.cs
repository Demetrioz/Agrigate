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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Device> InsertDevice(
        DeviceBase device, 
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves all devices
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<DeviceBase>> GetDevices(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves detailed information about a specific device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<DeviceDetails> GetDeviceDetails(
        long deviceId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves historic telemetry for a particular device
    /// </summary>
    /// <param name="deviceId">The id of the device to retrieve telemetry 
    /// for</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<TelemetryBase>> GetDeviceTelemetry(
        long deviceId,
        CancellationToken cancellationToken = default
    );
}