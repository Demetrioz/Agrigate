using Agrigate.Core.Actors;
using Agrigate.Core.Services.MqttService;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Agrigate.Core.Tests;

/// <summary>
/// Implementation of MQTTActor for testing
/// </summary>
public class MQTTTestActor : MQTTActor
{
    public MQTTTestActor(IMqttService mqttService) : base(mqttService)
    {
        ReceiveAsync<ConnectToBrokerTest>(async message => 
            await ConnectToBroker(
                message.ClientId, 
                message.Host,
                message.Port, 
                message.Username,
                message.Password, 
                message.Protocol));

        ReceiveAsync<DisconnectFromBrokerTest>(async message =>
            await DisconnectFromBroker(message.Client));

        ReceiveAsync<SubscribeToTopicTest>(async message =>
            await SubscribeToTopic(
                message.Client,
                message.Topic
            ));

        ReceiveAsync<PublishMessageTest>(async message =>
            await PublishMessage(
                message.Client,
                message.Topic,
                message.Payload
            ));
    }
}

/// <summary>
/// Message for testing ConnectToBroker()
/// </summary>
/// <param name="ClientId"></param>
/// <param name="Host"></param>
/// <param name="Port"></param>
/// <param name="Username"></param>
/// <param name="Password"></param>
/// <param name="Protocol"></param>
public record ConnectToBrokerTest(
    string ClientId,
    string Host,
    int Port,
    string Username,
    string Password,
    MqttProtocolVersion Protocol
);

/// <summary>
/// Message for testing DisconnectFromBroker()
/// </summary>
/// <param name="Client"></param>
public record DisconnectFromBrokerTest(IMqttClient Client);

/// <summary>
/// Message for testing SubscribeToTopic()
/// </summary>
/// <param name="Client"></param>
/// <param name="Topic"></param>
public record SubscribeToTopicTest(IMqttClient Client, string Topic);

/// <summary>
/// Message for testing PublishMessage()
/// </summary>
/// <param name="Client"></param>
/// <param name="Topic"></param>
/// <param name="Payload"></param>
public record PublishMessageTest(
    IMqttClient Client, 
    string Topic, 
    string Payload
);