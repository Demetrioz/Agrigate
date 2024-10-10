class RuleBase {
  final int id;
  final String name;
  final String summary;
  final bool isActive;

  const RuleBase(
      {required this.id,
      required this.name,
      required this.summary,
      required this.isActive});

  factory RuleBase.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'id': int id,
        'name': String name,
        'summary': String summary,
        'isActive': bool isActive,
      } =>
        RuleBase(
          id: id,
          name: name,
          summary: summary,
          isActive: isActive,
        ),
      _ => throw const FormatException('Failed to load RuleBase'),
    };
  }

  static List<RuleBase> fromJsonList(List<dynamic> list) => list
      .map((item) => RuleBase.fromJson(item as Map<String, dynamic>))
      .toList();
}
