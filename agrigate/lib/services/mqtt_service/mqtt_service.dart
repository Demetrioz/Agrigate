import 'dart:ui';

import 'package:agrigate/constants.dart';
import 'package:agrigate/pages/settings/notification_settings.dart';
import 'package:agrigate/services/mqtt_service/service_events.dart';
import 'package:agrigate/services/notification_service/notification_service.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_background_service/flutter_background_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:mqtt_client/mqtt_client.dart';
import 'package:mqtt_client/mqtt_server_client.dart';

// Android startup logic for initializing the MQTT Client when running as a
// background service
@pragma('vm:entry-point')
void onMqttServiceStart(ServiceInstance service) async {
  if (kDebugMode) debugPrint('Starting MQTT Service...');

  final mqttService = MqttService();

  await mqttService.loadSettingsAndConnect();

  // Register handlers for the background service
  service.on(kServiceEvents[ServiceEvent.initialize]!).listen((data) async {
    if (kDebugMode) debugPrint('Re-initializing the MQTT client');

    mqttService.loadSettingsAndConnect();
  });
}

// iOS startup logic for initializing the MQTT Client when running as a
// background service
@pragma('vm:entry-point')
Future<bool> onIosBackground(ServiceInstance service) async {
  WidgetsFlutterBinding.ensureInitialized();
  DartPluginRegistrant.ensureInitialized();

  return true;
}

// The actual MQTT Service
class MqttService {
  static final MqttService _instance = MqttService._internal();
  final _storage = const FlutterSecureStorage();

  MqttServerClient? _client;

  factory MqttService() => _instance;
  MqttService._internal();

  // Initializes the MQTT Background Service on iOS and Android
  static Future<void> initializeBackgroundService() async {
    if (kDebugMode) debugPrint('Initializing MQTT Service...');

    final service = FlutterBackgroundService();
    final serviceIsRunning = await service.isRunning();

    if (serviceIsRunning) {
      if (kDebugMode) debugPrint('Service is already initialized');
    } else {
      await service.configure(
        iosConfiguration: IosConfiguration(
          autoStart: true,
          onForeground: onMqttServiceStart,
          onBackground: onIosBackground,
        ),
        androidConfiguration: AndroidConfiguration(
          autoStart: true,
          isForegroundMode: false,
          autoStartOnBoot: true,
          onStart: onMqttServiceStart,
        ),
      );
    }
  }

  // Load connection settings and attempt to connect to the MQTT broker
  Future<void> loadSettingsAndConnect() async {
    try {
      final settings = await Future.wait([
        _storage.read(key: kNotificationConnection),
        _storage.read(key: kNotificationSecure),
        _storage.read(key: kNotificationHost),
        _storage.read(key: kNotificationPort),
        _storage.read(key: kNotificationClient),
        _storage.read(key: kNotificationUser),
        _storage.read(key: kNotificationPassword),
        _storage.read(key: kNotificationTopics),
      ]);

      final connectionType = settings[0].toString() == 'ConnectionType.mqtt'
          ? ConnectionType.mqtt
          : ConnectionType.websocket;
      final secureConnection = settings[1].toString() == 'true';
      final host = settings[2] ?? '';
      final port = int.tryParse(settings[3] ?? '') ?? 0;
      final client = settings[4] ?? '';
      final user = settings[5] ?? '';
      final password = settings[6] ?? '';
      final channel = settings[7] ?? '';

      if (_clientIsConnected()) _client!.disconnect();

      _initializeClient(
        host,
        client,
        port,
      );

      await _connectToBroker(
        user,
        password,
        connectionType,
        secureConnection,
      );

      if (_clientIsConnected() && channel.isNotEmpty) {
        _subscribeToTopics(channel);
      }
    } catch (e) {
      if (kDebugMode) {
        debugPrint('Error connecting to MQTT Broker: ${e.toString()}');
      }
    }
  }

  // Initialize the MQTT client
  void _initializeClient(String host, String clientId, int port) {
    _client = MqttServerClient.withPort(host, clientId, port);
  }

  // Connect to the MQTT Broker
  Future<void> _connectToBroker(
    String username,
    String password,
    ConnectionType connectionType,
    bool secure,
  ) async {
    if (kDebugMode) debugPrint('Attempting to connect to broker...');

    try {
      if (_client == null) {
        throw Exception('MQTT Client has not been initialized');
      }

      MqttConnectMessage connectMessage = MqttConnectMessage()
          .withWillTopic('willTopic')
          .withWillMessage('willMessage')
          .startClean()
          .withWillQos(MqttQos.atLeastOnce);

      if (username.isNotEmpty && password.isNotEmpty) {
        connectMessage.authenticateAs(username, password);
      }

      _client!.keepAlivePeriod = 60;
      _client!.autoReconnect = true;
      _client!.secure = secure;
      _client!.useWebSocket = connectionType == ConnectionType.websocket;
      _client!.connectionMessage = connectMessage;

      await _client!.connect();
      _client!.updates!.listen(_handleIncomingMessage);

      if (kDebugMode) debugPrint('Connected to broker!');
    } catch (e) {
      if (kDebugMode) debugPrint('Error connecing to broker: $e');
      _client?.disconnect();
    }
  }

  void _subscribeToTopics(String topics, {MqttQos qos = MqttQos.atLeastOnce}) {
    if (kDebugMode) debugPrint('Subscribing to $topics...');

    if (_clientIsConnected()) {
      final splitTopics = topics.replaceAll(' ', '').split(',');
      for (var topic in splitTopics) {
        _client!.subscribe(topic, qos);
      }
    }
  }

  // Handle incoming messages from the MQTT Broker
  void _handleIncomingMessage(
      List<MqttReceivedMessage<MqttMessage?>>? messageList) {
    final receivedMessage = messageList![0];

    final publishedMessage = receivedMessage.payload as MqttPublishMessage;
    final topic = receivedMessage.topic;
    final payload = MqttPublishPayload.bytesToStringAsString(
        publishedMessage.payload.message);

    _handleMessage(topic, payload);
  }

  // Routes messages from a subscribed MQTT topic to the correct handler
  void _handleMessage(String topic, String payload) async {
    if (kDebugMode) debugPrint('Received $payload from $topic');

    await NotificationService.showNotification(0, 'Alert!', payload);
  }

  bool _clientIsConnected() =>
      _client?.connectionStatus?.state == MqttConnectionState.connected ||
      _client?.connectionStatus?.state == MqttConnectionState.connecting;
}
