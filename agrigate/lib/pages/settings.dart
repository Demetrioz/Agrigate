import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class Settings extends StatefulWidget {
  const Settings({super.key});

  static const String title = 'Settings';
  static const String route = '/settings';

  @override
  State<Settings> createState() => _SettingsState();
}

class _SettingsState extends State<Settings> {
  late FlutterSecureStorage _storage;
  late TextEditingController _serverUrlController;

  void _loadSettings() async {
    try {
      final allSettings = await _storage.readAll();

      setState(() {
        _serverUrlController.text = allSettings[kServerUrl] ?? '';
      });
    } catch (e) {
      // TODO: Display error
    }
  }

  void _saveSettings() async {
    try {
      await _storage.write(
        key: kServerUrl,
        value: _serverUrlController.text,
      );
    } catch (e) {
      // TODO: Display error
    }
  }

  @override
  void initState() {
    super.initState();

    _storage = const FlutterSecureStorage();
    _serverUrlController = TextEditingController();

    _loadSettings();
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
