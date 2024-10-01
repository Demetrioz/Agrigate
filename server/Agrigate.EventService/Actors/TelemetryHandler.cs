using Agrigate.Core.Actors;
using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Services.TelemetryService;
using Agrigate.Core.Services.TelemetryService.Models;
using Agrigate.EventService.Actors.Rules;
using Agrigate.EventService.Configuration;
using Agrigate.EventService.Messages;
using Akka.Actor;
using Akka.Event;
using Akka.Hosting;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace Agrigate.EventService.Actors;

/// <summary>
/// An actor that connects to an MQTT broker and subscribes to a topic in order
/// to receive telemetry readings
/// </summary>
public class TelemetryHandler : MQTTActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TelemetryOptions _config;
    private readonly IActorRef? _ruleEngine;

    private IMqttClient? _client;

    public TelemetryHandler(
        IServiceProvider serviceProvider,
        IMqttService mqttService,
        IRequiredActor<RuleEngine> ruleEngine
    ) : base(mqttService)
    {
        _serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));

        _config = _serviceProvider
            .GetRequiredService<IOptions<TelemetryOptions>>().Value
            ?? throw new ArgumentNullException(nameof(TelemetryOptions));

        _ruleEngine = ruleEngine.ActorRef
            ?? throw new ArgumentNullException(nameof(ruleEngine));
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
    private async Task HandleTelemetryEvent(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            var message = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            var telemetry = JsonConvert.DeserializeObject<TelemetryBase>(message) 
                ?? throw new ApplicationException("Cannot insert null telemetry");

            using var scope = _serviceProvider.CreateScope();
            var telemetryService = scope.ServiceProvider.GetRequiredService<ITelemetryService>();

            var dbTelemetry = await telemetryService.InsertDeviceTelemetry(telemetry);

            _ruleEngine.Tell(new ActivateEngine(dbTelemetry.DeviceId, dbTelemetry.Id));
        }
        catch (Exception ex)
        {
            Logger.Error("Unable to process telemetry: {0}", ex.Message);
        }
    }
}