import 'package:flutter/material.dart';

class AgrigateRouteObserver extends NavigatorObserver {
  String? lastRoute;

  @override
  void didReplace({Route? newRoute, Route? oldRoute}) {
    lastRoute = newRoute?.settings.name;
  }

  @override
  void didPush(Route route, Route? previousRoute) {
    lastRoute = route.settings.name;
  }

  @override
  void didPop(Route<dynamic> route, Route<dynamic>? previousRoute) {
    lastRoute = previousRoute?.settings.name;
  }

  @override
  void didRemove(Route<dynamic> route, Route<dynamic>? previousRoute) {
    lastRoute = previousRoute?.settings.name;
  }
}
