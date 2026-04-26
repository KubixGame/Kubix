using System.IO;
using Kubix.Contracts;
using Kubix.Contracts.Maps;
using UnityEngine;

namespace Kubix.Editor;

public sealed class DraftMapRepository
{
    private const string DraftFolderName = "DraftMaps";
    private readonly MapDefinitionJsonSerializer _serializer = new();

    public string GetDraftFolder()
    {
        var folder = Path.Combine(Application.persistentDataPath, DraftFolderName);
        Directory.CreateDirectory(folder);
        return folder;
    }

    public string GetDraftPath(string mapId)
    {
        return Path.Combine(GetDraftFolder(), mapId + ".json");
    }

    public void SaveRawJson(string mapId, string rawJson)
    {
        File.WriteAllText(GetDraftPath(mapId), rawJson);
    }

    public string? LoadRawJson(string mapId)
    {
        var path = GetDraftPath(mapId);
        return File.Exists(path) ? File.ReadAllText(path) : null;
    }

    public void SaveMap(MapDefinition mapDefinition)
    {
        SaveRawJson(mapDefinition.MapId, _serializer.Serialize(mapDefinition));
    }

    public MapDefinition? LoadMap(string mapId)
    {
        var rawJson = LoadRawJson(mapId);
        return rawJson == null ? null : _serializer.Deserialize(rawJson);
    }
}
