import 'dart:async';

import 'package:agrigate/agrigate.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/observers/route_observer.dart';
import 'package:agrigate/providers/notification_provider.dart';
import 'package:agrigate/services/api_service/api_service.dart';
import 'package:agrigate/services/mqtt_service/mqtt_service.dart';
import 'package:agrigate/services/notification_service/notification_service.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

// Allow services to be accessed from other areas of the app
final apiService = ApiService();
late MqttService? mqttService;

// Create a stream so the app can respond to notification-related events
// since the plugin is intiialized in main()
final StreamController<String?> notificationStream =
    StreamController<String?>.broadcast();

// Observe routes so we know if we should navigate to the Notififcations page
// on when tapping a notification
final AgrigateRouteObserver routeObserver = AgrigateRouteObserver();

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

  runApp(MultiProvider(
    providers: [
      ChangeNotifierProvider(
        create: (_) => NotificationProvider(),
      ),
    ],
    child: const Agrigate(),
  ));
}
