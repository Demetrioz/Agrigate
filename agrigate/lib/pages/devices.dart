import 'package:agrigate/components/devices/create_device_sheet.dart';
import 'package:agrigate/components/devices/device_card.dart';
import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Devices extends StatefulWidget {
  const Devices({super.key});

  static const String title = 'Devices';
  static const String route = '/devices';

  @override
  State<Devices> createState() => _DevicesState();
}

class _DevicesState extends State<Devices> {
  void _addNewDevice() {
    showModalBottomSheet(
      context: context,
      builder: (context) => const CreateDeviceSheet(),
    );
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Devices',
      floatingAction: _addNewDevice,
      content: const Column(
        children: [
          DeviceCard(
            id: 1,
            name: 'Moisture',
            location: 'Raised Bed 1',
            isActive: true,
          ),
          DeviceCard(
            id: 2,
            name: 'Temp & Humidity',
            location: 'Greenhouse',
            isActive: false,
          ),
        ],
      ),
    );
  }
}
