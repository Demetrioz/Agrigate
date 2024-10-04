import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Settings extends StatefulWidget {
  const Settings({super.key});

  static const String title = 'Settings';
  static const String route = '/settings';

  @override
  State<Settings> createState() => _SettingsState();
}

class _SettingsState extends State<Settings> {
  late TextEditingController _serverUrlController;

  void _saveSettings() async {
    // TODO: Make API Call
  }

  @override
  void initState() {
    super.initState();

    _serverUrlController = TextEditingController();
  }

  @override
  void dispose() {
    _serverUrlController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Settings',
      content: Column(
        children: [
          AgrigateTextfield(
            label: 'Server URL',
            controller: _serverUrlController,
          ),
          ElevatedButton(
            onPressed: _saveSettings,
            child: const Text('Save'),
          )
        ],
      ),
    );
  }
}
