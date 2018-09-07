**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----

## Setting up a new project

Follow the [Setup and installation guide](../setup-and-installing.md) to make sure you have everything installed.

To use the SpatialOS GDK for Unity in a new project, you need to set up your project manifest, and then set up world initialization. Your new project must have the [same structure as a SpatialOS project](https://docs.improbable.io/reference/latest/shared/reference/project-structure).

### Set up your project manifest
Add the following dependencies to the `packages` manifest located inside `workers/unity/Packages/manifest.json`:
```
{
  "dependencies": {
    "com.improbable.gdk.core": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.core",
    "com.improbable.gdk.buildsystem": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.buildsystem",
    "com.improbable.gdk.playerlifecycle": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.playerlifecycle",
    "com.improbable.gdk.testutils": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.testutils",
    "com.improbable.gdk.tools": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.tools",
    "com.improbable.gdk.transformsynchronization": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.transformsynchronization",
    "com.unity.package-manager-ui": "1.9.11",
    "com.unity.modules.physics": "1.0.0"
  }
}
```

### Set up world initialization

By default, Unity creates a `DefaultWorld` and then searches the whole project for systems it can add to this world. You can use the `DefaultWorld` to run logic that is completely independent of SpatialOS. However, you can’t use it to run systems that interact with SpatialOS, because each worker creates its own world and stores additional information about its connection.

You need to choose whether to use the `DefaultWorld` or whether to disable it, depending on the logic your project contains.

#### Projects containing SpatialOS and non-SpatialOS logic

If your project contains both SpatialOS logic and logic that is completely independent of SpatialOS, you can use the `DefaultWorld`. However, you need to add the `DisableAutoCreation` attribute to each system that interacts with SpatialOS. This prevents Unity from adding these systems to the `DefaultWorld`.

#### Projects containing only SpatialOS logic

If your project contains only SpatialOS logic, you can disable the `DefaultWorld`.

To disable the default world:

1. In the Unity Editor, go to **Edit** > **Project Settings** > **Player** and add `UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP` to the **Scripting Define Symbols** field. This disables the creation of the `DefaultWorld`.
2. Create an initialization script that contains the logic for setting up the injection hooks and cleaning up the worlds. For example:

```csharp
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace YourProject
{
    public static class OneTimeInitialisation
    {
        private static bool initialized;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            // ensure that it will only be run when the first scene gets loaded
            if (initialized)
            {
                return;
            }

            initialized = true;
            WorldsInitializationHelper.SetupInjectionHooks();
            PlayerLoopManager.RegisterDomainUnload(WorldsInitializationHelper.DomainUnloadShutdown, 1000);
        }
    }
}

```
This sets up the injection hooks needed to run Unity's hybrid ECS and ensures that all worlds are properly cleaned up. You need to set the `initialized` field to `true` to ensure it is only run once, otherwise `Init` is run whenever a scene gets loaded due to the `RuntimeInitializeOnLoadMethod` attribute.

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).