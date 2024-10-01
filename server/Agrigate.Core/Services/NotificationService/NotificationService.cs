using Agrigate.Core.Configuration;
using Agrigate.Core.Services.MqttService;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Agrigate.Core.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly NotificationOptions _config;
    private readonly IMqttService _mqttService;

    public NotificationService(
        IOptions<NotificationOptions> options,
        IMqttService mqttService
    )
    {
        _config = options.Value 
            ?? throw new ArgumentNullException(nameof(options));
        _mqttService = mqttService 
            ?? throw new ArgumentNullException(nameof(mqttService));
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
        await client.DisconnectAsync();
    }
}