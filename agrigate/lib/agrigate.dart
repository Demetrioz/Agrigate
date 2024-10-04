import 'package:agrigate/pages/device_info.dart';
import 'package:agrigate/pages/devices.dart';
import 'package:agrigate/pages/home.dart';
import 'package:agrigate/pages/settings.dart';
import 'package:flutter/material.dart';

class Agrigate extends StatelessWidget {
  const Agrigate({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Agrigate',
      theme: ThemeData(
          colorScheme: ColorScheme.fromSeed(seedColor: Colors.green),
          useMaterial3: true),
      routes: {
        '/': (context) => const Home(),
        '/devices': (context) => const Devices(),
        '/deviceInfo': (context) => const DeviceInfo(),
        '/settings': (context) => const Settings()
      },
    );
  }
}
