namespace Agrigate.Api.Core.ValueTypes;

/// <summary>
/// Value type for enforcing non-null & non-whitespace strings
/// </summary>
public record struct NonEmptyString
{
    public string Value { get; }

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="propertyName">The name of the property being set</param>
    /// <param name="value">The value being set</param>
    /// <exception cref="ArgumentException"></exception>
    public NonEmptyString(string propertyName, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{propertyName} cannot be empty");
        
        Value = value;
    }
}