import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/pages/device_info.dart';
import 'package:agrigate/pages/devices.dart';
import 'package:agrigate/pages/home.dart';
import 'package:agrigate/pages/notifications.dart';
import 'package:agrigate/pages/settings/settings.dart';
import 'package:agrigate/services/notification_service/notification_service.dart';
import 'package:flutter/material.dart';

class Agrigate extends StatefulWidget {
  const Agrigate({super.key});

  @override
  State<Agrigate> createState() => _AgrigateState();
}

class _AgrigateState extends State<Agrigate> {
  @override
  void initState() {
    super.initState();

    if (kIsMobile) {
      NotificationService.requestNotificationPermissions();
    }
  }

  @override
  void dispose() {
    notificationStream.close();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: kAgrigate,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.green,
        ),
        useMaterial3: true,
      ),
      routes: {
        Home.route: (context) => const Home(),
        Devices.route: (context) => const Devices(),
        DeviceInfo.route: (context) => const DeviceInfo(),
        Settings.route: (context) => const Settings(),
        Notifications.route: (context) => const Notifications(),
      },
      navigatorObservers: [
        routeObserver,
      ],
    );
  }
}
