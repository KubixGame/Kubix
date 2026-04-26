using System;
using UnityEngine;

namespace Kubix.Editor;

public sealed class SelectionController : MonoBehaviour
{
    public GameObject? SelectedObject { get; private set; }

    public event Action<GameObject?>? SelectionChanged;

    public void Select(GameObject? gameObject)
    {
        if (SelectedObject == gameObject)
        {
            return;
        }

        SelectedObject = gameObject;
        SelectionChanged?.Invoke(SelectedObject);
    }

    public void Clear()
    {
        Select(null);
    }
}
