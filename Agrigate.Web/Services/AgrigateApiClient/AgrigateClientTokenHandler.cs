using System.Net.Http.Headers;
using Agrigate.Web.Services.JwtService;

namespace Agrigate.Web.Services.AgrigateApiClient;

/// <summary>
/// A handler to apply a JWT to each request made by the AgrigateApiClient
/// </summary>
public class AgrigateClientTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="jwtService"></param>
    public AgrigateClientTokenHandler(
        IHttpContextAccessor httpContextAccessor,
        IJwtService jwtService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
    }
    
    /// <summary>
    /// Retrieves a JWT and attaches it to the request prior to sending
    /// </summary>
    /// <param name="request">The request to send</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        var isAuthenticated = _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated;

        var token = isAuthenticated is true && !string.IsNullOrWhiteSpace(currentUser)
            ? await _jwtService.GetOrCreateToken(currentUser)
            : "";
            
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return await base.SendAsync(request, cancellationToken);
    }
}