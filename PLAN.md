# Build The Kubix MVP Desktop App, Easy Editor, Publish Flow, And Companion Website

This ExecPlan is a living document. The sections `Progress`, `Surprises & Discoveries`, `Decision Log`, and `Outcomes & Retrospective` must be kept up to date as work proceeds.

This document must be maintained in accordance with `.agent/PLANS.md`.

## Stage Gate Protocol

Development moves one approved stage at a time. At the end of each stage, Codex must run the available checks, fix critical issues discovered during those checks, update this plan, create a Git commit, and stop for user review. The user must explicitly accept the stage before the next stage begins.

Every stage should leave behind a rollback point in Git. A stage may contain several small commits when that makes review safer, but the stage is not considered complete until the checks and stage summary are recorded here.

Current stage: `Stage 1: Core Logic Kernel`.

## Purpose / Big Picture

Kubix should let a child install one desktop application, sign in, build a simple 3D map by dragging objects with the mouse, add simple gameplay logic with child-friendly blocks, test the map, publish it, and let another player find and play that map from a marketplace inside the same app. After that works, the project should add a companion website that shows the same catalog and account system but sends the user into the desktop app to actually play.

The first visible success is not "the architecture exists." The first visible success is this concrete loop: create a map, add one simple rule, click play, publish it, open a second account, and launch that published map from the app catalog. The website comes only after that loop works in the desktop product.

## Progress

- [x] (2026-04-23 23:48 +05:00) Product direction clarified: Unity-based desktop-first platform, easy editor first, website second, mobile later.
- [x] (2026-04-23 23:58 +05:00) Planning mistake corrected: replaced copied generic guidance with a repo-specific planning setup and started this project-specific ExecPlan.
- [x] (2026-04-24 00:33 +05:00) Created the repository skeleton for the Unity client, backend API, shared contracts, web app, docs, and tools.
- [x] (2026-04-24 00:39 +05:00) Added shared C# contract models for maps, transforms, easy logic graphs, publishing, room assignments, and sample map JSON.
- [x] (2026-04-24 00:48 +05:00) Added Unity-oriented C# source skeletons for app state, easy editor services, draft storage, playtest runtime, logic interpretation, publishing validation, and room assignment calls.
- [x] (2026-04-24 00:58 +05:00) Added a dependency-free Node backend skeleton with health, auth, publish, games, thumbnail, and room-assignment endpoints plus JSON-file storage.
- [x] (2026-04-24 01:09 +05:00) Added a static web marketplace/auth/download shell that consumes the planned API shape.
- [x] (2026-04-24 01:34 +05:00) Expanded the backend domain model with profile routes, stable author identifiers in catalog summaries, and reusable publish/room creation helpers.
- [x] (2026-04-24 01:40 +05:00) Added web profile pages, a local static web dev server, and API-facing scripts for profile browsing.
- [x] (2026-04-24 01:47 +05:00) Added Unity-side map serialization helpers, editor map state, object catalog support, placed-object creation helpers, and HTTP clients for auth/catalog calls.
- [x] (2026-04-24 01:52 +05:00) Added offline Node tests for backend domain rules; verified 6 passing tests with `node --test services/api/tests/domain.test.js`.
- [x] (2026-04-24 02:03 +05:00) Added a first visible Unity editor sandbox path: runtime scene bootstrap, editable 3D ground/grid, placement controls, primitive factory, and a quick-start scene guide.
- [x] (2026-04-26) Stage 0: initialized local Git history, added a safe `.gitignore`, created baseline commit `00ff290`, renamed the primary branch to `main`, and created `codex/stage-1-logic-core`.
- [ ] (2026-04-26) Stage 0 remote follow-up: create the GitHub repository and add `origin` once GitHub credentials or tooling are available.
- [ ] Build the real Unity project shell with scenes, prefabs, login UI, home/catalog shell, and editor scene navigation inside the Unity Editor.
- [ ] Replace the static editor/runtime code skeleton with actual interactive 3D scenes, object prefabs, and in-editor manipulation.
- [ ] Validate and choose the long-term backend runtime: keep Node for MVP momentum or migrate the same route contract to ASP.NET Core once `.NET 8` is available locally.
- [ ] Add authoritative multiplayer room execution so published maps run on a server-controlled match process rather than trusting the client.
- [ ] Add account-backed map publishing from the Unity app and hook the web shell to a verified running local API.
- [ ] Harden the MVP with tests, limits, moderation hooks, crash recovery, and packaging for internal trials.

