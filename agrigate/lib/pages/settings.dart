import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
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
  final FlutterSecureStorage _storage = const FlutterSecureStorage();
  final TextEditingController _serverUrlController = TextEditingController();
  final TextEditingController _apiKeyController = TextEditingController();

  void _loadSettings() async {
    try {
      final allSettings = await _storage.readAll();

      setState(() {
        _serverUrlController.text = allSettings[kServerUrl] ?? '';
        _apiKeyController.text = allSettings[kApiKey] ?? '';
      });
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error loading settings',
            message: e.toString(),
          ),
        );
      }
    }
  }

  void _saveSettings() async {
    try {
      await Future.wait([
        _storage.write(
          key: kServerUrl,
          value: _serverUrlController.text,
        ),
        _storage.write(
          key: kApiKey,
          value: _apiKeyController.text,
        )
      ]);

      // Update the Api service to use the newly saved URL
      apiService.initialize(
        updatedUrl: _serverUrlController.text,
        updatedApiKey: _apiKeyController.text,
      );
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error saving settings',
            message: e.toString(),
          ),
        );
      }
    }
  }

  @override
  void initState() {
    super.initState();

    _loadSettings();
  }

  @override
  void dispose() {
    _serverUrlController.dispose();
    _apiKeyController.dispose();

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
          AgrigateTextfield(
            label: 'API Key',
            controller: _apiKeyController,
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
