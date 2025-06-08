namespace Agrigate.Web.Services.AgrigateApiClient;

/// <summary>
/// Client for interacting with the Agrigate API
/// </summary>
public interface IAgrigateApiClient
{
    /// <summary>
    /// Makes a request to the Agrigate API
    /// </summary>
    /// <param name="method">The method to execute (GET / POST / PATCH / ETC)</param>
    /// <param name="path">The URI to make a request to</param>
    /// <param name="body">The body of the request</param>
    /// <param name="queryParams">Query parameters for the request</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> Request<T>(
        HttpMethod method, 
        string path, 
        object? body = null, 
        Dictionary<string, string>? queryParams = null
    );
}