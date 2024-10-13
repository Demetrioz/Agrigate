import 'package:agrigate/components/device_info/device_rule_sheet.dart';
import 'package:flutter/material.dart';

class RuleCard extends StatelessWidget {
  const RuleCard({
    super.key,
    required this.id,
    required this.name,
    required this.summary,
    required this.isActive,
  });

  final int id;
  final String name;
  final String summary;
  final bool isActive;

  void _showRuleCard(BuildContext context) {
    showModalBottomSheet(
      context: context,
      builder: (context) => DeviceRuleSheet(id: id),
    );
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
        // onTap: () => _showRuleCard(context),
      ),
    );
  }
}
