<%(TOC)%>

# Snapshots

_This document relates to both [MonoBehaviour and ECS workflows]({{urlRoot}}/reference/workflows/overview)._

A [snapshot](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#snapshot) is a representation of the state of a simulated world at some point in time. It stores each [entity](https://docs.improbable.io/reference/13.2/shared/glossary#entity) (as long as the entity has the [Persistence component](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#persistence)) and the values of the entity’s [components’](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#component) [properties](https://docs.improbable.io/reference/13.2/shared/glossary#property).

You use a snapshot as the starting point for your [world](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#spatialos-world) when you [deploy](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#deploying), [locally](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#local-deployment) or [to the cloud](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#cloud-deployment).

## How to create a snapshot

You set up snapshots through code and generate them through your Unity Editor.

### Set up the snapshot

To do this, look at the [Playground project](https://github.com/spatialos/UnityGDK/tree/master/workers/unity/Assets/Playground) which comes with the GDK for Unity.

In the project’s [`Editor/SnapshotGenerator`](https://github.com/spatialos/UnityGDK/tree/master/workers/unity/Assets/Playground/Editor/SnapshotGenerator) folder, there is a simple example of generating a snapshot through code.

You can use this as a base for your own project’s snapshot generation by copying the file to the same folder in your own project and editing the `SnapshotGenerator` class to add SpatialOS entities to a snapshot.

### Generate the snapshot

To generate the snapshot, in your Unity Editor menu: **SpatialOS** > **Generate snapshot** to open the snapshot generator window, then select `Generate snapshot` there.

This saves the generated snapshot to `snapshots/default.snapshot`, which is where SpatialOS expects to find it unless explicitly told to use another path when you start the deployment.

## How to start a deployment from a snapshot

You can start local or cloud deployments using the `spatial local launch` and `spatial cloud launch` commands respectively - see the documentation on [Deploying your game]({{urlRoot}}/reference/concepts/deployments) for details.

Both of these commands can take the optional command line parameter `--snapshot=<path>`. This starts the deployment with the snapshot at the given path instead of the default `snapshots/default.snapshot`.
