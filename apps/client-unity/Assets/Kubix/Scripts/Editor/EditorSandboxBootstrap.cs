using UnityEngine;

namespace Kubix.Editor;

public sealed class EditorSandboxBootstrap : MonoBehaviour
{
    [SerializeField] private Color _backgroundColor = new(0.92f, 0.95f, 1f);

    private void Awake()
    {
        var camera = EnsureCamera();
        EnsureLight();
        EnsureGround();
        EnsureGrid();
        var objectsRoot = EnsureObjectsRoot();
        var selectionController = gameObject.GetComponent<SelectionController>() ?? gameObject.AddComponent<SelectionController>();
        var placementController = gameObject.GetComponent<PlacementRuntimeController>() ?? gameObject.AddComponent<PlacementRuntimeController>();
        placementController.Initialize(camera, objectsRoot, selectionController);

        var hud = gameObject.GetComponent<SimpleEditorHud>() ?? gameObject.AddComponent<SimpleEditorHud>();
        hud.Initialize(placementController);
    }

    private Camera EnsureCamera()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            var cameraObject = new GameObject("Editor Camera");
            camera = cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
        }

        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = _backgroundColor;
        camera.transform.position = new Vector3(-12f, 16f, -12f);
        camera.transform.rotation = Quaternion.Euler(35f, 45f, 0f);

        if (camera.GetComponent<EditorCameraController>() == null)
        {
            camera.gameObject.AddComponent<EditorCameraController>();
        }

        return camera;
    }

    private void EnsureLight()
    {
        var lightObject = GameObject.Find("Editor Directional Light");
        if (lightObject != null)
        {
            return;
        }

        lightObject = new GameObject("Editor Directional Light");
        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.25f;
        light.color = new Color(1f, 0.98f, 0.95f);
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    private void EnsureGround()
    {
        if (GameObject.Find("Editor Ground") != null)
        {
            return;
        }

        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Editor Ground";
        ground.transform.position = new Vector3(0f, -0.5f, 0f);
        ground.transform.localScale = new Vector3(40f, 1f, 40f);
        var material = new Material(Shader.Find("Standard"));
        material.color = new Color(0.86f, 0.9f, 0.96f);
        ground.GetComponent<Renderer>().material = material;
    }

    private void EnsureGrid()
    {
        if (GameObject.Find("Editor Grid") != null)
        {
            return;
        }

        var grid = new GameObject("Editor Grid");
        grid.AddComponent<EditorGridRenderer>();
    }

    private static Transform EnsureObjectsRoot()
    {
        var root = GameObject.Find("Placed Objects");
        if (root != null)
        {
            return root.transform;
        }

        return new GameObject("Placed Objects").transform;
    }
}
