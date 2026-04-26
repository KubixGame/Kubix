const knownEvents = new Set([
  "map_started",
  "player_touched_zone",
  "button_pressed",
  "item_picked",
  "timer_elapsed",
  "finish_reached"
]);

const knownConditions = new Set([
  "always",
  "has_item",
  "money_at_least",
  "health_above",
  "health_below",
  "object_enabled",
  "random_chance"
]);

const knownActions = new Set([
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
]);

const knownFlows = new Set(["wait", "and"]);

export function validateEasyLogicGraph(mapDefinition) {
  const errors = [];
  const graph = mapDefinition.logic || { nodes: [], edges: [] };
  const objectIds = new Set((mapDefinition.objects || []).map(objectDefinition => objectDefinition.objectId));
  const nodesById = new Map();

  for (const node of graph.nodes || []) {
    if (!node.nodeId) {
      errors.push("Logic node has an empty id.");
      continue;
    }

    if (nodesById.has(node.nodeId)) {
      errors.push(`Duplicate logic node id: ${node.nodeId}.`);
    }

    nodesById.set(node.nodeId, node);
    validateNodeType(node, errors);
    validateNodeReferences(node, objectIds, errors);
  }

  for (const edge of graph.edges || []) {
    if (!nodesById.has(edge.fromNodeId)) {
      errors.push(`Edge starts from missing node: ${edge.fromNodeId}.`);
    }

    if (!nodesById.has(edge.toNodeId)) {
      errors.push(`Edge points to missing node: ${edge.toNodeId}.`);
    }
  }

  detectCycles(graph, errors);
  return { isValid: errors.length === 0, errors };
}

export function executeEasyLogicGraph(graph, eventType, eventPayload, runtimeState, events = {}) {
  const nodesById = new Map((graph.nodes || []).map(node => [node.nodeId, node]));

  for (const node of graph.nodes || []) {
    if (node.nodeType !== "event" || node.eventType !== eventType) {
      continue;
    }

    if (!parametersMatch(node.parameters || {}, eventPayload)) {
      continue;
    }

    traverseFromNode(node.nodeId, graph, nodesById, runtimeState, events, 0);
  }

  return runtimeState;
}

function traverseFromNode(nodeId, graph, nodesById, runtimeState, events, stepCount) {
  if (stepCount > 256) {
    return;
  }

  const outgoing = (graph.edges || []).filter(edge => edge.fromNodeId === nodeId);
  for (const edge of outgoing) {
    const node = nodesById.get(edge.toNodeId);
    if (!node) {
      continue;
    }

    if (node.nodeType === "condition") {
      if (evaluateCondition(node.conditionType, node.parameters || {}, runtimeState)) {
        traverseFromNode(node.nodeId, graph, nodesById, runtimeState, events, stepCount + 1);
      }
      continue;
    }

    if (node.nodeType === "action") {
      executeAction(node.actionType, node.parameters || {}, runtimeState, events);
      traverseFromNode(node.nodeId, graph, nodesById, runtimeState, events, stepCount + 1);
      continue;
    }

    if (node.nodeType === "flow" && (node.flowType === "and" || node.flowType === "wait")) {
      traverseFromNode(node.nodeId, graph, nodesById, runtimeState, events, stepCount + 1);
    }
  }
}

function evaluateCondition(conditionType, parameters, runtimeState) {
  switch (conditionType) {
    case "always":
      return true;
    case "has_item":
      return splitInventory(runtimeState.inventory).has(parameters.itemId);
    case "money_at_least":
      return toNumber(runtimeState.money, 0) >= toNumber(parameters.value, 0);
    case "health_above":
      return toNumber(runtimeState.health, 100) > toNumber(parameters.value, 0);
    case "health_below":
      return toNumber(runtimeState.health, 100) < toNumber(parameters.value, 0);
    case "object_enabled":
      return runtimeState[`object:${parameters.objectId}:enabled`] !== "false";
    case "random_chance":
      return deterministicChance(parameters, runtimeState);
    default:
      return false;
  }
}

