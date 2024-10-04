import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Devices extends StatefulWidget {
  const Devices({super.key});

  @override
  State<Devices> createState() => _DevicesState();
}

class _DevicesState extends State<Devices> {
  void navigateToDevice() {
    Navigator.pushNamed(
      context,
      '/deviceInfo',
      arguments: {'deviceId': 23, 'title': 'TestDevice - Raised Bed 1'},
    );
  }

  void _addNewDevice() {
    showModalBottomSheet(
        context: context,
        builder: (context) {
          return SizedBox(
            height: 300,
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    const Text(
                      'Add Device',
                      style: TextStyle(fontSize: 20),
                    ),
                    const SizedBox(height: 10),
                    const TextField(
                      decoration: InputDecoration(
                        label: Text('Device Name'),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                          borderSide: BorderSide(color: Colors.black),
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                    const TextField(
                      decoration: InputDecoration(
                        label: Text('Device Location'),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                          borderSide: BorderSide(color: Colors.black),
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                    OutlinedButton(
                      onPressed: () {},
                      child: const Text('Save'),
                    ),
                  ],
                ),
              ),
            ),
          );
        });
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Devices',
      floatingAction: _addNewDevice,
      content: Column(
        children: [
          Card(
            child: ListTile(
              title: const Text("Moisture"),
              subtitle: const Text("Raised Bed 1"),
              trailing: const Icon(Icons.circle, color: Colors.red),
              onTap: navigateToDevice,
            ),
          ),
          Card(
            child: ListTile(
              title: const Text("Temp & Humidity"),
              subtitle: const Text("Greenhouse"),
              trailing: const Icon(Icons.circle, color: Colors.green),
              onTap: navigateToDevice,
            ),
          ),
        ],
      ),
    );
  }
}
