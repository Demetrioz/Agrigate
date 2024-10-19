import 'dart:io';

import 'package:agrigate/components/common/agrigate_checkbox.dart';
import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/components/common/callout.dart';
import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/services/mqtt_service/service_events.dart';
import 'package:flutter/material.dart';
import 'package:flutter_background_service/flutter_background_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

enum ConnectionType { mqtt, websocket }

class NotificationSettings extends StatefulWidget {
  const NotificationSettings({super.key});

  @override
  State<NotificationSettings> createState() => _NotificationSettingsState();
}

class _NotificationSettingsState extends State<NotificationSettings> {
  final _storage = const FlutterSecureStorage();
  final _hostController = TextEditingController();
  final _portController = TextEditingController();
  final _clientController = TextEditingController();
  final _userController = TextEditingController();
  final _passwordController = TextEditingController();
  final _channelController = TextEditingController();
  final _showNotificationCallout = !(Platform.isAndroid || Platform.isIOS);

  ConnectionType _connectionType = ConnectionType.mqtt;
  bool _secureConnection = true;

  void _updateConnectionType(Set<ConnectionType> selectedType) {
    setState(() {
      _connectionType = selectedType.first;
    });
  }

  void _updateSecure(bool? updatedValue) {
    if (updatedValue != null) {
      setState(() {
        _secureConnection = updatedValue;
      });
    }
  }

  void _loadSettings() async {
    try {
      final settings = await Future.wait([
        _storage.read(key: kNotificationConnection),
        _storage.read(key: kNotificationSecure),
        _storage.read(key: kNotificationHost),
        _storage.read(key: kNotificationPort),
        _storage.read(key: kNotificationClient),
        _storage.read(key: kNotificationUser),
        _storage.read(key: kNotificationPassword),
        _storage.read(key: kNotificationTopics),
      ]);

      setState(() {
        _connectionType = settings[0].toString() == 'ConnectionType.mqtt'
            ? ConnectionType.mqtt
            : ConnectionType.websocket;
        _secureConnection = settings[1].toString() == 'true';
        _hostController.text = settings[2] ?? '';
        _portController.text = settings[3] ?? '';
        _clientController.text = settings[4] ?? '';
        _userController.text = settings[5] ?? '';
        _passwordController.text = settings[6] ?? '';
        _channelController.text = settings[7] ?? '';
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
            key: kNotificationConnection, value: _connectionType.toString()),
        _storage.write(
            key: kNotificationSecure, value: _secureConnection.toString()),
        _storage.write(key: kNotificationHost, value: _hostController.text),
        _storage.write(key: kNotificationPort, value: _portController.text),
        _storage.write(key: kNotificationClient, value: _clientController.text),
        _storage.write(key: kNotificationUser, value: _userController.text),
        _storage.write(
            key: kNotificationPassword, value: _passwordController.text),
        _storage.write(key: kNotificationTopics, value: _channelController.text)
      ]);

      if (kIsMobile) {
        FlutterBackgroundService()
            .invoke(kServiceEvents[ServiceEvent.initialize]!);
      } else {
        mqttService?.loadSettingsAndConnect();
      }
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
    _hostController.dispose();
    _portController.dispose();
    _clientController.dispose();
    _userController.dispose();
    _passwordController.dispose();
    _channelController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final callout = _showNotificationCallout
        ? const Callout(
            type: CalloutType.warning,
            text:
                'Notifications will only work while the application is running')
        : const SizedBox.shrink();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        callout,
        SegmentedButton<ConnectionType>(
          segments: const [
            ButtonSegment<ConnectionType>(
              value: ConnectionType.mqtt,
              label: Text('MQTT'),
            ),
            ButtonSegment(
              value: ConnectionType.websocket,
              label: Text('Websocket'),
            ),
          ],
          selected: <ConnectionType>{_connectionType},
          onSelectionChanged: _updateConnectionType,
        ),
        AgrigateCheckbox(
          label: 'Secure Connection?',
          value: _secureConnection,
          alignment: MainAxisAlignment.center,
          handleChange: _updateSecure,
        ),
        AgrigateTextfield(
          label: 'Host',
          controller: _hostController,
        ),
        AgrigateTextfield(
          label: 'Port',
          controller: _portController,
        ),
        AgrigateTextfield(
          label: 'ClientId',
          controller: _clientController,
        ),
        AgrigateTextfield(
          label: 'Username',
          controller: _userController,
        ),
        AgrigateTextfield(
          label: 'Password',
          obscureText: true,
          controller: _passwordController,
        ),
        AgrigateTextfield(
          label: 'Topics',
          controller: _channelController,
          helperText:
              'A Comma separated list of topics where notifications are published',
        ),
        ElevatedButton(
          onPressed: _saveSettings,
          child: const Text('Save'),
        ),
      ],
    );
  }
}
