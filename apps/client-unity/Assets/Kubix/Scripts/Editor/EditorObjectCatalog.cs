using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kubix.Editor;

[CreateAssetMenu(menuName = "Kubix/Editor Object Catalog", fileName = "EditorObjectCatalog")]
public sealed class EditorObjectCatalog : ScriptableObject
{
    [Serializable]
    public sealed class CatalogEntry
    {
        public string ObjectType = "floor_block";
        public string DisplayName = "Floor Block";
        public Color Color = Color.white;
        public GameObject? Prefab;
        public Vector3 DefaultScale = Vector3.one;
    }

    [SerializeField] private List<CatalogEntry> _entries = new();

    public IReadOnlyList<CatalogEntry> Entries => _entries;

    public CatalogEntry? Find(string objectType)
    {
        return _entries.Find(entry => entry.ObjectType == objectType);
    }
}
