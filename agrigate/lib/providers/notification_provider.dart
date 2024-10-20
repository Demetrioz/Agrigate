import 'package:flutter/material.dart';

class NotificationProvider with ChangeNotifier {
  bool _hasUnreadNotifications = false;

  bool get hasUnreadNotifications => _hasUnreadNotifications;

  void setHasUnreadNotifications(bool value) {
    _hasUnreadNotifications = value;
    notifyListeners();
  }
}
