namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// The definition of an upper limit condition
/// </summary>
public class UpperLimitDefinition
{
    public double Value { get; set; }
}

/// <summary>
/// The definition of a lower limit condition
/// </summary>
public class LowerLimitDefinition
{
    public double Value { get; set; }
}

/// <summary>
/// The definition of a range condition
/// </summary>
public class RangeDefinition
{
    public double UpperLimit { get; set; }
    public double LowerLimit { get; set; }
}