import 'package:agrigate/components/common/agrigate_dropdown.dart';
import 'package:agrigate/models/rules/base_definition.dart';
import 'package:flutter/material.dart';

class DeviceRuleAction extends StatefulWidget {
  const DeviceRuleAction({
    super.key,
    required this.definitions,
  });

  final List<BaseDefinition> definitions;

  @override
  State<DeviceRuleAction> createState() => _DeviceRuleActionState();
}

class _DeviceRuleActionState extends State<DeviceRuleAction> {
  final TextEditingController _typeController = TextEditingController();
  final TextEditingController _channelController = TextEditingController();

  void _handleTypeChange(selection) {
    print('Selected $selection');
  }

  @override
  void dispose() {
    _typeController.dispose();
    _channelController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return widget.definitions.isEmpty
        ? const CircularProgressIndicator()
        : Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              AgrigateDropdown(
                label: 'Type',
                options: widget.definitions
                    .map((item) => DropdownMenuEntry(
                        value: item.type,
                        label: kRuleActionLabels[item.type] ?? 'Unknown'))
                    .toList(),
                controller: _typeController,
                initialSelection: 0,
                onSelected: _handleTypeChange,
              ),
              AgrigateDropdown(
                label: 'Channel',
                options: const [DropdownMenuEntry(value: 0, label: 'MQTT')],
                controller: _channelController,
                initialSelection: 0,
              ),
            ],
          );
  }
}
