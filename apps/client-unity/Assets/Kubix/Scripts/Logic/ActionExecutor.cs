using System;
using System.Collections.Generic;
using System.Globalization;

namespace Kubix.Logic;

public sealed class ActionExecutor
{
    public event Action<string>? MessageRaised;
    public event Action? MatchEnded;
    public event Action<string, string>? PlayerTeleported;
    public event Action<string, bool>? ObjectEnabledChanged;

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
            case "teleport_player":
                PlayerTeleported?.Invoke(parameters.GetValueOrDefault("playerId", "current"), parameters.GetValueOrDefault("destinationObjectId", string.Empty));
                break;
            case "open_door":
                SetObjectEnabled(runtimeState, parameters.GetValueOrDefault("objectId", string.Empty), false);
                break;
            case "close_door":
                SetObjectEnabled(runtimeState, parameters.GetValueOrDefault("objectId", string.Empty), true);
                break;
            case "give_item":
                AddInventoryItem(runtimeState, parameters.GetValueOrDefault("itemId", string.Empty));
                break;
            case "remove_item":
                RemoveInventoryItem(runtimeState, parameters.GetValueOrDefault("itemId", string.Empty));
                break;
            case "end_game":
                MatchEnded?.Invoke();
                break;
        }
    }

    private static float GetFloat(Dictionary<string, string> parameters, string key, float fallback)
    {
        return parameters.TryGetValue(key, out var value) && float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) ? parsed : fallback;
    }

    private static void ApplyFloatDelta(Dictionary<string, string> runtimeState, string key, float delta)
    {
        var current = runtimeState.TryGetValue(key, out var value) && float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : 0f;

        runtimeState[key] = (current + delta).ToString("0.##", CultureInfo.InvariantCulture);
    }

    private void SetObjectEnabled(Dictionary<string, string> runtimeState, string objectId, bool enabled)
    {
        if (string.IsNullOrWhiteSpace(objectId))
        {
            return;
        }

        runtimeState[$"object:{objectId}:enabled"] = enabled ? "true" : "false";
        ObjectEnabledChanged?.Invoke(objectId, enabled);
    }

    private static void AddInventoryItem(Dictionary<string, string> runtimeState, string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            return;
        }

        var inventory = runtimeState.GetValueOrDefault("inventory", string.Empty);
        var items = new HashSet<string>(inventory.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        items.Add(itemId);
        runtimeState["inventory"] = string.Join(",", items);
    }

    private static void RemoveInventoryItem(Dictionary<string, string> runtimeState, string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            return;
        }

        var inventory = runtimeState.GetValueOrDefault("inventory", string.Empty);
        var items = new HashSet<string>(inventory.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        items.Remove(itemId);
        runtimeState["inventory"] = string.Join(",", items);
    }
}
