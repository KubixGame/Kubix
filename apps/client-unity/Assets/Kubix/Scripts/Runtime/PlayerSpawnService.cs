using System.Linq;
using Kubix.Contracts.Maps;
using UnityEngine;

namespace Kubix.Runtime;

public sealed class PlayerSpawnService
{
    public Vector3 ResolveSpawnPosition(MapDefinition mapDefinition, Vector3 fallback)
    {
        var spawn = mapDefinition.Objects.FirstOrDefault(objectDefinition => objectDefinition.ObjectType == "spawn_point");
        if (spawn == null)
        {
            return fallback;
        }

        return new Vector3(
            spawn.Transform.Position.X,
            spawn.Transform.Position.Y,
            spawn.Transform.Position.Z
        );
    }
}
