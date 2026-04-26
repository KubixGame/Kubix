using System.Collections.Generic;

namespace Kubix.Logic;

public sealed class EasyLogicGraphViewModel
{
    public List<string> EventNodes { get; } = new();
    public List<string> ConditionNodes { get; } = new();
    public List<string> ActionNodes { get; } = new();
}
