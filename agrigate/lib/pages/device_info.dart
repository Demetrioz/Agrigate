import 'package:agrigate/components/common/agrigate_spacer.dart';
import 'package:agrigate/components/device_info/device_rule_sheet.dart';
import 'package:agrigate/components/device_info/rule_card.dart';
import 'package:agrigate/components/device_info/telemetry_reading.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/pages/page_base.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';

class DeviceInfo extends StatefulWidget {
  const DeviceInfo({super.key});

  static const String title = 'Device Info';
  static const String route = '/deviceInfo';

  @override
  State<DeviceInfo> createState() => _DeviceInfoState();
}

class _DeviceInfoState extends State<DeviceInfo> {
  int deviceId = 0;
  String title = '';

  List<Color> gradientColors = [
    Colors.green,
    Colors.greenAccent,
  ];

  void _loadDeviceDetails() {
    // print(deviceId);
    // print(title);
  }

  void _addNewRule() {
    showModalBottomSheet(
      context: context,
      builder: (context) => const DeviceRuleSheet(),
    );
  }

  Widget bottomTitleWidgets(double value, TitleMeta meta) {
    const style = TextStyle(
      fontWeight: FontWeight.bold,
      fontSize: 16,
    );
    Widget text;
    switch (value.toInt()) {
      case 2:
        text = const Text('MAR', style: style);
        break;
      case 5:
        text = const Text('JUN', style: style);
        break;
      case 8:
        text = const Text('SEP', style: style);
        break;
      default:
        text = const Text('', style: style);
        break;
    }

    return SideTitleWidget(
      axisSide: meta.axisSide,
      child: text,
    );
  }

  Widget leftTitleWidgets(double value, TitleMeta meta) {
    const style = TextStyle(
      fontWeight: FontWeight.bold,
      fontSize: 15,
    );
    String text;
    switch (value.toInt()) {
      case 1:
        text = '10K';
        break;
      case 3:
        text = '30k';
        break;
      case 5:
        text = '50k';
        break;
      default:
        return Container();
    }

    return Text(text, style: style, textAlign: TextAlign.left);
  }

  LineChartData mainData() {
    return LineChartData(
      gridData: FlGridData(
        show: true,
        drawVerticalLine: true,
        horizontalInterval: 1,
        verticalInterval: 1,
        getDrawingHorizontalLine: (value) {
          return const FlLine(
            // color: AppColors.mainGridLineColor,
            strokeWidth: 1,
          );
        },
        getDrawingVerticalLine: (value) {
          return const FlLine(
            // color: AppColors.mainGridLineColor,
            strokeWidth: 1,
          );
        },
      ),
      titlesData: FlTitlesData(
        show: true,
        rightTitles: const AxisTitles(
          sideTitles: SideTitles(showTitles: false),
        ),
        topTitles: const AxisTitles(
          sideTitles: SideTitles(showTitles: false),
        ),
        bottomTitles: AxisTitles(
          sideTitles: SideTitles(
            showTitles: true,
            reservedSize: 30,
            interval: 1,
            getTitlesWidget: bottomTitleWidgets,
          ),
        ),
        leftTitles: AxisTitles(
          sideTitles: SideTitles(
            showTitles: true,
            interval: 1,
            getTitlesWidget: leftTitleWidgets,
            reservedSize: 42,
          ),
        ),
      ),
      borderData: FlBorderData(
        show: true,
        border: Border.all(color: const Color(0xff37434d)),
      ),
      minX: 0,
      maxX: 11,
      minY: 0,
      maxY: 6,
      lineBarsData: [
        LineChartBarData(
          spots: const [
            FlSpot(0, 3),
            FlSpot(2.6, 2),
            FlSpot(4.9, 5),
            FlSpot(6.8, 3.1),
            FlSpot(8, 4),
            FlSpot(9.5, 3),
            FlSpot(11, 4),
          ],
          isCurved: true,
          gradient: LinearGradient(
            colors: gradientColors,
          ),
          barWidth: 5,
          isStrokeCapRound: true,
          dotData: const FlDotData(
            show: false,
          ),
          belowBarData: BarAreaData(
            show: true,
            gradient: LinearGradient(
              colors: gradientColors
                  .map((color) => color.withOpacity(0.3))
                  .toList(),
            ),
          ),
        ),
      ],
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

      if (newDeviceId != deviceId && newTitle != title) {
        setState(() {
          deviceId = newDeviceId;
          title = newTitle;
        });
      }

      _loadDeviceDetails();
    } catch (e) {
      // TODO: Dispaly error
    }
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: title,
      floatingAction: _addNewRule,
      content: Column(
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
          AspectRatio(
            aspectRatio: 1.7,
            child: LineChart(mainData()),
          ),
          const Padding(
            padding: EdgeInsets.symmetric(
              horizontal: kMedium,
              vertical: kLarge,
            ),
            // TODO: Map all recent telemetry
            child: TelemetryReading(name: 'Moisture', value: 443),
          ),
          const Divider(),
          const Text(
            'Telemetry Rules',
            style: TextStyle(
              fontSize: kLarge,
            ),
          ),
          // TODO: Map all rules
          const RuleCard(
            id: 1,
            name: 'Low Moisture',
            summary: 'Lower Limit - Notification',
            isActive: true,
          )
        ],
      ),
    );
  }
}
