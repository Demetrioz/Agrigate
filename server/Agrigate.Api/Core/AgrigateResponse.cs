namespace Agrigate.Api.Core;

/// <summary>
/// Possible results of an Agrigate request
/// </summary>
public enum ResponseStatus
{
    Success,
    Failure
}

/// <summary>
/// A common response from any Agrigate endpoint
/// </summary>
public class AgrigateResponse
{
    /// <summary>
    /// The status of the request
    /// </summary>
    public ResponseStatus Status { get; set; }

    /// <summary>
    /// Data returned from the request
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// An error message if the request fails
    /// </summary>
    public string? Error { get; set; }
}