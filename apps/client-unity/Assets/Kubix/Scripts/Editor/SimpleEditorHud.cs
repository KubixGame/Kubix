using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kubix.Editor;

public sealed class SimpleEditorHud : MonoBehaviour
{
    [SerializeField] private PlacementRuntimeController? _placementController;
    [SerializeField] private string _mapTitle = "New Kubix Map";

    private readonly List<string> _objectTypes = new()
    {
        "floor_block",
        "wall_block",
        "ramp",
        "spawn_point",
        "finish_zone",
        "damage_zone",
        "button",
        "door",
        "coin",
        "teleport_point"
    };

    public void Initialize(PlacementRuntimeController placementController)
    {
        _placementController = placementController;
    }

    private void OnGUI()
    {
        GUI.color = new Color(1f, 1f, 1f, 0.96f);
        GUILayout.BeginArea(new Rect(18f, 18f, 320f, 250f), GUI.skin.box);
        GUILayout.Label("Kubix Editor Sandbox");
        GUILayout.Label("ЛКМ: поставить объект");
        GUILayout.Label("ПКМ: выбрать объект");
        GUILayout.Label("Delete: удалить объект");
        GUILayout.Label("Стрелки: двигать выбранный объект");
        GUILayout.Label("Z/X: вращать");
        GUILayout.Label("C/V: менять размер");
        GUILayout.Space(8f);
        GUILayout.Label("Текущая карта");
        _mapTitle = GUILayout.TextField(_mapTitle);
        _placementController?.SetMapTitle(_mapTitle);
        GUILayout.Space(8f);
        GUILayout.Label("Библиотека объектов");

        foreach (var objectType in _objectTypes)
        {
            if (GUILayout.Button(ToDisplayName(objectType)))
            {
                _placementController?.SelectObjectType(objectType);
            }
        }

        GUILayout.EndArea();
    }

    private static string ToDisplayName(string objectType)
    {
        return objectType.Replace("_", " ", StringComparison.OrdinalIgnoreCase);
    }
}