## Surprises & Discoveries

- Observation: The strongest product requirement is not rendering quality or even multiplayer scale; it is child-friendly creation. The editor must default to direct manipulation, not coordinate entry.
  Evidence: The product requirement explicitly rejects manual `X/Y/Z` placement as the primary editing experience and requires placement by dragging objects with the mouse.

- Observation: Splitting the product into separate player and studio applications too early would slow delivery without improving the first user-visible outcome.
  Evidence: The agreed product direction is one autonomous desktop app first, with playing, editing, auth, and catalog in the same product.

- Observation: User-authored logic must not be raw C# scripts. The safe path is a custom block language stored as data and executed by a controlled interpreter.
  Evidence: The editor must be understandable for children from roughly first grade, and the logic system must later support both Easy and Hard modes without exposing full programming syntax.

- Observation: The local machine used for this session does not have `dotnet` available in `PATH`.
  Evidence: `where.exe dotnet` returned no result, and `dotnet --info` failed because the command was not recognized.

- Observation: The local environment refused Node socket binding during verification, even for a simple HTTP server on port `5000`.
  Evidence: Running `node services/api/src/server.js` produced `listen UNKNOWN: unknown error ... :5000`.

- Observation: Backend business rules can still be validated productively in this environment using offline domain tests even when HTTP socket verification is blocked.
  Evidence: `node --test services/api/tests/domain.test.js` passed 6 tests covering publish validation, room assignment, profile shaping, and catalog summaries.

- Observation: The fastest way to make the editor visible is to generate a temporary sandbox scene at runtime from one bootstrap component, then replace parts of it with proper prefabs and UI later.
  Evidence: The new Unity-side bootstrap path only needs an empty scene plus `EditorSandboxBootstrap` to produce a ground plane, grid, camera, HUD, and object placement flow.

- Observation: No Git repository was initialized in the workspace before Stage 0.
  Evidence: Checking `C:\Users\nadob\OneDrive\Desktop\Kubix\.git` returned `NO_GIT`.

- Observation: GitHub CLI is not available in the current shell, so automatic GitHub repository creation may be blocked from this environment.
  Evidence: `gh --version` failed because `gh` is not recognized as a command.

- Observation: Stage 0 can complete locally, but the GitHub remote cannot be created from the current shell.
  Evidence: Local commit `00ff290` exists and branch `codex/stage-1-logic-core` was created; no `gh` command is available to create or authenticate a GitHub repository.

## Decision Log

- Decision: Use Unity as the core engine for the desktop client and for the first server-run game sessions.
  Rationale: Kubix needs stylized 3D, physics, tool-building, future mobile support, and a large amount of custom editor logic. Unity reduces implementation risk for those needs.
  Date/Author: 2026-04-23 / Codex

- Decision: Build the desktop app first, then the website, then polish both.
  Rationale: The main value of Kubix is creation plus play, which belongs in the desktop app. The website is important, but it is secondary until the publish-and-play loop works.
  Date/Author: 2026-04-23 / Codex

- Decision: The first desktop app is a 2-in-1 product that includes player, editor, auth, catalog, and publishing.
  Rationale: A single app minimizes scope and keeps the MVP focused on one working ecosystem instead of three disconnected products.
  Date/Author: 2026-04-23 / Codex

