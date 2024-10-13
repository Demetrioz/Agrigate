import 'package:agrigate/models/devices/telemetry_base.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';

List<Color> gradientColors = [
  Colors.green,
  Colors.greenAccent,
];

Widget xAxisValues(double value, TitleMeta meta) {
  const style = TextStyle(
    fontWeight: FontWeight.bold,
    fontSize: 16,
  );

  Widget text;
  int hoursBack = -2;

  switch (value.toInt()) {
    case 0:
      hoursBack = 23;
      break;
    case 2:
      hoursBack = 19;
      break;
    case 4:
      hoursBack = 15;
      break;
    case 6:
      hoursBack = 11;
      break;
    case 8:
      hoursBack = 7;
      break;
    case 10:
      hoursBack = 3;
      break;
    case 12:
      hoursBack = -1;
      break;
    default:
      break;
  }

  text = hoursBack >= -1
      ? Text(
          '${DateTime.now().subtract(Duration(hours: hoursBack)).hour.toString()}:00',
          style: style,
        )
      : text = const Text('', style: style);

  return SideTitleWidget(
    axisSide: meta.axisSide,
    child: text,
  );
}

Widget yAxisValues(double value, TitleMeta meta) {
  const style = TextStyle(
    fontWeight: FontWeight.bold,
    fontSize: 15,
  );

  String text = value % 2 == 0 ? value.toString() : '';
  return Text(text, style: style, textAlign: TextAlign.left);
}

FlSpot calculateTelemetryChartPosition(TelemetryBase telemetry) {
  // First get the current datetime and timestamp datetime
  final now = DateTime.now();
  final nowDay = now.day;
  final nowHour = now.hour;

  final telemetryDate = telemetry.timestamp.toLocal();
  final telemetryDay = telemetryDate.day;
  final telemetryHour = telemetryDate.hour;
  final telemetryMinute = telemetryDate.minute;

  // The right-limit of the chart should be one hour more than the
  // current hour so we can show telemetry appropriately.
  // ie - if it's 05:45, the chart should go up to 06:00
  final rightLimit = nowHour + 1;

  // We're using a 24 hour clock with the current time equating to
  // 24, so determine the offset used for calculating where each
  // time should be displayed
  final hourDifference = 24 - rightLimit;

  // Divide the hour and minute values by 2 since the chart has 12
  // x-axis values which each represent 2 hours
  double hourValue = (hourDifference + telemetryHour) / 2;
  double minuteValue = (telemetryMinute / 60) / 2;

  // Adjust the hour appropriately if the time is from the prior day
  if (hourValue > 12) {
    hourValue -= 12;
  } else if (hourValue == 12 && telemetryDay < nowDay) {
    hourValue = 0;
  }

  // Calcualte the final x-axis position
  double xPosition = hourValue + minuteValue;

  return FlSpot(xPosition, telemetry.value);
}

LineChartData generateChartData(
  double minValue,
  double maxValue,
  Iterable<List<TelemetryBase>> telemetryGroups,
) {
  return LineChartData(
    gridData: FlGridData(
      show: true,
      drawVerticalLine: true,
      horizontalInterval: 1,
      verticalInterval: 1,
      getDrawingVerticalLine: (value) {
        return const FlLine(
          strokeWidth: 1,
        );
      },
    ),
    titlesData: const FlTitlesData(
      show: true,
      rightTitles: AxisTitles(
        sideTitles: SideTitles(
          showTitles: false,
        ),
      ),
      topTitles: AxisTitles(
        sideTitles: SideTitles(
          showTitles: false,
        ),
      ),
      bottomTitles: AxisTitles(
        sideTitles: SideTitles(
          showTitles: true,
          reservedSize: 30,
          interval: 1,
          getTitlesWidget: xAxisValues,
        ),
      ),
      leftTitles: AxisTitles(
        sideTitles: SideTitles(
          showTitles: true,
          interval: 1,
          getTitlesWidget: yAxisValues,
          reservedSize: 42,
        ),
      ),
    ),
    borderData: FlBorderData(
      show: true,
      border: Border.all(
        color: const Color(0xff37434d),
      ),
    ),
    minX: 0,
    maxX: 12,
    minY: minValue - 1,
    maxY: maxValue + 1,
    lineBarsData: telemetryGroups
        .map(
          (group) => LineChartBarData(
            spots:
                group.map((t) => calculateTelemetryChartPosition(t)).toList(),
            gradient: LinearGradient(
              colors: gradientColors,
            ),
            isCurved: true,
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
        )
        .toList(),
  );
}
