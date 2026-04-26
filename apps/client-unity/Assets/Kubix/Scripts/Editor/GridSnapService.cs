using UnityEngine;

namespace Kubix.Editor;

public sealed class GridSnapService
{
    public float GridSize { get; private set; } = 1f;
    public bool Enabled { get; private set; } = true;

    public void SetGridSize(float gridSize)
    {
        GridSize = Mathf.Max(0.25f, gridSize);
    }

    public void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }

    public Vector3 Snap(Vector3 position)
    {
        if (!Enabled)
        {
            return position;
        }

        return new Vector3(
            Mathf.Round(position.x / GridSize) * GridSize,
            Mathf.Round(position.y / GridSize) * GridSize,
            Mathf.Round(position.z / GridSize) * GridSize
        );
    }
}