- Decision: The editor must use drag-and-drop placement with visual handles as the default interaction model.
  Rationale: Manual coordinates are too abstract for the target age range and would weaken the main differentiation of the product.
  Date/Author: 2026-04-23 / Codex

- Decision: Start with Easy mode only, but store maps and logic in a format that can later power Hard mode too.
  Rationale: The product needs a low entry barrier now, but the data model must not force a rewrite when the advanced editor arrives.
  Date/Author: 2026-04-23 / Codex

- Decision: Use an authoritative server for match state as soon as online play and publishing are introduced.
  Rationale: Money, health, victory, item state, and map-triggered gameplay cannot be trusted to arbitrary clients if players are sharing maps.
  Date/Author: 2026-04-23 / Codex

- Decision: Scaffold the backend in dependency-free Node.js immediately instead of waiting for `.NET 8` to appear in the environment.
  Rationale: The repository was empty and the user asked to maximize real progress in one pass. A Node backend keeps route contracts, storage shape, and marketplace flow moving forward now, while preserving the option to port the same API contract to ASP.NET Core later.
  Date/Author: 2026-04-24 / Codex

- Decision: Keep author links stable by exposing `authorId` in game summaries and details.
  Rationale: Display names are not reliable identifiers, and profile pages need durable routing that survives display-name changes.
  Date/Author: 2026-04-24 / Codex

- Decision: Use Unity Editor, not VSCode, as the place to view and iterate on the 3D editor scene.
  Rationale: VSCode is suitable for script authoring, but scene composition, play mode, camera behavior, lighting, and runtime interaction need Unity Editor.
  Date/Author: 2026-04-24 / Codex

- Decision: Treat Stage 0 as a hard gate before Stage 1 feature work.
  Rationale: The project is already large enough that continuing without Git history would make rollback and review unreliable.
  Date/Author: 2026-04-26 / Codex

## Outcomes & Retrospective

The repository is no longer only a plan. It now contains a real product skeleton: shared map and logic contracts, Unity-oriented C# source files for editor and runtime layers, a local backend API skeleton, a static website marketplace shell, and project runbooks. The main remaining gap is that the current machine session could not run Unity or `.NET`, and it refused local socket listening for the Node verification step. That means the next contributor should focus first on opening the real Unity project, validating the backend in a normal local environment, and replacing code skeletons with true interactive scenes and live data flows.

Stage 0 local safety is now in place. The project has a local Git repository, baseline commit `00ff290`, primary branch `main`, and working branch `codex/stage-1-logic-core`. GitHub remote setup remains open because no GitHub CLI or authenticated remote-creation path is available in the current shell.

## Context and Orientation

Treat this repository as an early-stage product repository. Do not assume the codebase already contains the Unity client, backend API, or website unless you can see those directories in the working tree. If some of the paths below already exist, use them instead of creating duplicates. If they do not exist, create them exactly as named here so future contributors can orient themselves quickly.

Kubix has four technical parts in the MVP:

The first part is the `desktop client`, a Unity application that the user installs on Windows. It must contain the home/catalog experience, login, the Easy map editor, local playtest, published game launch, and publishing UI.

The second part is the `backend API`, a server application that stores accounts, game metadata, published map versions, thumbnails, and room assignments. In this repository it should live under `services/api`.

The third part is the `shared contracts` library, which means the C# classes that define map data, logic graph data, catalog metadata, and API payloads shared across the Unity app and the backend. In this repository it should live under `shared/Kubix.Contracts`.

The fourth part is the `website`, a separate web application that shows the Kubix catalog, profiles, and auth pages, but does not run gameplay. In this repository it should live under `apps/web`.

The phrase `authoritative server` means that the server, not the player’s computer, decides important game facts such as health, money, win state, inventory, and map-triggered actions. In Kubix this matters because user-made maps will contain logic, and published logic cannot be trusted to run only on the client.

