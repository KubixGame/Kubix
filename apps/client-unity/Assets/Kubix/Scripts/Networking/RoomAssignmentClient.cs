using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Networking;

public sealed class RoomAssignmentClient
{
    private readonly HttpClient _httpClient;

    public RoomAssignmentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> CreateRoomAssignmentAsync(string apiBaseUrl, string gameId, string versionId)
    {
        var payload = $"{{\"gameId\":\"{gameId}\",\"versionId\":\"{versionId}\"}}";
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        return _httpClient.PostAsync($"{apiBaseUrl.TrimEnd('/')}/rooms/assign", content);
    }
}
