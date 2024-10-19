import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class NotificationCard extends StatelessWidget {
  const NotificationCard({
    super.key,
    required this.text,
    required this.date,
    required this.important,
  });

  final String text;
  final DateTime date;
  final bool important;

  @override
  Widget build(BuildContext context) {
    final localDate = date.toLocal();
    final formattedTime = DateFormat.Hm().format(localDate);
    final formattedDate = DateFormat.MMMMd().format(localDate);

    return Card(
      child: ListTile(
        leading: Icon(
          Icons.notifications,
          color: important ? Colors.red : Colors.blue,
        ),
        title: Text('$formattedTime $formattedDate'),
        subtitle: Text(text),
      ),
    );
  }
}
