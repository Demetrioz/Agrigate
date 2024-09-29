using Agrigate.Core.Services.TelemetryService.Models;
using Agrigate.Domain.Entities;

namespace Agrigate.Core.Services.TelemetryService;

/// <summary>
/// Service for interacting with telemetry
/// </summary>
public interface ITelemetryService
{
    /// <summary>
    /// Inserts telemetry from a device into the database
    /// </summary>
    /// <param name="reading"></param>
    /// <returns></returns>
    public Task<Telemetry> InsertDeviceTelemetry(TelemetryBase reading);
}