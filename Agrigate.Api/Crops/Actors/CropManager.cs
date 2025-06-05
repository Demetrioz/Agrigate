using Agrigate.Api.Crops.Messages;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;

namespace Agrigate.Api.Crops.Actors;

/// <summary>
/// The root actor for interacting with crops
/// </summary>
public class CropManager : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggingAdapter _logger;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CropManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = Context.GetLogger<SerilogLoggingAdapter>();

        ReceiveAsync<CropCommands.CreateCropDetail>(CreateCropDetail);
    }

    /// <summary>
    /// Creates a new CropDetail record
    /// </summary>
    /// <param name="command"></param>
    private async Task CreateCropDetail(CropCommands.CreateCropDetail command)
    {
        _logger.Info("Creating crop detail...");
        Sender.Tell(command.Detail);
    }
}