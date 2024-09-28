using Agrigate.Core.Services.MqttService;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MQTTnet.Protocol;

namespace Agrigate.Core.Actors;

/// <summary>
/// A base actor that contains common functionalty for interacting with an MQTT
/// broker
/// </summary>
public abstract class MQTTActor : AgrigateActor
{
    protected readonly IMqttService MqttService;

    public MQTTActor(IMqttService mqttService)
    {
        MqttService = mqttService
            ?? throw new ArgumentNullException(nameof(mqttService));
    }

    /// <summary>
    /// Creates a new client an connects to an MQTT broker using the provided
    /// settings
    /// </summary>
    /// <param name="clientId">The clientId used to connect to the broker
    /// </param>
    /// <param name="host">The hostname of the broker</param>
    /// <param name="port">The port used by the broker</param>
    /// <param name="username">The username to connect with</param>
    /// <param name="password">The password to connect with</param>
    /// <param name="protocol">The protocol to use</param>
    /// <param name="secure">Whether the connection should be made using 
    /// TLS</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IMqttClient> ConnectToBroker(
        string clientId,
        string host,
        int port,
        string? username,
        string? password,
        MqttProtocolVersion protocol,
        bool secure = true,
        CancellationToken cancellationToken = default
    )
    {
        var client = MqttService.CreateMqttClient();
        var tlsOptions = new MqttClientTlsOptions
        {
            UseTls = secure
        };

        var options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(host, port)
            .WithCredentials(username, password)
            .WithProtocolVersion(protocol)
            .WithTlsOptions(tlsOptions)
            .Build();

        await client.ConnectAsync(options, cancellationToken);
        return client;
    }

    /// <summary>
    /// Disconnects the specified client from the broker
    /// </summary>
    /// <param name="client">The client that should be disconnected</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task DisconnectFromBroker(
        IMqttClient client,
        CancellationToken cancellationToken = default
    )
    {
        var options = new MqttClientDisconnectOptionsBuilder()
            .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
            .Build();

        await client.DisconnectAsync(options, cancellationToken);
    }

    /// <summary>
    /// Subscribes an MQTT client to a particular topic
    /// </summary>
    /// <param name="client">The client that should subscribe to the topic
    /// </param>
    /// <param name="topic">The name of the topic</param>
    /// <param name="qos">The QOS level of the subscription</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task SubscribeToTopic(
        IMqttClient client, 
        string topic,
        MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce,
        CancellationToken cancellationToken = default
    )
    {
        var options = MqttService.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => 
            { 
                f.WithTopic(topic); 
                f.WithQualityOfServiceLevel(qos);
            })
            .Build();

        await client.SubscribeAsync(options, cancellationToken);
    }

    /// <summary>
    /// Uses the provided client to publish a message to the MQTT broker
    /// </summary>
    /// <param name="client">The client that should publish a message</param>
    /// <param name="topic">The topic the message should be published to</param>
    /// <param name="payload">The payload of the message</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task PublishMessage(
        IMqttClient client,
        string topic,
        string payload,
        CancellationToken cancellationToken = default
    )
    {
        var messsage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await client.PublishAsync(messsage, cancellationToken);
    }
}