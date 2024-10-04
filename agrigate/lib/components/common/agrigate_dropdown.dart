import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class AgrigateDropdown<T> extends StatelessWidget {
  const AgrigateDropdown({
    super.key,
    required this.label,
    required this.options,
    this.initialSelection,
  });

  final String label;
  final List<DropdownMenuEntry> options;
  final T? initialSelection;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(ksmall),
      child: DropdownMenu(
        inputDecorationTheme: const InputDecorationTheme(
          border: OutlineInputBorder(
            borderRadius: BorderRadius.all(
              Radius.circular(kMedium),
            ),
          ),
        ),
        menuStyle: const MenuStyle(
          shape: WidgetStatePropertyAll<RoundedRectangleBorder>(
            RoundedRectangleBorder(
              borderRadius: BorderRadius.all(
                Radius.circular(kMedium),
              ),
            ),
          ),
        ),
        label: Text(label),
        initialSelection: initialSelection,
        dropdownMenuEntries: options,
      ),
    );
  }
}
