import 'package:flutter/material.dart';

class AgrigateDrawer extends StatelessWidget {
  const AgrigateDrawer({super.key});

  void navigateToPage(BuildContext context, String route) {
    final routeName = ModalRoute.of(context)?.settings.name;

    Navigator.pop(context);
    if (routeName != route) {
      Navigator.pushNamed(context, route);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        children: [
          DrawerHeader(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                const Text(
                  'Agrigate',
                  style: TextStyle(fontSize: 24),
                ),
                Image.asset(
                  'assets/images/logo.png',
                  height: 60,
                )
              ],
            ),
          ),
          ListTile(
            leading: const Icon(Icons.home),
            title: const Text('Home'),
            onTap: () => navigateToPage(context, '/'),
          ),
          ListTile(
            leading: const Icon(Icons.router_outlined),
            title: const Text('Devices'),
            onTap: () => navigateToPage(context, '/devices'),
          ),
          ListTile(
            leading: const Icon(Icons.settings),
            title: const Text('Settings'),
            onTap: () => navigateToPage(context, '/settings'),
          ),
        ],
      ),
    );
  }
}
