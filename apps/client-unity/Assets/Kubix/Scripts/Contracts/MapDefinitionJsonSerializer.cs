using System.Text.Json;
using System.Text.Json.Serialization;
using Kubix.Contracts.Maps;

namespace Kubix.Contracts;

public sealed class MapDefinitionJsonSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public string Serialize(MapDefinition mapDefinition)
    {
        return JsonSerializer.Serialize(mapDefinition, _options);
    }

    public MapDefinition? Deserialize(string rawJson)
    {
        return JsonSerializer.Deserialize<MapDefinition>(rawJson, _options);
    }
}
