import 'dart:io';

import 'package:agrigate/main.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';

@pragma('vm:entry-point')
void notificationTapBackground(NotificationResponse response) {
  notificationStream.add(response.payload);
}

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

    await _flutterLocalNotificationsPlugin.initialize(
      initializationSettings,
      onDidReceiveNotificationResponse: (NotificationResponse response) async {
        notificationStream.add(response.payload);
      },
      onDidReceiveBackgroundNotificationResponse: notificationTapBackground,
    );
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
      actions: [
        AndroidNotificationAction(
          'AgrigateAlert',
          'View Alert',
          showsUserInterface: true,
        ),
      ],
    );

    const LinuxNotificationDetails linuxPlatformChannelSpecifics =
        LinuxNotificationDetails(
      actions: [
        LinuxNotificationAction(
          key: 'AgrigateAlert',
          label: 'View Alert',
        ),
      ],
    );

    const NotificationDetails platformChannelSpecifics = NotificationDetails(
      android: androidPlatformChannelSpecifics,
      linux: linuxPlatformChannelSpecifics,
    );

    await _flutterLocalNotificationsPlugin.show(
        id, title, body, platformChannelSpecifics);
  }

  void onDidReceiveNotificationResponse(NotificationResponse response) async {
    notificationStream.add(response.payload);
  }
}
