import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class CreateDeviceSheet extends StatefulWidget {
  const CreateDeviceSheet({super.key});

  @override
  State<CreateDeviceSheet> createState() => _CreateDeviceSheetState();
}

class _CreateDeviceSheetState extends State<CreateDeviceSheet> {
  late TextEditingController _nameController;
  late TextEditingController _locationController;

  void _createDevice() async {
    // TODO: Make API Call

    Navigator.pop(context);
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
                onPressed: _createDevice,
                child: const Text('Save'),
              )
            ],
          ),
        ),
      ),
    );
  }
}
