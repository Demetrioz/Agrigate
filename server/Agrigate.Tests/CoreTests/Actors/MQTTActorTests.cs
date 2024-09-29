using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Tests;
using MQTTnet;
using MQTTnet.Formatter;
using MQTTnet.Client;
using NSubstitute;

namespace Agrigate.Tests.CoreTests.ActorTests;

[TestFixture]
public class MQTTActorTests : TestKit
{
    private IMqttClient _mockClient;
    private IMqttService _mockMqttService;

    [SetUp]
    public void Setup()
    {
        _mockClient = Substitute.For<IMqttClient>();
        _mockClient.ConnectAsync(
            Arg.Any<MqttClientOptions>(), 
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(new MqttClientConnectResult()));
        _mockClient.DisconnectAsync(
            Arg.Any<MqttClientDisconnectOptions>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(true));
        _mockClient.SubscribeAsync(
            Arg.Any<MqttClientSubscribeOptions>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(new MqttClientSubscribeResult(
            0, 
            [], 
            "", 
            [])));
        _mockClient.PublishAsync(
            Arg.Any<MqttApplicationMessage>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(new MqttClientPublishResult(
            0, 
            MqttClientPublishReasonCode.Success, 
            "", 
            [])));
        

        _mockMqttService = Substitute.For<IMqttService>();
        _mockMqttService.CreateMqttClient()
            .Returns(_mockClient);
        _mockMqttService.CreateSubscribeOptionsBuilder()
            .Returns(new MqttClientSubscribeOptionsBuilder());
    }

    [Test]
    public void MQTTActor_Instantiates()
    {
        var actor = Sys.ActorOf(Props.Create(() => new MQTTTestActor(_mockMqttService)));
        Assert.That(actor, Is.Not.Null);
    }

    [Test]
    public async Task ConnectToBroker_Succeeds()
    {
        var actor = Sys.ActorOf(Props.Create(() => new MQTTTestActor(_mockMqttService)));
        
        actor.Tell(new ConnectToBrokerTest(
            "testClient",
            "testHost",
            1234,
            "testUser",
            "testPass",
            MqttProtocolVersion.V311
        ));

        await ExpectNoMsgAsync();
        _mockMqttService
            .Received(1)
            .CreateMqttClient();
        await _mockClient
            .Received(1)
            .ConnectAsync(
                Arg.Any<MqttClientOptions>(), 
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DisconnectFromBroker_Succeeds()
    {
        var actor = Sys.ActorOf(Props.Create(() => new MQTTTestActor(_mockMqttService)));

        actor.Tell(new DisconnectFromBrokerTest(_mockClient));

        await ExpectNoMsgAsync();
        await _mockClient
            .Received(1)
            .DisconnectAsync(
                Arg.Any<MqttClientDisconnectOptions>(), 
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeToTopic_Succeeds()
    {
        var actor = Sys.ActorOf(Props.Create(() => new MQTTTestActor(_mockMqttService)));

        actor.Tell(new SubscribeToTopicTest(_mockClient, "testTopic"));

        await ExpectNoMsgAsync();
        _mockMqttService
            .Received(1)
            .CreateSubscribeOptionsBuilder();
        await _mockClient
            .Received(1)
            .SubscribeAsync(
                Arg.Any<MqttClientSubscribeOptions>(),
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task PublishMessage_Succeeds()
    {
        var actor = Sys.ActorOf(Props.Create(() => new MQTTTestActor(_mockMqttService)));

        actor.Tell(new PublishMessageTest(
            _mockClient, 
            "testTopic", 
            "testPayload"
        ));

        await ExpectNoMsgAsync();
        await _mockClient
            .Received(1)
            .PublishAsync(
                Arg.Any<MqttApplicationMessage>(),
                Arg.Any<CancellationToken>());
    }
}