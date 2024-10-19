import 'package:agrigate/components/common/error_dialog.dart';
import 'package:agrigate/components/notifications/notification_card.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/models/notifications/notification_base.dart';
import 'package:agrigate/pages/page_base.dart';
import 'package:agrigate/providers/notification_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class Notifications extends StatefulWidget {
  const Notifications({super.key});

  static const String title = 'Notifications';
  static const String route = '/notifications';

  @override
  State<Notifications> createState() => _NotificationsState();
}

class _NotificationsState extends State<Notifications> {
  bool _isLoading = false;
  List<NotificationBase> _notifications = [];

  void _loadNotifications() async {
    try {
      setState(() {
        _isLoading = true;
      });

      final result = await apiService.getNotifications();

      if (mounted) {
        context.read<NotificationProvider>().setHasUnreadNotifications(false);
      }

      setState(() {
        _notifications = result;
      });

      if (result.isEmpty) return;

      final viewedIds = result
          .where((notification) => !notification.hasBeenViewed)
          .map((notification) => notification.id)
          .toList();

      if (viewedIds.isEmpty) return;

      await apiService.markNotificationsViewed(viewedIds);
    } catch (e) {
      if (mounted) {
        showDialog(
          context: context,
          builder: (BuildContext context) => ErrorDialog(
            title: 'Error loading notifications',
            message: e.toString(),
          ),
        );
      }
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  @override
  void initState() {
    super.initState();

    _loadNotifications();
  }

  @override
  Widget build(BuildContext context) {
    return PageBase(
      title: 'Notifications',
      content: _isLoading
          ? const Center(
              child: CircularProgressIndicator(),
            )
          : _notifications.isEmpty
              ? const Center(
                  child: Text('No notifications found'),
                )
              : ListView.builder(
                  shrinkWrap: true,
                  itemCount: _notifications.length,
                  itemBuilder: (BuildContext ctx, int index) {
                    final item = _notifications.elementAt(index);

                    return NotificationCard(
                      text: item.text,
                      date: item.timestamp,
                      important: !item.hasBeenViewed,
                    );
                  },
                ),
    );
  }
}
