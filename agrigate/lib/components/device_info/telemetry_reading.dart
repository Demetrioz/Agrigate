import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class TelemetryReading extends StatelessWidget {
  const TelemetryReading({
    super.key,
    required this.name,
    required this.value,
  });

  final String name;
  final double value;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(
          name,
          style: const TextStyle(
            fontSize: kLarge,
          ),
        ),
        Text(
          value.toString(),
          style: const TextStyle(
            fontSize: kLarge,
          ),
        )
      ],
    );
  }
}
