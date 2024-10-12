import 'package:agrigate/components/common/agrigate_spacer.dart';
import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/models/devices/telemetry_base.dart';
import 'package:agrigate/utilities/list_utilities.dart';
import 'package:agrigate/utilities/telemetry_chart_utilities.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';

class TelemetryChart extends StatefulWidget {
  const TelemetryChart({
    super.key,
    this.deviceId,
  });

  final int? deviceId;

  @override
  State<TelemetryChart> createState() => _TelemetryChartState();
}

class _TelemetryChartState extends State<TelemetryChart> {
  bool _isLoading = false;
  double _maxTelemetryValue = 0;
  double _minTelemetryValue = 0;
  Map<String, List<TelemetryBase>> _groupedTelemetry = {};

  void _loadDeviceTelemetry() async {
    try {
      setState(() {
        _isLoading = true;
      });

      final now = DateTime.now();
      final telemetry = (await apiService.getDeviceTelemetry(widget.deviceId!))
          .where((t) =>
              t.timestamp.isAfter(now.subtract(const Duration(hours: 23))))
          .toList();

      setState(() {
        _minTelemetryValue =
            telemetry.reduce((a, b) => a.value < b.value ? a : b).value;
        _maxTelemetryValue =
            telemetry.reduce((a, b) => a.value > b.value ? a : b).value;
        _groupedTelemetry = groupBy(telemetry, (t) => t.key);
      });
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error loading telemetry',
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
  void didChangeDependencies() {
    super.didChangeDependencies();

    if (widget.deviceId != null &&
        widget.deviceId! > 0 &&
        _groupedTelemetry.isEmpty) {
      _loadDeviceTelemetry();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        const Text(
          'Telemetry History',
          style: TextStyle(
            fontSize: kLarge,
          ),
        ),
        const AgrigateSpacer(
          size: Size.medium,
          type: SpacerType.vertical,
        ),
        _isLoading
            ? const Center(
                child: CircularProgressIndicator(),
              )
            : Padding(
                padding: const EdgeInsets.symmetric(horizontal: kMedium),
                child: AspectRatio(
                  aspectRatio: 1.7,
                  child: LineChart(
                    generateChartData(
                      _minTelemetryValue,
                      _maxTelemetryValue,
                      _groupedTelemetry.values,
                    ),
                  ),
                ),
              ),
      ],
    );
  }
}
