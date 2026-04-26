using System.Collections.Generic;

namespace Kubix.Contracts.Logic;

public sealed record EasyFlowNode(
    string NodeId,
    string FlowType,
    Dictionary<string, string> Parameters
) : EasyNode(NodeId, "flow");
