using System;
using System.Collections.Generic;
using Kubix.Logic;

namespace Kubix.Runtime;

public sealed class MatchStateController
{
    public Dictionary<string, string> RuntimeState { get; } = new()
    {
        ["health"] = "100",
        ["money"] = "0"
    };

    public event Action<string>? MessageRaised;
    public event Action? MatchEnded;

    public ActionExecutor CreateActionExecutor()
    {
        var executor = new ActionExecutor();
        executor.MessageRaised += message => MessageRaised?.Invoke(message);
        executor.MatchEnded += () => MatchEnded?.Invoke();
        return executor;
    }
}