The phrase `room server` means one running game session for one published map or one private test session. Kubix does not need one giant world with all players together. It needs many independent map sessions, with some sessions more popular than others.

The phrase `Easy logic graph` means the child-friendly block-based logic model stored as plain data, not source code. A graph is simply a set of connected logic blocks such as "When player touches zone" and "Then open door." The same stored format must be usable by both the Unity client and the server-side runtime.

The repository should be organized like this unless an existing layout already serves the same purpose:

    /AGENTS.md
    /PLAN.md
    /.agent/PLANS.md
    /apps/client-unity
    /apps/web
    /services/api
    /shared/Kubix.Contracts
    /shared/Kubix.Contracts.Tests
    /services/api/Kubix.Api.Tests
    /docs

Within `apps/client-unity`, use a clear namespace and folder layout under `Assets/Kubix/`. Create these folders if they do not already exist:

    Assets/Kubix/Scripts/App
    Assets/Kubix/Scripts/Auth
    Assets/Kubix/Scripts/Catalog
    Assets/Kubix/Scripts/Editor
    Assets/Kubix/Scripts/Logic
    Assets/Kubix/Scripts/Runtime
    Assets/Kubix/Scripts/Networking
    Assets/Kubix/Scripts/Publishing
    Assets/Kubix/Scenes
    Assets/Kubix/Prefabs
    Assets/Kubix/Tests/EditMode
    Assets/Kubix/Tests/PlayMode

Within `services/api`, use an ASP.NET Core application named `Kubix.Api`. Within `shared/Kubix.Contracts`, use plain serializable C# classes with no Unity-specific dependencies.

## Milestones

### Milestone 1: Bootstrap the repository and prove the desktop shell can launch

At the end of this milestone, a novice should be able to clone the repository, create or open the Unity project, run the backend API locally, and open the desktop app into a stub home screen with navigation to Home, My Games, Editor, and Profile. This milestone matters because it creates the scaffolding every later feature needs, while still producing something observable.

Acceptance for this milestone is simple. Starting the API should return a health response, and starting the Unity client should show a non-empty app shell instead of an empty scene. Nothing needs to publish yet. The goal is a runnable skeleton.

### Milestone 2: Build the Easy map editor and local draft loop

At the end of this milestone, the user can open the Editor inside the desktop app, place primitive objects from a small library, drag them around with the mouse, rotate and scale them with visual handles, snap them to the grid, save a draft map locally, close the editor, reopen the draft, and launch a local playtest scene. This is the first true proof that Kubix is not just a launcher shell.

Acceptance is a simple user flow. The user creates a floor, a spawn point, a finish zone, and one obstacle, saves the draft, reopens it, and sees the same arrangement. The user then clicks playtest and spawns into that draft map.

### Milestone 3: Add the Easy logic graph and demonstrate one complete mechanic

At the end of this milestone, the user can attach Easy logic blocks to map objects or the map itself and create at least one complete mechanic without writing code. The required first mechanic is: when the player touches the finish zone, show a win message and end the match. The second required mechanic is: when the player touches a trap zone, take damage. These two examples prove both event-based and state-changing actions.

Acceptance is behavioral. A local playtest must show the trap dealing damage and the finish declaring victory. The logic must be stored as data, reloaded from the saved draft, and run again correctly after reopening the map.

### Milestone 4: Add online session execution with an authoritative room server

At the end of this milestone, the same Easy map and Easy logic can run in a networked session controlled by the server. The server decides spawn, damage, and win state. The Unity project should be able to build both a desktop client and a server/headless build for room sessions, or a compatible dedicated runtime path if the team chooses a separate executable layout later.

Acceptance is a local multi-process test. Start the backend API, start one room server, launch two client instances into the same published or test map, and prove that the damage and win state are consistent for both clients because the server is the source of truth.

### Milestone 5: Add accounts, publishing, catalog, and play-from-marketplace inside the app

