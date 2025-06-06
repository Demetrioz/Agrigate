namespace Agrigate.Api.Core.ValueTypes;

/// <summary>
/// Value type for enforcing a positive integer
/// </summary>
public record struct PositiveInt
{
    public int Value { get; }

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="propertyName">The name of the property being set</param>
    /// <param name="value">The value being set</param>
    /// <exception cref="ArgumentException"></exception>
    public PositiveInt(string propertyName, int value)
    {
        if (value <= 0)
            throw new ArgumentException($"{propertyName} must be greater than 0");
        
        Value = value;
    }
}