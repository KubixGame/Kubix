using System.Collections.Generic;
using System.Linq;
using Kubix.Contracts.Logic;
using Kubix.Contracts.Maps;

namespace Kubix.Logic;

public sealed class EasyLogicValidator
{
    private static readonly HashSet<string> KnownEventTypes = new()
    {
        "map_started",
        "player_touched_zone",
        "button_pressed",
        "item_picked",
        "timer_elapsed",
        "finish_reached"
    };

    private static readonly HashSet<string> KnownConditionTypes = new()
    {
        "always",
        "has_item",
        "money_at_least",
        "health_above",
        "health_below",
        "object_enabled",
        "random_chance"
    };

    private static readonly HashSet<string> KnownActionTypes = new()
    {
        "damage_player",
        "heal_player",
        "add_money",
        "remove_money",
        "show_message",
        "teleport_player",
        "open_door",
        "close_door",
        "give_item",
        "remove_item",
        "end_game"
    };

    private static readonly HashSet<string> KnownFlowTypes = new()
    {
        "wait",
        "and"
    };

    public EasyLogicValidationResult Validate(MapDefinition mapDefinition)
    {
        var result = new EasyLogicValidationResult();
        var graph = mapDefinition.Logic;
        var objectIds = mapDefinition.Objects.Select(objectDefinition => objectDefinition.ObjectId).ToHashSet();
        var nodesById = new Dictionary<string, EasyNode>();

        foreach (var node in graph.Nodes)
        {
            if (string.IsNullOrWhiteSpace(node.NodeId))
            {
                result.AddError("Logic node has an empty id.");
                continue;
            }

            if (!nodesById.TryAdd(node.NodeId, node))
            {
                result.AddError($"Duplicate logic node id: {node.NodeId}.");
            }

            ValidateNodeType(node, result);
            ValidateNodeReferences(node, objectIds, result);
        }

        foreach (var edge in graph.Edges)
        {
            if (!nodesById.ContainsKey(edge.FromNodeId))
            {
                result.AddError($"Edge starts from missing node: {edge.FromNodeId}.");
            }

            if (!nodesById.ContainsKey(edge.ToNodeId))
            {
                result.AddError($"Edge points to missing node: {edge.ToNodeId}.");
            }
        }

        DetectCycles(graph, result);
        return result;
    }

    private static void ValidateNodeType(EasyNode node, EasyLogicValidationResult result)
    {
        switch (node)
        {
            case EasyEventNode eventNode when !KnownEventTypes.Contains(eventNode.EventType):
                result.AddError($"Unknown event type: {eventNode.EventType}.");
                break;
            case EasyConditionNode conditionNode when !KnownConditionTypes.Contains(conditionNode.ConditionType):
                result.AddError($"Unknown condition type: {conditionNode.ConditionType}.");
                break;
            case EasyActionNode actionNode when !KnownActionTypes.Contains(actionNode.ActionType):
                result.AddError($"Unknown action type: {actionNode.ActionType}.");
                break;
            case EasyFlowNode flowNode when !KnownFlowTypes.Contains(flowNode.FlowType):
                result.AddError($"Unknown flow type: {flowNode.FlowType}.");
                break;
        }
    }

    private static void ValidateNodeReferences(EasyNode node, HashSet<string> objectIds, EasyLogicValidationResult result)
    {
        var parameters = node switch
        {
            EasyEventNode eventNode => eventNode.Parameters,
            EasyConditionNode conditionNode => conditionNode.Parameters,
            EasyActionNode actionNode => actionNode.Parameters,
            EasyFlowNode flowNode => flowNode.Parameters,
            _ => null
        };

        if (parameters == null)
        {
            return;
        }

        foreach (var key in new[] { "targetObjectId", "objectId", "destinationObjectId" })
        {
            if (parameters.TryGetValue(key, out var objectId) && !string.IsNullOrWhiteSpace(objectId) && !objectIds.Contains(objectId))
            {
                result.AddError($"Node {node.NodeId} references missing object {objectId}.");
            }
        }
    }

    private static void DetectCycles(EasyLogicGraph graph, EasyLogicValidationResult result)
    {
        var outgoing = graph.Edges.GroupBy(edge => edge.FromNodeId).ToDictionary(group => group.Key, group => group.Select(edge => edge.ToNodeId).ToList());
        var visiting = new HashSet<string>();
        var visited = new HashSet<string>();

        foreach (var node in graph.Nodes)
        {
            Visit(node.NodeId, outgoing, visiting, visited, result);
        }
    }

    private static void Visit(string nodeId, Dictionary<string, List<string>> outgoing, HashSet<string> visiting, HashSet<string> visited, EasyLogicValidationResult result)
    {
        if (visited.Contains(nodeId))
        {
            return;
        }

        if (!visiting.Add(nodeId))
        {
            result.AddError($"Cycle detected at node {nodeId}.");
            return;
        }

        if (outgoing.TryGetValue(nodeId, out var nextNodes))
        {
            foreach (var nextNode in nextNodes)
            {
                Visit(nextNode, outgoing, visiting, visited, result);
            }
        }

        visiting.Remove(nodeId);
        visited.Add(nodeId);
    }
}
