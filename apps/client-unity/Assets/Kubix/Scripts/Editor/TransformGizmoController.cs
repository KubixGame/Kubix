using UnityEngine;

namespace Kubix.Editor;

public sealed class TransformGizmoController : MonoBehaviour
{
    [SerializeField] private SelectionController? _selectionController;
    [SerializeField] private float _rotationStep = 15f;
    [SerializeField] private float _scaleStep = 0.5f;

    private GridSnapService _gridSnapService = new();

    public void Initialize(SelectionController selectionController, GridSnapService gridSnapService)
    {
        _selectionController = selectionController;
        _gridSnapService = gridSnapService;
    }

    public void MoveSelected(Vector3 worldPosition)
    {
        if (_selectionController?.SelectedObject == null)
        {
            return;
        }

        _selectionController.SelectedObject.transform.position = _gridSnapService.Snap(worldPosition);
    }

    public void RotateSelected(Vector3 axis)
    {
        if (_selectionController?.SelectedObject == null)
        {
            return;
        }

        _selectionController.SelectedObject.transform.Rotate(axis * _rotationStep, Space.World);
    }

    public void ScaleSelected(Vector3 direction)
    {
        if (_selectionController?.SelectedObject == null)
        {
            return;
        }

        var currentScale = _selectionController.SelectedObject.transform.localScale;
        var nextScale = currentScale + direction * _scaleStep;
        _selectionController.SelectedObject.transform.localScale = new Vector3(
            Mathf.Max(0.5f, nextScale.x),
            Mathf.Max(0.5f, nextScale.y),
            Mathf.Max(0.5f, nextScale.z)
        );
    }
}
