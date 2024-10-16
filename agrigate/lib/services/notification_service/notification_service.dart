import 'dart:io';

import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class NotificationService {
  static final NotificationService _instance = NotificationService._internal();

  factory NotificationService() => _instance;
  NotificationService._internal();

  static final FlutterLocalNotificationsPlugin
      _flutterLocalNotificationsPlugin = FlutterLocalNotificationsPlugin();

  static const String channelId = 'agrigate_notifications';
  static const channelName = 'AGRIGATE_NOTIFICATIONS';
  static const channelDescription =
      'Channel for MQTT notifications from Agrigate';

  static Future<void> initialize() async {
    const InitializationSettings initializationSettings =
        InitializationSettings(
      iOS: DarwinInitializationSettings(),
      android: AndroidInitializationSettings('@drawable/ic_notification'),
      linux: LinuxInitializationSettings(
          defaultActionName: 'onDidReceiveNotificationResponse'),
    );

    await _flutterLocalNotificationsPlugin.initialize(initializationSettings);
  }

  static void requestNotificationPermissions() {
    if (Platform.isAndroid) {
      _flutterLocalNotificationsPlugin
          .resolvePlatformSpecificImplementation<
              AndroidFlutterLocalNotificationsPlugin>()
          ?.requestNotificationsPermission();
    }
  }

  static Future<void> showNotification(
      int id, String title, String body) async {
    const AndroidNotificationDetails androidPlatformChannelSpecifics =
        AndroidNotificationDetails(
      channelId,
      channelName,
      channelDescription: channelDescription,
      importance: Importance.max,
      priority: Priority.high,
    );

    const NotificationDetails platformChannelSpecifics =
        NotificationDetails(android: androidPlatformChannelSpecifics);

    await _flutterLocalNotificationsPlugin.show(
        id, title, body, platformChannelSpecifics);
  }

  void onDidReceiveNotificationResponse(
    NotificationResponse notifcationResponse,
  ) async {
    // await Navigator.pushNamed(context, Notifications.route);
  }
}
