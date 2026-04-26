using System.Collections.Generic;
using Kubix.Contracts.Logic;

namespace Kubix.Contracts.Maps;

public sealed record MapDefinition(
    string MapId,
    string Title,
    List<PlacedObject> Objects,
    EasyLogicGraph Logic
);
