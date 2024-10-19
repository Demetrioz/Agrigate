using Agrigate.Core.Configuration;
using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Services.NotificationService;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
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
        _mockMqttService.CreateMqttClient().Returns(_mockClient);
    }

    [Test]
    public async Task SendMqttNotification_Succeeds()
    {
        using var db = TestHelpers
            .GetUniqueTestDb(nameof(SendMqttNotification_Succeeds));

        var topic = "testTopic";
        var payload = new
        {
            Test = true,
            Data = "Test payload"
        };

        var notificationService = new NotificationService(
            _options, 
            _mockMqttService,
            db
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

    [Test]
    public async Task SaveNotification_Succeeds()
    {
        using var db = TestHelpers
            .GetUniqueTestDb(nameof(SaveNotification_Succeeds));
        
        string title = "Test";
        string text = "TestText";
        
        var notificationService = new NotificationService(
            _options,
            _mockMqttService,
            db
        );

        var result = await notificationService.SaveNotification(title, text);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Title, Is.EqualTo(title));
            Assert.That(result.Text, Is.EqualTo(text));
        });
    }

    [Test]
    public async Task GetRecentNotifications_Succeeds()
    {
        using var db = TestHelpers
            .GetUniqueTestDb(nameof(GetRecentNotifications_Succeeds));

        await AddNotificationsToDb(db);

        var notificationService = new NotificationService(
            _options,
            _mockMqttService,
            db
        );

        var result = await notificationService.GetRecentNotifications();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(10));
            Assert.That(result.First().Title, Is.EqualTo("Test12"));
            Assert.That(result.ElementAt(1).Title, Is.EqualTo("Test6"));
        });
    }

    [Test]
    public async Task MarkNotificationsRead_Succeeds()
    {
        using var db = TestHelpers
            .GetUniqueTestDb(nameof(MarkNotificationsRead_Succeeds));

        await AddNotificationsToDb(db);

        var readNotifications = new List<long> { 1, 3, 5 };

        var notificationService = new NotificationService(
            _options,
            _mockMqttService,
            db
        );

        await notificationService.MarkNotificationsRead(readNotifications);
        var results = await notificationService.GetRecentNotifications();

        var readResults = results
            .Where(n => readNotifications.Contains(n.Id))
            .ToList();

        Assert.Multiple(() => 
        {
            Assert.That(readResults, Has.Count.EqualTo(3));
            CollectionAssert.AreEquivalent(
                readNotifications, 
                readResults.Select(n => n.Id).ToList()
            );
        });
    }

    private async Task AddNotificationsToDb(AgrigateContext db)
    {
        var now = DateTimeOffset.UtcNow;
        var newNotifications = new List<Notification>
        {
            new Notification {Title = "Test1", Text = "Test1", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test2", Text = "Test2", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test3", Text = "Test3", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test4", Text = "Test4", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test5", Text = "Test5", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test6", Text = "Test6", Created = now, Modified = now, Timestamp = now.AddDays(7) },
            new Notification {Title = "Test7", Text = "Test7", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test8", Text = "Test8", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test9", Text = "Test9", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test10", Text = "Test10", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test11", Text = "Test11", Created = now, Modified = now, Timestamp = now },
            new Notification {Title = "Test12", Text = "Test12", Created = now, Modified = now, Timestamp = now.AddDays(8) },
        };

        db.AddRange(newNotifications);
        await db.SaveChangesAsync();
    }
}