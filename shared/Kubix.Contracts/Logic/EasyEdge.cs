namespace Kubix.Contracts.Logic;

public sealed record EasyEdge(
    string FromNodeId,
    string ToNodeId,
    string EdgeType
);
