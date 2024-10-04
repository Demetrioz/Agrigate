import 'package:agrigate/agrigate.dart';
import 'package:agrigate/services/api_service/api_service.dart';
import 'package:flutter/material.dart';

// Allow apiService to be accessed from other areas of the app
final apiService = ApiService();

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();

  await apiService.initialize();

  runApp(const Agrigate());
}
