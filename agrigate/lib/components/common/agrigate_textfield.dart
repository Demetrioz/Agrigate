import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class AgrigateTextfield extends StatefulWidget {
  const AgrigateTextfield({
    super.key,
    required this.label,
    required this.controller,
    this.helperText,
    this.obscureText,
  });

  final String label;
  final TextEditingController controller;
  final String? helperText;
  final bool? obscureText;

  @override
  State<AgrigateTextfield> createState() => _AgrigateTextfieldState();
}

class _AgrigateTextfieldState extends State<AgrigateTextfield> {
  bool _obscureText = false;
  IconData _icon = Icons.visibility;

  void _toggleVisibility() {
    final updatedValue = !_obscureText;
    setState(() {
      _obscureText = updatedValue;
      _icon = updatedValue ? Icons.visibility : Icons.visibility_off;
    });
  }

  @override
  void initState() {
    super.initState();

    setState(() {
      _obscureText = widget.obscureText ?? false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(ksmall),
      child: TextField(
        decoration: InputDecoration(
          label: Text(widget.label),
          suffixIcon: widget.obscureText == true
              ? IconButton(
                  onPressed: _toggleVisibility,
                  icon: Icon(_icon),
                )
              : null,
          border: const OutlineInputBorder(
            borderRadius: BorderRadius.all(
              Radius.circular(kMedium),
            ),
          ),
          helper: widget.helperText == null ? null : Text(widget.helperText!),
        ),
        obscureText: _obscureText,
        controller: widget.controller,
      ),
    );
  }
}