At the end of this milestone, one user can register, sign in, publish a map with title, description, genre, visibility, and thumbnail, and a second user can sign in, see that map in the catalog, open the game details page, and start the game from the marketplace in the desktop app. This is the MVP milestone that turns a private tool into a platform.

Acceptance is the first complete Kubix loop: account A creates and publishes a map; account B logs in and launches that map from the in-app catalog.

### Milestone 6: Build the companion website

At the end of this milestone, the website shows the same catalog, profile pages, and game pages as the app, supports account auth, and clearly tells the user to open or install Kubix to play. The website must not attempt to run the game itself in the MVP.

Acceptance is a cross-surface flow. A user opens a game page in the browser, signs in if needed, sees the game metadata and author info, and clicks a button that either opens the installed Kubix app or takes them to the download page.

### Milestone 7: Harden the MVP for real testing

At the end of this milestone, the product is ready for closed internal trials or small pilot groups. That means limits exist on object counts and logic complexity, crashes are logged, thumbnails and metadata are moderated at a basic level, and the packaging/install flow is documented. This is where the system becomes safe enough to hand to real users without immediately breaking.

Acceptance is operational. The team can install the app on a fresh Windows machine, sign in, create and publish a small map, play it from another account, and inspect logs when something fails.

## Plan of Work

Start by creating the repository skeleton because every future contributor needs predictable paths. In the repository root, create `apps`, `services`, `shared`, and `docs` if they do not already exist. Under `services/api`, create an ASP.NET Core application `Kubix.Api`. Under `shared/Kubix.Contracts`, create a plain C# class library for shared data contracts. Under `apps/client-unity`, create a Unity project and organize all Kubix-specific code under `Assets/Kubix/`. Do not place long-term project code under random default sample folders.

In the backend, add the smallest useful server first: a health endpoint, account storage, map metadata storage, and file storage abstraction. Use ASP.NET Core with .NET 8. Use PostgreSQL for persistent metadata in development and production, with a connection string configured through environment variables or `appsettings.Development.json`. Do not start with Redis, event buses, or microservices. One API is enough for the MVP.

In `shared/Kubix.Contracts`, define the core data models before the editor grows. Create `MapDefinition`, `PlacedObject`, `TransformData`, `EasyLogicGraph`, `EasyNode`, `EasyEventNode`, `EasyConditionNode`, `EasyActionNode`, `PublishedGameSummary`, `PublishedGameDetails`, `PublishMapRequest`, `PublishMapResponse`, `AccountProfile`, and `RoomAssignment`. Keep these classes serializable to JSON and free of Unity-specific types. For example, use your own `Vector3Data` instead of `UnityEngine.Vector3`.

In the Unity client, build the app shell before building the editor. Create scenes named `BootstrapScene`, `HomeScene`, `EditorScene`, and `PlaytestScene` in `Assets/Kubix/Scenes`. `BootstrapScene` decides whether the user sees login or the home UI. `HomeScene` contains marketplace browsing and navigation. `EditorScene` contains the Easy editor. `PlaytestScene` loads a draft or published map into the player runtime.

Build the Easy editor as a direct-manipulation tool, not a form. In `Assets/Kubix/Scripts/Editor`, create a placement system that lets the user pick an object card from a small library, click or drag to place it, and then move, rotate, and scale it with gizmos. Make `snap to grid` on by default. Keep numeric transform input hidden or disabled in Easy mode. The first object library should include floor block, wall block, ramp, spawn point, finish zone, button, door, damage zone, coin, and teleport point.

Store editor state in `MapDefinition`. Every placed object must have a stable identifier so logic references survive save/load. Add local draft persistence before online publishing. Store drafts in a local app data folder in JSON so a novice can verify that saving actually writes data and loading actually restores it.

