<%(TOC)%>

# Snapshots

A [snapshot]({{urlRoot}}/reference/glossary#snapshot) is a representation of the state of a simulated world at some point in time. It stores each [persistent]({{urlRoot}}/reference/glossary#persistence) [entity]({{urlRoot}}/reference/glossary#spatialos-entity) and the values of their [components’]({{urlRoot}}/reference/glossary#spatialos-component) properties.

You use a snapshot as the starting point for your [world]({{urlRoot}}/reference/glossary#spatialos-world) when you [deploy]({{urlRoot}}/reference/glossary#deploying).

## How to create a snapshot

Currently, you generate a snapshot with code. The GDK provides a [`Snapshot`]({{urlRoot}}/api/core/snapshot) class which allows you to easily add entities and write the snapshot to disk. A snapshot consists of a set of entities. In the GDK, these entities are defined by [its template]({{urlRoot}}/reference/concepts/entity-templates).

A simple example of generating a snapshot:

```csharp
public static void GenerateSnapshot() 
{
    // Create a snapshot object
    var snapshot = new Snapshot();

    // Create a template...
    var template = new EntityTemplate();
    template.AddComponent(new Position.Snapshot(playerSpawnerLocation), WorkerUtils.UnityGameLogic);
    template.AddComponent(new Metadata.Snapshot("Cube"), WorkerUtils.UnityGameLogic);
    template.AddComponent(new Persistence.Snapshot(), WorkerUtils.UnityGameLogic);
    template.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient, WorkerUtils.MobileClient);
    template.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

    // ..and add it to the snapshot.
    snapshot.AddEntity(template);

    // WriteToFile operates relative to your working directory.
    // In the Unity Editor, this is the Assets directory of your Unity project.
    snapshot.WriteToFile("../../../snapshots/default.snapshot");
}
```

A common usage pattern is to expose the snapshot generation through a Unity Editor menu item or window. You can see an example of this in the [FPS Starter Project](https://github.com/spatialos/gdk-for-unity-fps-starter-project/blob/<%(Var key="current_version")%>/workers/unity/Assets/Fps/Scripts/Editor/SnapshotGenerator/SnapshotMenu.cs). This allows you to quickly generate a snapshot with a few clicks!

## How to start a deployment from a snapshot

You can start a local deployment via the Unity Editor menu: **SpatialOS** > **Local launch** (Ctrl+L/Cmd+L). This starts a deployment with the snapshot found at `<spatialos_project_root>/snapshots/default.snapshot`. If you wish to launch via the CLI or with a different snapshot, see the section below.

You can start a cloud deployment using the [Deployment Launcher Feature Module]({{urlRoot}}/modules/deployment-launcher/overview). This feature module allows you to configure which snapshot you want to start your cloud deployment with.

<%(#Expandable title="Launching a local deployment via the CLI or with a different snapshot")%>
You can launch a local deployment with the CLI using the following command:

```text
spatial local launch --snapshot=<path_to_snapshot> --enable_pre_run_check=false
```

<%(/Expandable)%>
