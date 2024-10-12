enum DefinitionType {
  condition,
  action,
}

const Map<int, String> kRuleConditionLabels = {
  0: 'Upper Limit',
  1: 'Lower Limit',
  2: 'Range',
};

const Map<int, String> kRuleActionLabels = {
  0: 'Notification',
};

class BaseDefinition {
  final int type;
  final List<Map<String, dynamic>> data;

  const BaseDefinition({
    required this.type,
    required this.data,
  });

  factory BaseDefinition.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'type': int type,
        'data': List<dynamic> data,
      } =>
        BaseDefinition(
          type: type,
          data: List<Map<String, dynamic>>.from(data),
        ),
      _ => throw const FormatException('Failed to load BaseDefinition')
    };
  }

  static List<BaseDefinition> fromJsonList(List<dynamic> list) => list
      .map((item) => BaseDefinition.fromJson(item as Map<String, dynamic>))
      .toList();
}
