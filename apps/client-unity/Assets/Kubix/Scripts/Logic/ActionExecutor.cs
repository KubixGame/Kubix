using System;
using System.Collections.Generic;

namespace Kubix.Logic;

public sealed class ActionExecutor
{
    public event Action<string>? MessageRaised;
    public event Action? MatchEnded;

    public void Execute(string actionType, Dictionary<string, string> parameters, Dictionary<string, string> runtimeState)
    {
        switch (actionType)
        {
            case "damage_player":
                ApplyFloatDelta(runtimeState, "health", -GetFloat(parameters, "amount", 0f));
                break;
            case "heal_player":
                ApplyFloatDelta(runtimeState, "health", GetFloat(parameters, "amount", 0f));
                break;
            case "add_money":
                ApplyFloatDelta(runtimeState, "money", GetFloat(parameters, "amount", 0f));
                break;
            case "remove_money":
                ApplyFloatDelta(runtimeState, "money", -GetFloat(parameters, "amount", 0f));
                break;
            case "show_message":
                MessageRaised?.Invoke(parameters.GetValueOrDefault("text", string.Empty));
                break;
            case "end_game":
                MatchEnded?.Invoke();
                break;
        }
    }

    private static float GetFloat(Dictionary<string, string> parameters, string key, float fallback)
    {
        return parameters.TryGetValue(key, out var value) && float.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private static void ApplyFloatDelta(Dictionary<string, string> runtimeState, string key, float delta)
    {
        var current = runtimeState.TryGetValue(key, out var value) && float.TryParse(value, out var parsed)
            ? parsed
            : 0f;

        runtimeState[key] = (current + delta).ToString("0.##");
    }
}
