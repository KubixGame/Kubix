using System.Collections.Generic;

namespace Kubix.Contracts.Logic;

public sealed record EasyActionNode(
    string NodeId,
    string ActionType,
    Dictionary<string, string> Parameters
) : EasyNode(NodeId, "action");
