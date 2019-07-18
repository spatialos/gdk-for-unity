# Transform Synchronization Feature Module

This feature module contains functionality that synchronizes your entities' SpatialOS transform by reading from or writing to the native Unity transform representation.

<%(#Expandable title="Do you support the Unity ECS Transforms package?")%>
We don't support the Unity ECS Transform package at this time.

If this is a feature that you strongly desire, please let us know on either [Discord](https://discord.gg/SCZTCYm) or our [forums](https://forums.improbable.io/latest?tags=unity-gdk).
<%(/Expandable)%>

## Installation

### 1. Add the package

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.1/manual/index.html#installing-a-new-package).

Or add it to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "io.improbable.gdk.transformsynchronization": "<%(Var key="current_version")%>"
  },
}
```

### 2. Reference the assemblies

The Transform Synchronization Feature Modules contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs depending on whether you have an assembly defintion file in your own project or not.

**I have an assembly definition file**

Open your assembly definition file and add `Improbable.Gdk.TransformSynchronization` to the reference list.

**I don't have an assembly definition file**

If you don't have an assembly defintion file in your project, Unity will automatically reference the `Improbable.Gdk.TransformSynchronization` assembly for you.
