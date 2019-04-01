# Build configuration

The Build System feature module uses a scriptable object for its build configuration. 

You can create a build configuration asset from the Unity Editor by selecting **Assets** > **Create** > **SpatialOS** > **Build Configuration**.

> **Note:** The build configuration scriptable object is a singleton scriptable object. You will observe errors if you create multiple of these assets.

## Build configuration UI

The build configuration asset has a user-friendly UI that you can use to configure your builds. You can view this UI by opening the scriptable object in the Unity Inspector window.

The build is configured per worker type and for each worker type there is both a local and cloud deployment configuration.

For each worker type, you can configure:

* Which Unity Scenes to bundle in the build.

For each worker type and deployment type pair, you can configure:

* Which platforms you want your worker to build for.
* Which build options are enabled on a per-platform basis.

| | |
|---|---|
| **Build Option** | **Description** |
| Build | Denotes whether to build this target or not. |
| Required | Denotes whether a build failure while building this target should trigger a build-wide failure. |
| Development | Denotes whether the build should be a development build with debug symbols. |
| Server build | Denotes whether the worker is running in headless mode. |
| Compression | Which compression scheme to use in the build. |

> **Note:** All server-workers **must** have a Linux build target enabled for the cloud target because server-workers are ran on Linux machines in cloud deployments.

### Build and Required options

These two options describe the behaviour of a worker-type, deployment target, and platform combination.

| | | |
|---|---|---|
|**Build Enabled?**|**Required Enabled?**|**Behaviour**|
| No | No | Build is not run. |
| Yes | No | Build is run. If the build support for the platform is not installed, _skip_ the build rather than failing. |
| No | Yes | Invalid state and undefined behaviour. If this state ever occurs, please [raise a bug report](https://github.com/spatialos/gdk-for-unity/issues/new). |
| Yes | Yes | Build is run. If the build support for the platform is not installed, fail the build. |

> **Note:** Running a worker build [from the CLI]({{urlRoot}}/modules/build-system/cli) ignores the Required flag. If the build support is not installed, the build will fail. 