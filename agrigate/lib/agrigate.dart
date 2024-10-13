import 'package:agrigate/constants.dart';
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
      },
    );
  }
}