Build the Easy logic graph next. In `Assets/Kubix/Scripts/Logic`, implement a runtime that evaluates child-friendly logic blocks from data. The required first block families are `When`, `If`, `Then`, `Wait`, and `And`. Do not expose C# scripting. The first required events are map started, player touched zone, player pressed button, item picked up, timer elapsed, and finish reached. The first required conditions are has item, enough money, health above or below a value, object enabled, and random chance. The first required actions are create, delete, show, hide, move, teleport, open, close, give item, remove item, add money, remove money, damage, heal, show message, play sound, and end game.

Once local playtest works, add online execution. Keep the room server simple. Use the same core gameplay code in a server-run mode where the server owns match state. The initial network target is small sessions such as 2 to 16 players. Do not optimize for hundreds of players in one room in the MVP. In `Assets/Kubix/Scripts/Networking`, keep networking code separate from editor code. In `services/api`, add an endpoint that returns a `RoomAssignment` for a published map so the app knows where to connect.

After that, wire publishing and catalog flows. In the API, add endpoints for register, login, current profile, save published map metadata, upload thumbnail, list published games, and get published game details. In the Unity client, add UI under Home for browse, search, game card display, game details, and launch. The publish flow should validate the map before upload. At minimum, require a title, one spawn point, and a playable scene.

The website is last. Create a web app in `apps/web` using Next.js with TypeScript so it can render game detail pages and profile pages well. The website should consume the same backend API used by the client. It should never become the source of truth for game state. It is only a catalog, account, and growth surface in the MVP.

Throughout all milestones, keep Easy mode and future Hard mode separate in the UI but unified in the data model. The Easy editor must write data that a future Hard editor can read without conversion loss. If you need fields that Easy mode does not expose yet, keep them in the data contracts but leave them unused until the advanced editor exists.

## Concrete Steps

Run all commands from the repository root `C:\Users\nadob\OneDrive\Desktop\Kubix` unless a step says otherwise.

Before creating code, ensure the development machine has these tools installed:

- Unity LTS through Unity Hub
- .NET 8 SDK
- Node.js 22 LTS
- PostgreSQL 16 or another team-approved local PostgreSQL version
- Git

Create the backend and shared contracts scaffolding:

    dotnet new sln -n Kubix
    dotnet new webapi -n Kubix.Api -o services/api/Kubix.Api
    dotnet new classlib -n Kubix.Contracts -o shared/Kubix.Contracts
    dotnet new xunit -n Kubix.Contracts.Tests -o shared/Kubix.Contracts.Tests
    dotnet new xunit -n Kubix.Api.Tests -o services/api/Kubix.Api.Tests
    dotnet sln Kubix.sln add services/api/Kubix.Api/Kubix.Api.csproj
    dotnet sln Kubix.sln add shared/Kubix.Contracts/Kubix.Contracts.csproj
    dotnet sln Kubix.sln add shared/Kubix.Contracts.Tests/Kubix.Contracts.Tests.csproj
    dotnet sln Kubix.sln add services/api/Kubix.Api.Tests/Kubix.Api.Tests.csproj
    dotnet add services/api/Kubix.Api/Kubix.Api.csproj reference shared/Kubix.Contracts/Kubix.Contracts.csproj
    dotnet add shared/Kubix.Contracts.Tests/Kubix.Contracts.Tests.csproj reference shared/Kubix.Contracts/Kubix.Contracts.csproj
    dotnet add services/api/Kubix.Api.Tests/Kubix.Api.Tests.csproj reference services/api/Kubix.Api/Kubix.Api.csproj

Create the website only after Milestone 5 begins:

    npx create-next-app@latest apps/web --ts --eslint --app --src-dir --use-npm --no-tailwind

Create the Unity project in `apps/client-unity` using Unity Hub. Use a 3D template and name the project `Kubix.Client`. After the project opens, create the `Assets/Kubix` folder structure described earlier.

After the backend scaffold exists, verify it runs:

    dotnet run --project services/api/Kubix.Api/Kubix.Api.csproj

