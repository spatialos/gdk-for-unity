# GameObject Creation Feature Module

This feature module contains an default implementation of spawning GameObjects for your SpatialOS entities and offers the ability to customize that process.

## Installation

### 1. Add the package

To add this feature module to your project, add the following line to the `dependencies` section in your `workers/my-unity-project/Packages/manifest.json`:

```json
    "com.improbable.gdk.gameobjectcreation": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.gameobjectcreation",
```

> **Note:** `<path-to-the-gdk>` is the relative path between your `manifest.json` and the `gdk-for-unity` project.

### 2. Reference the assemblies

The GameObject Creation Feature Modules contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs if you have an assembly defintion file in your own project or not.

#### I have an assembly definition file

Open your assembly definition file and add `Improbable.Gdk.GameObjectCreation` to the reference list.

#### I don't have an assembly definition file

If you don't have an assembly defintion file in your project, Unity will automatically reference the `Improbable.Gdk.GameObjectCreation` assembly for you.