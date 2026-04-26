using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kubix.Editor;

public sealed class PlacementTool : MonoBehaviour
{
    [Serializable]
    public sealed class LibraryEntry
    {
        public string ObjectType = "floor_block";
        public GameObject? Prefab;
    }

    [SerializeField] private List<LibraryEntry> _library = new();
    [SerializeField] private Transform? _root;

    private readonly Dictionary<string, GameObject> _prefabsByType = new();
    private GridSnapService _gridSnapService = new();
    private string _currentObjectType = "floor_block";

    private void Awake()
    {
        foreach (var entry in _library)
        {
            if (entry.Prefab == null || string.IsNullOrWhiteSpace(entry.ObjectType))
            {
                continue;
            }

            _prefabsByType[entry.ObjectType] = entry.Prefab;
        }
    }

    public void Initialize(GridSnapService gridSnapService)
    {
        _gridSnapService = gridSnapService;
    }

    public void SelectObjectType(string objectType)
    {
        if (_prefabsByType.ContainsKey(objectType))
        {
            _currentObjectType = objectType;
        }
    }

    public GameObject? PlaceAtWorldPosition(Vector3 worldPosition)
    {
        if (!_prefabsByType.TryGetValue(_currentObjectType, out var prefab))
        {
            return null;
        }

        var snappedPosition = _gridSnapService.Snap(worldPosition);
        return Instantiate(prefab, snappedPosition, Quaternion.identity, _root);
    }
}
