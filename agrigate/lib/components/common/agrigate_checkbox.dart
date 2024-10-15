import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class AgrigateCheckbox extends StatelessWidget {
  const AgrigateCheckbox({
    super.key,
    required this.label,
    required this.value,
    required this.handleChange,
    this.alignment,
  });

  final String label;
  final bool value;
  final ValueChanged<bool?> handleChange;
  final MainAxisAlignment? alignment;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(ksmall),
      child: Row(
        mainAxisAlignment: alignment ?? MainAxisAlignment.start,
        children: [
          Checkbox(
            value: value,
            onChanged: handleChange,
          ),
          Text(
            label,
            style: const TextStyle(fontSize: kMedium),
          ),
        ],
      ),
    );
  }
}
