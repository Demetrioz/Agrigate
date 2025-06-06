namespace Agrigate.Api.Core.Messages;

/// <summary>
/// A generic type for signifying error events
/// </summary>
public interface IErrorEvent
{
    /// <summary>
    /// A message describing the error
    /// </summary>
    public string Message { get; }
}

/// <summary>
/// Events that occur throughout Agrigate
/// </summary>
public static class CoreEvents
{
    
    /// <summary>
    /// Event signifying that a validation error occured
    /// </summary>
    /// <param name="Message"></param>
    public sealed record ValidationError(string Message) : IErrorEvent;
    
    /// <summary>
    /// Event signifying that an unexpected error occured
    /// </summary>
    /// <param name="Message"></param>
    public sealed record UnexpectedError(string Message) : IErrorEvent;
}