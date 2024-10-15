import 'package:agrigate/agrigate.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/services/api_service/api_service.dart';
import 'package:agrigate/services/mqtt_service/mqtt_service.dart';
import 'package:agrigate/services/notification_service/notification_service.dart';
import 'package:flutter/material.dart';

// Allow apiService to be accessed from other areas of the app
final apiService = ApiService();
late MqttService? mqttService;

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();

  await apiService.initialize();
  await NotificationService.initialize();

  if (kIsMobile) {
    MqttService.initializeBackgroundService();
  } else {
    mqttService = MqttService();
    await mqttService!.loadSettingsAndConnect();
  }

  runApp(const Agrigate());
}
