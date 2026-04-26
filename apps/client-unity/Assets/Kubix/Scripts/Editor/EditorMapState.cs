using System;
using System.Collections.Generic;
using Kubix.Contracts.Logic;
using Kubix.Contracts.Maps;

namespace Kubix.Editor;

public sealed class EditorMapState
{
    public string MapId { get; private set; } = "map_" + Guid.NewGuid().ToString("N")[..8];
    public string Title { get; private set; } = "New Kubix Map";
    public List<PlacedObject> Objects { get; } = new();
    public EasyLogicGraph Logic { get; private set; } = new(new List<EasyNode>(), new List<EasyEdge>());

    public MapDefinition ToMapDefinition()
    {
        return new MapDefinition(MapId, Title, new List<PlacedObject>(Objects), Logic);
    }

    public void SetTitle(string title)
    {
        Title = string.IsNullOrWhiteSpace(title) ? "New Kubix Map" : title.Trim();
    }

    public void Load(MapDefinition mapDefinition)
    {
        MapId = mapDefinition.MapId;
        Title = mapDefinition.Title;
        Objects.Clear();
        Objects.AddRange(mapDefinition.Objects);
        Logic = mapDefinition.Logic;
    }

    public void AddObject(PlacedObject placedObject)
    {
        Objects.Add(placedObject);
    }

    public void ReplaceObject(PlacedObject placedObject)
    {
        var index = Objects.FindIndex(candidate => candidate.ObjectId == placedObject.ObjectId);
        if (index >= 0)
        {
            Objects[index] = placedObject;
        }
    }

    public void RemoveObject(string objectId)
    {
        Objects.RemoveAll(candidate => candidate.ObjectId == objectId);
    }
}
