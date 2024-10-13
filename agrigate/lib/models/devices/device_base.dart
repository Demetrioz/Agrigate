class DeviceBase {
  final int id;
  final String name;
  final String location;
  final bool isActive;

  const DeviceBase({
    required this.id,
    required this.name,
    required this.location,
    required this.isActive,
  });

  factory DeviceBase.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'id': int id,
        'name': String name,
        'location': String location,
        'isActive': bool isActive,
      } =>
        DeviceBase(
          id: id,
          name: name,
          location: location,
          isActive: isActive,
        ),
      _ => throw const FormatException('Failed to load DeviceBase'),
    };
  }

  static List<DeviceBase> fromJsonList(List<dynamic> list) => list
      .map((item) => DeviceBase.fromJson(item as Map<String, dynamic>))
      .toList();
}
