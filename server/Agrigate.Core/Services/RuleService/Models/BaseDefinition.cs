using System.Text.Json;

namespace Agrigate.Core.Services.RuleService.Models;

/// <summary>
/// A generic definition for a rule condition or action
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseDefinition<T>
{
    public required T Type { get; set; }
    public required JsonDocument Data { get; set; }
}