import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/components/devices/create_device_sheet.dart';
import 'package:agrigate/components/devices/device_card.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/models/devices/device_base.dart';
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
  bool _isLoading = false;
  List<DeviceBase> _devices = [];

  void _loadDevices() async {
    try {
      setState(() {
        _isLoading = true;
      });

      final result = await apiService.getDevices();

      setState(() {
        _devices = result;
      });
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error loading devices',
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

  void _addNewDevice() async {
    final newDevice = await showModalBottomSheet(
      context: context,
      builder: (context) => const CreateDeviceSheet(),
    );

    if (newDevice != null) {
      setState(() {
        _devices.add(newDevice);
      });
    }
  }

  @override
  void initState() {
    super.initState();

    _loadDevices();
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Devices',
      floatingAction: _addNewDevice,
      content: _isLoading
          ? const Center(
              child: CircularProgressIndicator(),
            )
          : _devices.isEmpty
              ? const Center(
                  child: Text('No devices found'),
                )
              : ListView.builder(
                  shrinkWrap: true,
                  itemCount: _devices.length,
                  itemBuilder: (BuildContext ctx, int index) {
                    final item = _devices.elementAt(index);

                    return DeviceCard(
                      id: item.id,
                      name: item.name,
                      location: item.location,
                      isActive: item.isActive,
                    );
                  }),
    );
  }
}
