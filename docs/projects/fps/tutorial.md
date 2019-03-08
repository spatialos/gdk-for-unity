# Add your first feature: A Health Pick-up tutorial for the FPS Starter Project 

![In-game view of the health pickup prefab]({{assetRoot}}assets/health-pickups-tutorial/health-pickup-visible-1.png)

<%(TOC)%>

Before starting this tutorial, make sure you have followed the [Get started]({{urlRoot}}/content/get-started/get-started) guide which sets up the FPS Starter Project. This tutorial follows on from that guide.

## What does the tutorial cover?

You will add health pack pick-ups to the game. These pick-ups grant health to players who walk over them.

To implement this feature you will:

* Define a new [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) called `HealthPickup` and define its [component properties]({{urlRoot}}/content/glossary#spatialos-component).
* Add `HealthPickup` entities to the [snapshot]({{urlRoot}}/content/glossary#snapshot) so they appear in the [SpatialOS world]({{urlRoot}}/content/glossary#spatialos-world) at startup.
* Write logic so that health packs grant health to players.

## Open the FPS Starter Project in your Unity Editor
1. Launch your Unity Editor.
1. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

## Define a new SpatialOS entity

In this section we're going to define a new [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) called `HealthPickup`. SpatialOS entities are made up of SpatialOS components, which store the properties associated with that entity. Components are defined in your project's [schema]({{urlRoot}}/content/glossary#schema).<br>

### Define a new SpatialOS component
The first step in defining a new SpatialOS entity is defining its components and their properties. Let's do that now:

1. Using your file manager, navigate to `gdk-for-unity-fps-starter-project/schema`, then create a `pickups` directory.
1. Inside the `gdk-for-unity-fps-starter-project/schema/pickups/` directory, use a text editor of your choice to create a new file called `health_pickup.schema`.
1. Paste in the following definition and save the file:

```
package pickups;

component HealthPickup {
    id = 21001;
    bool is_active = 1;
    uint32 health_value = 2;
}
```
    This defines a new SpatialOS component called `HealthPickup`, and adds two properties:<br>
    * `is_active`: A flag indicating whether the health pack is active, or has been consumed.<br>
    * `health_value`: An integer value, indicating the amount of health the pack will grant to a player.<br>
    You may also notice the `id` property, this is a [Component ID](https://docs.improbable.io/reference/latest/shared/schema/reference#ids), you can ignore it for now.

1. Any time you modify your `schema` files you **must** then run code generation. To do this, select **Generate code** from the **SpatialOS** menu in your Unity Editor.<br><br>
Code generation creates C# helper classes based on the components and properties defined in the [schemalang](https://docs.improbable.io/reference/latest/shared/glossary#schemalang) snippet above. It therefore must be run in order to make use of your newly defined `HealthPickup` component within your game logic.<br>
**Note:** When writing schema files, your properties must use snake case (for example, "health_value"), but the code generation process will create the helper classes in PascalCase (for example, "HealthValue").

<%(#Expandable title="What happens if I don't run code generation?")%>If you do not run code generation after modifying your `schema` files (which includes adding, removing or editing existing `.schema` files) then the associated C# helper classes will not be generated. This will mean that your C# interface to the data model of your game will not match your the structures defined in your `schema`. This can be very confusing!

**Note:** Code generation is automatically run once whenever you open the FPS Starter Project in your Unity editor.

If you are worried your generated code is in a bad state (such as having helper classes for since-deleted components and properties) you can run **Generate code (force)** from the **SpatialOS menu** to ensure existing generated code is cleaned and regenerated.
<%(/Expandable)%>

<%(#Expandable title="Where is the generated code?")%>The generated classes for your component can be found in the `Assets/Generated/Source/improbable/` directory of your Unity project. Feel free to have a look if you want to see what happens behind the Scenes when you use a component. Note that you don’t need to understand the generated code in order to follow this tutorial.
<%(/Expandable)%>

### Define a new SpatialOS entity

Now that we've defined and generated the `HealthPickup` component and its properties, let's deifine the `HealthPickup` entity we're going to attach that component to.

All SpatialOS GDK projects contain a C# file that, once for each type of entity in your project, declares a function that defines which components should be instantiated when a new type of that entity is added to a [SpatialOS World]({{urlRoot}}/content/glossary#spatialos-world). The object that these functions return is an [entity template]({{urlRoot}}/content/entity-templates).

`HealthPickup` is a new type of entity, so we must create a new entity template. To do this, we'll need to add a new function within the `FpsEntityTemplates` class:

1. In your Unity Editor, locate `Assets/Fps/Scripts/Config/FpsEntityTemplates.cs` and open it in your code editor.
1. Ensure your code can reference the `Pickups` namespace by adding `using Pickups;` to the top of the file.
1. Define `HealthPickup`, an entirely new type of entity, by adding this new function within the `FpsEntityTemplates` class:
```csharp
public static EntityTemplate HealthPickup(Vector3f position, uint healthValue)
{
    var gameLogic = WorkerUtils.UnityGameLogic;

    var healthPickupComponent = new Pickups.HealthPickup.Snapshot { IsActive = true, HealthValue = healthValue };

    var entityTemplate = new EntityTemplate();
    entityTemplate.AddComponent(new Position.Snapshot { Coords = new Coordinates(position.X, position.Y, position.Z) }, gameLogic);
    entityTemplate.AddComponent(new Metadata.Snapshot { EntityType = "HealthPickup"}, gameLogic);
    entityTemplate.AddComponent(new Persistence.Snapshot(), gameLogic);
    entityTemplate.AddComponent(healthPickupComponent, gameLogic);
    entityTemplate.SetReadAccess(gameLogic, WorkerUtils.UnityClient);
    entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, gameLogic);

    return entityTemplate;
}
```

Let's break down what the above snippet does:<br>

 * The struct `Pickups.HealthPickup.Snapshot` was generated from the `HealthPickup` component you previously defined in schemalang.<br>
 * The line `entityTemplate.AddComponent(healthPickupComponent, gameLogic);` adds an instance of this struct to the `HealthPickup` entity.<br>
 * The line `entityTemplate.SetReadAccess(gameLogic, WorkerUtils.UnityClient);` states that both [server-workers]({{urlRoot}}/content/glossary#server-worker) (`gameLogic`) and [client-workers]({{urlRoot}}/content/glossary#client-worker) (`UnityClient`) have [read access]({{urlRoot}}/content/glossary#read-access) to this entity (that they can see health packs).
 * The line `entityTemplate.SetComponentWriteAccess(EntityAcl.ComponentId, gameLogic);` states that only server-workers have [write access]({{urlRoot}}/content/glossary#write-access) to the `healthPickupComponent`.<br>
We state this because we don't want clients to be able to alter how much health is in a health pack, that would be cheating.
 * You may also notice `Position`, `Metadata` and `Persistence`, these are [standard library](https://docs.improbable.io/reference/13.6/shared/schema/standard-schema-library) components that you can ignore for now.

<%(#Expandable title="How would you give only a specific client write-access for a component?")%>Some component data should be editable/updateable by the player's client, but not by the clients of any other players. In the FPS Starter Project the `Player` entity template function in `FpsEntityTemplates.cs` grants the player's client write access over a number of components: `clientMovement`, `clientRotation`, `clientHeartbeat` etc.

The information that specifies exactly _which_ client should be granted permission is passed into the function in the `clientAttributeSet` parameter. If you'd like to read more on where this information comes from you can read about the [entity lifecycle]({{urlRoot}}/content/entity-lifecycle).<%(/Expandable)%>

<%(#Expandable title="Can I rename my worker types?")%>Yes, worker types are customizable. As is typical for GDK projects, the FPS Starter Project uses `UnityGameLogic` for server-side workers, and `UnityClient` for client-side workers. To find out more about renaming worker types you can read about [build configuration]({{urlRoot}}/content/build).<%(/Expandable)%>

<%(#Expandable title="Can I specify more than one worker type to have write-access to a single component?")%>Yes, you are not restricted to just one worker type being granted write-access, but it's something to be careful of.

To find out about how to do this, read up about [worker attribute sets](https://docs.improbable.io/reference/latest/shared/worker-configuration/bridge-config#worker-attribute-sets).
<%(/Expandable)%>

## Add your new entities to the snapshot

In this section we’re going to add health pack entities to the SpatialOS world. There are two ways to do this:

* At runtime, by passing an `EntityTemplate` object to an entity creation function.
* At start-up, by adding health pack entities to the [Snapshot]({{urlRoot}}/content/glossary#snapshot), so they are already in the world when the game begins.

For health packs we will do the latter, so that when the game begins there will already be health packs in pre-defined locations.

### Edit the snapshot generation script.

The **SpatialOS** menu in your Unity Editor contains a **"Generate FPS Snapshot"** option. This option runs `Assets/Fps/Scripts/Editor/SnapshotGenerator/SnapshotMenu.cs`. We will now modify this script to add `HealthPack` entities to our snapshot:<br><br>

1. In your Unity Editor, locate `Assets/Fps/Scripts/Editor/SnapshotGenerator/SnapshotMenu.cs` and open it in your code editor.
1. Ensure your code can reference the `Vector3f` namespace by adding `using Improbable;` to the top of the file.
1. The function below contains logic for adding health pack entities to the snapshot object, and sets the amount of health the packs restore to `100`. Paste it inside the `SnapshotMenu` class.

```csharp
private static void AddHealthPacks(Snapshot snapshot)
{
    var healthPack = FpsEntityTemplates.HealthPickup(new Vector3f(5, 0, 0), 100);
    snapshot.AddEntity(healthPack);
}
```

1. Call your new function by pasting the below snippet inside the `GenerateFpsSnapshot()`. Be sure to paste this above the `SaveSnapshot` lines, so that it's run during snapshot generation.
```csharp
    AddHealthPacks(localSnapshot);
    AddHealthPacks(cloudSnapshot);
```

In your own game may want to consider moving default values (such as health pack positions, and health values) into a settings file. But for now, we will keep this example simple.

### Update the snapshot

All SpatialOS GDK projects contain a directory named `snapshots` in the root of the project. If you have updated the snapshot generation script `SnapshotMenu.cs`, as we did in the step above, or if you've altered components in an entity template, then your snapshot will be out of date, and must be regenerated.

1. Regenerate the `default.snapshot` file from the **SpatialOS** menu in your Unity Editor, by running **"Generate FPS Snapshot"**.
1. If you launch a local deployment (`Ctrl + L` in your Unity Editor), you should be able to see one `HealthPickup` entity in the [Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector).<br>
![World view in the Inspector showing the `HealthPickup` entity]({{assetRoot}}assets/health-pickups-tutorial/health-pickup-inspector-1.png)<br>
If we were to test the game at this point, the health pack entity would appear in the inspector but not in-game. This is because we have not yet defined how to represent the entity on your client or server-workers. We'll do this in the next section.
1. Before you move on, in the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.

<%(#Expandable title="Can I make my snapshots human-readable?")%>Yes, there is a `spatial` command that will convert snapshots to a human-readable format. However, you cannot launch a deployment from a human-readable snapshot, so it must be converted back to binary before it is usable. To find out more about working with snapshots you can read about the [spatial snapshot command](https://docs.improbable.io/reference/latest/shared/operate/snapshots#convert-a-snapshot).

While they are human-readable and you can manually edit the values of the properties within, however be careful not to make mistakes that will inhibit the conversion back to binary form!<%(/Expandable)%>

## Represent your new entity on your workers

In this section we’re going to decide how to represent the `HealthPickup` entity in our client-workers and server-workers, and implement this.

### Plan your entity representations

First we must think about how each of the workers should represent the entity, so let's quickly review how we want this game mechanic to play out:

* Players should see a health pack hovering just above the ground.
* When a player collides with the health pack. it is consumed and disappears.
* A health pack should only be consumed if the player is not already at full health.
* Consumed health packs re-appear after a cool-down, and are ready for use again.

We can neatly separate this logic between the client-side and server-side representations:

* The `UnityClient` client-worker should display a visual representation for each health pack in the world. It should only display health packs that are currently "active".
* The `UnityGameLogic` server-worker should, when a player collides with a health pack, check whether that player is injured and allows the player to consume the health pack if they are.

The FPS Starter Project uses the SpatialOS GDK's [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities). In this workflow SpatialOS entities are represented by Unity prefabs. Crucially, you can use different prefabs to represent the same type of entity on different types of workers. This allows you to separate client-side and server-side entity representation, as we planned above.

<%(#Expandable title="How does the GDK pair SpatialOS entities with Unity prefabs?")%>

The FPS Starter Project uses the [GDK GameObject Creation](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.gameobjectcreation) package which handles the instantiation of GameObjects to represent SpatialOS entities. This tracks associations between entities and prefabs by matching their `Metadata` component's metadata string to the names of prefabs in the `Assets/Fps/Resources/Prefabs/` directory. If the worker receives information about a new SpatialOS entity then the GameObject Creation package immediately instantiates a GameObject of the appropriate type to represent that entity.
<br><br>
Client-side entity prefabs are stored in `Assets/Fps/Resources/Prefabs/UnityClient`, while server-side ones are located at `Assets/Fps/Resources/Prefabs/UnityGameLogic`.<%(/Expandable)%>

<%(#Expandable title="What are the 'Authoritative' and 'NonAuthoritative' sub-folders for?")%>The `Assets/Fps/Resources/Prefabs/UnityClient/` folder contains two sub-folders, `Authoritative` and `NonAuthoritative`, and _both_ of them contain a `Player` prefab!

The FPS Starter Project has some custom logic specific to its `Player` entities. When creating your own entity prefabs for the `UnityClient` worker you can put them directly into `Assets/Fps/Resources/Prefabs/UnityClient/`.

If you are interested in why the FPS Starter Project named those sub-directories `Authoritative` and `NonAuthoritative`, it relates to write-access for components on the entity.

At any point in time a single entity may be known about by multiple workers, even of the same type. In a large game you might have multiple `UnityGameLogic` workers. These often overlap, which means that they both "know about" some of the same entities. However, only **one** of those workers can have write-access permissions to components on an entity at any given time.

That means even two workers of the same type (e.g. `UnityGameLogic`) may not have the same responsibilities for a particular entity. The **authoritative** worker (i.e. the one that has write-access) may be responsible for executing some logic and updating the entity's component data. The **non-authoritative** worker only has read-access, which it may use to drive its own local representation of that entity, but it shouldn't try to update the entity's component values (and wouldn't be able to if it tried!). These two workers have _different representations_ of the same entity, even though they are the same type of worker.

The `Player` entity has a special relationship with the `UnityClient` instance that is authoritative over it. That `UnityClient` is running on the gamer's machine, and that gamer "owns" that `Player` entity. It's their representation in the world. As such, there are big differences between how the `Player` entity should be represented on the authoritative client (it should have a camera, collect player input etc.) compared to if the `Player` entity represents someone else in the game. The FPS Starter Project has some additional logic to manage these representations as a way of keeping the code more organised.

Authority is a tricky topic with SpatialOS, particularly as write-access is actually defined on a per-component basis rather than a per-entity basis. You can find out more by reading up about [component authority]({{urlRoot}}/content/glossary#authority).<%(/Expandable)%>

##### Create a UnityClient entity prefab

1. In your Unity Editor, locate `Assets/Fps/Prefabs/HealthPickup.prefab`.
1. Create a copy of this prefab and place it in `Assets/Fps/Resources/Prefabs/UnityClient/`.
1. Add a new script component to the root of your `HealthPickup` prefab, name it `HealthPickupClientVisibility`, and replace its contents with the following code snippet:

```csharp
using Improbable.Gdk.GameObjectRepresentation;
using Pickups;
using UnityEngine;

namespace Fps
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class HealthPickupClientVisibility : MonoBehaviour
    {
        [Require] private HealthPickup.Requirable.Reader healthPickupReader;

        private MeshRenderer cubeMeshRenderer;

        private void OnEnable()
        {
            cubeMeshRenderer = GetComponentInChildren<MeshRenderer>();
            healthPickupReader.ComponentUpdated += OnHealthPickupComponentUpdated;
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

<%(#Expandable title="Can I name the prefab something else?")%>Your choice of prefab name can be anything, but **must** match the string you used for the entity's `Metadata` component when you wrote the entity template function for this entity.

This is because the FPS Starter Project uses the GDK GameObject Creation package as part of the MonoBehaviour workflow. To find out more you can read up about the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities).<%(/Expandable)%>

<%(#Expandable title="What's the best way to create a prefab?")%>Prefabs are the Unity Engine approach for creating templates of GameObject hierarchies. [Creating Prefabs (Unity Documentation)](https://docs.unity3d.com/Manual/CreatingPrefabs.html) explains how to create one.

Freshly created prefabs are initially empty, so you can drag other prefabs or GameObjects onto them to add them to the hierarchy.

It can often be easiest to drag prefabs into a Scene to edit them - just remember to apply your change (in the Unity Inspector panel) and delete them from the Scene when you are done editing! In upcoming versions of Unity Engine you will be able to make use of [prefab mode](https://blogs.unity3d.com/2018/06/20/introducing-new-prefab-workflows/) for this task.<%(/Expandable)%>

<%(#Expandable title="Wait! Why aren't we removing the callback when the script is disabled?")%>The GDK automatically clears event handlers when a script is disabled, therefore you do not need to manually remove the `OnHealthPickupComponentUpdated` callback.<%(/Expandable)%>

This script is mostly standard C# code that you could find in any game built with Unity Engine. There are a few annotations which are specific to the SpatialOS GDK though, let's break those down:

```csharp
[WorkerType(WorkerUtils.UnityClient)]
```

This annotation decorating the class indicates the `WorkerType` of the script (in this case, `UnityClient`). This provides information for SpatialOS when it is building out your separate workers: a script with the client annotation should never be enabled on a `UnityGameLogic` worker. You can use these `WorkerType` annotations to control where your code runs. If a script should exist and be able to run on both client-side and server-side workers then this annotation can be omitted.

This `HealthPickupClientVisibility` script is going to rely on the `is_active` property of your new `HealthPickup` component, so the package declared in the `health_pickup.schema` file appears in a `using` statement at the top of the script:

```csharp
using Pickups;
```

This namespace is part of the helper classes that the code generation phase created from your schema.

To make use of component data, either to read from it or write to it, we can use SpatialOS GDK syntax to inject the component into the script.

```csharp
[Require] private HealthPickup.Requirable.Reader healthPickupReader;
```

The worker on which the code is running interprets this statement as an instruction to enable this script component on a particular entity's associated GameObject, but only if that entity has a `HealthPickup` component and the worker has read-access to that component. Read-access is rarely limited, but the same syntax can be used with `Writer` instead of `Reader`, which would make the requirement even stricter. With a `Writer`, the script would only be enabled on the single worker that has write-access to the `HealthPickup` component on that entity.

These `[Require]` attributes are another powerful way to control where your code is executed. For the purpose of this script we only need to _read_ the health pack's data when deciding how to visualise it, so only a `Reader` is necessary.

You can see a use of the `HealthPickup` component's data in the `UpdateVisibility()` function:

```csharp
cubeMeshRenderer.enabled = healthPickupReader.Data.IsActive;
```

When you wrote the schema for the `HealthPickup` component you included a bool property called `is_active`, and code generation has created the `IsActive` member within the reader's `Data` object. We'll cover updating component property values later in this tutorial.

Setting the `cubeMeshRenderer.enabled` according to whether the health pack is "active" or not only works if `cubeMeshRender` correctly references the mesh renderer.

The client-side representation of the health pack entity is now complete! Next we will test the game so far and make sure we are visualising the health packs correctly.

<%(#Expandable title="Are entities always represented by GameObjects?")%>No, exactly how entities are represented on each of your workers is up to you.

The GDK also offers an [ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities) represents them as a grouping of Unity ECS components. If you are more familiar with the traditional Unity GameObject style of development then the GDK provides a [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities) for you.

You are not limited to these options either, and can configure your worker to create something very custom when it encounters a particular entity type.<%(/Expandable)%>

## Test your changes

Our aim is to have health packs which restore lost health to players. So what have we accomplished so far?

* You defined the schema for your health packs: a new component containing properties for how much health it will grant and whether it's ready to do so.
* You created an entity template function which provides a central definition of a particular entity type and can create `Entity` objects.
* You added an instance of the health pack entity type to the snapshot so it will be present in the world when the game begins.
* You associated a local representation with your new SpatialOS entity so that Unity will know how to visually represent any health pack it encounters.

You can launch a local deployment of your updated game world from the **SpatialOS menu** within your Unity Editor by selecting **"Local launch"** (`Ctrl + L`). This will open a terminal that should tell you when the world is ready.

Once the world is ready you can:

* View all entities in the inspector from your browser: http://localhost:21000/inspector/
* Open the `FPS-Development` Scene in your Unity Editor. The Scene file is located in **Assets** > **Fps** > **Scene**.
* Press Play in your Unity Editor to play the game.

![In-game view of the health pickup prefab]({{assetRoot}}assets/health-pickups-tutorial/health-pickup-visible-1.png)

You'll know it's worked if you can see a `HealthPickup` entity in the inspector, and find a floating health pack when running around in-game. But currently it just sits there, inert. If you walk into it, nothing happens. Let's fix that!

Our next step will be to add some game logic to the health pack so that it reacts to player collisions and grants them health.

<%(#Expandable title="How does the Inspector decide the entity name?")%>In your entity template function the compulsory `Metadata` component required a string as a parameter, and we gave it "HealthPickup", but could have used any string. The metadata is intended to be a friendly identifier for the entity type, and as such is used by the Inspector to label your entity.

If you are using the SpatialOS GDK's MonoBehaviour workflow then the `Metadata` string must match the name of the entity prefab that will represent it.<%(/Expandable)%>

Before you move on, in the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.

## Add health pack logic

If we were to test the game at this point we would now see the health pack entity in-game, but we've not yet given it the consumption behaviour.

The server-side logic we want to capture for this game mechanic is:

* Tracking player collisions with the health pack.
* Checking the pre-conditions: player must be injured, health pack must be active.
* Granting health to a player when the conditions are met.

To do this we will need to create a server-side representation of the health pack, and add a script to the health pack which can both read its own component data (to check if the health pack is active) as well as that of the player entity (to check if it is injured). After that, if conditions are met, it then must also be able to _update_ its own component data (to set itself to "inactive"), and that of the player entity (to grant it health).

### Create a UnityGameLogic entity prefab

In the FPS Starter Project the server-side worker is called `UnityGameLogic`.

Create a copy of `Assets/Fps/Prefabs/HealthPickup.prefab` in the `Assets/Fps/Resources/Prefabs/UnityGameLogic/` folder.

Then, add a script component to your new prefab called `HealthPickupServerBehaviour` and replace its contents with the following code snippet which contains a couple of pieces of code we still need to write:

```csharp
using System.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.Health;
using Pickups;
using UnityEngine;

namespace Fps
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class HealthPickupServerBehaviour : MonoBehaviour
    {
        [Require] private HealthPickup.Requirable.Writer healthPickupWriter;
        [Require] private HealthComponent.Requirable.CommandRequestSender healthCommandRequestSender;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            // OnTriggerEnter is fired regardless of whether the MonoBehaviour is enabled or disabled.
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
            // Replace this comment with your code for updating the health pack component's "active" property.
        }

        private void HandleCollisionWithPlayer(GameObject player)
        {
            var playerSpatialOsComponent = player.GetComponent<SpatialOSComponent>();

            if (playerSpatialOsComponent == null)
            {
                return;
            }

            // Replace this comment with your code for notifying the Player entity that it will receive health.

            // Toggle health pack to its "consumed" state.
            SetIsActive(false);
        }
    }
}
```

This code snippet contains two comments that are placeholders for code which we will now write.

The first placeholder is in a function called `SetIsActive`. `IsActive` is the name of the bool property in the `HealthPickup` component you created, and we want this function to perform a **component update**, setting the value of `IsActive` to a given value. We want this update to be synchronized across all workers - updating the true state of that entity - and that can only be done by the single worker that has write-access.

<%(#Expandable title="Why is only one worker at a time able to have write-access for a component?")%>This prevents simultaneous changes putting the world into an inconsistent state. It is known as the single writer principle. If you want to learn more when you're done with the tutorial, have a look at [Understanding read and write access](https://docs.improbable.io/reference/latest/shared/design/understanding-access).<%(/Expandable)%>

To make sure this script is only enabled on the worker with write-access it already has a statement at the top of the class which "requires" a `Writer`:

```csharp
[Require] private HealthPickup.Requirable.Writer healthPickupWriter;
```

A `Writer` can also be used to _read_ component data from its respective component (in this case, `HealthPickup`), but it provides an API for updating the values of properties too.

Unity Engine's `OnTriggerEnter` function is a special-case when a script is disabled. Other functions will only be called if the script component's `enabled` property is true, but `OnTriggerEnter` will be called even if it is false. This means it is an exception to the normal behaviour of the `[Require]` syntax. Because of this, scripts which use `OnTriggerEnter` **must** check whether the `Writer` is null (indicating a lack of authority) before using functions on the writer.

To update the `HealthPickup` component we must construct a `HealthPickup.Update` object, and **send** it to SpatialOS. You can replace the comment in the `SetIsActive` with the following snippet to handle creating and sending this update:

```csharp
healthPickupWriter?.Send(new HealthPickup.Update
{
    IsActive = new Option<BlittableBool>(isActive)
});
```

There is one more comment we must replace with code before our logic will work, which is found in the `HandleCollisionWithPlayer` function. The `Player` entity prefab has already been given a "Player" tag, so this function will be called any time a player walks through a health pack.

### Cross-worker interaction using commands

Our `HandleCollisionWithPlayer` function will run on a `UnityGameLogic` worker (because of the `[WorkerType(WorkerUtils.UnityGameLogic)]` annotation). That worker executes the code on behalf of each `HealthPack` entity for which is has `HealthPickup` component write-access (because of the `Writer` requirement). If your game has multiple `UnityGameLogic` workers then the worker with write-access for the `HealthPack` entity may not be the same worker that has write-access for the `Player` entity who has tried to pick up the health pack.

If we want a `HealthPack` entity's script to update the `Player` entity's current health we must notify the responsible worker and _request_ the update. SpatialOS allows you to do this with [commands](https://docs.improbable.io/reference/latest/shared/glossary#command), which are defined in **schema**.

The `Player` entity already has a `Health` component which defines both a property representing current health and a **command** called `modify_health`. The definition of that command in `health.schema` is:

```
command improbable.common.Empty modify_health(HealthModifier);
```

This command definition states that a `HealthModifier` object must be passed as an argument with the command request. `HealthModifier` is a type that is also defined in `health.schema`, which simply bundles a few properties together.

When you send a command it acts as a request which SpatialOS will deliver to the single worker that has write-access for the component in which the command is defined. If that worker has handling logic for that type of command then it will be triggered, and many commands also return a response. A command response can provide information generated as a result of the request, or can be used to indicate whether the request was accepted or not.

For this command the return type is specified as `improbable.common.Empty`, as we don't want to return any information to the sender.

In your `HandleCollisionWithPlayer` function, you can replace the placeholder comment with the following code snippet:

```csharp
healthCommandRequestSender.SendModifyHealthRequest(playerSpatialOsComponent.SpatialEntityId, new HealthModifier
{
    Amount = healthPickupWriter.Data.HealthValue
});
```

The top of the `HealthPickupServerBehaviour` class has a `[Require]` statement for `HealthComponent.Requireable.CommandRequestSender` which, appropriately enough, is necessary for sending commands that are defined in the `Health` component. Even though the `Health` component belongs to the `Player` entity, and this script is on the `HealthPickup` entity, this statement is still required.

Command request sender objects are automatically generated for you during code generation, based on your schema, and it provides a "send" function for each of the component's commands. Just by defining your commands in schema and running code generation all of these helper classes are generated ready for you to use.

The code snippet calls `SendModifyHealthRequest`, specifies the entity id of the recipient entity (in this case the player entity), and constructs a `HealthModifier` object with the appropriate data. In our case the amount of health we wish to grant is based on the `HealthPickup` entity's `HealthValue` property, so we retrieve the value from `healthPickupWriter.Data.HealthValue`.

The `ModifyHealth` command is already used by the FPS Starter Project for applying damage as part of the shooting game mechanics. As this is the case we don't need to write any _additional_ logic for applying the health increase.

<%(#Expandable title="How are clients prevented from sending health-giving commands?")%>They're not - any worker can send a command. However, only the worker which has write access to the component holding a command is allowed to handle it. Each command request contains information about the caller of the command which could be used to enforce restrictions. Have a look at `ServerHealthModifierSystem.cs` in the Health feature module.<%(/Expandable)%>

<%(#Expandable title="Could you put the collision logic on the 'Player' instead?")%>Yes, and that would actually be better in some ways. If the collision with a health pickup is detected by the player, the command to update health can be replaced with a component update. This is a major simplification and should also have better performance. As a rule of thumb, always prefer component updates over commands. We have introduced you to commands so that you know their power, but it is your responsibility to use them wisely.<%(/Expandable)%>

### Respawn health packs

Our health packs use the `IsActive` property to indicate whether they can be visualised (and whether they will grant health on collision). One final feature we will add is a co-routine to re-activate consumed health packs after a cool-down period.

In your `HealthPickupServerBehaviour` class, define a coroutine function that looks something like this:

```csharp
private IEnumerator RespawnCubeRoutine()
{
    yield return new WaitForSeconds(15f);
    SetIsActive(true);
}
```

Your coroutine should be started at the end of the `HandleCollisionWithPlayer` function. You should also start the coroutine in `OnEnable` for any health pack entities which are inactive (`IsActive` equal to `false`) to begin with. Running coroutines should also be stopped in `OnDisable`.

<%(#Expandable title="Expand for completed `HealthPickupServerBehaviour` snippet.")%>
```csharp
using System.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.Health;
using Pickups;
using UnityEngine;

namespace Fps
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class HealthPickupServerBehaviour : MonoBehaviour
    {
        [Require] private HealthPickup.Requirable.Writer healthPickupWriter;
        [Require] private HealthComponent.Requirable.CommandRequestSender healthCommandRequestSender;

        private Coroutine respawnCoroutine;

        private void OnEnable()
        {
            // If the pickup is inactive on initial checkout - turn off collisions and start the respawning process.
            if (!healthPickupWriter.Data.IsActive)
            {
                respawnCoroutine = StartCoroutine(RespawnCubeRoutine());
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
            healthPickupWriter?.Send(new HealthPickup.Update
                {
                    IsActive = new Option<BlittableBool>(isActive)
                });
        }

        private void HandleCollisionWithPlayer(GameObject player)
        {
            var playerSpatialOsComponent = player.GetComponent<SpatialOSComponent>();

            if (playerSpatialOsComponent == null)
            {
                return;
            }

            healthCommandRequestSender.SendModifyHealthRequest(spatialOsComponent.SpatialEntityId, new HealthModifier
            {
                Amount = healthPickupWriter.Data.HealthValue
            });

            // Toggle health pack to its "consumed" state
            SetIsActive(false);

            // Begin cool-down period before re-activating health pack
            respawnCoroutine = StartCoroutine(RespawnCubeRoutine());
        }

        private IEnumerator RespawnCubeRoutine()
        {
            yield return new WaitForSeconds(15f);
            SetIsActive(true);
        }
    }
}
```
<%(/Expandable)%>

<%(#Expandable title="Why would IsActive be false in OnEnable()?")%>In a large game, where there are multiple workers of each type (such as multiple `UnityGameLogic` workers), it is the SpatialOS load-balancer that decides how to divide write-access for components between the available workers. At run-time the load-balancer will dynamically adjust these authorities to provide the best performance.

The coroutine is local only to the worker that started it. If the worker loses write-access to the component then the script will become disabled for that entity (and if you've added a `StopCoroutine` call to `OnDisable` then the coroutine will be appropriately cancelled). In theory this leaves the entity in a state where `InActive` is false, and no coroutine is running that will eventually change that.

Just as one worker lost write-access, another will be granted it by the load-balancer. That newly authoritative worker now meets the criteria specified by the `[Require]` syntax in the `HealthPickupServerBehaviour` class, and so the script will become enabled.

This is why you should specify in `OnEnable()` that if the `IsActive` property is false at that point then the coroutine should be initiated.

The newly authoritative worker will not know how long the cool-down had already been running on the previous worker, so the cool-down timer is ultimately "refreshed" at this point. If this was a problem for our mechanic then we could store the timer's progress in a new property, but in this case we will keep it simple.<%(/Expandable)%>

### Optional: Ignore healthy players

The `HandleCollisionWithPlayer` function in your `HealthPickupServerBehaviour.cs` script currently attempts to heal any colliding player. If the player is already on full health we might want to ignore them so that the health pack is not consumed and the health modifier command is never sent.

At the beginning of the `HandleCollisionWithPlayer` function you could add an if-statement which reads the player's current health from a Monobehaviour (that you will need to write) and returns early if their health is at the maximum. In the code that handles incoming `ModifyHealthRequest` commands the player's health is clamped to the value of the `Health` component's `max_health` property, but it's preferable to avoid sending the request if we know it will be denied anyway.

## Test health pick-ups locally

The distributed game logic is now in place, and we can test if it is working correctly. To test this feature, you can follow these steps:

<%(#Expandable title="1. Enable the player health bar.")%>Modify the `OnScreenUI` prefab by enabling the **OnScreenUI** > **InGameHud** > **HealthBar** game object. This will display a health bar in the top left corner to make it easier for you to see how the health of a player changes.

![A GIF showing the steps to enable the health bar UI]({{assetRoot}}assets/health-pickups-tutorial/health-bar-enable.gif)
<%(/Expandable)%>

<%(#Expandable title="2. Build your workers.")%>Select **SpatialOS** > **Build For Local** > **UnityClient**.

This is necessary because you have modified the code for the workers. If you are running your workers from within your Unity Editor a build is not necessary, however in a moment we will launch a built-out client-worker. Building the workers is therefore essential.<%(/Expandable)%>

<%(#Expandable title="3. Launch a local deployment.")%>From the **SpatialOS** menu, select **Local launch**. This opens a terminal which notifies you when the deployment is up and running.<br>

Alternatively you can enter `Ctrl + L` in your Unity Editor.

It also provides a convenient link for the local SpatialOS Inspector.<%(/Expandable)%>

<%(#Expandable title="4. Launch a built-out `UnityClient` worker.")%>From the **SpatialOS** menu, select **Launch standalone client**.

This will launch an instance of your `UnityClient` in a separate window. This uses the built-out `UnityClient` worker, so make sure you have performed  a "Build UnityClient for local" as in step 1.<%(/Expandable)%>

<%(#Expandable title="5. Launch a second client in-editor.")%>With the `FPS-Development` Scene open in your Unity Editor, select the Unity `Play` button.<%(/Expandable)%>

<%(#Expandable title="6. Use one client to shoot the other.")%>To see the effects of a health pack restoring a player's health it's a good idea to damage them first. Particularly if you made the optional changes to enforce the maximum health for players, you'll want to confirm that the health pack isn't disappearing without performing its health-giving duty.

This may require some switching between the editor and your standalone client, but you should be able to steer one player entity to the other and shoot them a few times.

You can use the SpatialOS inspector to help you find where the two players are, and navigate them to the same location. You can also select the icon for the damaged player and, in the right-side of the SpatialOS inspector, view the component data for the player's `Health` component to confirm that damage from the gunfire has been applied.<%(/Expandable)%>

<%(#Expandable title="7. Use the inspector to check the damage has been applied.")%>When a local deployment is running you can open the SpatialOS local inspector in your browser: http://localhost:21000/inspector/

By selecting the visual marker for an entity you can view its component values in the right-side panel, by expanding the `Components` section.

Component values can be found by expanding the namespace for that component. For `Player` health you can find this under the namespace **improbable** > **gdk** > **health** > **HealthComponent** > **health**.

![A picture showing how the above looks]({{assetRoot}}assets/health-pickups-tutorial/inspect-health.jpg)<%(/Expandable)%>

<%(#Expandable title="8. Walk the damaged player over the health pack and check if it is consumed and applied.")%>Once again, you can use the SpatialOS inspector to guide you if you aren't quite sure where on the map the player and the health pack are in relation to each other.

**Don't forget to check how much health the player has before walking through the health pack so you can compare the before and after!**

When the injured player collides with the health pack it should become invisible on the client. You can also check in the SpatialOS inspector to see whether the `HealthPickup` component for the health pack entity now shows its `IsActive` property value as `false`.

Finally, using the SpatialOS inspector, check how much health the player has after walking through the health pack. The `Player` health component can be found under the namespace **improbable** > **gdk** > **health** > **HealthComponent** > **health**.<%(/Expandable)%>

If you implemented the respawn coroutine then you should also see the health pack reappear after a short time. Here's how it should look:

<%(Video file="{{assetRoot}}assets/health-pickups-tutorial/health-pickup-demo.mp4")%>

<br/>
<br/>
**That's it! Well done, and welcome to the GDK!**

We’d love to know what you think, and invite you to join our community on [our forums](https://forums.improbable.io/), or on [Discord](https://discordapp.com/invite/SCZTCYm).
