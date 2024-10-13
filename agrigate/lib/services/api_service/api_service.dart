import 'package:agrigate/constants.dart';
import 'package:agrigate/models/devices/device_base.dart';
import 'package:agrigate/models/devices/device_details.dart';
import 'package:agrigate/models/devices/telemetry_base.dart';
import 'package:agrigate/models/rules/base_definition.dart';
import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class ApiService {
  static final ApiService _instance = ApiService._internal();

  factory ApiService() => _instance;
  ApiService._internal();

  Dio? _dio;
  FlutterSecureStorage? _storage;

  Future<void> initialize({
    String? updatedUrl,
    String? updatedApiKey,
  }) async {
    _storage ??= const FlutterSecureStorage();
    _dio ??= Dio();

    final baseUrl = updatedUrl?.isEmpty ?? true
        ? await _storage!.read(key: kServerUrl) ?? ''
        : updatedUrl ?? '';
    final apiKey = updatedApiKey?.isEmpty ?? true
        ? await _storage!.read(key: kApiKey) ?? ''
        : updatedApiKey ?? '';

    _dio!.options.baseUrl = baseUrl;
    _dio!.options.headers[kApiHeaderName] = apiKey;
  }

  Future<List<DeviceBase>> getDevices() async {
    final response = await _dio!.get('/Devices');

    // TODO: Move this logic to an interceptor
    if (response.data['status'] == 1) {
      throw Exception(response.data['error']);
    }

    final result = DeviceBase.fromJsonList(response.data['data']);
    return result;
  }

  Future<DeviceBase> createDevice(String name, String location) async {
    final response = await _dio!.post(
      '/Devices',
      data: {
        'Name': name,
        'Location': location,
      },
    );

    if (response.data['status'] == 1) {
      throw Exception((response.data['error']));
    }

    final fullDevice = response.data['data'];
    final result = DeviceBase(
      id: fullDevice['id'],
      name: fullDevice['name'],
      location: fullDevice['location'],
      isActive: false,
    );
    return result;
  }

  Future<DeviceDetails> getDeviceDetails(int id) async {
    final response = await _dio!.get('/Devices/$id');

    if (response.data['status'] == 1) {
      throw Exception((response.data['error']));
    }

    final result = DeviceDetails.fromJson(response.data['data']);
    return result;
  }

  Future<List<TelemetryBase>> getDeviceTelemetry(int id) async {
    final response = await _dio!.get('/Devices/$id/Telemetry');

    if (response.data['status'] == 1) {
      throw Exception((response.data['error']));
    }

    final result = TelemetryBase.fromJsonList(response.data['data']);
    return result;
  }

  Future<List<BaseDefinition>> getRuleDefinitions(DefinitionType type) async {
    final response = await _dio!.get(
        '/Rules/${type == DefinitionType.condition ? 'Conditions' : 'Actions'}');

    if (response.data['status'] == 1) {
      throw Exception((response.data['error']));
    }

    final result = BaseDefinition.fromJsonList(response.data['data']);
    return result;
  }
}
