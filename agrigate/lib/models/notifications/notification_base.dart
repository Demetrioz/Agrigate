class NotificationBase {
  final int id;
  final String title;
  final String text;
  final bool hasBeenViewed;
  final DateTime timestamp;

  const NotificationBase({
    required this.id,
    required this.title,
    required this.text,
    required this.hasBeenViewed,
    required this.timestamp,
  });

  factory NotificationBase.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'id': int id,
        'title': String title,
        'text': String text,
        'hasBeenViewed': bool hasBeenViewed,
        'timestamp': String timestamp
      } =>
        NotificationBase(
          id: id,
          title: title,
          text: text,
          hasBeenViewed: hasBeenViewed,
          timestamp: DateTime.parse(timestamp),
        ),
      _ => throw const FormatException('Failed to load NotificationBase'),
    };
  }

  static List<NotificationBase> fromJsonList(List<dynamic> list) => list
      .map((item) => NotificationBase.fromJson(item as Map<String, dynamic>))
      .toList();
}
