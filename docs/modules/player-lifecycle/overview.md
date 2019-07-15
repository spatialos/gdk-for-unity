# Player Lifecycle Feature Module

The Player Lifecycle Feature Module provides player creation functionality and simple player lifecycle management.

The module includes the following features:

* Send player creation requests and handle responses.
* Ability to provide arbitrary serialized data as part of the player creation loop.
* Send and acknowledge player [heartbeats]({{urlRoot}}/modules/player-lifecycle/heartbeating).

You can find more information about the underlying APIs provided in our [API reference docs]({{urlRoot}}/api/player-lifecycle-index).

## Installation

### 1. Add the package

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.1/manual/index.html#installing-a-new-package).

Or add it to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "io.improbable.gdk.playerlifecycle": "<%(Var key="current_version")%>"
  },
}
```

### 2. Reference the assemblies

The Player Lifecycle Feature Module contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs depending on whether you have an assembly definition file in your own project or not.

**I have an assembly definition file**

Open your assembly definition file and add `Improbable.Gdk.PlayerLifecycle` to the reference list.

**I don't have an assembly definition file**

If you don't have an assembly definition file in your project, Unity will automatically reference the `Improbable.Gdk.PlayerLifecycle` assembly for you.
