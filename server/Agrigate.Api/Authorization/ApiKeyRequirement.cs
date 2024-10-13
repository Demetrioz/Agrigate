using Microsoft.AspNetCore.Authorization;

namespace Agrigate.Api.Authorization;

/// <summary>
/// A requirement for a request to contain a header with an API key
/// </summary>
public class ApiKeyRequirement : IAuthorizationRequirement
{
}