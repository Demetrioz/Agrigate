using Agrigate.Core.Configuration;
using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Services.NotificationService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Agrigate.Core.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly NotificationOptions _config;
    private readonly IMqttService _mqttService;
    private readonly AgrigateContext _db;

    public NotificationService(
        IOptions<NotificationOptions> options,
        IMqttService mqttService,
        AgrigateContext db
    )
    {
        _config = options.Value 
            ?? throw new ArgumentNullException(nameof(options));
        _mqttService = mqttService 
            ?? throw new ArgumentNullException(nameof(mqttService));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task SendMqttNotification(
        string topic, 
        string payload,
        CancellationToken cancellationToken = default
    )
    {
        using var client = _mqttService.CreateMqttClient();
        var tlsOptions = new MqttClientTlsOptions
        {
            UseTls = _config.SecureConnection
        };

        var options = new MqttClientOptionsBuilder()
            .WithClientId(_config.ClientId)
            .WithTcpServer(_config.Host, _config.Port)
            .WithCredentials(_config.Username, _config.Password)
            .WithProtocolVersion(MqttProtocolVersion.V311)
            .WithTlsOptions(tlsOptions)
            .Build();

        await client.ConnectAsync(options, cancellationToken);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await client.PublishAsync(message, cancellationToken);
        await client.DisconnectAsync(cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Notification> SaveNotification(
        string title,
        string text,
        DateTimeOffset? timestamp = null,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTimeOffset.UtcNow;
        var newNotification = new Notification
        {
            Title = title,
            Text = text,
            Timestamp = timestamp ?? now,
            HasBeenViewed = false,
            Created = now,
            Modified = now,
            IsDeleted = false
        };

        _db.Add(newNotification);
        await _db.SaveChangesAsync(cancellationToken);
        return newNotification;
    }

    /// <inheritdoc />
    public async Task<List<NotificationBase>> GetRecentNotifications(
        CancellationToken cancellationToken = default
    )
    {
        return await _db.Notifications
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.Timestamp)
            .Select(d => new NotificationBase
            {
                Title = d.Title,
                Text = d.Text,
                HasBeenViewed = d.HasBeenViewed,
                Timestamp = d.Timestamp,
            })
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}