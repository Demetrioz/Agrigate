namespace Agrigate.Api.Core.ValueTypes;

/// <summary>
/// Value type for enforcing a null or positive double
/// </summary>
public record struct NullOrPositiveDouble
{
    public double? Value { get; }

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="propertyName">The name of the property being set</param>
    /// <param name="value">The value being set</param>
    /// <exception cref="ArgumentException"></exception>
    public NullOrPositiveDouble(string propertyName, double? value)
    {
        if (value is <= 0)
            throw new ArgumentException($"{propertyName} must be null or positive");
        
        Value = value;
    }
}