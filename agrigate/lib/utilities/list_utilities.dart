Map<TKey, List<TType>> groupBy<TKey, TType>(
    Iterable<TType> items, TKey Function(TType) selector) {
  var map = <TKey, List<TType>>{};

  for (var item in items) {
    final key = selector(item);

    if (!map.containsKey(key)) {
      map[key] = [];
    }

    map[key]!.add(item);
  }

  return map;
}
