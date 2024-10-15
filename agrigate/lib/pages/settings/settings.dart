import 'package:agrigate/pages/page_base.dart';
import 'package:agrigate/pages/settings/notification_settings.dart';
import 'package:agrigate/pages/settings/system_settings.dart';
import 'package:flutter/material.dart';

class Settings extends StatefulWidget {
  const Settings({super.key});

  static const String title = 'Settings';
  static const String route = '/settings';

  @override
  State<Settings> createState() => _SettingsState();
}

class _SettingsState extends State<Settings> {
  int _selectedPage = 0;

  final List<Widget> _pages = const [
    SystemSettings(),
    NotificationSettings(),
  ];

  void _setSelectedPage(int page) {
    setState(() {
      _selectedPage = page;
    });
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Settings',
      content: _pages.elementAt(_selectedPage),
      navigationBar: NavigationBar(
        selectedIndex: _selectedPage,
        destinations: const [
          NavigationDestination(
            icon: Icon(Icons.lan),
            label: 'System',
          ),
          NavigationDestination(
            icon: Icon(Icons.notifications),
            label: 'Notifications',
          ),
        ],
        onDestinationSelected: _setSelectedPage,
      ),
    );
  }
}
