using System.Linq;
using Kubix.Contracts.Maps;

namespace Kubix.Publishing;

public sealed class PublishValidator
{
    public (bool IsValid, string Error) Validate(MapDefinition map)
    {
        if (string.IsNullOrWhiteSpace(map.Title))
        {
            return (false, "Map title is required.");
        }

        if (!map.Objects.Any(objectDefinition => objectDefinition.ObjectType == "spawn_point"))
        {
            return (false, "At least one spawn point is required.");
        }

        if (!map.Objects.Any(objectDefinition => objectDefinition.ObjectType == "finish_zone"))
        {
            return (false, "At least one finish zone is required.");
        }

        return (true, string.Empty);
    }
}
