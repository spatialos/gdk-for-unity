<%(TOC)%>

# Health Pick-up tutorial for the FPS Starter Project

![In-game view of the health pickup prefab]({{assetRoot}}assets/health-pickups-tutorial/health-pickup-visible-1.png)

Before starting this tutorial, make sure you have followed the [Get started]({{urlRoot}}/projects/fps/get-started/get-started) guide which sets up the FPS Starter Project. This tutorial follows on from that guide.

## What does the tutorial cover?

You will add health pack pick-ups to the game. These health pack pick-ups are subject to the following design constraints:

1. The pick-ups have static positions in the game world and are present in the world at start-up time.
1. A pick-up grants health to a single player which walks over them.
1. After the health pack is consumed, it is no longer visible to clients and no longer grants health.
1. After a period of time, the health pack is "respawned" and is available to be picked up again.
1. Critical interactions like: collision detection between the player and the health pack and granting the health is done on the game logic worker. That is to say, we do not trust the client.
1. (Optional) Players which have full health cannot pick up the health packs.

To implement this feature you will:

* Add a new [SpatialOS component]({{urlRoot}}/reference/glossary#spatialos-component) to hold the state of the health packs.
* Define a new [SpatialOS entity template](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#entity-template) called `HealthPickup`.
* Add `HealthPickup` entities to the [snapshot]({{urlRoot}}/reference/glossary#snapshot) so they are loaded in the [SpatialOS world]({{urlRoot}}/reference/glossary#spatialos-world) at startup.
* Write game logic such that the health packs grant health to players.
* Write game logic to "respawn" health packs after they have been consumed.

## Open the FPS Starter Project in your Unity Editor

1. Launch your Unity Editor.
1. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

## Design a SpatialOS component for our health packs

To satisfy our design constraints, we need our health packs to contain some state that is replicated between our server-workers and client-workers:

* A property that represents whether the health pack is "active".
* A property that represents the "value" of the health pack, or, how much health the health pack will grant when consumed.

> Strictly speaking, the property that represents the value of the health pack _could be_ a hardcoded value in your server-worker instances, but this design opens up the possibility for dynamic health packs which have varying values.

We write the definitions for these properties in [schemalang]({{urlRoot}}/reference/glossary#schema), a SpatialOS specific language. Let's do that now!

**Step 1.** Using your file manager, navigate to `gdk-for-unity-fps-starter-project/`, then create a `schema` directory.

**Step 2.** Inside the `gdk-for-unity-fps-starter-project/schema/` directory, create a `pickups` directory.

**Step 3.** Inside the `gdk-for-unity-fps-starter-project/schema/pickups/` directory, use a text editor of your choice to create a new file called `health_pickup.schema`.

**Step 4.** Copy and paste the following definition into the file and save the file:

```schemalang
package pickups;

component HealthPickup {
    id = 21001;
    bool is_active = 1;
    uint32 health_value = 2;
}
```

This defines a new SpatialOS component called `HealthPickup`, and defines the two properties that we discussed above:

* `is_active`: A boolean that indicates if the health pack is active.
* `health_value`: An integer value which indicates the amount of health the pack will grant to a player.

<%(#Expandable title="What is the <code>id</code> property?")%>
The `id` property is a required property for every SpatialOS component defined in schema. Each component **must** have a unique ID in your project.

See the [schemalang reference](https://docs.improbable.io/reference/13.6/shared/schema/reference#ids) for more information.
<%(/Expandable)%>

<%(#Expandable title="Why are we assigning the fields?")%>
Each field in a component must have a unique id (within the component) which it is assigned to.

This allows us to maintain backwards compatibility when schema changes. See [schemalang reference](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/reference#components) for more info.
<%(/Expandable)%>

**Step 5.** Add this folder to the GDK tools configuration.

From your Unity Editor menu, select **SpatialOS** > **Gdk tools configuration**. In the **Schema sources** section inside the **Gdk tools configuration** window, add the path `../../schema` to the list of schema sources.

**Step 6.** Run the code generator.

From your Unity Editor menu, select **SpatialOS** > **Generate code** to invoke the code generator.

The code generator creates C# code based on the components and types defined in the [schemalang](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#schemalang). Any time you modify your `schema` files you **must** then run the code generator to see your changes reflected in code.

> When writing schema files, your properties must use snake case (for example, `health_value`), but the the code generator will create C# code in the [standard C# capitalisation conventions](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions).

Code generation is automatically run whenever you open a GDK for Unity project in your Unity editor.

If you are worried your generated code is in a bad state, select **SpatialOS** > **Generate code (force)** from the Unity Editor menu to delete the generated code and regenerate it.

<%(#Expandable title="Where is the generated code?")%>
The generated code for your component can be found in the `Assets/Generated/Source/improbable/` directory of your Unity project. Feel free to have a look if you want to see what happens behind the scenes.

Note that you don’t need to understand the generated code in order to follow this tutorial.
<%(/Expandable)%>

## Define a new SpatialOS entity

Now that we've defined and generated the `HealthPickup` component and its properties, let's define the `HealthPickup` entity template!

An entity template simply declares which SpatialOS components are present on the entity, the initial values of those components, and which worker types can read from or write to the components.

The `HealthPickup` entity is a new type of entity, so we must create a new entity template. To do this, we'll need to add a new function within the `FpsEntityTemplates` class:

**Step 1.** In your Unity Editor, locate `Assets/Fps/Scripts/Config/FpsEntityTemplates.cs` and open it in your code editor.

**Step 2.** Add `using Pickups;` and `using UnityEngine;` to the top of the file to ensure that we can reference the generated `Pickups` namespace and Unity engine types.

**Step 3.** Define a new static function within the `FpsEntityTemplates` class which takes the position of the health pack and the value of the health pack as parameters and returns an `EntityTemplate` instance:

```csharp
public static EntityTemplate HealthPickup(Vector3 position, uint healthValue)
{
    // Create a HealthPickup component snapshot which is initially active and grants "heathValue" on pickup.
    var healthPickupComponent = new Pickups.HealthPickup.Snapshot(true, healthValue);

    var entityTemplate = new EntityTemplate();
    entityTemplate.AddComponent(new Position.Snapshot(Coordinates.FromUnityVector(position)), WorkerUtils.UnityGameLogic);
    entityTemplate.AddComponent(new Metadata.Snapshot("HealthPickup"), WorkerUtils.UnityGameLogic);
    entityTemplate.AddComponent(new Persistence.Snapshot(), WorkerUtils.UnityGameLogic);
    entityTemplate.AddComponent(healthPickupComponent, WorkerUtils.UnityGameLogic);
    entityTemplate.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient);
    entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

    return entityTemplate;
}
```

Let's pull out some of the more interesting lines from the above snippet:

* The line `entityTemplate.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient);` states that both the [server-worker]({{urlRoot}}/reference/glossary#server-worker) (`WorkerUtils.UnityGameLogic`) and [client-worker]({{urlRoot}}/reference/glossary#client-worker) (`UnityClient`) have [read access]({{urlRoot}}/reference/glossary#read-access) to this entity (that they can see health packs).
* The line `entityTemplate.AddComponent(healthPickupComponent, WorkerUtils.UnityGameLogic);` adds an instance of the `HealthPickup` component to the `HealthPickup` entity and sets the write access to the `UnityGameLogic` worker type.

> Our design constraints state that we should not trust the client, so we only give write-access to the `HealthPickup` component to the [server-worker]({{urlRoot}}/reference/glossary#server-worker). This means that a client cannot change the health value of the health pack or force it to be active.

<%(#Expandable title="What are <code>Position</code>, <code>Metadata</code>, and <code>Persistence</code>?")%>
These SpatialOS components are [standard library](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/standard-schema-library) components.

* `Position` declares the location of this entity and is used for loadbalancing.
* `Metadata` declares the "entity type" and is used to populate the information in the Inspector.
* `Persistence` declares whether this entity should be saved to a snapshot.

<%(/Expandable)%>

<%(#Expandable title="How would you give only a specific client write-access for a component?")%>
You can state that a specific client has write-access for a component by passing in a value of `workerId: {myWorkerId}` for the write access, where `myWorkerId` is the [worker ID](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#worker-id) of the client.

Some component data should be editable by the player's client, but not by the clients of any other players. In the FPS Starter Project the `Player` entity template function in `FpsEntityTemplates.cs` grants the player's client write access over a number of components: `clientMovement`, `clientRotation`, `clientHeartbeat` etc.
<%(/Expandable)%>

<%(#Expandable title="Can I specify more than one worker type to have write-access to a single component?")%>Yes, you are not restricted to just one worker type being granted write-access.

To find out about how to do this, read up about [layers](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/worker-configuration/layers#layers).
<%(/Expandable)%>

## Add your new entity to the snapshot

In this section we’re going to add a health pack entity to the SpatialOS world. There are two ways to do this:

* At runtime, by sending a `CreateEntity` command with an `EntityTemplate` object to the SpatialOS Runtime.
* At start-up, by adding a health pack entity to the [Snapshot]({{urlRoot}}/reference/glossary#snapshot), so it's loaded into the world when the SpatialOS Runtime loads.

We will do the latter, so that when the game begins there will already be a health pack in a pre-defined location.

---

The **SpatialOS** menu in your Unity Editor contains a **"Generate FPS Snapshot"** option. This menu item runs `Improbable.Gdk.Fps.GenerateFpsSnapshot()`. We will now modify this function to add a `HealthPack` entity to our snapshot:

**Step 1.** In your Unity Editor, locate `Assets/Fps/Scripts/Editor/SnapshotGenerator/SnapshotMenu.cs` and open it in your code editor.

**Step 3.** Copy and paste the function below into the `SnapshotMenu` class.

```csharp
private static void AddHealthPacks(Snapshot snapshot)
{
    // Invoke our static function to create an entity template of our health pack with 100 heath.
    var healthPack = FpsEntityTemplates.HealthPickup(new Vector3(5, 0, 0), 100);

    // Add the entity template to the snapshot.
    snapshot.AddEntity(healthPack);
}
```

> In your own game may want to consider moving default values (such as health pack positions, and health values) into a settings file. But for now, we will keep this example simple.

**Step 4.** Copy and paste the below snippet inside `GenerateDefaultSnapshot()` and `GenerateSessionSnapshot()` to call your new function.

```csharp
    AddHealthPacks(snapshot);
```

<%(#Expandable title="What should <code>SnapshotMenu</code> look like when its done?")%>
```csharp
using System.IO;
using Improbable;
using Improbable.Gdk.Core;
using UnityEditor;
using UnityEngine;

namespace Fps
{
    public class SnapshotMenu : MonoBehaviour
    {
        private static readonly string DefaultSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/default.snapshot");

        private static readonly string CloudSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/cloud.snapshot");

        private static readonly string SessionSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/session.snapshot");

        [MenuItem("SpatialOS/Generate FPS Snapshot")]
        private static void GenerateFpsSnapshot()
        {
            SaveSnapshot(DefaultSnapshotPath, GenerateDefaultSnapshot());
            SaveSnapshot(CloudSnapshotPath, GenerateDefaultSnapshot());
            SaveSnapshot(SessionSnapshotPath, GenerateSessionSnapshot());
        }

        private static Snapshot GenerateDefaultSnapshot()
        {
            var snapshot = new Snapshot();
            snapshot.AddEntity(FpsEntityTemplates.Spawner(Coordinates.Zero));
            AddHealthPacks(snapshot);
            return snapshot;
        }

        private static Snapshot GenerateSessionSnapshot()
        {
            var snapshot = new Snapshot();
            snapshot.AddEntity(FpsEntityTemplates.Spawner(Coordinates.Zero));
            snapshot.AddEntity(FpsEntityTemplates.DeploymentState());
            AddHealthPacks(snapshot);
            return snapshot;
        }

        private static void SaveSnapshot(string path, Snapshot snapshot)
        {
            snapshot.WriteToFile(path);
            Debug.LogFormat("Successfully generated initial world snapshot at {0}", path);
        }

        private static void AddHealthPacks(Snapshot snapshot)
        {
            // Invoke our static function to create an entity template of our health pack with 100 heath.
            var healthPack = FpsEntityTemplates.HealthPickup(new Vector3(5, 0, 0), 100);

            // Add the entity template to the snapshot.
            snapshot.AddEntity(healthPack);
        }
    }
}
```
<%(/Expandable)%>

**Step 5.** Regenerate our snapshot. From the Unity Editor, select **SpatialOS** > **Generate FPS Snapshot**.

Now that you have changed the snapshot generation script in `SnapshotMenu.cs`, we will need to regenerate the snapshot to see the new `HealthPickup` entity appear in your game world.

You can validate that the snapshot was updated by launching a local deployment (`Ctrl + L`/`Cmd + L` in your Unity Editor) and looking in the [Inspector](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/inspector).

> **Note:** If you have a local deployment running, make sure to close it before launching a new one!

![World view in the Inspector showing the `HealthPickup` entity]({{assetRoot}}assets/health-pickups-tutorial/health-pickup-inspector-1.png)

If we were to test the game at this point, the health pack entity would appear in the inspector but not in-game. This is because we have not yet defined how to represent the entity on your client or server-workers. We'll do this in the next section.

<%(#Expandable title="How does the Inspector decide the entity name?")%>
The Inspector uses the `entity_type` string field from the [schema standard library](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/schema/standard-schema-library) component `Metadata` as the entity name.
<%(/Expandable)%>

<%(#Expandable title="Where are my snapshots?")%>

All SpatialOS GDK projects contain a directory named `snapshots` in the root of the project. Your snapshots can be found in that directory:
`gdk-for-unity-fps-starter-project/snapshots`.

<%(/Expandable)%>

<%(#Expandable title="Can I make my snapshots human-readable?")%>

Yes, there is a `spatial` command that will convert snapshots to a human-readable format. However, you cannot launch a deployment from a human-readable snapshot, so it must be converted back to binary before it is usable. To find out more about working with snapshots you can read about the [spatial snapshot command](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/snapshots#convert-a-snapshot).

While they are human-readable and you can manually edit the values of the properties within, be careful to not make mistakes that will inhibit the conversion back to binary form!
<%(/Expandable)%>

**Step 6.** Before you move on, in the terminal window that's running the SpatialOS process, enter **Ctrl+C** or stop the process.

## Plan your entity representations

In this section we’re going to decide how to represent the `HealthPickup` entity in our client-workers and in our server-workers. Let's review the relevant design constraints listed above:

1. A pick-up grants health to a single player which walks over them.
1. After the health pack is consumed, it is no longer visible to clients and no longer grants health.
1. After a period of time, the health pack is "respawned" and is available to be picked up again.
1. Critical interactions like: collision detection between the player and the health pack and granting the health is done on the game logic worker. That is to say, we do not trust the client.

From these we can derive some rules about how the `UnityClient` and `UnityGameLogic` workers should represent the `HealthPickup` entity:

* The `UnityClient` client-worker should display a visual representation for each health pack in the world. It should only display health packs that are currently "active". It _should not_ do any collision detection.
* The `UnityGameLogic` server-worker should have a physical representation for each health pack in order to do the collision detection. The collider on the health pack should be turned off when the health pack is not active. It _does not need to_ visualise the health pack.

---

The FPS Starter Project uses the SpatialOS GDK's [MonoBehaviour workflow]({{urlRoot}}/workflows/overview#monobehaviour-centric-workflow). In this workflow SpatialOS entities are represented by Unity prefabs. Crucially, you can use different prefabs to represent the same type of entity on different types of workers. This allows you to separate client-side and server-side entity representation, as we planned above.

<%(#Expandable title="How does the FPS Starter Project pair SpatialOS entities with Unity prefabs?")%>

The FPS Starter Project uses the `AdvancedEntityPipeline` implementation of the [`IEntityGameObjectCreator` interface]({{urlRoot}}/api/game-object-creation/i-entity-game-object-creator) from the [GameObject Creation]({{urlRoot}}/modules/game-object-creation/overview) feature module to handle the instantiation of GameObjects to represent SpatialOS entities.

This tracks associations between entities and prefabs by matching their `Metadata` component's metadata string to the names of prefabs in the `Assets/Fps/Resources/Prefabs/` directory. If the worker receives information about a new SpatialOS entity then the GameObject Creation package immediately instantiates a GameObject of the appropriate type to represent that entity.

Client-side entity prefabs are stored in `Assets/Fps/Resources/Prefabs/UnityClient`, while server-side ones are located at `Assets/Fps/Resources/Prefabs/UnityGameLogic`.
<%(/Expandable)%>

<%(#Expandable title="What are the 'Authoritative' and 'NonAuthoritative' sub-folders for?")%>
The `Assets/Fps/Resources/Prefabs/UnityClient/` folder contains two sub-folders, `Authoritative` and `NonAuthoritative`, and _both_ of them contain a `Player` prefab!

The FPS Starter Project has some custom logic built into the `AdvancedEntityPipeline` for handling its `Player` entities. When creating your own entity prefabs for the `UnityClient` worker you can put them directly into `Assets/Fps/Resources/Prefabs/UnityClient/`.

The `Player` entity has a special relationship with the `UnityClient` instance that is authoritative over it. That `UnityClient` is running on the gamer's machine, and that gamer "owns" that `Player` entity. It's their representation in the world. As such, there are big differences between how the `Player` entity should be represented on the authoritative client (it should have a camera, collect player input etc.) compared to if the `Player` entity represents someone else in the game. The FPS Starter Project has some additional logic to manage these representations as a way of keeping the code more organised.
<%(/Expandable)%>

## Implement client-side entity representation

The client-side logic we want to implement for this feature is:

* Visualise active health packs hovering just above the ground.
* Do not visualise inactive health packs.

**Step 1.** In your Unity Editor, locate `Assets/Fps/Prefabs/HealthPickup.prefab`.

**Step 2.** Select this prefab and press `Ctrl+D`/`Cmd + D` to duplicate it.

**Step 3.** Move this duplicated prefab to `Assets/Fps/Resources/Prefabs/UnityClient`.

**Step 4.** Rename the duplicated prefab to `HealthPickup` (the process of duplication will have appended an unnecessary `1` to the file name).

**Step 5.** Select the duplicated prefab to open it.

**Step 6.** Still in your Unity Editor, add a new script component to the root of your duplicated `HealthPickup` prefab by selecting **Add Component** > **New Script** in the Inspector window.

**Step 7** Name this script `HealthPickupClientVisibility` and open in in your code editor.

This script will contain the logic to toggle the visibility of the health pack when the health pack becomes active/inactive.

**Step 8** Replace the contents of `HealthPickupClientVisibility` with the following snippet:

```csharp
using Improbable.Gdk.Subscriptions;
using Pickups;
using UnityEngine;

namespace Fps
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class HealthPickupClientVisibility : MonoBehaviour
    {
        [Require] private HealthPickupReader healthPickupReader;

        private MeshRenderer cubeMeshRenderer;

        private void OnEnable()
        {
            cubeMeshRenderer = GetComponentInChildren<MeshRenderer>();
            healthPickupReader.OnUpdate += OnHealthPickupComponentUpdated;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            cubeMeshRenderer.enabled = healthPickupReader.Data.IsActive;
        }

        private void OnHealthPickupComponentUpdated(HealthPickup.Update update)
        {
            UpdateVisibility();
        }
    }
}
```

This script is mostly standard C# code that you could find in any game built with Unity Engine. There are a few parts which are specific to the SpatialOS GDK though, let's break those down:

* `[WorkerType(WorkerUtils.UnityClient)]`<br/>
This `WorkerType` annotation marks this `MonoBehaviours` to only be enabled for a specific worker-type. In this case, this `MonoBehaviour` will only be enabled on `UnityClient` client-workers, ensuring that it will never run on your server-workers.

> While we also separate our prefabs by worker types, its good practice to annotate `MonoBehaviour`s that are worker specific with `WorkerType` annotations.<br/><br/>It makes it explicit to the reader where the `MonoBehaviour` should run and serves as a safety check against accidentally putting this behaviour on a prefab meant for a different worker type.

* `[Require] private HealthPickupReader healthPickupReader;`<br/>
This is a `Reader` object, which allows you to interact with your SpatialOS components easily at runtime. In particular, this is a `HealthPickupReader`, which allows you to access the value of the `HealthPickup` component of the underlying linked entity. For more information about Readers, see the [Reader API]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/overview#reader-api).

> The `[Require]` annotation on the `HealthPickupReader` is very important. This tells the GDK to [inject]({{urlRoot}}/reference/glossary#inject) this object when its requirements are fulfilled. A Reader's requirements is that the underlying SpatialOS component is checked out on your worker-instance, regardless of authority. <br/><br/>**A `Monobehaviour` will only be enabled if all required objects have their requirements satisfied.**

* `healthPickupReader.OnUpdate += OnHealthPickupComponentUpdated;`<br/>
Here, we bind a method to an event on the `Reader`. This means that whenever the `HealthPickup` component is updated, we will trigger a callback on `OnHealthPickupComponentUpdated`. This allow you to react to changes in components.

* `cubeMeshRenderer.enabled = healthPickupReader.Data.IsActive;`<br/>
Here, we access the **current** data of the `HealthPickup` component of the underlying linked entity. We toggle the `MeshRenderer` on the `GameObject` - this ensures that the visibility of the `GameObject` is always kept in sync with the `is_active` property on the `HealthPickup` component.

<%(#Expandable title="Are entities always represented by GameObjects?")%>

No, exactly how entities are represented on each of your workers is up to you.

The GDK also offers an [ECS workflow]({{urlRoot}}/workflows/overview) which represents them as a grouping of Unity ECS entity and components.

If you are more familiar with the traditional Unity GameObject style of development then the GDK provides a [MonoBehaviour workflow]({{urlRoot}}/workflows/overview#monobehaviour-centric-workflow) for you.

You are not limited to these options either, and can configure your worker to create something very custom when it encounters a particular entity type.

<%(/Expandable)%>

<%(#Expandable title="Can I name the prefab something else?")%>

Your choice of prefab name can be anything, but **must** match the string you used for the entity's `Metadata` component when you wrote the entity template function for this entity.

This is implementation detail from the `AdvancedEntityPipeline` that the FPS Starter Project uses to create GameObjects.

<%(/Expandable)%>

<%(#Expandable title="Wait! Why aren't we removing the callback when the script is disabled?")%>The GDK automatically clears event handlers when a script is disabled, therefore you do not need to manually remove the `OnHealthPickupComponentUpdated` callback.<%(/Expandable)%>

### Test the client-side representation

Now we've added some game logic to interact with our `HealthPickup` entity we should test our changes.

> We advise using a test-iterate cycle when developing with the GDK for Unity. You can take advantage of the quick iteration time afforded by running multiple workers in your Unity Editor.

**Step 1.** In your Unity Editor, launch a local deployment of your game by selecting **SpatialOS** > **Local launch** or using the shortcut `Ctrl + L`/`Cmd + L`.

**Step 2.** Open the `FPS-Development` Scene in your Unity Editor. The Scene file is located in `Assets/Fps/Scene`.

**Step 3.** Disable the `SimulatedPlayerCoordinatorWorker` prefab in the Scene. This will prevent any simulated player clients from spawning.

**Step 3.** Press **Play** in your Unity Editor to play the game.

You'll know that your previous changes have worked if you can see a `HealthPickup` entity in the inspector, and find a floating health pack when running around in-game. Currently it just floats there. If you walk into it, nothing happens. Let's fix that!

![In-game view of the health pickup prefab]({{assetRoot}}assets/health-pickups-tutorial/health-pickup-visible-1.png)

**Step 4.** Before you move on, in the terminal window that's running the SpatialOS process, enter **Ctrl+C** or stop the process.

## Implement server-side entity representation

The client-side logic we want to implement for this feature is:

* Detect player collisions with the health pack.
* Grant health to the player that collides with the health pack.
* Turn the health pack to inactive on collision.
* After a period of time, turn the health pack back to active.

**Step 1.** In your Unity Editor, locate `Assets/Fps/Prefabs/HealthPickup.prefab`.

**Step 2.** Select this prefab and press `Ctrl+D`/`Cmd + D` to duplicate it.

**Step 3.** Move this duplicated prefab to `Assets/Fps/Resources/Prefabs/UnityGameLogic`.

**Step 4.** Rename the duplicated prefab to `HealthPickup` (the process of duplication will have appended an unnecessary ` 1` to the file name).

**Step 5.** Select the duplicated prefab to open it.

**Step 6.** Still in your Unity Editor, add a new script component to the root of your duplicated `HealthPickup` prefab by selecting **Add Component** > **New Script** in the Inspector window.

**Step 7** Name this script `HealthPickupServerBehaviour` and open in in your code editor.

This script will contain the logic to listen for collisions, grant health to players, and toggle the active state.

**Step 8** Replace the contents of `HealthPickupServerBehaviour` with the following snippet:

```csharp
using System.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Health;
using Improbable.Gdk.Subscriptions;
using Pickups;
using UnityEngine;

namespace Fps
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class HealthPickupServerBehaviour : MonoBehaviour
    {
        [Require] private HealthPickupWriter healthPickupWriter;
        [Require] private HealthComponentCommandSender healthCommandRequestSender;

        private Coroutine respawnCoroutine;

        private void OnEnable()
        {
            // If the pickup is inactive on initial checkout - turn off collisions and start the respawning process.
            if (!healthPickupWriter.Data.IsActive)
            {
                respawnCoroutine = StartCoroutine(RespawnHealthPackRoutine());
            }
        }

        private void OnDisable()
        {
            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // OnTriggerEnter is fired regardless of whether the MonoBehaviour is enabled/disabled.
            if (healthPickupWriter == null)
            {
                return;
            }

            if (!other.CompareTag("Player"))
            {
                return;
            }

            HandleCollisionWithPlayer(other.gameObject);
        }

        private void SetIsActive(bool isActive)
        {
            healthPickupWriter?.SendUpdate(new HealthPickup.Update
                {
                    IsActive = new Option<bool>(isActive)
                });
        }

        private void HandleCollisionWithPlayer(GameObject player)
        {
            var playerSpatialOsComponent = player.GetComponent<LinkedEntityComponent>();

            if (playerSpatialOsComponent == null)
            {
                return;
            }

            healthCommandRequestSender.SendModifyHealthCommand(playerSpatialOsComponent.EntityId, new HealthModifier
            {
                Amount = healthPickupWriter.Data.HealthValue
            });

            // Toggle health pack to its "consumed" state
            SetIsActive(false);

            // Begin cool-down period before re-activating health pack
            respawnCoroutine = StartCoroutine(RespawnHealthPackRoutine());
        }

        private IEnumerator RespawnHealthPackRoutine()
        {
            yield return new WaitForSeconds(15f);
            SetIsActive(true);
        }
    }
}
```

Let’s break down what the above snippet does:

* `[WorkerType(WorkerUtils.UnityGameLogic)]`<br/>
This `WorkerType` annotation marks this `MonoBehaviours` to only be enabled for a specific worker-type. In this case, this `MonoBehaviour` will only be enabled on `UnityGameLogic` server-workers, ensuring that it will never run on your client-workers.

* `[Require] private HealthPickupWriter healthPickupWriter;`<br/>
This is a `Writer` object, which allows you to interact with and modify your SpatialOS components easily at runtime. In particular, this is a `HealthPickupWriter`, which allows you to access and write to the value of the `HealthPickup` component of the underlying linked entity. For more information about Readers, see the [Writer API]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/overview#writer-api).

> The `[Require]` annotation on the `HealthPickupWriter` is very important. This tells the GDK to [inject]({{urlRoot}}/reference/glossary#inject) this object when its requirements are fulfilled. A Writer's requirements is that the underlying SpatialOS component is checked out on your worker-instance, and your worker-instance is authoritative over that component.<br/><br/>**A `Monobehaviour` will only be enabled if all required objects have their requirements satisfied.**

* `private void OnTriggerEnter(Collider other)`<br/>
Most functions will **only** be called if the `MonoBehaviour` is enabled, but `OnTriggerEnter` is called even when it is disabled. It is unusual in this sense. For this reason, scripts which use `OnTriggerEnter` **must** check whether objects that are have annotations `[Require]` are null (indicating that the requirements were not met) before using functions on those objects.

* `healthPickupWriter?.SendUpdate(new HealthPickup.Update(...));`<br>
Here, we send a component update to the SpatialOS Runtime. The fields within the `Update` struct indicate whether the corresponding field should be updated. If the `Option` is empty, the field will **not** be updated. If the `Option` is not empty, the field will be updated.

* `private void HandleCollisionWithPlayer(GameObject player)`<br/>
This function will be called any time a player walks through a health pack. It handles cross-worker interaction using [commands](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/glossary#command). When you send a command it acts as a request, which SpatialOS delivers to the worker-instance that has write-access for the component that the command is intended for.

> Cross-worker interactions can be necessary when your game has multiple `UnityGameLogic` server-workers, because the worker with write-access for the `HealthPack` entity may not be the same worker that has write-access to the `Player` entity who has collided with that health pack.

* `private IEnumerator RespawnHealthPackRoutine()`<br>
This coroutine re-activates consumed health packs after a cool-down period. It starts at the end of the `HandleCollisionWithPlayer` function as well as in `OnEnable` for any health pack entities which are inactive. Any running coroutines are stopped in `OnDisable`.

<%(#Expandable title="Why is only one worker at a time able to have write-access for a component?")%>
This prevents simultaneous changes putting the world into an inconsistent state. It is known as the single writer principle. If you want to learn more when you're done with the tutorial, have a look at [Authority and interest](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/authority-and-interest/introduction).
<%(/Expandable)%>

<%(#Expandable title="How are clients prevented from sending health-giving commands?")%>
They're not - any worker can send a command. However, only the worker which has write access to the component holding a command is allowed to handle it. Each command request contains information about the caller of the command which could be used to enforce restrictions. Have a look at `ServerHealthModifierSystem.cs` in the Health feature module.
<%(/Expandable)%>

<%(#Expandable title="Why would <code>healthPickupWriter.Data.IsActive</code> be false in OnEnable()?")%>
In a large game, where there are multiple workers of each type (such as multiple `UnityGameLogic` workers), the SpatialOS load-balancer decides how to divide write-access for components between the available workers.

The coroutine is local only to the worker that started it. If the worker loses write-access to the component then the script will become disabled for that entity (and remember, we call `StopCoroutine` in `OnDisable` to cancel the coroutine). In theory, this leaves the entity in a state where `InActive` is false, and no coroutine is running that will eventually change that.

Just as one worker lost write-access, another will be granted it by the load-balancer. That newly authoritative worker now meets the criteria specified by the `[Require]` syntax in the `HealthPickupServerBehaviour` class, and so the script will become enabled.

This is why in `OnEnable()` you should start the coroutine if `healthPickupWriter.Data.IsActive` is false.

The newly authoritative worker will not know how long the cool-down had already been running on the previous worker, so the cool-down timer is ultimately "refreshed" at this point. If this was a problem for our mechanic then we could store the timer's progress in a new property, but in this case we will keep it simple.
<%(/Expandable)%>

### (Optional) Ignore healthy players

This section is intended to reinforce what you've learned but is entirely optional. If you don't want to implement this, you can move onto the [next section](#test-your-changes-in-a-local-deployment).

The `HandleCollisionWithPlayer` function in your `HealthPickupServerBehaviour.cs` script currently attempts to heal any colliding player. If the player is already on full health, we might want to ignore them so that the health pack is not consumed.

**Step 1.** Write a `MonoBehaviour` that exposes a player's health and attach it to the appropriate `Player` prefab.

**Step 2.** At the beginning of the `HandleCollisionWithPlayer` function add an if-statement which reads the player's current health from the `MonoBehaviour` in the step above and early return if their health is at the maximum.

## Test your changes in a local deployment

The game logic is now in place, and we can test if it is working correctly. Follow these steps to test the feature:

**Step 1.** Enable the player health bar.

Modify the `OnScreenUI` prefab by enabling the **OnScreenUI** > **InGameScreens** > **InGameHud** > **HealthBar** game object.

This will display a health bar in the top left corner to make it easier for you to see how the health of a player changes.

**Step 2.** Build your workers.

Select **SpatialOS** > **Build For Local** > **UnityClient**.

To fully test our changes, we will need to launch two clients so you can shoot yourself. We will launch one of these as a built-out client-worker.

If you are running your workers from within your Unity Editor a build is not necessary, however in a moment we will launch a built-out client-worker. Building the workers is therefore essential.

**Step 3.** Launch a local deployment.

From the Unity Editor menu, select **SpatialOS** > **Local launch**. This opens a terminal which notifies you when the deployment is up and running.

Alternatively you can enter `Ctrl + L`/`Cmd + L` in your Unity Editor.

**Step 4.** Launch a server-worker and client-worker in-editor.

With the `FPS-Development` Scene open in your Unity Editor, select the Unity `Play` button.

**Step 5.** Launch a built-out `UnityClient` worker-instance

From the Unity Editor menu, select **SpatialOS** > **Launch standalone client**.

This will launch an instance of your `UnityClient` in a separate window. This uses the built-out `UnityClient` worker, so make sure you have performed a "Build UnityClient for local" as in step 1.

**Step 6.** Use one client to shoot the other.

To see the effects of a health pack restoring a player's health you'll need to damage them first.

This may require some switching between the editor and your standalone client, but you should be able to steer one player entity to the other and shoot them a few times.

You can use the SpatialOS inspector to help you find where the two players are, and navigate them to the same location.

**Step 7.** Use the inspector to check the damage has been applied.

When a local deployment is running you can open the SpatialOS local inspector in your browser: http://localhost:21000/inspector/

By selecting the visual marker for an entity you can view its component values in the right-side panel, by expanding the `Components` section.

Component values can be found by expanding the namespace for that component. For `Player` health you can find this under the namespace **improbable** > **gdk** > **health** > **HealthComponent** > **health**.

![A picture showing how the above looks]({{assetRoot}}assets/health-pickups-tutorial/inspect-health.jpg)

**Step 8.** Walk the damaged player over the health pack and check if it is consumed and applied.
Once again, you can use the SpatialOS inspector to guide you if you aren't quite sure where on the map the player and the health pack are in relation to each other.

**Don't forget to check how much health the player has before walking through the health pack so you can compare the before and after!**

When the injured player collides with the health pack it should become invisible on the client. You can also check in the SpatialOS inspector to see whether the `HealthPickup` component for the health pack entity now shows its `IsActive` property value as `false`.

Finally, using the SpatialOS inspector, check how much health the player has after walking through the health pack. The `Player` health component can be found under the namespace **improbable** > **gdk** > **health** > **HealthComponent** > **health**.

You should also see the health pack reappear after a short time.

---

Here's how it should look:

<%(Video file="{{assetRoot}}assets/health-pickups-tutorial/health-pickup-demo.mp4")%>

<br/>

**That's it! Well done, and welcome to the GDK!**

We’d love to know what you think, and invite you to join our community on [our forums](https://forums.improbable.io/), or on [Discord](https://discordapp.com/invite/SCZTCYm).
