# AGENTS.md

## Project

`Kubix` is a kid-friendly UGC 3D multiplayer platform inspired by Roblox, but with a much lower creation barrier.

Core idea:
- Players can create maps/games.
- Players can publish maps to a shared public catalog.
- Other players can browse and play published maps.
- The main product advantage is a **very accessible map + logic editor** for children, starting from roughly `1st grade`.

This is **not** a pixel/cube-only game. Visual target is closer to stylized Roblox-like 3D than to Minecraft.

## Current Product Direction

Unless the user explicitly changes direction, assume the following decisions are currently locked:

- `Unity` is the default engine direction.
- Build `desktop app first`, then build the website, then polish both.
- The first desktop app is `2-in-1`:
  - player
  - editor/studio
  - game catalog / marketplace
  - auth
  - publishing
- The `website` is a companion product, not the main runtime:
  - account registration / login
  - marketplace/catalog
  - game pages
  - profile pages
  - prompts users to open/install the app to play
- `Mobile app` comes later.

Do **not** prematurely split the desktop product into separate player/studio apps unless the user asks for it again later.

## Editor UX Rules

The editor must prioritize child-friendly direct manipulation over technical precision.

Required defaults:
- Create objects from a library/palette.
- Place objects visually on the canvas/scene.
- Move objects by dragging with the mouse.
- Rotate and scale objects with visual gizmos/handles.
- Keep `snap to grid` enabled by default.

Important:
- `X/Y/Z` manual coordinate entry is **not** the main creation flow.
- Manual numeric transforms are allowed only as an advanced option in the harder mode.

If a proposed feature makes the editor feel closer to a traditional dev tool than a child-friendly builder, challenge it.

## Audience

Primary audiences:
- children
- schools
- private learning centers
- later B2B / B2G buyers

Design implication:
- low cognitive load matters as much as raw power
- learning progression is a product feature, not just an onboarding detail

## Mode System

The product should use **one underlying data model** with different editor profiles.

Planned modes:
- `Easy` mode first
- `Hard` mode later
- `Medium` mode may be added later as a bridge

Rules:
- Easy mode must be a strict subset of the full system.
- Anything made in Easy mode must open and work in Hard mode without conversion loss.
- Hard-mode-only content can be view-only or partially locked when opened in Easy mode.

## MVP Goal

The MVP is a working vertical slice, not a concept demo.

A valid MVP flow is:
1. user registers / logs in
2. user opens the desktop app
3. user creates a simple 3D map
4. user adds simple gameplay logic
5. user playtests the map
6. user publishes the map
7. another user finds it in the catalog and plays it

If work does not push this loop forward, it is probably not MVP-critical.

## MVP Scope

### Desktop app
- login / registration
- home marketplace/catalog
- search and browse games
- game detail page
- launch and play published games
- create/edit map
- simple logic editor
- local playtest
- publish/update map
- list of user-created maps

### Website
- registration / login
- marketplace/catalog
- game pages
- profiles
- CTA to open/install the desktop app

No browser gameplay in MVP.

## Editor Scope

### Easy map editor
Keep only the minimum needed to build primitive but fun maps:
- blocks / primitive objects
- template objects from a library
- spawn point
- finish point
- zone / hitbox
- button / trigger
- door / gate
- collectible / coin
- teleport
- simple items/tools
- color/material presets

Avoid in Easy mode:
- raw coordinate editing as primary UX
- advanced physics tuning
- layers/tags/collision masks
- object hierarchies exposed like a pro engine editor
- custom model import
- complex lighting controls

### Easy logic editor
Start with these block families only:
- `When`
- `If`
- `Then`
- `Wait`
- simple `And`

Easy-mode events:
- map started
- player touched object/zone
- player clicked/pressed button
- player picked up item
- time passed
- player reached finish

Easy-mode conditions:
- player has item
- player has enough money
- player health greater/less than value
- object is enabled/disabled
- random chance

Easy-mode actions:
- create object
- delete object
- show/hide object
- move object
- teleport player
- open/close door
- give/remove item
- add/remove money
- damage/heal player
- show message
- play sound
- end game / declare winner

Easy mode should be enough for:
- obby
- race
- trap maps
- collectathon
- key + door puzzle
- simple tycoon loop
- checkpoint flow

### Hard mode later
Hard mode can add:
- variables
- else branches
- loops
- functions
- lists/arrays
- teams
- rounds/lobbies
- scoreboards
- UI logic
- persistence/save data
- NPCs/pathfinding
- tags/groups/layers
- advanced transforms / exact numeric editing
- advanced multiplayer/game-state logic

## Technical Direction

Default architecture assumption:
- desktop Unity client
- backend API
- database
- asset/map storage
- separate room/match servers for live game sessions

Multiplayer assumptions:
- authoritative server for important gameplay state
- players are distributed across many maps/instances, not one giant shared world
- do not optimize for 1000 players in one room for MVP

Map/runtime assumptions:
- versioned map format
- versioned logic format
- hard limits on object counts and logic complexity
- protection against infinite loops/spammy events
- cooldown/debounce support

## Product Principles

- Accessibility beats raw editor power in early versions.
- Do not overengineer the first release.
- Prefer templates/prefabs/recipes over exposing low-level systems.
- Use child-friendly wording in UX instead of engine jargon.
- Do not expose raw programming concepts too early if a simpler mental model works.
- The platform is both a game product and a learning product.

## Non-Goals For MVP

Do not expand MVP into these unless the user asks:
- separate player app and studio app
- browser-based gameplay runtime
- full Roblox-scale creator tooling
- unrestricted scripting
- advanced AI/NPC systems
- custom model import pipeline
- huge-scale single-room networking
- polished enterprise B2B/B2G admin suite before the core loop works

## Roblox References

Roblox may be used as UX and product inspiration.

Do **not** assume Roblox source code is available for reuse.
Use public Roblox references only as inspiration for:
- marketplace layout
- creator workflows
- terminology comparisons
- product benchmarking

## Working Conventions For Agents

- Preserve the current product direction unless the user changes it.
- Keep recommendations grounded in MVP delivery, not idealized platform design.
- If proposing new systems, explain why they are needed now rather than later.
- Favor one coherent architecture over multiple optional branches unless the user asks for a comparison.
- When in doubt, prefer direct manipulation UX over parameter-heavy forms.

## ExecPlans

When writing complex features or significant refactors, use an `ExecPlan` from design through implementation.

Rules:
- The authoritative local conventions live in `.agent/PLANS.md`.
- The primary project-level living ExecPlan is `PLAN.md`.
- Read `.agent/PLANS.md` in full before authoring or revising an ExecPlan.
- If `.agent/PLANS.md` is ever missing, recreate behavior consistent with its latest checked-in guidance and still create and maintain an ExecPlan in the most appropriate work artifact for the task.
- ExecPlans are expected for major features, architectural changes, or non-trivial refactors.
- Do not jump straight into large implementation work without a plan.