function executeAction(actionType, parameters, runtimeState, events) {
  switch (actionType) {
    case "damage_player":
      applyDelta(runtimeState, "health", -toNumber(parameters.amount, 0));
      break;
    case "heal_player":
      applyDelta(runtimeState, "health", toNumber(parameters.amount, 0));
      break;
    case "add_money":
      applyDelta(runtimeState, "money", toNumber(parameters.amount, 0));
      break;
    case "remove_money":
      applyDelta(runtimeState, "money", -toNumber(parameters.amount, 0));
      break;
    case "show_message":
      events.messages?.push(parameters.text || "");
      break;
    case "teleport_player":
      events.teleports?.push({ playerId: parameters.playerId || "current", destinationObjectId: parameters.destinationObjectId || "" });
      break;
    case "open_door":
      setObjectEnabled(runtimeState, events, parameters.objectId, false);
      break;
    case "close_door":
      setObjectEnabled(runtimeState, events, parameters.objectId, true);
      break;
    case "give_item":
      addInventoryItem(runtimeState, parameters.itemId);
      break;
    case "remove_item":
      removeInventoryItem(runtimeState, parameters.itemId);
      break;
    case "end_game":
      runtimeState.matchEnded = "true";
      break;
  }
}

function validateNodeType(node, errors) {
  if (node.nodeType === "event" && !knownEvents.has(node.eventType)) {
    errors.push(`Unknown event type: ${node.eventType}.`);
  }

  if (node.nodeType === "condition" && !knownConditions.has(node.conditionType)) {
    errors.push(`Unknown condition type: ${node.conditionType}.`);
  }

  if (node.nodeType === "action" && !knownActions.has(node.actionType)) {
    errors.push(`Unknown action type: ${node.actionType}.`);
  }

  if (node.nodeType === "flow" && !knownFlows.has(node.flowType)) {
    errors.push(`Unknown flow type: ${node.flowType}.`);
  }
}

function validateNodeReferences(node, objectIds, errors) {
  const parameters = node.parameters || {};
  for (const key of ["targetObjectId", "objectId", "destinationObjectId"]) {
    if (parameters[key] && !objectIds.has(parameters[key])) {
      errors.push(`Node ${node.nodeId} references missing object ${parameters[key]}.`);
    }
  }
}

function detectCycles(graph, errors) {
  const outgoing = new Map();
  for (const edge of graph.edges || []) {
    if (!outgoing.has(edge.fromNodeId)) {
      outgoing.set(edge.fromNodeId, []);
    }
    outgoing.get(edge.fromNodeId).push(edge.toNodeId);
  }

  const visiting = new Set();
  const visited = new Set();

  for (const node of graph.nodes || []) {
    visit(node.nodeId, outgoing, visiting, visited, errors);
  }
}

function visit(nodeId, outgoing, visiting, visited, errors) {
  if (visited.has(nodeId)) {
    return;
  }

  if (visiting.has(nodeId)) {
    errors.push(`Cycle detected at node ${nodeId}.`);
    return;
  }

  visiting.add(nodeId);
  for (const nextNodeId of outgoing.get(nodeId) || []) {
    visit(nextNodeId, outgoing, visiting, visited, errors);
  }
  visiting.delete(nodeId);
  visited.add(nodeId);
}

function parametersMatch(expected, actual) {
  return Object.entries(expected).every(([key, value]) => actual[key] === value);
}

function applyDelta(runtimeState, key, delta) {
  runtimeState[key] = formatNumber(toNumber(runtimeState[key], 0) + delta);
}

function setObjectEnabled(runtimeState, events, objectId, enabled) {
  if (!objectId) {
    return;
  }

  runtimeState[`object:${objectId}:enabled`] = enabled ? "true" : "false";
  events.objectChanges?.push({ objectId, enabled });
}

function addInventoryItem(runtimeState, itemId) {
  if (!itemId) {
    return;
  }

  const items = splitInventory(runtimeState.inventory);
  items.add(itemId);
  runtimeState.inventory = [...items].sort().join(",");
}

function removeInventoryItem(runtimeState, itemId) {
  if (!itemId) {
    return;
  }

  const items = splitInventory(runtimeState.inventory);
  items.delete(itemId);
  runtimeState.inventory = [...items].sort().join(",");
}

function splitInventory(inventory) {
  return new Set(String(inventory || "").split(",").map(item => item.trim()).filter(Boolean));
}

function deterministicChance(parameters, runtimeState) {
  const percent = Math.max(0, Math.min(100, toNumber(parameters.percent, 0)));
  const text = `${runtimeState.random_seed || "0"}:${parameters.salt || "default"}`;
  let hash = 0;
  for (const char of text) {
    hash = ((hash << 5) - hash + char.charCodeAt(0)) | 0;
  }

  return Math.abs(hash) % 100 < percent;
}

function toNumber(value, fallback) {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function formatNumber(value) {
  return Number.isInteger(value) ? String(value) : value.toFixed(2).replace(/\.?0+$/, "");
}
