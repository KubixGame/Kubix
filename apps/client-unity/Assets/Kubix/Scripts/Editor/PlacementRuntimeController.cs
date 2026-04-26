using System.Collections.Generic;
using Kubix.Contracts.Maps;
using UnityEngine;

namespace Kubix.Editor;

public sealed class PlacementRuntimeController : MonoBehaviour
{
    [SerializeField] private Camera? _sceneCamera;
    [SerializeField] private Transform? _objectsRoot;
    [SerializeField] private SelectionController? _selectionController;

    private readonly GridSnapService _gridSnapService = new();
    private readonly EditorMapState _mapState = new();
    private readonly PlacedObjectFactory _placedObjectFactory = new();
    private readonly Dictionary<GameObject, string> _objectIdsByGameObject = new();

    private string _currentObjectType = "floor_block";

    public void Initialize(Camera sceneCamera, Transform objectsRoot, SelectionController selectionController)
    {
        _sceneCamera = sceneCamera;
        _objectsRoot = objectsRoot;
        _selectionController = selectionController;
    }

    public void SelectObjectType(string objectType)
    {
        _currentObjectType = objectType;
    }

    public void SetMapTitle(string title)
    {
        _mapState.SetTitle(title);
    }

    private void Update()
    {
        if (_sceneCamera == null || _objectsRoot == null || _selectionController == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceAtCursor();
        }

        if (Input.GetMouseButtonDown(1))
        {
            TrySelectAtCursor();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteSelected();
        }

        UpdateSelectedTransform();
    }

    private void TryPlaceAtCursor()
    {
        if (!TryGetGroundPosition(out var groundPosition))
        {
            return;
        }

        var snapped = _gridSnapService.Snap(groundPosition);
        var gameObject = EditorPrimitiveFactory.Create(_currentObjectType, snapped, _objectsRoot!);
        var placedObject = _placedObjectFactory.Create(_currentObjectType, snapped, gameObject.transform.localScale);
        gameObject.name = $"{_currentObjectType}_{placedObject.ObjectId}";
        _objectIdsByGameObject[gameObject] = placedObject.ObjectId;
        _mapState.AddObject(placedObject);
        _selectionController?.Select(gameObject);
    }

    private void TrySelectAtCursor()
    {
        var ray = _sceneCamera!.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            if (hitInfo.collider.gameObject.CompareTag("EditorPlaceable"))
            {
                _selectionController?.Select(hitInfo.collider.gameObject);
                return;
            }
        }

        _selectionController?.Clear();
    }

    private void DeleteSelected()
    {
        var selected = _selectionController?.SelectedObject;
        if (selected == null)
        {
            return;
        }

        if (_objectIdsByGameObject.TryGetValue(selected, out var objectId))
        {
            _mapState.RemoveObject(objectId);
            _objectIdsByGameObject.Remove(selected);
        }

        Destroy(selected);
        _selectionController?.Clear();
    }

    private void UpdateSelectedTransform()
    {
        var selected = _selectionController?.SelectedObject;
        if (selected == null)
        {
            return;
        }

        var position = selected.transform.position;
        var changed = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            position += Vector3.forward;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            position += Vector3.back;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            position += Vector3.left;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            position += Vector3.right;
            changed = true;
        }

        if (changed)
        {
            selected.transform.position = _gridSnapService.Snap(position);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            selected.transform.Rotate(0f, -15f, 0f, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            selected.transform.Rotate(0f, 15f, 0f, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            selected.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            selected.transform.localScale = Vector3.Max(Vector3.one * 0.5f, selected.transform.localScale - new Vector3(0.5f, 0.5f, 0.5f));
        }

        SyncSelectedObjectState(selected);
    }

    private void SyncSelectedObjectState(GameObject gameObject)
    {
        if (!_objectIdsByGameObject.TryGetValue(gameObject, out var objectId))
        {
            return;
        }

        var placedObject = new PlacedObject(
            objectId,
            ExtractObjectType(gameObject.name),
            new TransformData(
                new Vector3Data(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z),
                new RotationData(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z),
                new Vector3Data(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z)
            ),
            new Dictionary<string, string>()
        );

        _mapState.ReplaceObject(placedObject);
    }

    private static string ExtractObjectType(string name)
    {
        var separatorIndex = name.IndexOf("_obj_", System.StringComparison.Ordinal);
        return separatorIndex > 0 ? name[..separatorIndex] : name;
    }

    private bool TryGetGroundPosition(out Vector3 groundPosition)
    {
        var ray = _sceneCamera!.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out var distance))
        {
            groundPosition = ray.GetPoint(distance);
            return true;
        }

        groundPosition = default;
        return false;
    }
}
