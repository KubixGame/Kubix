# First Unity Editor Scene

If you want to **see** the first 3D editor scene, do it in `Unity Editor`, not in VSCode.

`VSCode` is the right place to edit C# scripts.  
`Unity Editor` is the right place to view:

- the 3D scene
- camera
- light
- placed objects
- play mode
- future editor UI

## Fastest way to see the scene

1. Open or create the Unity project in:

   `apps/client-unity`

2. Create a new scene in:

   `Assets/Kubix/Scenes/EditorScene.unity`

3. Add one empty GameObject to the scene.

4. Name it:

   `Editor Sandbox Bootstrap`

5. Attach this component:

   `Kubix.Editor.EditorSandboxBootstrap`

6. Press Play.

## What should appear

If the scripts compile successfully in Unity, you should see:

- a 3D ground platform
- a visible grid
- a controllable camera
- a simple editor overlay in the top-left corner
- buttons for object types like floor, wall, spawn, finish, damage zone, door, coin

## Current controls

- `WASD` move camera
- hold `Right Mouse Button` to look around
- mouse wheel zoom
- `Left Mouse Button` place selected object on the ground
- `Right Mouse Button` select an object
- `Arrow keys` move selected object on the grid
- `Z / X` rotate selected object
- `C / V` grow/shrink selected object
- `Delete` remove selected object

## Why this approach

This is the fastest way to get a visible 3D editor prototype without waiting for the full polished UI, prefabs, and production scene setup.

It gives you a real editable 3D sandbox first. After that, the next step is to replace the temporary runtime-generated scene pieces with:

- proper Unity scenes
- proper prefabs
- proper UI Toolkit or Canvas UI
- proper save/load hookup to `MapDefinition`
