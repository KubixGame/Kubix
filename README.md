# Kubix

Kubix is a kid-friendly 3D UGC game platform inspired by Roblox. The current repository contains the first implementation skeleton for:

- a Unity-oriented desktop client structure
- a shared map and logic contract layer
- a local backend API skeleton
- a companion website marketplace shell

## Current state

This repository is not yet a fully runnable product. The codebase now contains:

- `apps/client-unity/Assets/Kubix/...` source folders and foundational C# scripts for editor, logic, runtime, publishing, and networking
- `shared/Kubix.Contracts/...` C# contract models for maps, logic graphs, and catalog data
- `services/api/src/server.js` as a dependency-free local API skeleton with auth, publish, catalog, and room-assignment endpoints
- `apps/web/...` as a static marketplace/auth shell wired to the API routes

## Important environment note

The current machine session used to scaffold this repo does not have `dotnet` in `PATH`, and the local environment refused socket listening for Node during verification. Because of that, the backend was scaffolded in plain Node.js to keep development moving and the API could not be fully smoke-tested over HTTP in this session.

## Suggested next local steps

1. Install `.NET 8 SDK`.
2. Install Unity LTS and create/open the real Unity project in `apps/client-unity`.
3. Open the C# files in `apps/client-unity/Assets/Kubix/Scripts`.
4. Decide whether to keep the initial API on Node or migrate it to ASP.NET Core as planned in `PLAN.md`.
5. Wire the Unity project to the shared contract model and begin scene/prefab creation.

## Key files

- `PLAN.md`
- `AGENTS.md`
- `.agent/PLANS.md`
- `shared/Kubix.Contracts`
- `apps/client-unity/Assets/Kubix/Scripts`
- `services/api/src/server.js`
- `apps/web/index.html`
