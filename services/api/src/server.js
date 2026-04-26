import http from "node:http";
import { createId, getBearerToken, json, readJsonBody, text } from "./helpers.js";
import { readStore, writeStore } from "./store.js";
import {
  createPublishedGame,
  createRoomAssignment,
  mapSummary,
  resolveCurrentUser,
  toProfile
} from "./domain.js";

const port = Number(process.env.PORT || 5000);
const host = process.env.HOST || "0.0.0.0";

const server = http.createServer(async (request, response) => {
  const url = new URL(request.url || "/", `http://${request.headers.host || `${host}:${port}`}`);

  if (request.method === "OPTIONS") {
    response.writeHead(204, {
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Headers": "Content-Type, Authorization",
      "Access-Control-Allow-Methods": "GET, POST, OPTIONS"
    });
    response.end();
    return;
  }

  if (request.method === "GET" && url.pathname === "/health") {
    text(response, 200, "OK");
    return;
  }

  if (request.method === "POST" && url.pathname === "/auth/register") {
    const store = readStore();
    const body = await readJsonBody(request);
    const username = String(body.username || "").trim().toLowerCase();
    const password = String(body.password || "");
    const displayName = String(body.displayName || body.username || "").trim();

    if (!username || !password || !displayName) {
      json(response, 400, { error: "username, displayName, and password are required." });
      return;
    }

    if (store.users.some(user => user.username === username)) {
      json(response, 409, { error: "username already exists." });
      return;
    }

    const user = {
      userId: createId("user"),
      username,
      displayName,
      password
    };

    const token = createId("token");
    store.users.push(user);
    store.tokens[token] = user.userId;
    writeStore(store);

    json(response, 201, {
      token,
      profile: toProfile(user)
    });
    return;
  }

  if (request.method === "POST" && url.pathname === "/auth/login") {
    const store = readStore();
    const body = await readJsonBody(request);
    const username = String(body.username || "").trim().toLowerCase();
    const password = String(body.password || "");
    const user = store.users.find(candidate => candidate.username === username && candidate.password === password);

    if (!user) {
      json(response, 401, { error: "invalid credentials." });
      return;
    }

    const token = createId("token");
    store.tokens[token] = user.userId;
    writeStore(store);

    json(response, 200, {
      token,
      profile: toProfile(user)
    });
    return;
  }

  if (request.method === "GET" && url.pathname === "/me") {
    const store = readStore();
    const user = resolveCurrentUser(request, store, getBearerToken);
    if (!user) {
      json(response, 401, { error: "unauthorized." });
      return;
    }

    json(response, 200, toProfile(user));
    return;
  }

  if (request.method === "GET" && url.pathname.startsWith("/profiles/")) {
    const store = readStore();
    const segments = url.pathname.split("/").filter(Boolean);
    const profileId = segments[1];
    const user = store.users.find(candidate => candidate.userId === profileId || candidate.username === profileId);
    if (!user) {
      json(response, 404, { error: "profile not found." });
      return;
    }

    const authoredGames = store.games
      .filter(game => game.authorId === user.userId && game.visibility === "public")
      .map(mapSummary);

    json(response, 200, {
      ...toProfile(user),
      publishedGames: authoredGames
    });
    return;
  }

  if (request.method === "GET" && url.pathname === "/games") {
    const store = readStore();
    const authorId = url.searchParams.get("authorId");
    const genre = url.searchParams.get("genre");
    let games = store.games.filter(game => game.visibility === "public");

    if (authorId) {
      games = games.filter(game => game.authorId === authorId);
    }

    if (genre) {
      games = games.filter(game => game.genre.toLowerCase() === genre.toLowerCase());
    }

    json(response, 200, games.map(mapSummary));
    return;
  }

  if (request.method === "GET" && url.pathname.startsWith("/games/")) {
    const store = readStore();
    const segments = url.pathname.split("/").filter(Boolean);
    const gameId = segments[1];
    const game = store.games.find(candidate => candidate.gameId === gameId);
    if (!game) {
      json(response, 404, { error: "game not found." });
      return;
    }

    json(response, 200, game);
    return;
  }

  if (request.method === "POST" && url.pathname === "/maps/publish") {
    const store = readStore();
    const user = resolveCurrentUser(request, store, getBearerToken);
    if (!user) {
      json(response, 401, { error: "unauthorized." });
      return;
    }

    const body = await readJsonBody(request);
    const creation = createPublishedGame(body, user);
    if (!creation.ok) {
      json(response, 400, { error: creation.error });
      return;
    }

    store.games.unshift(creation.value.game);
    writeStore(store);
    json(response, 201, creation.value.response);
    return;
  }

  if (request.method === "POST" && /\/games\/[^/]+\/thumbnail$/.test(url.pathname)) {
    const store = readStore();
    const user = resolveCurrentUser(request, store, getBearerToken);
    if (!user) {
      json(response, 401, { error: "unauthorized." });
      return;
    }

    const segments = url.pathname.split("/").filter(Boolean);
    const gameId = segments[1];
    const game = store.games.find(candidate => candidate.gameId === gameId);
    if (!game) {
      json(response, 404, { error: "game not found." });
      return;
    }

    if (game.authorId !== user.userId) {
      json(response, 403, { error: "forbidden." });
      return;
    }

    const body = await readJsonBody(request);
    game.thumbnailUrl = String(body.thumbnailUrl || game.thumbnailUrl);
    writeStore(store);
    json(response, 200, { gameId, thumbnailUrl: game.thumbnailUrl });
    return;
  }

  if (request.method === "POST" && url.pathname === "/rooms/assign") {
    const store = readStore();
    const body = await readJsonBody(request);
    const room = createRoomAssignment(body, host);

    store.rooms.push(room);
    writeStore(store);
    json(response, 200, room);
    return;
  }

  json(response, 404, { error: "route not found." });
});

server.listen(port, host, () => {
  console.log(`Kubix API listening on http://${host}:${port}`);
});