Expected early success output will look similar to this:

    info: Microsoft.Hosting.Lifetime[14]
          Now listening on: http://localhost:5000
    info: Microsoft.Hosting.Lifetime[0]
          Application started. Press Ctrl+C to shut down.

Add a health endpoint and verify it manually:

    curl http://localhost:5000/health

Expected response:

    OK

After Unity scenes exist, open `BootstrapScene` in the editor and press Play. The expected result for the first shell milestone is that the app shows a title, placeholder nav, and a button that switches to the editor scene without throwing errors in the Console.

After local draft save/load is implemented, test it by creating a map with one floor block, one spawn point, and one finish zone. Save the draft, close Play Mode or the app, reopen the draft, and confirm the object positions and identifiers remain stable.

After Easy logic is implemented, run the local playtest and verify this scenario:

    1. Spawn on the draft map.
    2. Walk into the damage zone.
    3. Observe health decrease.
    4. Walk into the finish zone.
    5. Observe the win message and match end state.

After authoritative multiplayer is implemented, run one server process and two client processes. Both clients should see the same authoritative result when either player touches a trap or finish zone. A client must not be able to declare victory only on its own machine.

## Validation and Acceptance

The MVP is accepted only when a human can perform the full loop below without editing code during the test:

Start the backend API. Start the room-server runtime or server mode. Launch the Kubix desktop app. Register or log in as user A. Open the editor. Build a simple 3D map by dragging objects with the mouse. Add at least one trap and one finish rule using Easy logic blocks. Run local playtest and prove both mechanics work. Publish the map with title, description, and thumbnail. Sign out. Sign in as user B. Open the in-app marketplace. Find the map by title. Open the game details page. Click Play. Join the room. Reach the finish and see a server-authoritative win. Then open the website, find the same game page, and confirm the site shows the game and prompts the user to open or install Kubix instead of trying to run gameplay in the browser.

Along the way, these automated checks must exist and pass:

- `dotnet test Kubix.sln` must pass for contract and API tests.
- Unity Edit Mode tests must pass for map serialization and Easy logic evaluation.
- Unity Play Mode tests must pass for at least one local playtest scenario.
- API integration tests must prove register, login, publish, list, and details flows.

Before the implementation, the named tests should not exist or should fail. After implementation, they must pass and directly prove the new behavior.

## Idempotence and Recovery

These steps should be safe to repeat if done carefully. Scaffolding commands are only safe to rerun before custom code is added. After files exist, do not rerun `dotnet new` or `create-next-app` into the same directories; instead, open and edit the generated projects. If a scaffold command was run in the wrong place, delete the mistakenly created directory before any custom work begins and rerun it in the correct path.

Keep migrations additive. If database schema changes are introduced later, create forward-only migrations and keep a seed path for local development. Do not hand-edit a live database as the primary workflow.

For Unity, commit scenes and prefabs frequently because scene merges are fragile. If a scene becomes corrupted, restore it from Git rather than manually trying to repair YAML by guesswork.

For local draft maps, store them under a dedicated Kubix app data directory and keep a simple export-to-JSON option so drafts can be backed up before risky serializer changes.

## Artifacts and Notes

The first meaningful evidence to capture during implementation should include these small artifacts:

    A screenshot of the desktop app shell showing Home, My Games, Editor, and Profile.

    A JSON excerpt from a saved draft map showing object identifiers, transform data, and an Easy logic graph.

    A short API response example for a published game card:

        {
          "id": "game_123",
          "title": "My First Obby",
          "authorName": "KidBuilder",
          "thumbnailUrl": "/media/game_123.png",
          "genre": "Obby"
        }

    A short room assignment example:

        {
          "gameId": "game_123",
          "host": "127.0.0.1",
          "port": 7777,
          "roomId": "room_456"
        }

    A short Easy logic example in plain data form:

        When: player_touched_finish
        Then: show_message("You win!")
        Then: end_game

