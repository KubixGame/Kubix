using System;
using System.Collections.Generic;

namespace Kubix.Logic;

public sealed class EventDispatchService
{
    private readonly Dictionary<string, Action<Dictionary<string, string>>> _handlers = new();

    public void Register(string eventType, Action<Dictionary<string, string>> handler)
    {
        _handlers[eventType] = handler;
    }

    public void Dispatch(string eventType, Dictionary<string, string> payload)
    {
        if (_handlers.TryGetValue(eventType, out var handler))
        {
            handler(payload);
        }
    }
}
