import 'package:agrigate/models/devices/rule_base.dart';
import 'package:agrigate/models/devices/telemetry_base.dart';

class DeviceDetails {
  final int id;
  final String name;
  final String location;
  final bool isActive;
  final List<RuleBase> rules;
  final List<TelemetryBase> distinctTelemetry;

  const DeviceDetails({
    required this.id,
    required this.name,
    required this.location,
    required this.isActive,
    required this.rules,
    required this.distinctTelemetry,
  });

  factory DeviceDetails.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'id': int id,
        'name': String name,
        'location': String location,
        'isActive': bool isActive,
        'rules': List<dynamic> rules,
        'distinctTelemetry': List<dynamic> telemetry,
      } =>
        DeviceDetails(
          id: id,
          name: name,
          location: location,
          isActive: isActive,
          rules: RuleBase.fromJsonList(rules),
          distinctTelemetry: TelemetryBase.fromJsonList(telemetry),
        ),
      _ => throw const FormatException('Failed to load DeviceDetails'),
    };
  }
}
