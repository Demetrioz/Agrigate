class ApiResponse<T> {
  final int status;
  final T? data;
  final String? error;

  const ApiResponse({
    required this.status,
    this.data,
    this.error,
  });
}
