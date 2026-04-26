using System;
using System.Collections.Generic;
using Kubix.Contracts.Maps;
using UnityEngine;

namespace Kubix.Editor;

public sealed class PlacedObjectFactory
{
    public PlacedObject Create(string objectType, Vector3 position, Vector3 scale)
    {
        return new PlacedObject(
            "obj_" + Guid.NewGuid().ToString("N")[..8],
            objectType,
            new TransformData(
                new Vector3Data(position.x, position.y, position.z),
                new RotationData(0, 0, 0),
                new Vector3Data(scale.x, scale.y, scale.z)
            ),
            new Dictionary<string, string>()
        );
    }
}
