import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:flutter/material.dart';

class CreateDeviceSheet extends StatefulWidget {
  const CreateDeviceSheet({super.key});

  @override
  State<CreateDeviceSheet> createState() => _CreateDeviceSheetState();
}

class _CreateDeviceSheetState extends State<CreateDeviceSheet> {
  bool _isLoading = false;
  late TextEditingController _nameController;
  late TextEditingController _locationController;

  void _createDevice() async {
    try {
      if (_nameController.text.isEmpty) {
        throw Exception('A name is required');
      }

      if (_locationController.text.isEmpty) {
        throw Exception('A location is required');
      }

      setState(() {
        _isLoading = true;
      });

      final newDevice = await apiService.createDevice(
        _nameController.text,
        _locationController.text,
      );

      if (mounted) {
        Navigator.pop(context, newDevice);
      }
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error creating device',
            message: e.toString(),
          ),
        );
      }
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  @override
  void initState() {
    super.initState();

    _nameController = TextEditingController();
    _locationController = TextEditingController();
  }

  @override
  void dispose() {
    _nameController.dispose();
    _locationController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: kSmallBottomSheetHeight,
      child: Padding(
        padding: const EdgeInsets.all(kMedium),
        child: SingleChildScrollView(
          child: Column(
            children: [
              const Text(
                'Add Device',
                style: TextStyle(
                  fontSize: kLarge,
                ),
              ),
              AgrigateTextfield(
                label: 'Device Name',
                controller: _nameController,
              ),
              AgrigateTextfield(
                label: 'Device Location',
                controller: _locationController,
              ),
              OutlinedButton(
                onPressed: _isLoading ? null : _createDevice,
                child: const Text('Save'),
              )
            ],
          ),
        ),
      ),
    );
  }
}
