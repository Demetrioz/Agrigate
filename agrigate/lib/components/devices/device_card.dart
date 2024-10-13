import 'package:flutter/material.dart';

class DeviceCard extends StatelessWidget {
  const DeviceCard(
      {super.key,
      required this.id,
      required this.name,
      required this.location,
      required this.isActive});

  final int id;
  final String name;
  final String location;
  final bool isActive;

  void _navigateToDevice(BuildContext context) => Navigator.pushNamed(
        context,
        '/deviceInfo',
        arguments: {
          'deviceId': id,
          'title': '$name - $location',
        },
      );

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        title: Text(name),
        subtitle: Text(location),
        trailing: Icon(
          Icons.circle,
          color: isActive ? Colors.green : Colors.red,
        ),
        onTap: () => _navigateToDevice(context),
      ),
    );
  }
}
