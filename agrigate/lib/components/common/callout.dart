import 'package:agrigate/components/common/agrigate_spacer.dart';
import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

enum CalloutType {
  info,
  warning,
  danger,
}

class Callout extends StatelessWidget {
  const Callout({
    super.key,
    required this.type,
    required this.text,
  });

  final CalloutType type;
  final String text;

  @override
  Widget build(BuildContext context) {
    MaterialAccentColor color = type == CalloutType.danger
        ? Colors.redAccent
        : type == CalloutType.warning
            ? Colors.amberAccent
            : Colors.lightBlueAccent;

    IconData icon = type == CalloutType.danger
        ? Icons.error
        : type == CalloutType.warning
            ? Icons.warning
            : Icons.help;

    return Padding(
      padding: const EdgeInsets.all(kMedium),
      child: DecoratedBox(
        decoration: BoxDecoration(
          color: color,
          borderRadius: BorderRadius.circular(kMedium),
        ),
        child: Padding(
          padding: const EdgeInsets.all(kLarge),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(icon),
              const AgrigateSpacer(
                size: Size.medium,
                type: SpacerType.horizontal,
              ),
              Flexible(
                child: Text(text),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
