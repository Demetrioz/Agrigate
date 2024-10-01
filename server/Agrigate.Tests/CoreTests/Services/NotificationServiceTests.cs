using Agrigate.Core.Configuration;
using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Services.NotificationService;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using NSubstitute;

namespace Agrigate.Tests.CoreTests.Services;

[TestFixture]
public class NotificationServiceTests
{
    private IOptions<NotificationOptions> _options;
    private IMqttClient _mockClient;
    private IMqttService _mockMqttService;

    [SetUp]
    public void Setup()
    {
        var config = new NotificationOptions
        {
            ClientId = "client",
            Host = "host.com",
            Port = 8883,
            Username = "user",
            Password = "password",
            SecureConnection = true
        };
        _options = Options.Create(config);

        _mockClient = Substitute.For<IMqttClient>();
        _mockClient.ConnectAsync(
            Arg.Any<MqttClientOptions>(), 
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(new MqttClientConnectResult()));
        _mockClient.DisconnectAsync(
            Arg.Any<MqttClientDisconnectOptions>(),
            Arg.Any<CancellationToken>()
        ).Returns(Task.FromResult(true));
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
    }

    [Test]
    public async Task SendMqttNotificationSucceeds()
    {
        var topic = "testTopic";
        var payload = new
        {
            Test = true,
            Data = "Test payload"
        };

        var notificationService = new NotificationService(
            _options, 
            _mockMqttService
        );

        await notificationService.SendMqttNotification(
            topic, 
            JsonConvert.SerializeObject(payload)
        );

        _mockMqttService
            .Received(1)
            .CreateMqttClient();

        await _mockClient
            .Received(1)
            .ConnectAsync(
                Arg.Any<MqttClientOptions>(), 
                Arg.Any<CancellationToken>());

        await _mockClient
            .Received(1)
            .PublishAsync(
                Arg.Any<MqttApplicationMessage>(),
                Arg.Any<CancellationToken>());

        await _mockClient
            .Received(1)
            .DisconnectAsync(
                Arg.Any<MqttClientDisconnectOptions>(), 
                Arg.Any<CancellationToken>());
    }
}