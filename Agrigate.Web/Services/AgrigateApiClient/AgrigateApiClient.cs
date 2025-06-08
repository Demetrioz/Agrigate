using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Agrigate.Core.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Agrigate.Web.Services.AgrigateApiClient;

/// <inheritdoc />
public class AgrigateApiClient : IAgrigateApiClient
{
    private readonly HttpClient _client;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="client"></param>
    /// <param name="options"></param>
    public AgrigateApiClient(HttpClient client, IOptions<AgrigateConfiguration> options)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _client.BaseAddress = new Uri(options?.Value?.ApiUrl ?? throw new ArgumentNullException(nameof(options)));
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <inheritdoc />
    public async Task<T> Request<T>(
        HttpMethod method,
        string path,
        object? body = null,
        Dictionary<string, string>? queryParams = null
    )
    {
        if (queryParams != null)
            path = QueryHelpers.AddQueryString(path, queryParams!);

        HttpContent? content = null;
        if (body != null)
            content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );
        
        using var response = method.Method switch
        {
            "GET" => await _client.GetAsync(path),
            "POST" => await _client.PostAsync(path, content),
            "PUT" => await _client.PutAsync(path, content),
            "PATCH" => await _client.PatchAsync(path, content),
            "DELETE" => await _client.DeleteAsync(path),
            _ => throw new ApplicationException($"Method {method} not supported")
        };
        
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// Handles parsing and deserializing a response
    /// </summary>
    /// <param name="response">The response from an HTTP request</param>
    /// <typeparam name="T">The type that the payload should be deserialized into</typeparam>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    private async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Unable to make request! {response.StatusCode}: {response.ReasonPhrase}");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(responseBody);
        if (result is null)
            throw new ApplicationException($"Data is not of type {typeof(T)}");
        
        return result;
    }
}