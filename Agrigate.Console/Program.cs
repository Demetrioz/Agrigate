// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Duende.IdentityModel.Client;

using var client = new HttpClient();
var discovery = await client.GetDiscoveryDocumentAsync("http://localhost:5001");

if (discovery.IsError)
{
    Console.WriteLine(discovery.Error);
    return;
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discovery.TokenEndpoint,
    ClientId = "Agrigate.Console",
    ClientSecret = "secret",
    Scope = "Agrigate.Api"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.AccessToken);

using var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken!);

var apiResponse = await apiClient.GetAsync("http://localhost:5002/WeatherForecast/Test");
if (!apiResponse.IsSuccessStatusCode)
{
    Console.WriteLine(apiResponse.StatusCode);
    return;
}

var doc = JsonDocument.Parse(await apiResponse.Content.ReadAsStringAsync()).RootElement;
Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));

apiResponse = await apiClient.GetAsync("http://localhost:5002/WeatherForecast/Test2");
if (!apiResponse.IsSuccessStatusCode)
{
    Console.WriteLine(apiResponse.StatusCode);
    return;
}

doc = JsonDocument.Parse(await apiResponse.Content.ReadAsStringAsync()).RootElement;
Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));