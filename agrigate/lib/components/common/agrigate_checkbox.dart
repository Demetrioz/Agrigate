import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class AgrigateCheckbox extends StatelessWidget {
  const AgrigateCheckbox(
      {super.key,
      required this.label,
      required this.value,
      required this.handleChange});

  final String label;
  final bool value;
  final ValueChanged<bool?> handleChange;

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Checkbox(
          value: value,
          onChanged: handleChange,
        ),
        Text(
          label,
          style: const TextStyle(fontSize: kLarge),
        ),
      ],
    );
  }
}
