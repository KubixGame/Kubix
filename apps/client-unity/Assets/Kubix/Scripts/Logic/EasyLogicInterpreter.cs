using System.Collections.Generic;
using System.Linq;
using Kubix.Contracts.Logic;

namespace Kubix.Logic;

public sealed class EasyLogicInterpreter
{
    private readonly ConditionEvaluator _conditionEvaluator;
    private readonly ActionExecutor _actionExecutor;

    public EasyLogicInterpreter(ConditionEvaluator conditionEvaluator, ActionExecutor actionExecutor)
    {
        _conditionEvaluator = conditionEvaluator;
        _actionExecutor = actionExecutor;
    }

    public void Execute(EasyLogicGraph graph, string eventType, Dictionary<string, string> eventPayload, Dictionary<string, string> runtimeState)
    {
        var nodesById = graph.Nodes.ToDictionary(node => node.NodeId);

        foreach (var node in graph.Nodes.OfType<EasyEventNode>())
        {
            if (node.EventType != eventType)
            {
                continue;
            }

            if (!EventMatches(node.Parameters, eventPayload))
            {
                continue;
            }

            TraverseFromNode(node.NodeId, graph, nodesById, runtimeState);
        }
    }

    private void TraverseFromNode(string nodeId, EasyLogicGraph graph, Dictionary<string, EasyNode> nodesById, Dictionary<string, string> runtimeState)
    {
        var outgoing = graph.Edges.Where(edge => edge.FromNodeId == nodeId);
        foreach (var edge in outgoing)
        {
            if (!nodesById.TryGetValue(edge.ToNodeId, out var nextNode))
            {
                continue;
            }

            switch (nextNode)
            {
                case EasyConditionNode conditionNode:
                    if (_conditionEvaluator.Evaluate(conditionNode.ConditionType, conditionNode.Parameters, runtimeState))
                    {
                        TraverseFromNode(conditionNode.NodeId, graph, nodesById, runtimeState);
                    }
                    break;
                case EasyActionNode actionNode:
                    _actionExecutor.Execute(actionNode.ActionType, actionNode.Parameters, runtimeState);
                    TraverseFromNode(actionNode.NodeId, graph, nodesById, runtimeState);
                    break;
            }
        }
    }

    private static bool EventMatches(Dictionary<string, string> expected, Dictionary<string, string> actual)
    {
        foreach (var pair in expected)
        {
            if (!actual.TryGetValue(pair.Key, out var actualValue) || actualValue != pair.Value)
            {
                return false;
            }
        }

        return true;
    }
}
