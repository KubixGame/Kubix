using System.Collections.Generic;

namespace Kubix.Contracts.Logic;

public sealed record EasyLogicGraph(
    List<EasyNode> Nodes,
    List<EasyEdge> Edges
);
