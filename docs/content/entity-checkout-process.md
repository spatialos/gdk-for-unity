**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Entity checkout process

For every entity we receive from SpatialOS, an ECS entity representing the SpatialOS entity is automatically created in the Worker's world through the `SpatialOSReceiveSystem`.

For each of your entity's SpatialOS components, the following Unity ECS components are automatically added:
- `SpatialOS<name of schema component>`: Codegenerated struct of type `ISpatialComponentData` or `Component` for accessing component field values. Fields of these components are automatically kept up-to-date.
- An [authority maker tag](authority.md) (`Authoritative<T>`, `NotAuthoritative<T>` or `AuthorityLostImminent<T>`) based on the Worker's component write authority.

In addition, the following components are added to your entity as well:
- `SpatialEntityId`: Holds the corresponding SpatialOS EntityId of an entity.
- `NewlyAddedSpatialOSEntity`: Tag component for marking entities that were just checked-out. This component is automatically removed from your entity at the end of the frame it was created.
- `WorldCommandSender`: Component that exposes the API for [sending world commands](commands.md#world-commands) (e.g. create entity, delete entity).
- A `CommandRequestSender<T>` for every SpatialOS schema component `T` that defines a command: Component that exposes the API for [sending schema commands](commands.md#sending-command-requests) defined in `T`.

### Performing Setup Logic on Entities

An Archetype initialization feature is provided as part of the `DemoGame` project. This optional feature allows you to automatically add pre-specified Unity ECS components to newly checked-out entities based on the `ArchetypeComponent` schema component.

To use this feature:
1. Add the `ArchetypeInitializationSystem` to your Worker's world.
2. Add an `ArchetypeComponent` schema component to SpatialOS entities that should make use of this feature. Set the `archetype_name` field of the component before the SpatialOS entity is checked-out by your Unity worker (e.g. `archetype_name = "MyArchetype"`).
3. Specify a list of components to be added to entities in `workers\unity\Assets\DemoGame\Config\ArchetypeConfig.cs`:
```csharp
public static class ArchetypeConfig
{
    public static readonly Dictionary<string, Dictionary<string, ComponentType[]>>
        WorkerTypeToArchetypeNameToComponentTypes = new Dictionary<string, Dictionary<string, ComponentType[]>>
        {
            {
                UnityGameLogic.WorkerType,
                new Dictionary<string, ComponentType[]>()
                {
                    { "MyArchetype", new ComponentType[] { typeof(ComponentToBeAdded), typeof(AnotherComponentToBeAdded) } }
                }
            },
            {
                UnityClient.WorkerType,
                new Dictionary<string, ComponentType[]>()
                {
                    { "MyArchetype", new ComponentType[] { typeof(ComponentToBeAdded), typeof(AnotherComponentToBeAdded) } }
                }
            }
        };
}
```
The components specified in `WorkerTypeToArchetypeNameToComponentTypes` are automatically added to entities that have an `ArchetypeComponent` with a matching `archetype_name`.

To perform more complex setup logic, you can add components that themselves act as temporary markers for performing additional setup logic. E.g. you could add a `NeedsHelperChildEntity` ECS component as part of the Archetype initialization feature and have an additional `CreateHelperChildEntitySystem` inject that component.

**Note:** It is currently not possible to set the initial field values of the ECS components that were added through this feature. Those fields need to be set manually in a follow-up setup step, e.g. with a system that also injects the `NewlyAddedSpatialOSEntity` component and is run after the `ArchetypeInitializationSystem`.

### Representing your Entity with a GameObject

A GameObject initialization feature is provided as part of the `DemoGame` project. This optional feature allows you to automatically create and delete a companion GameObject for representing a Unity ECS entity in the scene based on the `Prefab` schema component. This feature is useful if you want to make use of Unity features that are currently only accessible through the scene, e.g. physics.

To use this feature:
1. Add the `GameObjectInitializationSystem` to your Worker's world.
2. Add an `Prefab` schema component to SpatialOS entities that should make use of this feature. Set the `prefab` field of the component before the SpatialOS entity is checked-out by your Unity worker (e.g. `prefab = "MyPrefab"`).
3. Add a `Transform` schema component to SpatialOS entities that should make use of this feature. This is to help the feature determine where the corresponding GameObject shall be spawned in the scene.
4. Create a prefab for representing your entity in the `Resources` folder of your Unity project. Give it a name, e.g. `MyPrefabClient`, `MyPrefabServer`.
5. Specify a prefab mapping for matching entities with their corresponding prefabs in `workers\unity\Assets\DemoGame\Config\PrefabConfig.cs`:
```csharp
public static class PrefabConfig
    {
        public static readonly Dictionary<string, PrefabMapping> PrefabMappings = new Dictionary<string, PrefabMapping>
        {
            {
                "MyPrefab",
                new PrefabMapping { UnityGameLogic = "MyPrefabServer", UnityClient = "MyPrefabClient" }
            }
        };
    }
```
Upon checking out an entity that has both a `Prefab` and a `Transform` component, a companion GameObject is automatically instantiated in the active scene based on its `Prefab` component. The companion GameObject is automatically deleted upon removing the entity from the worker.

All GameObject components attached to the companion GameObject at the time it is instantiated are also added to the entity as ECS entity components. **Note:** Adding additional GameObject components to companion GameObjects after they were instantiated does not result in the components being added to the corresponding ECS entity.

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).