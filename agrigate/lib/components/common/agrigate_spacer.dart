import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

enum SpacerType { vertical, horizontal }

class AgrigateSpacer extends StatelessWidget {
  const AgrigateSpacer({
    super.key,
    required this.size,
    required this.type,
  });

  final Size size;
  final SpacerType type;

  @override
  Widget build(BuildContext context) {
    double value = ksmall;

    if (size == Size.medium) {
      value = kMedium;
    } else if (size == Size.large) {
      value = kLarge;
    }

    return SizedBox(
      height: type == SpacerType.vertical ? value : 0,
      width: type == SpacerType.horizontal ? value : 0,
    );
  }
}
