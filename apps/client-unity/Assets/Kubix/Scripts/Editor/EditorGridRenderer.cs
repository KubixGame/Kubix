using UnityEngine;

namespace Kubix.Editor;

public sealed class EditorGridRenderer : MonoBehaviour
{
    [SerializeField] private int _gridHalfSize = 20;
    [SerializeField] private Color _gridColor = new(0f, 0f, 0f, 0.15f);

    private Material? _lineMaterial;

    private void OnRenderObject()
    {
        if (_lineMaterial == null)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            _lineMaterial = new Material(shader);
            _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            _lineMaterial.SetInt("_ZWrite", 0);
        }

        _lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(_gridColor);

        for (var i = -_gridHalfSize; i <= _gridHalfSize; i++)
        {
            GL.Vertex3(i, 0.01f, -_gridHalfSize);
            GL.Vertex3(i, 0.01f, _gridHalfSize);

            GL.Vertex3(-_gridHalfSize, 0.01f, i);
            GL.Vertex3(_gridHalfSize, 0.01f, i);
        }

        GL.End();
        GL.PopMatrix();
    }
}
