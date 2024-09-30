import 'package:agrigate/pages/page_base.dart';
import 'package:flutter/material.dart';

class Agrigate extends StatelessWidget {
  const Agrigate({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Agrigate',
      theme: ThemeData(
          colorScheme: ColorScheme.fromSeed(seedColor: Colors.green),
          useMaterial3: true),
      home: PageBase(
        content: Container(),
      ),
    );
  }
}
