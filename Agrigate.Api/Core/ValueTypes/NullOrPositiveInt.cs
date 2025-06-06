namespace Agrigate.Api.Core.ValueTypes;

/// <summary>
/// Value type for enforcing a null or positive integer
/// </summary>
public record struct NullOrPositiveInt
{
    public int? Value { get; }

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="propertyName">The name of the property being set</param>
    /// <param name="value">The value being set</param>
    /// <exception cref="ArgumentException"></exception>
    public NullOrPositiveInt(string propertyName, int? value)
    {
        if (value is <= 0)
            throw new ArgumentException($"{propertyName} must be null or positive");
        
        Value = value;
    }
}