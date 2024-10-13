using Agrigate.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Agrigate.Api.Authorization;

/// <summary>
/// A handler for API requests that have an ApiKeyRequirement
/// </summary>
public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthenticationService _authenticationService;

    public ApiKeyHandler(
        IHttpContextAccessor httpContextAccessor,
        IAuthenticationService authenticationService
    )
    {
        _httpContextAccessor = httpContextAccessor
            ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _authenticationService = authenticationService
            ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    /// <summary>
    /// Checks for a header containing the API Key and validates it
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ApiKeyRequirement requirement
    )
    {
        var apiKey = _httpContextAccessor.HttpContext
            ?.Request
            ?.Headers[Constants.Authentication.ApiKeyHeader]
            .ToString();

        if (
            string.IsNullOrWhiteSpace(apiKey)
            || !_authenticationService.IsValidKey(apiKey)
        )
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}