import { createId } from "./helpers.js";

export function mapSummary(game) {
  return {
    gameId: game.gameId,
    title: game.title,
    authorId: game.authorId,
    authorName: game.authorName,
    thumbnailUrl: game.thumbnailUrl,
    genre: game.genre
  };
}

export function resolveCurrentUser(request, store, getBearerToken) {
  const token = getBearerToken(request);
  if (!token) {
    return null;
  }

  const userId = store.tokens[token];
  if (!userId) {
    return null;
  }

  return store.users.find(user => user.userId === userId) || null;
}

export function toProfile(user) {
  return {
    userId: user.userId,
    username: user.username,
    displayName: user.displayName
  };
}

export function validatePublishRequest(body) {
  const map = body.map || {};
  const title = String(body.title || map.title || "").trim();
  const description = String(body.description || "").trim();
  const hasSpawn = Array.isArray(map.objects) && map.objects.some(objectDefinition => objectDefinition.objectType === "spawn_point");
  const hasFinish = Array.isArray(map.objects) && map.objects.some(objectDefinition => objectDefinition.objectType === "finish_zone");

  if (!title) {
    return { ok: false, error: "title is required." };
  }

  if (!hasSpawn) {
    return { ok: false, error: "at least one spawn_point is required." };
  }

  if (!hasFinish) {
    return { ok: false, error: "at least one finish_zone is required." };
  }

  return {
    ok: true,
    value: {
      title,
      description,
      map,
      genre: String(body.genre || "Adventure"),
      visibility: String(body.visibility || "public"),
      thumbnailUrl: String(body.thumbnailUrl || "/media/default-game.png")
    }
  };
}

export function createPublishedGame(body, user) {
  const validation = validatePublishRequest(body);
  if (!validation.ok) {
    return validation;
  }

  const gameId = createId("game");
  const versionId = createId("version");

  return {
    ok: true,
    value: {
      game: {
        gameId,
        title: validation.value.title,
        description: validation.value.description,
        authorId: user.userId,
        authorName: user.displayName,
        thumbnailUrl: validation.value.thumbnailUrl,
        genre: validation.value.genre,
        visibility: validation.value.visibility,
        currentVersionId: versionId,
        map: validation.value.map
      },
      response: {
        gameId,
        versionId,
        message: "Map published successfully."
      }
    }
  };
}

export function createRoomAssignment(body, host) {
  return {
    roomId: createId("room"),
    host,
    port: 7777,
    gameId: String(body.gameId || "").trim(),
    versionId: String(body.versionId || "version_local").trim()
  };
}
