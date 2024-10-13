using Agrigate.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Agrigate.Api.Services;

/// <inheritdoc />
public class AuthenticationService : IAuthenticationService
{
    public readonly string _apiKey;

    public AuthenticationService(IOptions<ApiOptions> options)
    {
        _apiKey = options?.Value?.ApiKey 
            ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public bool IsValidKey(string apiKey) 
    {
        return !string.IsNullOrWhiteSpace(apiKey) && apiKey == _apiKey;
    }
}