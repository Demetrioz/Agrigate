class TelemetryBase {
  final int id;
  final String key;
  final double value;
  final DateTime timestamp;

  const TelemetryBase({
    required this.id,
    required this.key,
    required this.value,
    required this.timestamp,
  });

  factory TelemetryBase.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'id': int id,
        'key': String key,
        'value': dynamic value,
        'timestamp': String timestamp,
      } =>
        TelemetryBase(
          id: id,
          key: key,
          value: value is int ? value.toDouble() : value,
          timestamp: DateTime.parse(timestamp),
        ),
      _ => throw const FormatException('Failed to load TelemetryBase'),
    };
  }

  static List<TelemetryBase> fromJsonList(List<dynamic> list) => list
      .map((item) => TelemetryBase.fromJson(item as Map<String, dynamic>))
      .toList();
}
