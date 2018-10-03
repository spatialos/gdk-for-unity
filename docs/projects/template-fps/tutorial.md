
# Setup

// Make sure you setup using the setup guide

# What will be covered?

This tutorial will implement a simple health pack pickup that will grant health to players who walk over it. The amount of health granted will be fixed, and the health pack will be consumed through use, and respawn a little later.

To implement this feature we will:

* Define a new health pack entity and its data
* Add health pack entities to snapshot so they appear in the world
* Write pick-up logic so packs grant health to players

# Defining a new entity type

Every SpatialOS entity consists of SpatialOS components, defined in the project's [schema](fix).

In the FPS Template project the existing entities, such as the `Player` entity type, have components for movement, health, shooting and other mechanics. The project is constructed from GDK packages, such as the `SpatialOS GDK Health` package, which you can open from within the Unity project. In many of these packages you will find a `Schema` folder, with files like `health.schema`.

### Entity components

We will give your health pack entities two pieces of data by defining it in a new component:

* The amount of health the pack will granted.
* A flag indicating whether the health pack is active, or has been consumed.


Create a `schema` directory in your project root, if it doesn't already exist. Then create a `pickups` directory within that.

<%(#Expandable title="Where is my 'project root'?")%>Your SpatialOS 'project root' is the directory top-level directory which also contains your `spatialos.json` configuration file, and likely a directory called `workers` that contains your Unity project. You will not be able to see this directory from within your Unity editor, and should use File Explorer (on Windows) or Finder (on Mac) to create your new directory.<%(/Expandable)%>

Within your `/schema/pickups/` directory create a new file called `HealthPickup.schema` and paste in the following definition:

```
package pickups;
component HealthPickup {
    id = 21001;
    bool is_active = 1;
    uint32 health_value = 2;
}
```

[!!!](TODO: Explain namespaces and packages? Maybe not.)

This defines a new SpatialOS component called `HealthPickup`, and adds two properties: `is_active` and `health_value`. The is must be chosen by you and the only requirement is that it is unique among all other components in the project. Each of the properties has a property number (e.g. `= 1`) associated with it, which is **not** an initial value. It is an id number which identifies the order in which these properties will appear within the component.

[!!!](TODO: How do you run codegen?)

To write C# code that makes use of your newly defined `HealthPickup` component a range of helper classes are generated. It's worth noting that when writing schemalang your properties must use snake case (e.g. "health_value"), and the code generation process will create the helper classes with capitalised camel-case (i.e. "HealthValue").

### Entity templates

SpatialOS components must be grouped together to create the definition of your health pack entity type.

Typical for SpatialOS GDK projects, the FPS Template contains a C# file that declares a function for each type of entity. Calling the function returns an `Entity` object, with the contents of the function defining which components form that entity type. Extending an existing entity type is as easy as adding additional components while the entity type is being constructed.

You can find this file in your Unity project: `/Assets/Fps/Scripts/Config/FpsEntityTemplates.cs`.

To define an entirely new entity type we will need to add a new function within the `FpsEntityTemplates` class:

```
public static Entity HealthPickup(Vector3f position, float healthValue)
{
    var gameLogic = WorkerUtils.UnityGameLogic;

    var healthPickupComponent = Pickups.HealthPickup.Component.CreateSchemaComponentData(true, healthValue);

    return EntityBuilder.Begin()
        .AddPosition(position.X, position.Y, position.Z, gameLogic)
        .AddMetadata("HealthPickup", gameLogic)
        .SetPersistence(true)
        .SetReadAcl(AllWorkerAttributes)
        .AddComponent(healthPickupComponent, gameLogic)
        .Build();
}
```

The [EntityBuilder](fix) syntax provides a compact way to declare the relevant components. You may notice that `.AddPosition`, `.AddMetadata`, and `.SetPersistence` appear in the entity template function of _every_ entity type. This is because these are mandatory "well-known components" that SpatialOS GDK for Unity expects.

[!!!](TODO: Talk about position being default/required)

[!!!](TODO: Troubleshooting for HealthComponent not recognised?)

#### Adding components

Your `HealthComponent` has generated a the `Pickups.HealthPickup.Component.CreateSchemaComponentData()` function. The property numbers (i.e. `= 1` and `= 2`) determine the order of properties the function expects as parameters.

This component can then be added to the `HealthPickup` entity using the line: `.AddComponent(healthPickupComponent, gameLogic)`. The three "well-known components" (`Position`, `Metadata` and `Persistence`) must appear in that order, but after that you are free to add your remaining components in any order you like. Just remember that to complete the pattern the final statement must be `.Build();`.

#### Setting permissions (ACLs)

[Access Control Lists](fix) are how SpatialOS specifies which workers have permission to read-from or write-to the values of certain components. There may be data which you want to be kept private to server-side workers (because clients might use that knowledge to cheat!). Some components should definitely restrict their write-access to specific workers (e.g. a particular player's client) or to server-side workers only, to prevent exploits. For example, in an RPG a player should probably not be able to update the amount of gold they are carrying (at least, not without the server-side validating they aren't cheating!).

In the EntityBuilder syntax, the `.SetReadAcl(AllWorkerAttributes)` statement stated that all worker types should be able to read the data for this entity.

For each of the other components, such as your newly added `HealthPickup` component, the worker type which is given write-access is specified as a second argument to the component-adding function, e.g. `WorkerUtils.UnityGameLogic`. This is simply a string which identifies which type of `WorkerPlatform` should be granted the relevant permission.

For this project, `UnityGameLogic` indicates that that worker is one for handling server-side game logic. The identifier `WorkerUtils.UnityClient` would indicate that all clients are granted the relevant permission, but in this case we don't want clients to be able to alter how much health a health pack grants players, so we pass `WorkerUtils.UnityGameLogic` as the second parameter when adding the `healthPickupComponent`.

[!!!](TODO: Check whether this is correct about single string ACLs)

<%(#Expandable title="How would you give only a specific client write-access for a component?")%>Some component data should be editable/updateable by the player's client, but not by the clients of any other players. In the FPS Template project the `Player` entity template function in `FpsEntityTemplates.cs` grants the player's client write-access over a number of components: clientMovement, clientRotation, clientHeartbeat etc.

The information that specifies exactly _which_ client should be granted permission is passed into the function in the `clientAttributeSet` parameter. If you'd like to read more on where this information comes from you can read about the [entity lifecycle](fix).<%(/Expandable)%>

<%(#Expandable title="Can I rename my worker types?")%>Yes, worker types are customizable. As is typical for GDK projects, the FPS Template uses `UnityGameLogic` for server-side workers, and `UnityClient` for client-side workers. To find out more about renaming worker types you can read about [build configuration](fix)<%(/Expandable)%>

### Adding entities to the world

Once an entity template function exists you have a way to construct the _template_ of an entity. To add a health pack entity to the world

* At runtime, by passing an `Entity` object to an entity creation function.
* At start-time, by adding an entity instance to the [Snapshot](fix) so it is already in the world when the game begins.

For health packs we will do the latter. When the game begins there will already be health packs in pre-defined locations.

#### Editing snapshot generation

The SpatialOS menu option in the Unity editor include an item **"Generate FPS Snapshot"**. This runs the script `/Assets/Fps/Scripts/Editor/SnapshotGenerator/SnapshotMenu.cs`, which you can find from within your Unity editor.

We will modify the snapshot generating logic in two steps:
1. Adding a function to create and add a `HealthPack` entity.
2. Fix the package import settings so use of `Vector3f` is valid.

Within the `SnapshotMenu` class, add a new function that will contain logic for adding health pack entities to the snapshot object:

```
private static void AddHealthPacks(Snapshot snapshot)
{
    var healthPack = FpsEntityTemplates.HealthPickup(new Vector3f { X = 5, Y = 0, Z = 0 }, 100);
    snapshot.AddEntity(healthPack);
}
```

The `Vector3f` type is a struct provided in the `Improbable` namespace, and initially the above code snippet will produce errors. Adding `using Improbable;` to the top of `SnapshotMenu.cs` is necessary, but an additional change is necessary. In your Unity editor Project hierarchy, navigate to `/Assets/Fps/Scripts/Editor/`, and select `Improbable.Fps.Editor` so that its Import Settings can be viewed in the Unity inspector panel.

The `Improbable.FPS.Editor Import Settings` contains a section called `References`. You must click the `+` button to add a new reference, double-click the new field and in the asset-finder pop-up that appears you must double-click `Improbable.Gdk.Generated`. This is because the `Vector3f` struct is defined within that particular package, and we must declare our intent.

Once you've added and applied this new package reference your use of `Vector3f` in `SnapshotGenerator.cs` will now be valid.

<%(#Expandable title="Why does the FPS Template use packages?")%>Unity's packaging system is a great way to organize your code. SpatialOS GDK projects use packages extensively to provide modular code that can be easily imported and re-used across projects. If you'd like to know more about packages you can read up about [GDK Feature Modules](fix) which make good use of packages.<%(/Expandable)%>

This script now creates a health pack entity at the origin, and sets the amount of health it will restore to 100. Don't forget to call your new function from within `GenerateDefaultSnapshot()` (and pass it the `snapshot` object) or else it wont be run during snapshot generation!

It's a great idea to separate default values (such as health pack positions, and health values) into a settings file. In the FPS Template project you can find lots of examples of using Unity `ScriptableObject` components for exactly that. But for now, we will keep this example simple.

#### Updating the snapshot

Snapshot files are found in your project root directory, in a directory named `snapshots`. The FPS Template project includes a snapshot called `default.snapshot`.

If you have updated the snapshot generation function (as you just did in the step above), or if you've altered which components are specified in one of your entity templates, then your snapshot will be out of date. The snapshot is a big list of entities, and all their individual component values, so any change to these and the snapshot file must be regenerated.

You can regenerate the `default.snapshot` file from the SpatialOS menu option in the Unity editor, by running **"Generate FPS Snapshot"**.

<%(#Expandable title="Can I make my snapshots human-readable?")%>Yes, there is a `spatial` command that will convert snapshots to a human-readable format. However, you cannot launch a deployment from a human-readable snapshot, so it must be converted back to binary before it is usable. To find out more about working with snapshots you can read about the [spatial snapshot command](fix).

While they are human-readable you are able to manually edit the values of the properties within, however be careful not to make mistakes that will inhibit the conversion back to binary form!<%(/Expandable)%>

### Representing the entity on your workers

If we were to test the game at this point, the health pack entity would appear in the inspector but not in-game. This is because every worker needs to know how to represent the entity.

SpatialOS will manage which subset of the world's entities each worker knows about, and provide them with the corresponding component data. You must define what the worker will do when it finds out about an entity it isn't currently tracking. Fortunately the SpatialOS GDK for Unity provides some great tools for exactly that!

#### Planning your entity representations

First we must think about how each of the workers will want to represent the entity, so let's return to how we want our game mechanic to play out:

* Players should see a hovering health pack.
* When a player runs through the health pack it is consumed and 'disappears', leaving just a marker on the ground.
* Consuming a health pack should only occur if the player has taken damage.
* Consumed health packs re-appear after a cool-down, and are ready for use again.

We can neatly separate this logic between the client-side and server-side representations:

* The `UnityClient` worker should display a visual representation for each health pack, based on whether the health pack is currently 'active'.
* The `UnityGameLogic` worker should react to collisions with players, check whether they are injured, and consume the health pack if they are.

#### Creating GameObject representations

The FPS Template project uses the SpatialOS GDK's GameObject workflow, which is the familiar way of working with Unity Engine.

In the GameObject workflow you can associate a Unity prefab with your entity type, with separate prefabs for your `UnityClient` and `UnityGameLogic` workers. All entity prefabs should be added to `/Assets/Resources/Prefabs/UnityClient` and `/Assets/Resources/Prefabs/UnityGameLogic` respectively.

The FPS Template project uses the "GDK GameObject Creation" package which handles the instantiation of GameObjects to represent SpatialOS entities. This tracks associations between entities and prefabs by matching their `Metadata` component's metadata string to the names of prefabs in the `/Assets/Resources/Prefabs/` directory. If the worker receives information about a new SpatialOS entity then the GameObject Creation package immediately instantiates a GameObject of the appropriate type to represent that entity.

<%(#Expandable title="What are the 'Authoritative' and 'NonAuthoritative' sub-folders for?")%>The `/Assets/Resources/Prefabs/UnityClient/` folder contains two sub-folders, `Authoritative` and `NonAuthoritative`, and _both_ of them contain a `Player` prefab!

The FPS Template project has some custom logic specific to its `Player` entities. When creating your own entity prefabs for the `UnityClient` worker you can put them directly into `/Assets/Resources/Prefabs/UnityClient/`.

If you are interested in why the FPS Template project named those sub-directories `Authoritative` and `NonAuthoritative`, it relates to write-access for components on the entity.

At any point in time a single entity may be known about by multiple workers, even of the same type. In a large game you might have multiple `UnityGameLogic` workers. These often overlap, which means that they both 'know about' some of the same entities. However, only **one** of those workers can have write-access permissions to components on an entity at any given time.

That means even two workers of the same type (e.g. `UnityGameLogic`) may not have the same responsibilities for a particular entity. The **authoritative** worker (i.e. the one that has write-access) may be responsible for executing some logic and updating the entity's component data. The **non-authoritative** worker only has read-access, which it may use to drive its own local representation of that entity, but it shouldn't try to update the entity's component values (and wouldn't be able to if it tried!). These two workers have _different representations_ of the same entity, even though they are the same type of worker.

The `Player` entity has a special relationship with the `UnityClient` instance that is authoritative over it. That `UnityClient` is running on the gamer's machine, and that gamer 'owns' that `Player` entity. It's their representation in the world. As such, there are big differences between how `Player` entity should be represented on the authoritative client (it should have a camera, collect player input etc.) compared to if the `Player` entity represents someone else in the game. The FPS Template project has some additional logic to manage these representations as a way of keeping the code more organised.

Authority is a tricky topic with SpatialOS, particularly as write-access is actually defined on a per-component basis rather than a per-entity basis. You can find out more by reading up about [component authority](fix).<%(/Expandable)%>



##### Creating a UnityClient entity prefab

The FPS Template project contains a health pack prefab, `HealthPack.prefab`, in the folder: `/Assets/Resources/Prefabs/`.

In the `/Assets/Resources/Prefabs/UnityClient/` folder create a new prefab named **"HealthPickup"**. Give this two children, the health pack and the base plate (`/Assets/Resources/Prefabs/HealthPackCube.prefab` and `/Assets/Resources/Prefabs/HealthPackBase.prefab`).

<%(#Expandable title="Can I name the prefab something else?")%>Your choice of prefab name can be anything, but **must** match the string you used for the entity's `Metadata` component when you wrote the entity template function for this entity.

This is because the FPS Template project uses the GDK GameObject Creation package as part of the GameObject workflow. To find out more you can read up about the [GameObject workflow](fix).<%(/Expandable)%>



<%(#Expandable title="What's the best way to create a prefab?")%>Prefabs are the Unity Engine approach for creating templates of GameObject hierarchies.

If you right-click in your project file hierarchy, you'll find an option `Create > Prefab`, which will create for prefab at that file location and allow you to rename it. This prefab is initially empty, so you can drag other prefabs or GameObjects onto it to add them to the hierarchy.

If you are using Unity 2018 and earlier then it can often be easiest to drag prefabs into a scene to edit them - just remember to drag them to apply your change (in the Unity Inspector panel) and delete them from the scene when you are done editing! In upcoming versions of Unity Engine you will be able to make use of [prefab mode](https://blogs.unity3d.com/2018/06/20/introducing-new-prefab-workflows/) for this task.<%(/Expandable)%>

When creating entity prefabs it is usually a great idea to create a root GameObject which will contain your SpatialOS components and behaviours, with art assets added as children (which will also help with disabling inactive health packs later!).

Add a new script component to the root of your `HealthPickup` prefab, name it `HealthPickupClientVisibility`, and replace it's contents with the following code snippet:

```
using Improbable.Gdk.GameObjectRepresentation;
using Pickups;
using UnityEngine;

namespace Fps
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class HealthPickupClientVisibility : MonoBehaviour
    {
        [Require] private HealthPickup.Requirable.Reader healthPickupReader;

        [SerializeField] private MeshRenderer cubeMeshRenderer;

        private void OnEnable()
        {
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

This script is mostly standard C# code that you could find in any game built with Unity Engine. There are a few annotations which are specific to the SpatialOS GDK, which we can look at more closely.

```
[WorkerType(WorkerUtils.UnityClient)]
```

This annotation decorating the class indicates the `WorkerType` of the script (in this case, `UnityClient`). This provides information for SpatialOS when it is building out your separate workers: a script with the client annotation should never appear in `UnityGameLogic` worker. You can use these `WorkerType` annotations to control where your code runs. If a script should exist and run on both client-side and server-side workers then this annotation can be omitted.

This script relates is going to rely on the `active` property of your new `HealthPickup` component, so the package declared in the `HealthPickup.schema` file appears in this script in an include statement:

```
using Pickups;
```

This namespace is part of the helper classes that the code generation phase created from your schema.

To make use of component data, either to read from it or write to it, we can use SpatialOS GDK syntax to inject the component into the script.

```
[Require] private HealthPickup.Requirable.Reader healthPickupReader;
```

The worker on which the code is running interprets this statement as an instruction to only enable this script component on a particular entity's associated GameObject if that entity has a `HealthPickup` component, and the worker has read-access to that component. Read-access is rarely limited, but the same syntax can be use with `Writer` instead of `Reader`, which would make the requirement even more strict: The script would only be enabled on the single worker that has write-access to the `HealthPickup` component on that entity.

These `[Require]` statements are another powerful way to control where your code is executed. For the purpose of this script we only need to _read_ the health pack's data when deciding how to visualise it, so only a `Reader` is necessary.

You can see a use of the `HealthPickup` component's data in the line:

```
cubeMeshRenderer.enabled = healthPickupReader.Data.IsActive;
```

When you wrote the schema for the `HealthPickup` component you included a bool property called `is_active`, and code generation has created the `IsActive` member within the reader's `Data` object. We'll cover updating component property values later in this tutorial.

Setting the `cubeMeshRenderer.enabled` according to whether the health pack is "active" or not only works if `cubeMeshRender` correctly references the mesh renderer. Make sure you drag the child GameObject's mesh renderer to this field in the Unity Inspector panel to set the reference.

The client-side representation of the health pack entity is now complete!

##### Creating a UnityGameLogic entity prefab

In the FPS Template project the server-side worker is called `UnityGameLogic`.

If we were to test the game at this point we would now see the health pack entity in-game, but we've not yet given it the consumption behaviour.

Just as with client-side representation, navigate to the `/Assets/Resources/Prefabs/UnityGameLogic/` folder and create a `HealthPickup` prefab. Once again this name must match the `Metadata` string for the health pack entity so that the GameObject Creation system knows to associate them.








<%(#Expandable title="Are entities always represented by GameObjects?")%>No, exactly how entities are represented on each of your workers is up to you.

The GDK also offers an [ECS workflow](fix) represents them as a grouping of Unity ECS components. If you are more familiar with the traditional Unity GameObject style of development then the GDK provides a [GameObject workflow](fix) for you too.

You are not limited to these options either, and can configure your worker to create something very custom when it encounters a particular entity type. To find out more about you can read up about [entity representations](fix).<%(/Expandable)%>



<%(#Expandable title="Can workers differ in how they represent the same entity?")%>Yes! The local in-worker representation for entities can be customized for each of your worker types. For example, the server-side `UnityGameLogic` worker may represent the `Player` entity with a GameObject that contains no visual assets or sound effects, because the server does not need those assets to perform its duties.

Similarly, !!!!!!! COME BACK TO THIS [!!!](fix)<%(/Expandable)%>

### Testing your changes

Our aim is to have health packs which restore lost health to players. So what have we accomplished so far?

* You defined the schema for your health packs: a new component containing properties for how much health it will grant and whether it's ready to do so.
* You created an entity template function which provides a central definition of a particular entity type and can create `Entity` objects.
* You added an instance of the health pack entity type to the snapshot so it will be present in the world when the game begins.
* You associated a local representation with your new SpatialOS entity so that Unity will know how to visually represent any health pack it encounters.

You can launch a local deployment of your updated game world from the SpatialOS menu within the Unity editor by clicking **"Local launch"**. This will open a terminal that should tell you when the world is ready.

Once the world is ready you can:

* View all entities in the inspector from your browser: http://localhost:21000/inspector/
* Click Play in your Unity editor (if you have the `FPS-Development` scene open) to play the game.

You'll know it's worked if you can see a `HealthPickup` entity in the inspector, and find a floating health pack when running around in-game. But currently they just sit there, inert. If you walk into them then nothing happens. Let's fix that!

Our next step will be to add some game logic to the health pack so that it reacts to player collisions and grants them health.

<%(#Expandable title="How does the Inspector decide the entity name?")%>In your entity template function the compulsory `Metadata` component required a string as a parameter, and we gave it "HealthPickup", but could have used any string. The metadata is intended to be a friendly identifier for the entity type, and as such is used by the Inspector to label your entity.

If you are using the SpatialOS GDK's GameObject workflow then the `Metadata` string must match the name of the entity prefab that will represent it.<%(/Expandable)%>

# Adding health pack logic

// Where is the logic going to run?
