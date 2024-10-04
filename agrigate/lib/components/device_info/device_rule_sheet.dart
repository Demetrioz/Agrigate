import 'package:agrigate/components/common/agrigate_checkbox.dart';
import 'package:agrigate/components/common/agrigate_dropdown.dart';
import 'package:agrigate/components/common/agrigate_textfield.dart';
import 'package:agrigate/constants.dart';
import 'package:flutter/material.dart';

class DeviceRuleSheet extends StatefulWidget {
  const DeviceRuleSheet({super.key, this.id});

  final int? id;

  @override
  State<DeviceRuleSheet> createState() => _DeviceRuleSheetState();
}

class _DeviceRuleSheetState extends State<DeviceRuleSheet> {
  late TextEditingController _nameController;
  late TextEditingController _timespanController;
  late TextEditingController _conditionController;
  late TextEditingController _channelController;
  late TextEditingController _messageController;

  @override
  void initState() {
    super.initState();

    _nameController = TextEditingController();
    _timespanController = TextEditingController();
    _conditionController = TextEditingController();
    _channelController = TextEditingController();
    _messageController = TextEditingController();
  }

  @override
  void dispose() {
    _nameController.dispose();
    _timespanController.dispose();
    _conditionController.dispose();
    _channelController.dispose();
    _messageController.dispose();

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
                const AgrigateDropdown(
                  label: 'Operator',
                  options: [
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
            Row(
              children: [
                const AgrigateDropdown(
                  label: 'Type',
                  options: [
                    DropdownMenuEntry(value: 0, label: 'Upper Limit'),
                    DropdownMenuEntry(value: 1, label: 'Lower Limit'),
                    DropdownMenuEntry(value: 2, label: 'Range')
                  ],
                  initialSelection: 0,
                ),
                Expanded(
                  child: AgrigateTextfield(
                    label: 'Value',
                    controller: _conditionController,
                  ),
                ),
              ],
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
            const Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                AgrigateDropdown(
                  label: 'Type',
                  options: [
                    DropdownMenuEntry(value: 0, label: 'Notification'),
                  ],
                  initialSelection: 0,
                ),
                AgrigateDropdown(
                  label: 'Channel',
                  options: [DropdownMenuEntry(value: 0, label: 'MQTT')],
                  initialSelection: 0,
                ),
              ],
            ),
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
                  onPressed: () {},
                  child: const Text('Cancel'),
                ),
                OutlinedButton(
                  onPressed: () {},
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
