using System.Collections.Generic;

namespace Kubix.Contracts.Maps;

public sealed record PlacedObject(
    string ObjectId,
    string ObjectType,
    TransformData Transform,
    Dictionary<string, string> Properties
);
