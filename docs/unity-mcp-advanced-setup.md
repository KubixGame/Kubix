# Unity MCP Advanced Setup

This repository now includes a project-scoped Codex MCP configuration in `.codex/config.toml`.

What was added:

- Codex will start an MCP server named `unity_mcp` for this project.
- The server is bootstrapped by `tools/install-unity-mcp-advanced.ps1`.
- On first start, that script downloads or clones `HuntNight/unity-mcp-advanced` into `.external/unity-mcp-advanced`.
- The script then runs `npm install` in `.external/unity-mcp-advanced/unity-mcp`.
- After bootstrap, Codex starts the MCP server from `.external/unity-mcp-advanced/unity-mcp/index.js`.

This setup is intentionally project-scoped, not global. It affects this repository only.

## What still must be done inside Unity

The Node MCP server is only one half of the integration. `unity-mcp-advanced` also needs a Unity Editor package installed inside the Unity project so the server can talk to the editor over HTTP.

When the Unity client project exists, install the package from this local path:

    .external/unity-mcp-advanced/unity-mcp/tools/unity-bridge/unity-extension/package.json

The upstream repository describes the Unity-side flow like this:

- install Node.js 18+
- run `npm install` in `unity-mcp/`
- install the Unity package from `unity-mcp/tools/unity-bridge/unity-extension/package.json`
- open `Window -> Unity Bridge` in Unity
- start the bridge server

## Expected verification flow

After the Unity package is installed and the Unity Bridge window is running:

1. Open this repository in Codex.
2. Let the `unity_mcp` MCP server start.
3. Open the Unity project.
4. In Unity, open `Window -> Unity Bridge`.
5. Start the bridge server.
6. Ask Codex to call `unity_health`.

Expected result:

- `unity_health` should return a successful bridge diagnostic instead of a connection error.

## Known limitation right now

This repository does not yet contain the Unity project described in `PLAN.md`, so the Unity package cannot be installed here yet. The MCP bootstrap is in place now so the integration work does not have to be repeated later.

## Upstream source

- Repository: `https://github.com/HuntNight/unity-mcp-advanced`
- README summary used here: the project provides a Node MCP server at `unity-mcp/index.js` and a Unity package at `unity-mcp/tools/unity-bridge/unity-extension/package.json`.
