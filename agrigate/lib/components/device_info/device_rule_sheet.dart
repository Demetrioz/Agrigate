import 'package:agrigate/components/common/agrigate_checkbox.dart';
import 'package:agrigate/components/common/agrigate_dropdown.dart';
import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/components/device_info/device_rule_action.dart';
import 'package:agrigate/components/device_info/device_rule_condition.dart';
import 'package:agrigate/constants.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/models/rules/base_definition.dart';
import 'package:flutter/material.dart';

class DeviceRuleSheet extends StatefulWidget {
  const DeviceRuleSheet({super.key, this.id});

  final int? id;

  @override
  State<DeviceRuleSheet> createState() => _DeviceRuleSheetState();
}

class _DeviceRuleSheetState extends State<DeviceRuleSheet> {
  bool _isLoading = false;
  List<BaseDefinition> _conditionDefinitions = [];
  List<BaseDefinition> _actionDefinitions = [];
  final _nameController = TextEditingController();
  final _timespanController = TextEditingController();
  final _conditionController = TextEditingController();
  final _channelController = TextEditingController();
  final _messageController = TextEditingController();
  final _operatorController = TextEditingController();

  void _saveRule() async {
    try {
      setState(() {
        _isLoading = true;
      });
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error saving rule',
            message: e.toString(),
          ),
        );
      }
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  void _loadData() async {
    try {
      setState(() {
        _isLoading = true;
      });

      if (widget.id != null) {
        // TODO: Load rule widget.id
      }

      final conditionDefinitions =
          apiService.getRuleDefinitions(DefinitionType.condition);
      final actionDefinitions =
          apiService.getRuleDefinitions(DefinitionType.action);

      final results = await Future.wait([
        conditionDefinitions,
        actionDefinitions,
      ]);

      setState(() {
        _conditionDefinitions = results[0];
        _actionDefinitions = results[1];
      });
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error loading rule',
            message: e.toString(),
          ),
        );
      }
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  @override
  void initState() {
    super.initState();

    _loadData();
  }

  @override
  void dispose() {
    _nameController.dispose();
    _timespanController.dispose();
    _conditionController.dispose();
    _channelController.dispose();
    _messageController.dispose();
    _operatorController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(kMedium),
      child: SingleChildScrollView(
        child: Column(
          children: [
            Text(
              widget.id == null ? 'Add Rule' : 'Update Rule',
              style: const TextStyle(
                fontSize: kLarge,
              ),
            ),
            AgrigateTextfield(
              label: 'Name',
              controller: _nameController,
            ),
            AgrigateTextfield(
              label: 'Timespan',
              controller: _timespanController,
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                AgrigateDropdown(
                  label: 'Operator',
                  controller: _operatorController,
                  options: const [
                    DropdownMenuEntry(value: 0, label: 'And'),
                    DropdownMenuEntry(value: 1, label: 'Or')
                  ],
                  initialSelection: 0,
                ),
                AgrigateCheckbox(
                  label: 'Is Active',
                  value: true,
                  handleChange: (value) {},
                ),
              ],
            ),
            const Divider(),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Text(
                  'Conditions',
                  style: TextStyle(
                    fontSize: kMedium,
                  ),
                ),
                IconButton(
                  onPressed: () {},
                  icon: const Icon(Icons.add),
                )
              ],
            ),
            // TODO: Map from condition count
            DeviceRuleCondition(
              definitions: _conditionDefinitions,
            ),
            const Divider(),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Text(
                  'Actions',
                  style: TextStyle(
                    fontSize: kMedium,
                  ),
                ),
                IconButton(
                  onPressed: () {},
                  icon: const Icon(Icons.add),
                )
              ],
            ),
            // TODO: Map from action count
            DeviceRuleAction(definitions: _actionDefinitions),
            AgrigateTextfield(
              label: 'Channel',
              controller: _channelController,
            ),
            AgrigateTextfield(
              label: 'Message',
              controller: _messageController,
            ),
            const Divider(),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: [
                OutlinedButton(
                  onPressed: () => Navigator.of(context).pop(),
                  child: const Text('Cancel'),
                ),
                OutlinedButton(
                  onPressed: _isLoading ? null : _saveRule,
                  child: const Text('Save'),
                )
              ],
            )
          ],
        ),
      ),
    );
  }
}
