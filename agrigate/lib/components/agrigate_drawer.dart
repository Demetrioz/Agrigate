import 'package:agrigate/constants.dart';
import 'package:agrigate/pages/devices.dart';
import 'package:agrigate/pages/home.dart';
import 'package:agrigate/pages/notifications.dart';
import 'package:agrigate/pages/settings/settings.dart';
import 'package:agrigate/providers/notification_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class AgrigateDrawer extends StatelessWidget {
  const AgrigateDrawer({super.key});

  void navigateToPage(BuildContext context, String route) {
    final routeName = ModalRoute.of(context)!.settings.name;
    Navigator.pop(context);

    if (routeName != route) {
      Navigator.pushNamed(context, route);
    }
  }

  @override
  Widget build(BuildContext context) {
    final hasNotifications =
        context.watch<NotificationProvider>().hasUnreadNotifications;

    return Drawer(
      child: ListView(
        children: [
          DrawerHeader(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                const Text(
                  kAgrigate,
                  style: TextStyle(
                    fontSize: kLarge,
                  ),
                ),
                Image.asset(
                  kLogoPath,
                  height: kLogoHeight,
                )
              ],
            ),
          ),
          ListTile(
            leading: const Icon(Icons.home),
            title: const Text(Home.title),
            onTap: () => navigateToPage(context, Home.route),
          ),
          ListTile(
            leading: const Icon(Icons.router_outlined),
            title: const Text(Devices.title),
            onTap: () => navigateToPage(context, Devices.route),
          ),
          ListTile(
            leading: const Icon(Icons.chat),
            title: const Text(Notifications.title),
            onTap: () => navigateToPage(context, Notifications.route),
            trailing: hasNotifications
                ? const Icon(
                    Icons.error_outline,
                    color: Colors.red,
                  )
                : null,
          ),
          ListTile(
            leading: const Icon(Icons.settings),
            title: const Text(Settings.title),
            onTap: () => navigateToPage(context, Settings.route),
          ),
        ],
      ),
    );
  }
}
