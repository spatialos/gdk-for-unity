# GameObject Creation Feature Module

This feature module contains a default implementation of spawning GameObjects for your SpatialOS entities and offers the ability to customize that process.

## Installation

### 1. Add the package

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html#specifying-a-local-package-location).

The GameObject Creation Feature Module `package.json` can be found in the [`gdk-for-unity` repository](https://github.com/spatialos/gdk-for-unity) at:

```text
workers/unity/Packages/com.improbable.gdk.gameobjectcreation/package.json
```

### 2. Reference the assemblies

The GameObject Creation Feature Modules contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs depending on whether you have an assembly defintion file in your own project or not.

**I have an assembly definition file**

Open your assembly definition file and add `Improbable.Gdk.GameObjectCreation` to the reference list.

**I don't have an assembly definition file**

If you don't have an assembly defintion file in your project, Unity will automatically reference the `Improbable.Gdk.GameObjectCreation` assembly for you.