using MQTTnet;
using MQTTnet.Client;

namespace Agrigate.Core.Services.MqttService;

/// <inheritdoc />
public class MqttService : IMqttService
{
    private readonly MqttFactory _mqttFactory;

    public MqttService()
    {
        _mqttFactory = new MqttFactory();
    }

    /// <inheritdoc />
    public IMqttClient CreateMqttClient() => _mqttFactory.CreateMqttClient();

    /// <inheritdoc />
    public MqttClientSubscribeOptionsBuilder CreateSubscribeOptionsBuilder()
        => _mqttFactory.CreateSubscribeOptionsBuilder();
}