**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----

## Snapshots
A [snapshot](https://docs.improbable.io/reference/latest/shared/glossary#snapshot) is a representation of the state of a simulated world at some point in time. It stores each [entity](https://docs.improbable.io/reference/13.2/shared/glossary#entity) (as long as the entity has the [Persistence component](https://docs.improbable.io/reference/latest/shared/glossary#persistence)) and the values of the entity’s [components’](https://docs.improbable.io/reference/latest/shared/glossary#component) [properties](https://docs.improbable.io/reference/13.2/shared/glossary#property).

You use a snapshot as the starting point for your [world](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world) when you [deploy](https://docs.improbable.io/reference/latest/shared/glossary#deploying), [locally](https://docs.improbable.io/reference/latest/shared/glossary#local-deployment) or [to the cloud](https://docs.improbable.io/reference/latest/shared/glossary#cloud-deployment).

## How to create a snapshot
You set up snapshots through code and generate them through the Unity Editor.

### Set up the snapshot
To do this, look at the [Playground project](../../workers/unity/Assets/Playground) which comes with the GDK for Unity. 

In the project’s [`Editor/SnapshotGenerator`](../../workers/unity/Assets/Playground/Editor/SnapshotGenerator) folder, there is a simple example of generating a snapshot through code. 
You can use this as a base for your own project’s snapshot generation by copying the file to the same folder in your own project and editing the `SnapshotGenerator` class to add SpatialOS entities to a snapshot. 

### Generate the snapshot
To generate the snapshot, in the Unity Editor menu: **SpatialOS** > **Generate snapshot** to open the snapshot generator window, then click `Generate snapshot` there. 

This saves the generated snapshot to `snapshots/default.snapshot`, which is where SpatialOS expects to find it unless explicitly told to use another path when you start the deployment.

## How to start a deployment from a snapshot
You can start local or cloud deployments using the `spatial local launch` and `spatial cloud launch` commands respectively - see the documentation on [Building workers and deploying your game](build-and-deploy.md) for details. 

Both of these commands can take the optional command line parameter  `--snapshot=<path>`. This  starts the deployment with the snapshot at the given path instead of the default `snapshots/default.snapshot`.

-----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).