As implementation progresses, replace these placeholders with actual excerpts from the repository and test output.

## Interfaces and Dependencies

Use these dependencies unless a later validated discovery proves they block the MVP:

- Unity for the desktop client and first room-server runtime.
- ASP.NET Core on .NET 8 for the backend API.
- PostgreSQL for persistent metadata.
- JSON as the first serialization format for maps, logic graphs, and API payloads.
- Next.js with TypeScript for the website.

In `shared/Kubix.Contracts`, define these data types or their direct equivalents:

    namespace Kubix.Contracts.Maps;

    public sealed record Vector3Data(float X, float Y, float Z);
    public sealed record RotationData(float X, float Y, float Z);
    public sealed record TransformData(Vector3Data Position, RotationData Rotation, Vector3Data Scale);

    public sealed record PlacedObject(
        string ObjectId,
        string ObjectType,
        TransformData Transform,
        Dictionary<string, string> Properties
    );

    public sealed record MapDefinition(
        string MapId,
        string Title,
        List<PlacedObject> Objects,
        EasyLogicGraph Logic
    );

In `shared/Kubix.Contracts.Logic`, define these types or their direct equivalents:

    public sealed record EasyLogicGraph(List<EasyNode> Nodes, List<EasyEdge> Edges);
    public abstract record EasyNode(string NodeId, string NodeType);
    public sealed record EasyEventNode(string NodeId, string EventType, Dictionary<string, string> Parameters) : EasyNode(NodeId, "event");
    public sealed record EasyConditionNode(string NodeId, string ConditionType, Dictionary<string, string> Parameters) : EasyNode(NodeId, "condition");
    public sealed record EasyActionNode(string NodeId, string ActionType, Dictionary<string, string> Parameters) : EasyNode(NodeId, "action");
    public sealed record EasyEdge(string FromNodeId, string ToNodeId, string EdgeType);

In `shared/Kubix.Contracts.Catalog`, define these types or their direct equivalents:

    public sealed record PublishedGameSummary(
        string GameId,
        string Title,
        string AuthorName,
        string ThumbnailUrl,
        string Genre
    );

    public sealed record PublishedGameDetails(
        string GameId,
        string Title,
        string Description,
        string AuthorName,
        string ThumbnailUrl,
        string Genre,
        string CurrentVersionId
    );

    public sealed record RoomAssignment(
        string RoomId,
        string Host,
        int Port,
        string GameId,
        string VersionId
    );

In `apps/client-unity/Assets/Kubix/Scripts/Editor`, create or keep equivalent classes for:

    PlacementTool
    SelectionController
    TransformGizmoController
    GridSnapService
    DraftMapRepository
    EditorSceneBootstrap

In `apps/client-unity/Assets/Kubix/Scripts/Logic`, create or keep equivalent classes for:

    EasyLogicGraphViewModel
    EasyLogicInterpreter
    EventDispatchService
    ConditionEvaluator
    ActionExecutor

In `apps/client-unity/Assets/Kubix/Scripts/Runtime`, create or keep equivalent classes for:

    DraftPlaytestLoader
    PublishedMapLoader
    PlayerSpawnService
    MatchStateController

In `services/api/Kubix.Api`, add endpoints or equivalent controllers for:

    GET /health
    POST /auth/register
    POST /auth/login
    GET /me
    POST /maps/publish
    GET /games
    GET /games/{gameId}
    POST /games/{gameId}/thumbnail
    POST /rooms/assign

In `apps/web`, create these top-level routes or equivalents:

    /
    /games
    /games/[gameId]
    /profiles/[profileId]
    /login
    /register
    /download

## Revision Note

This plan was created on 2026-04-23 because the repository needed a real, project-specific execution plan for Kubix rather than a copied generic ExecPlan template. It captures the agreed product direction from the current discussion and should now be treated as the primary living plan for the MVP.
