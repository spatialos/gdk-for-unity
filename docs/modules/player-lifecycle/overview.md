<%(TOC)%>
# Player Lifecycle Feature Module

The Player Lifecycle Feature Module provides player creation functionality and simple player lifecycle management. You can find the Feature Module in the GDK repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle).

The module includes the following features:

* Send player creation requests and handle responses.
* Ability to provide arbitrary serialized data as part of the player creation loop.
* Send and acknowledge player [heartbeats]({{urlRoot}}/modules/player-lifecycle/heartbeating).

You can find more information about the underlying APIs provided in our [API reference docs]({{urlRoot}}/api/player-lifecycle-index).

## Installation

### 1. Add the package

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html#specifying-a-local-package-location).

The Player Lifecycle Feature Module `package.json` can be found in the [`gdk-for-unity` repository](https://github.com/spatialos/gdk-for-unity) at:

```text
workers/unity/Packages/com.improbable.gdk.playerlifecycle/package.json
```

### 2. Reference the assemblies

The Player Lifecycle Feature Module contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs depending on whether you have an assembly defintion file in your own project or not.

**I have an assembly definition file**

Open your assembly definition file and add `Improbable.Gdk.PlayerLifecycle` to the reference list.

**I don't have an assembly definition file**

If you don't have an assembly defintion file in your project, Unity will automatically reference the `Improbable.Gdk.PlayerLifecycle` assembly for you.
