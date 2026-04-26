using System.Collections.Generic;

namespace Kubix.Contracts.Logic;

public sealed record EasyEventNode(
    string NodeId,
    string EventType,
    Dictionary<string, string> Parameters
) : EasyNode(NodeId, "event");
