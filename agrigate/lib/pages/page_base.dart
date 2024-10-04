import 'package:agrigate/components/agrigate_drawer.dart';
import 'package:flutter/material.dart';

class PageBase extends StatefulWidget {
  const PageBase(
      {super.key,
      required this.content,
      required this.title,
      this.floatingAction,
      this.floatingActionIcon});

  final Widget content;
  final String title;
  final VoidCallback? floatingAction;
  final IconData? floatingActionIcon;

  @override
  State<PageBase> createState() => _PageBaseState();
}

class _PageBaseState extends State<PageBase> {
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
            constraints:
                BoxConstraints(minHeight: viewportConstraints.maxHeight),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: widget.content,
            ),
          ),
        );
      }),
      floatingActionButton: widget.floatingAction != null
          ? FloatingActionButton(
              onPressed: widget.floatingAction,
              child: Icon(widget.floatingActionIcon ?? Icons.add),
            )
          : null,
    );
  }
}
