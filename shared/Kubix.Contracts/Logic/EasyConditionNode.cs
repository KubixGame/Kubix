using System.Collections.Generic;

namespace Kubix.Contracts.Logic;

public sealed record EasyConditionNode(
    string NodeId,
    string ConditionType,
    Dictionary<string, string> Parameters
) : EasyNode(NodeId, "condition");
