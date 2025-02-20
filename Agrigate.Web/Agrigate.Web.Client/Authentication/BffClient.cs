namespace Agrigate.Web.Client.Authentication;

public class BffClient
{
    private readonly HttpClient _httpClient;
    
    public BffClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5003");
    }

    public async Task<HttpResponseMessage> GetUser()
    {
        return await _httpClient.GetAsync("bff/user?slide=false");
    }
}