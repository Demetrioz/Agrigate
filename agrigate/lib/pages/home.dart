import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Home extends StatelessWidget {
  const Home({super.key});

  static const String title = 'Home';
  static const String route = '/';

  @override
  Widget build(BuildContext context) {
    return const PageBase(
      title: 'Agrigate',
      content: Center(
        child: Text('Welcome to Agrigate!'),
      ),
    );
  }
}
