import 'package:flutter/material.dart';

class RuleCard extends StatelessWidget {
  const RuleCard({
    super.key,
    required this.name,
    required this.summary,
    required this.isActive,
  });

  final String name;
  final String summary;
  final bool isActive;

  void _showRuleCard() {
    // TODO: Show bottom sheet
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        title: Text(name),
        subtitle: Text(summary),
        trailing: Icon(
          Icons.circle,
          color: isActive ? Colors.green : Colors.red,
        ),
        onTap: _showRuleCard,
      ),
    );
  }
}
