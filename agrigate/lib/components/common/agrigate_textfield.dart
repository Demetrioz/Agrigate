import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class AgrigateTextfield extends StatelessWidget {
  const AgrigateTextfield({
    super.key,
    required this.label,
    required this.controller,
  });

  final String label;
  final TextEditingController controller;

  @override
  Widget build(BuildContext context) {
    return TextField(
      decoration: InputDecoration(
        label: Text(label),
        border: const OutlineInputBorder(
          borderRadius: BorderRadius.all(
            Radius.circular(kMedium),
          ),
        ),
      ),
    );
  }
}
