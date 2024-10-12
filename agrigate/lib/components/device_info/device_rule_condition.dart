import 'package:agrigate/components/common/agrigate_dropdown.dart';
import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/models/rules/base_definition.dart';
import 'package:flutter/material.dart';

class DeviceRuleCondition extends StatefulWidget {
  const DeviceRuleCondition({
    super.key,
    required this.definitions,
  });

  final List<BaseDefinition> definitions;

  @override
  State<DeviceRuleCondition> createState() => _DeviceRuleConditionState();
}

class _DeviceRuleConditionState extends State<DeviceRuleCondition> {
  final TextEditingController _typeController = TextEditingController();

  final TextEditingController _conditionController = TextEditingController();

  void _handleTypeChange(selection) {
    print('Selected $selection');
  }

  @override
  void dispose() {
    _typeController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return widget.definitions.isEmpty
        ? const CircularProgressIndicator()
        : Row(
            children: [
              AgrigateDropdown(
                label: 'Type',
                options: widget.definitions
                    .map(
                      (item) => DropdownMenuEntry(
                        value: item.type,
                        label: kRuleConditionLabels[item.type] ?? 'Unknown',
                      ),
                    )
                    .toList(),
                controller: _typeController,
                initialSelection: 0,
                onSelected: _handleTypeChange,
              ),
              Expanded(
                child: AgrigateTextfield(
                  label: 'Value',
                  controller: _conditionController,
                ),
              ),
            ],
          );
  }
}
