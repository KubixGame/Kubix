using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Auth;

public sealed class AuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> RegisterAsync(string apiBaseUrl, string username, string displayName, string password)
    {
        var payload = $"{{\"username\":\"{username}\",\"displayName\":\"{displayName}\",\"password\":\"{password}\"}}";
        return _httpClient.PostAsync($"{apiBaseUrl.TrimEnd('/')}/auth/register", new StringContent(payload, Encoding.UTF8, "application/json"));
    }

    public Task<HttpResponseMessage> LoginAsync(string apiBaseUrl, string username, string password)
    {
        var payload = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        return _httpClient.PostAsync($"{apiBaseUrl.TrimEnd('/')}/auth/login", new StringContent(payload, Encoding.UTF8, "application/json"));
    }
}
