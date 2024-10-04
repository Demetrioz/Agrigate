import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Settings extends StatefulWidget {
  const Settings({super.key});

  @override
  State<Settings> createState() => _SettingsState();
}

class _SettingsState extends State<Settings> {
  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Settings',
      content: Column(
        children: [
          const TextField(
            decoration: InputDecoration(
              label: Text('Server URL'),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.all(Radius.circular(12)),
                borderSide: BorderSide(color: Colors.black),
              ),
            ),
          ),
          ElevatedButton(onPressed: () {}, child: const Text('Save'))
        ],
      ),
    );
  }
}
