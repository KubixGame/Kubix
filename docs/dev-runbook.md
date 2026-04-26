# Kubix Development Runbook

## What exists right now

The repository currently contains:

- Unity-oriented C# source under `apps/client-unity/Assets/Kubix/Scripts`
- shared contracts under `shared/Kubix.Contracts`
- a local API skeleton under `services/api/src`
- a static website shell under `apps/web`

## How to continue the work

### Unity work

1. Install Unity LTS.
2. Create or open the Unity project in `apps/client-unity`.
3. Keep all project code inside `Assets/Kubix`.
4. Create the first scenes:
   - `BootstrapScene`
   - `HomeScene`
   - `EditorScene`
   - `PlaytestScene`
5. Create primitive prefabs for:
   - `floor_block`
   - `wall_block`
   - `spawn_point`
   - `finish_zone`
   - `damage_zone`
   - `button`
   - `door`
   - `coin`
   - `teleport_point`

### API work

The current API is in `services/api/src/server.js`.

Routes already scaffolded:

- `GET /health`
- `POST /auth/register`
- `POST /auth/login`
- `GET /me`
- `POST /maps/publish`
- `GET /games`
- `GET /games/{gameId}`
- `POST /games/{gameId}/thumbnail`
- `POST /rooms/assign`

Persistent local data is stored in:

- `services/api/data/store.json`

If that file does not exist, it is created from:

- `services/api/data/store.template.json`

### Web work

The website currently is a static shell under `apps/web`.

Pages already scaffolded:

- `index.html`
- `games.html`
- `game.html`
- `login.html`
- `register.html`
- `download.html`

JavaScript entry files are in:

- `apps/web/scripts`

## Current technical blockers from this session

- `dotnet` was not installed in `PATH`
- Unity was not available to validate scenes
- local Node HTTP binding failed in this environment, so backend verification was limited to code-level scaffolding

## First recommended next tasks

1. Open the Unity project and create the four scenes.
2. Wire `PlacementTool`, `SelectionController`, and `TransformGizmoController` to real prefabs.
3. Make `DraftMapRepository` save real `MapDefinition` JSON.
4. Run the Node API in a normal local shell and verify `/health` and `/games`.
5. Hook `apps/web/scripts/api.js` to the running API.
