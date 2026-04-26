using Kubix.Contracts.Logic;
using Kubix.Contracts.Maps;
using Kubix.Editor;
using Kubix.Logic;
using UnityEngine;

namespace Kubix.Runtime;

public sealed class DraftPlaytestLoader : MonoBehaviour
{
    [SerializeField] private string _draftMapId = "map_first_obby";

    private readonly DraftMapRepository _draftMapRepository = new();
    private MatchStateController? _matchStateController;
    private EasyLogicInterpreter? _interpreter;

    private void Start()
    {
        _matchStateController = new MatchStateController();
        _interpreter = new EasyLogicInterpreter(new ConditionEvaluator(), _matchStateController.CreateActionExecutor());
    }

    public void TriggerTestEvent(EasyLogicGraph graph, string eventType, System.Collections.Generic.Dictionary<string, string> payload)
    {
        if (_matchStateController == null || _interpreter == null)
        {
            return;
        }

        _interpreter.Execute(graph, eventType, payload, _matchStateController.RuntimeState);
    }

    public string? LoadDraftJson()
    {
        return _draftMapRepository.LoadRawJson(_draftMapId);
    }
}
