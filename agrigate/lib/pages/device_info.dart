import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/components/device_info/device_rule_sheet.dart';
import 'package:agrigate/components/device_info/rule_card.dart';
import 'package:agrigate/components/device_info/telemetry_chart.dart';
import 'package:agrigate/components/device_info/telemetry_reading.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/models/devices/device_details.dart';
import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class DeviceInfo extends StatefulWidget {
  const DeviceInfo({super.key});

  static const String title = 'Device Info';
  static const String route = '/deviceInfo';

  @override
  State<DeviceInfo> createState() => _DeviceInfoState();
}

class _DeviceInfoState extends State<DeviceInfo> {
  bool _isLoading = false;
  int _deviceId = 0;
  String _title = '';
  DeviceDetails? _details;

  void _loadDeviceDetails() async {
    try {
      setState(() {
        _isLoading = true;
      });

      final result = await apiService.getDeviceDetails(_deviceId);
      setState(() {
        _details = result;
      });
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error loading details',
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

  void _addNewRule() {
    showModalBottomSheet(
      context: context,
      builder: (context) => const DeviceRuleSheet(),
    );
  }

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();

    final arguments =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;

    try {
      final newDeviceId = arguments['deviceId'];
      final newTitle = arguments['title'];

      if (newDeviceId != _deviceId && newTitle != _title) {
        setState(() {
          _deviceId = newDeviceId;
          _title = newTitle;
        });
      }

      if (_details == null) {
        _loadDeviceDetails();
      }
    } catch (e) {
      showDialog(
        context: context,
        builder: (BuildContext context) => ErrorDialog(
          title: 'Error',
          message: e.toString(),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: _title,
      // floatingAction: _addNewRule,
      content: _isLoading
          ? const Center(
              child: CircularProgressIndicator(),
            )
          : _details == null
              ? const Center(
                  child: Text('Unable to load details'),
                )
              : Column(
                  children: [
                    TelemetryChart(
                      deviceId: _deviceId,
                    ),
                    Padding(
                      padding: const EdgeInsets.symmetric(
                        horizontal: kMedium,
                        vertical: kLarge,
                      ),
                      child: ListView.builder(
                        shrinkWrap: true,
                        itemCount: _details!.distinctTelemetry.length,
                        itemBuilder: (BuildContext ctx, int index) {
                          final item =
                              _details!.distinctTelemetry.elementAt(index);

                          return Padding(
                            padding: const EdgeInsets.symmetric(
                              vertical: ksmall,
                            ),
                            child: TelemetryReading(
                              name: item.key,
                              value: item.value,
                            ),
                          );
                        },
                      ),
                    ),
                    const Divider(),
                    const Text(
                      'Telemetry Rules',
                      style: TextStyle(
                        fontSize: kLarge,
                      ),
                    ),
                    _details!.rules.isEmpty
                        ? const Center(
                            child: Text('No Rules Available'),
                          )
                        : ListView.builder(
                            shrinkWrap: true,
                            itemCount: _details!.rules.length,
                            itemBuilder: (BuildContext ctx, int index) {
                              final item = _details!.rules.elementAt(index);

                              return RuleCard(
                                id: item.id,
                                name: item.name,
                                summary: item.summary,
                                isActive: item.isActive,
                              );
                            },
                          ),
                  ],
                ),
    );
  }
}
