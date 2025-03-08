namespace Agrigate.Core.Messages.Operations.Locations;

/// <summary>
/// Value type for enforcing Location Names
/// </summary>
public record struct LocationName
{
    public LocationName(string value)
    {
        Value = value;
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("LocationName cannot be empty", nameof(value));
    }
    
    public string Value { get; }
}

public record struct LocationId
{
    public LocationId(long value)
    {
        Value = value;
        if (value < 1)
            throw new ArgumentException("LocationId must be greater than zero", nameof(value));
    }

    public long Value { get; }
}

/// <summary>
/// Value type for enforcing ordered parent ids
/// </summary>
public record struct OrderedParentIds
{
    public OrderedParentIds(Queue<long> value)
    {
        Value = value;
        if (value.Count == 0)
            throw new ArgumentException("OrderedParentIds cannot be empty", nameof(value));
        
        if (value.Any(v => v < 1))
            throw new ArgumentException("ParentIds must be greater than zero", nameof(value));
    }
    
    public Queue<long> Value { get; }
    public long Dequeue() => Value.Dequeue();
}