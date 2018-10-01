
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

Within the `SnapshotMenu` class, add a new function that will contain logic for adding health pack entities to the snapshot object:

```
private static void AddHealthPacks(Snapshot snapshot)
{
    var healthPack = FpsEntityTemplates.HealthPickup(new Vector3f(0, 0, 0), 100f);
    snapshot.AddEntity(healthPack);
}
```

This creates a health pack entity at the origin, and sets the amount of health it will restore to 100. Don't forget to call your new function from within `GenerateDefaultSnapshot()` (and pass it the `snapshot` object) or else it wont be run during snapshot generation.

It's a great idea to separate default values (such as health pack positions, and health values) into a settings file. In the FPS Template project you can find lots of examples of using Unity `ScriptableObject` components for exactly that. But for now, we will keep this example simple.

#### Updating the snapshot

Snapshot files are found in your project root directory, in a directory named `snapshots`. The FPS Template project includes a snapshot called `default.snapshot`.

If you have updated the snapshot generation function (as you just did in the step above), or if you've altered which components are specified in one of your entity templates, then your snapshot will be out of date. The snapshot is a big list of entities, and all their individual component values, so any change to these and the snapshot file must be regenerated.

You can regenerate the `default.snapshot` file from the SpatialOS menu option in the Unity editor, but running **"Generate FPS Snapshot"**.

<%(#Expandable title="Can I make my snapshots human-readable?")%>Yes, there is a `spatial` command that will convert snapshots to a human-readable format. However, you cannot launch a deployment from a human-readable snapshot, so it must be converted back to binary before it is usable. To find out more about working with snapshots you can read about the [spatial snapshot command](fix).

While they are human-readable you are able to manually edit the values of the properties within, however be careful not to make mistakes that will inhibit the conversion back to binary form!<%(/Expandable)%>

### Representing the entity on your workers

[!!!](TODO: Write stuff about hooking up game object)

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

You'll know it's worked if you can see a `HealthPickup` entity in the inspector, and find a floating health pack when running around in-game.

Our next step will be to add some game logic to the health pack so that it reacts to player collisions and grants them health.

<%(#Expandable title="How does the Inspector decide the entity name?")%>In your entity template function the compulsory `Metadata` component required a string as a parameter, and we gave it "HealthPickup", but could have used any string. The metadata is intended to be a friendly identifier for the entity type, and as such is used by the Inspector to label your entity.<%(/Expandable)%>
