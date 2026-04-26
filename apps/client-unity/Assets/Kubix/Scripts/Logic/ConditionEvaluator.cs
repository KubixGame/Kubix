using System;
using System.Collections.Generic;

namespace Kubix.Logic;

public sealed class ConditionEvaluator
{
    public bool Evaluate(string conditionType, Dictionary<string, string> parameters, Dictionary<string, string> runtimeState)
    {
        return conditionType switch
        {
            "always" => true,
            "has_item" => runtimeState.TryGetValue("inventory", out var inventory) && inventory.Contains(parameters.GetValueOrDefault("itemId", string.Empty), StringComparison.Ordinal),
            "health_below" => TryCompareFloat(runtimeState.GetValueOrDefault("health", "100"), parameters.GetValueOrDefault("value", "0"), (left, right) => left < right),
            "health_above" => TryCompareFloat(runtimeState.GetValueOrDefault("health", "100"), parameters.GetValueOrDefault("value", "0"), (left, right) => left > right),
            "money_at_least" => TryCompareFloat(runtimeState.GetValueOrDefault("money", "0"), parameters.GetValueOrDefault("value", "0"), (left, right) => left >= right),
            _ => false
        };
    }

    private static bool TryCompareFloat(string leftText, string rightText, Func<float, float, bool> comparator)
    {
        return float.TryParse(leftText, out var left) &&
               float.TryParse(rightText, out var right) &&
               comparator(left, right);
    }
}
