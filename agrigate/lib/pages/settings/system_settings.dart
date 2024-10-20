import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class SystemSettings extends StatefulWidget {
  const SystemSettings({super.key});

  @override
  State<SystemSettings> createState() => _SystemSettingsState();
}

class _SystemSettingsState extends State<SystemSettings> {
  final _storage = const FlutterSecureStorage();
  final _serverUrlController = TextEditingController();
  final _apiKeyController = TextEditingController();

  void _loadSettings() async {
    try {
      final settings = await Future.wait([
        _storage.read(key: kServerUrl),
        _storage.read(key: kApiKey),
      ]);

      setState(() {
        _serverUrlController.text = settings[0] ?? '';
        _apiKeyController.text = settings[1] ?? '';
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
    return Column(
      children: [
        AgrigateTextfield(
          label: 'Server URL',
          controller: _serverUrlController,
        ),
        AgrigateTextfield(
          label: 'API Key',
          obscureText: true,
          controller: _apiKeyController,
        ),
        ElevatedButton(
          onPressed: _saveSettings,
          child: const Text('Save'),
        )
      ],
    );
  }
}
