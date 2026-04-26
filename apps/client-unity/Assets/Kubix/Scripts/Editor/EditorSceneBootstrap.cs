using UnityEngine;

namespace Kubix.Editor;

public sealed class EditorSceneBootstrap : MonoBehaviour
{
    [SerializeField] private PlacementTool? _placementTool;
    [SerializeField] private SelectionController? _selectionController;
    [SerializeField] private TransformGizmoController? _gizmoController;

    public GridSnapService GridSnapService { get; } = new();
    public DraftMapRepository DraftMapRepository { get; } = new();

    private void Awake()
    {
        if (_placementTool != null)
        {
            _placementTool.Initialize(GridSnapService);
        }

        if (_selectionController != null && _gizmoController != null)
        {
            _gizmoController.Initialize(_selectionController, GridSnapService);
        }
    }
}
