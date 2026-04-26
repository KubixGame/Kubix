import test from "node:test";
import assert from "node:assert/strict";
import { createPublishedGame, createRoomAssignment, mapSummary, toProfile, validatePublishRequest } from "../src/domain.js";

test("validatePublishRequest rejects maps without spawn", () => {
  const result = validatePublishRequest({
    title: "Broken Map",
    map: {
      objects: [
        { objectType: "finish_zone" }
      ]
    }
  });

  assert.equal(result.ok, false);
  assert.equal(result.error, "at least one spawn_point is required.");
});

test("validatePublishRequest rejects maps without finish", () => {
  const result = validatePublishRequest({
    title: "Broken Map",
    map: {
      objects: [
        { objectType: "spawn_point" }
      ]
    }
  });

  assert.equal(result.ok, false);
  assert.equal(result.error, "at least one finish_zone is required.");
});

test("createPublishedGame creates versioned public game", () => {
  const result = createPublishedGame(
    {
      title: "First Obby",
      description: "demo",
      genre: "Obby",
      visibility: "public",
      map: {
        title: "First Obby",
        objects: [
          { objectType: "spawn_point" },
          { objectType: "finish_zone" }
        ]
      }
    },
    {
      userId: "user_1",
      displayName: "Kid Builder"
    }
  );

  assert.equal(result.ok, true);
  assert.match(result.value.game.gameId, /^game_/);
  assert.match(result.value.game.currentVersionId, /^version_/);
  assert.equal(result.value.game.authorId, "user_1");
  assert.equal(result.value.game.authorName, "Kid Builder");
});

test("mapSummary includes stable author id", () => {
  const summary = mapSummary({
    gameId: "game_1",
    title: "Game",
    authorId: "user_123",
    authorName: "Builder",
    thumbnailUrl: "/thumb.png",
    genre: "Adventure"
  });

  assert.deepEqual(summary, {
    gameId: "game_1",
    title: "Game",
    authorId: "user_123",
    authorName: "Builder",
    thumbnailUrl: "/thumb.png",
    genre: "Adventure"
  });
});

test("toProfile strips password-shaped data", () => {
  const profile = toProfile({
    userId: "user_1",
    username: "builder",
    displayName: "Builder",
    password: "secret"
  });

  assert.deepEqual(profile, {
    userId: "user_1",
    username: "builder",
    displayName: "Builder"
  });
});

test("createRoomAssignment uses provided game and version ids", () => {
  const room = createRoomAssignment(
    {
      gameId: "game_5",
      versionId: "version_3"
    },
    "127.0.0.1"
  );

  assert.equal(room.gameId, "game_5");
  assert.equal(room.versionId, "version_3");
  assert.equal(room.port, 7777);
  assert.match(room.roomId, /^room_/);
});
