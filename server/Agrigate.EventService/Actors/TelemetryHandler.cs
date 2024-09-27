using Agrigate.Core.Actors;
using Agrigate.EventService.Configuration;
using Akka.Event;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MQTTnet.Protocol;

namespace Agrigate.EventService.Actors;

/// <summary>
/// An actor that connects to an MQTT broker and subscribes to a topic in order
/// to receive telemetry readings
/// </summary>
public class TelemetryHandler : MQTTActor
{
    private readonly TelemetryOptions _config;

    private IMqttClient? _client;

    public TelemetryHandler(IOptions<TelemetryOptions> options) 
    {
        _config = options.Value 
            ?? throw new ArgumentNullException(nameof(options));
    }

    protected override void PreStart()
    {
        Logger.Info("{0} starting...", nameof(TelemetryHandler));

        // Because this call is not awaited, execution of the current method continues before the call is completed
        #pragma warning disable CS4014 
        SubscribeToTelemetry();
        #pragma warning restore CS4014

        Logger.Info("{0} ready!", nameof(TelemetryHandler));
    }

    protected override void PostStop()
    {
        Logger.Info("{0} stopping...", nameof(TelemetryHandler));

        _client?.Dispose();

        Logger.Info("{0} stopped!", nameof(TelemetryHandler));
    }

    /// <summary>
    /// Creates an MQTT client, connects to a broker, and subscribes to a topic
    /// in order to receive telemetry messages
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task SubscribeToTelemetry(CancellationToken cancellationToken = default)
    {
        Logger.Info("{0} running...", nameof(SubscribeToTelemetry));

        try
        {
            _client = await ConnectToBroker(
                _config.ClientId,
                _config.Host,
                _config.Port,
                _config.Username,
                _config.Password,
                MqttProtocolVersion.V311,
                _config.SecureConnection,
                cancellationToken
            );

            _client.ApplicationMessageReceivedAsync += HandleTelemetryEvent;

            await SubscribeToTopic(
                _client,
                _config.Topic,
                MqttQualityOfServiceLevel.AtLeastOnce,
                cancellationToken
            );

            Logger.Info("{0} completed!", nameof(SubscribeToTelemetry));
        }
        catch (Exception ex)
        {
            Logger.Error("Error subscribing to telemetry: {0}", ex.Message);
            // TODO: Restart the actor, try again, or send notification
        }
    }

    /// <summary>
    /// Event handler for incoming telemetry messages
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private Task HandleTelemetryEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            Logger.Info("Received Message [{0}] from topic [{1}]", message, e.ApplicationMessage.Topic);
        }
        catch (Exception ex)
        {
            Logger.Error("Unable to process message: {1}", ex.Message);
        }

        return Task.CompletedTask;
    }
}