using System.Net.Http;
using System.Threading.Tasks;

namespace Kubix.Runtime;

public sealed class PublishedMapLoader
{
    private readonly HttpClient _httpClient;

    public PublishedMapLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<string> LoadPublishedMapJsonAsync(string apiBaseUrl, string gameId)
    {
        return _httpClient.GetStringAsync($"{apiBaseUrl.TrimEnd('/')}/games/{gameId}");
    }
}
