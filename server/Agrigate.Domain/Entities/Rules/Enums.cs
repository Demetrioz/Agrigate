namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// An operator that defines whether all conditions of a rule should be Anded
/// or Ored together
/// </summary>
public enum Operator
{
    And,
    Or
}

/// <summary>
/// A condition that can be applied to a rule
/// </summary>
public enum RuleCondition
{
    UpperLimit,
    LowerLimit,
    Range
}

/// <summary>
/// An action that can be taken once a rule's conditions have been met
/// </summary>
public enum RuleAction
{
    Notification
}

/// <summary>
/// Channels over which notifications can be sent
/// </summary>
public enum NotificationChannel
{
    MQTT,
    Email,
    SMS
}