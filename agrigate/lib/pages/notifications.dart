import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Notifications extends StatefulWidget {
  const Notifications({super.key});

  static const String title = 'Notifications';
  static const String route = '/notifications';

  @override
  State<Notifications> createState() => _NotificationsState();
}

class _NotificationsState extends State<Notifications> {
  @override
  Widget build(BuildContext context) {
    return const PageBase(
      content: Center(
        child: Text('Notifications'),
      ),
      title: 'Notifications',
    );
  }
}
