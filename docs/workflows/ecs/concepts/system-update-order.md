<%(TOC)%>

# ECS: System update order

The SpatialOS GDK for Unity (GDK) defines several update groups which run relative to [`PlayerLoop.Update`](https://docs.unity3d.com/2019.3/Documentation/ScriptReference/LowLevel.PlayerLoop.html). Most of your systems should belong to one of these groups to ensure that the system is run in the right order relative to GDK receive and send systems.

## Update groups

The groups are executed in the following order:

* `SpatialOSReceiveGroup` - This group contains all of the systems related to receiving and handling data from SpatialOS.
  * `InternalSpatialOSReceiveGroup` - This is used by the `SpatialOSReceiveSystem`.
  * `GameObjectInitializationGroup` - This group contains all systems used to link GameObjects to SpatialOS entities.
  * `RequireLifecycleGroup` - This group contains all systems used to inject objects that were marked with `[Require]` in Monobehaviours.
* `SpatialOSUpdateGroup` - **Most systems which use SpatialOS components should run in this group.**
* `SpatialOSSendGroup` - This group contains all systems related to sending data to SpatialOS .
  * `InternalSpatialOSSendGroup` - This group is used by the `SpatialOSSendSystem` to handle sending all replicated components to SpatialOS.
  * `InternalSpatialOSCleanGroup` - This group is used by the `CleanTemporaryComponentsSystem` to remove all [temporary components]({{urlRoot}}/workflows/ecs/concepts/temporary-components) at the end of each update loop.

<%(Callout type="warn" message="
Don’t use groups which have ``Internal`` in the name - these are for **internal use only**.
")%>

### How to assign an update group

Unity provides attributes to define the [update order of systems](https://docs.unity3d.com/Packages/com.unity.entities@0.0/manual/system_update_order.html). These attributes are:

* `[UpdateInGroup]`
* `[UpdateBefore]`
* `[UpdateAfter]`
* `[DisableAutoCreation]`

> Note that you can only have one attribute of each type assigned to a system. If multiple are assigned they may interfere with each other.

Here’s an example of how to assign an attribute to a system:

```csharp
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class ProcessColorChangeSystem : ComponentSystem
{
    // system code here
}
```

## Update vs FixedUpdate

The `SpatialOSUpdateGroup` runs relative to `PlayerLoop.Update`, but sometimes you might want to run a system on `PlayerLoop.FixedUpdate` because that will tick at regular intervals. An example of this is moving a GameObject at a fixed speed, which you should do using the given `FixedUpdateSystemGroup`, which runs relative to `PlayerLoop.FixedUpdate`.

You can find more information on `Update` and `FixedUpdate` [here (Unity documentation)](https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate).
