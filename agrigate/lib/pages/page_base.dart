import 'package:agrigate/components/agrigate_drawer.dart';
import 'package:agrigate/main.dart';
import 'package:agrigate/pages/notifications.dart';
import 'package:agrigate/providers/notification_provider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class PageBase extends StatefulWidget {
  const PageBase({
    super.key,
    required this.content,
    required this.title,
    this.floatingAction,
    this.floatingActionIcon,
    this.navigationBar,
  });

  final Widget content;
  final String title;
  final VoidCallback? floatingAction;
  final IconData? floatingActionIcon;
  final NavigationBar? navigationBar;

  @override
  State<PageBase> createState() => _PageBaseState();
}

class _PageBaseState extends State<PageBase> with RouteAware {
  void _handleNotificationEvent(String? payload) async {
    if (mounted) {
      switch (payload) {
        case 'ServiceEvent.notificationReceived':
          context.read<NotificationProvider>().setHasUnreadNotifications(true);
          break;
        default:
          _handleNotificationTap(payload);
      }
    }
  }

  // When tapping on a notification, we should navigate to the notifications
  // page, but only if we're the top-most widget and not already there
  void _handleNotificationTap(String? payload) async {
    if (mounted) {
      final widgetRoute = ModalRoute.of(context)!.settings.name;
      final topRoute = routeObserver.lastRoute;

      final canNavigate =
          widgetRoute == topRoute && widgetRoute != Notifications.route;

      if (canNavigate) {
        Navigator.pushNamed(context, Notifications.route);
      }
    }
  }

  @override
  void initState() {
    super.initState();

    notificationStream.stream.listen(_handleNotificationEvent);
  }

  @override
  Widget build(BuildContext context) {
    final route = ModalRoute.of(context)?.settings.name;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: Text(widget.title),
      ),
      drawer: route == '/' ? const AgrigateDrawer() : null,
      body: LayoutBuilder(builder: (lbContext, viewportConstraints) {
        return SingleChildScrollView(
          child: ConstrainedBox(
            constraints: BoxConstraints(
              minHeight: viewportConstraints.maxHeight,
            ),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: widget.content,
            ),
          ),
        );
      }),
      bottomNavigationBar: widget.navigationBar,
      floatingActionButton: widget.floatingAction != null
          ? FloatingActionButton(
              onPressed: widget.floatingAction,
              child: Icon(widget.floatingActionIcon ?? Icons.add),
            )
          : null,
    );
  }
}
