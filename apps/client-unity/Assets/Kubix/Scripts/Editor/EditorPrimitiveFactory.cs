using UnityEngine;

namespace Kubix.Editor;

public static class EditorPrimitiveFactory
{
    public static GameObject Create(string objectType, Vector3 position, Transform parent)
    {
        var gameObject = objectType switch
        {
            "floor_block" => CreateCube("Floor Block", position, new Vector3(4f, 1f, 4f), new Color(0.47f, 0.67f, 0.97f), parent),
            "wall_block" => CreateCube("Wall Block", position, new Vector3(4f, 4f, 1f), new Color(0.87f, 0.88f, 0.93f), parent),
            "ramp" => CreateRamp(position, parent),
            "spawn_point" => CreateCylinder("Spawn Point", position, new Vector3(2f, 0.25f, 2f), new Color(0.24f, 0.77f, 0.51f), parent),
            "finish_zone" => CreateCube("Finish Zone", position, new Vector3(2f, 2f, 2f), new Color(1f, 0.82f, 0.2f), parent),
            "damage_zone" => CreateCube("Damage Zone", position, new Vector3(3f, 1f, 3f), new Color(0.94f, 0.32f, 0.32f), parent),
            "button" => CreateCylinder("Button", position, new Vector3(1.4f, 0.25f, 1.4f), new Color(0.18f, 0.18f, 0.2f), parent),
            "door" => CreateCube("Door", position, new Vector3(2f, 4f, 0.5f), new Color(0.45f, 0.31f, 0.18f), parent),
            "coin" => CreateCylinder("Coin", position, new Vector3(0.8f, 0.1f, 0.8f), new Color(0.98f, 0.84f, 0.22f), parent),
            "teleport_point" => CreateCylinder("Teleport", position, new Vector3(2f, 0.15f, 2f), new Color(0.58f, 0.35f, 0.98f), parent),
            _ => CreateCube("Block", position, Vector3.one, Color.white, parent)
        };

        gameObject.tag = "EditorPlaceable";
        return gameObject;
    }

    private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Color color, Transform parent)
    {
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = name;
        gameObject.transform.SetParent(parent);
        gameObject.transform.position = position + new Vector3(0f, scale.y * 0.5f, 0f);
        gameObject.transform.localScale = scale;
        ApplyColor(gameObject, color);
        return gameObject;
    }

    private static GameObject CreateCylinder(string name, Vector3 position, Vector3 scale, Color color, Transform parent)
    {
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        gameObject.name = name;
        gameObject.transform.SetParent(parent);
        gameObject.transform.position = position + new Vector3(0f, scale.y * 0.5f, 0f);
        gameObject.transform.localScale = scale;
        ApplyColor(gameObject, color);
        return gameObject;
    }

    private static GameObject CreateRamp(Vector3 position, Transform parent)
    {
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = "Ramp";
        gameObject.transform.SetParent(parent);
        gameObject.transform.position = position + new Vector3(0f, 0.5f, 0f);
        gameObject.transform.localScale = new Vector3(4f, 1f, 4f);
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, -20f);
        ApplyColor(gameObject, new Color(0.68f, 0.8f, 0.97f));
        return gameObject;
    }

    private static void ApplyColor(GameObject gameObject, Color color)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            return;
        }

        var material = new Material(Shader.Find("Standard"));
        material.color = color;
        renderer.material = material;
    }
}
