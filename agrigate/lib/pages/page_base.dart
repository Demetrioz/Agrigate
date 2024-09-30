import 'package:flutter/material.dart';

class PageBase extends StatefulWidget {
  const PageBase({super.key, required this.content});

  final Widget content;

  @override
  State<PageBase> createState() => _PageBaseState();
}

class _PageBaseState extends State<PageBase> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: const Text('Agrigate'),
      ),
      body: LayoutBuilder(builder: (lbContext, viewportConstraints) {
        return SingleChildScrollView(
          child: ConstrainedBox(
            constraints:
                BoxConstraints(minHeight: viewportConstraints.maxHeight),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: widget.content,
            ),
          ),
        );
      }),
    );
  }
}
