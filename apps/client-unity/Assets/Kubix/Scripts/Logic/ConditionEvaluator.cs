using System;
using System.Collections.Generic;
using System.Globalization;

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
            "object_enabled" => runtimeState.GetValueOrDefault($"object:{parameters.GetValueOrDefault("objectId", string.Empty)}:enabled", "true") == "true",
            "random_chance" => EvaluateDeterministicChance(parameters, runtimeState),
            _ => false
        };
    }

    private static bool TryCompareFloat(string leftText, string rightText, Func<float, float, bool> comparator)
    {
        return float.TryParse(leftText, NumberStyles.Float, CultureInfo.InvariantCulture, out var left) &&
               float.TryParse(rightText, NumberStyles.Float, CultureInfo.InvariantCulture, out var right) &&
               comparator(left, right);
    }

    private static bool EvaluateDeterministicChance(Dictionary<string, string> parameters, Dictionary<string, string> runtimeState)
    {
        var threshold = parameters.GetValueOrDefault("percent", "0");
        var seed = runtimeState.GetValueOrDefault("random_seed", "0");
        var salt = parameters.GetValueOrDefault("salt", "default");

        if (!float.TryParse(threshold, NumberStyles.Float, CultureInfo.InvariantCulture, out var thresholdValue))
        {
            return false;
        }

        var hash = Math.Abs(HashCode.Combine(seed, salt));
        var roll = hash % 100;
        return roll < Math.Clamp(thresholdValue, 0f, 100f);
    }
}
