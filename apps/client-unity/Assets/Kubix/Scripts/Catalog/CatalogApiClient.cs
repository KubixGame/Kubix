using System.Net.Http;
using System.Threading.Tasks;

namespace Kubix.Catalog;

public sealed class CatalogApiClient
{
    private readonly HttpClient _httpClient;

    public CatalogApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<string> GetGamesAsync(string apiBaseUrl)
    {
        return _httpClient.GetStringAsync($"{apiBaseUrl.TrimEnd('/')}/games");
    }

    public Task<string> GetGameDetailsAsync(string apiBaseUrl, string gameId)
    {
        return _httpClient.GetStringAsync($"{apiBaseUrl.TrimEnd('/')}/games/{gameId}");
    }

    public Task<string> GetProfileAsync(string apiBaseUrl, string profileId)
    {
        return _httpClient.GetStringAsync($"{apiBaseUrl.TrimEnd('/')}/profiles/{profileId}");
    }
}
