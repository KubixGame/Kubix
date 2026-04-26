import test from "node:test";
import assert from "node:assert/strict";
import { executeEasyLogicGraph, validateEasyLogicGraph } from "../src/logic-core.js";

function baseMap(logic) {
  return {
    mapId: "map_test",
    title: "Logic Test Map",
    objects: [
      { objectId: "spawn_01", objectType: "spawn_point" },
      { objectId: "trap_01", objectType: "damage_zone" },
      { objectId: "finish_01", objectType: "finish_zone" },
      { objectId: "door_01", objectType: "door" }
    ],
    logic
  };
}

test("validates a simple trap graph", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_trap",
        nodeType: "event",
        eventType: "player_touched_zone",
        parameters: { targetObjectId: "trap_01" }
      },
      {
        nodeId: "action_damage",
        nodeType: "action",
        actionType: "damage_player",
        parameters: { amount: "25" }
      }
    ],
    edges: [
      { fromNodeId: "event_trap", toNodeId: "action_damage", edgeType: "then" }
    ]
  };

  const result = validateEasyLogicGraph(baseMap(graph));
  assert.equal(result.isValid, true);
  assert.deepEqual(result.errors, []);
});

test("trap graph removes health when touched", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_trap",
        nodeType: "event",
        eventType: "player_touched_zone",
        parameters: { targetObjectId: "trap_01" }
      },
      {
        nodeId: "action_damage",
        nodeType: "action",
        actionType: "damage_player",
        parameters: { amount: "25" }
      }
    ],
    edges: [
      { fromNodeId: "event_trap", toNodeId: "action_damage", edgeType: "then" }
    ]
  };

  const runtimeState = { health: "100" };
  executeEasyLogicGraph(graph, "player_touched_zone", { targetObjectId: "trap_01" }, runtimeState);

  assert.equal(runtimeState.health, "75");
});

test("finish graph shows message and ends game", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_finish",
        nodeType: "event",
        eventType: "finish_reached",
        parameters: { targetObjectId: "finish_01" }
      },
      {
        nodeId: "action_message",
        nodeType: "action",
        actionType: "show_message",
        parameters: { text: "You win!" }
      },
      {
        nodeId: "action_end",
        nodeType: "action",
        actionType: "end_game",
        parameters: {}
      }
    ],
    edges: [
      { fromNodeId: "event_finish", toNodeId: "action_message", edgeType: "then" },
      { fromNodeId: "action_message", toNodeId: "action_end", edgeType: "then" }
    ]
  };

  const runtimeState = {};
  const events = { messages: [] };
  executeEasyLogicGraph(graph, "finish_reached", { targetObjectId: "finish_01" }, runtimeState, events);

  assert.deepEqual(events.messages, ["You win!"]);
  assert.equal(runtimeState.matchEnded, "true");
});

test("validator catches missing object reference", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_missing",
        nodeType: "event",
        eventType: "player_touched_zone",
        parameters: { targetObjectId: "missing_zone" }
      }
    ],
    edges: []
  };

  const result = validateEasyLogicGraph(baseMap(graph));
  assert.equal(result.isValid, false);
  assert.match(result.errors.join("\n"), /missing_zone/);
});

test("validator catches cycles", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_a",
        nodeType: "event",
        eventType: "map_started",
        parameters: {}
      },
      {
        nodeId: "flow_a",
        nodeType: "flow",
        flowType: "and",
        parameters: {}
      }
    ],
    edges: [
      { fromNodeId: "event_a", toNodeId: "flow_a", edgeType: "then" },
      { fromNodeId: "flow_a", toNodeId: "event_a", edgeType: "then" }
    ]
  };

  const result = validateEasyLogicGraph(baseMap(graph));
  assert.equal(result.isValid, false);
  assert.match(result.errors.join("\n"), /Cycle detected/);
});

test("inventory and door actions update runtime state deterministically", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_button",
        nodeType: "event",
        eventType: "button_pressed",
        parameters: { targetObjectId: "door_01" }
      },
      {
        nodeId: "condition_key",
        nodeType: "condition",
        conditionType: "has_item",
        parameters: { itemId: "gold_key" }
      },
      {
        nodeId: "action_open",
        nodeType: "action",
        actionType: "open_door",
        parameters: { objectId: "door_01" }
      }
    ],
    edges: [
      { fromNodeId: "event_button", toNodeId: "condition_key", edgeType: "if" },
      { fromNodeId: "condition_key", toNodeId: "action_open", edgeType: "then" }
    ]
  };

  const runtimeState = { inventory: "gold_key", "object:door_01:enabled": "true" };
  const events = { objectChanges: [] };
  executeEasyLogicGraph(graph, "button_pressed", { targetObjectId: "door_01" }, runtimeState, events);

  assert.equal(runtimeState["object:door_01:enabled"], "false");
  assert.deepEqual(events.objectChanges, [{ objectId: "door_01", enabled: false }]);
});

test("random chance uses deterministic seed and salt", () => {
  const graph = {
    nodes: [
      {
        nodeId: "event_start",
        nodeType: "event",
        eventType: "map_started",
        parameters: {}
      },
      {
        nodeId: "condition_chance",
        nodeType: "condition",
        conditionType: "random_chance",
        parameters: { percent: "100", salt: "reward" }
      },
      {
        nodeId: "action_money",
        nodeType: "action",
        actionType: "add_money",
        parameters: { amount: "10" }
      }
    ],
    edges: [
      { fromNodeId: "event_start", toNodeId: "condition_chance", edgeType: "if" },
      { fromNodeId: "condition_chance", toNodeId: "action_money", edgeType: "then" }
    ]
  };

  const runtimeState = { money: "0", random_seed: "demo" };
  executeEasyLogicGraph(graph, "map_started", {}, runtimeState);

  assert.equal(runtimeState.money, "10");
});
