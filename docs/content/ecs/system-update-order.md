[//]: # (Doc of docs reference 31.1)
[//]: # (TODO - Tech writer review)
[//]: # (TODO - use discussions about content in here https://docs.google.com/document/d/1MPTP1qEo9LaYxFGLQFEN2SqEzu9MxlKjVfOYKPUbTXg/edit)

<%(TOC)%>
# System update order
 _This document relates to the [ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Unity provides attributes to define the [update order of systems (Unity documentation)](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#system-update-order). These attributes are: `UpdateInGroup`, `UpdateBefore` and `UpdateAfter`.

> You can only have one attribute of each type assigned to a system. If multiple are assigned they might override each other.

Here’s an example of how to assign an attribute to a system:

```csharp
[UpdateInGroup(typeof(SpatialOSUpdateGroup))]
public class ProcessColorChangeSystem : ComponentSystem
{
    …
}
```

The SpatialOS GDK for Unity (GDK) defines several update groups which run relative to [`PlayerLoop.Update`](https://docs.unity3d.com/ScriptReference/Experimental.LowLevel.PlayerLoop.html). Most of your systems should belong to one of these . Using one of these groups will ensure that the system can access reactive components and state changes get synchronised with minimum latency..

> Don’t use groups which have “Internal” in the name - these are for internal use only.

The groups are executed in the following order:

* `SpatialOSReceiveGroup` - This group contains all of the systems related to receiving and handling data from SpatialOS.
  * `InternalSpatialOSReceiveGroup` - This is used by the `SpatialOSReceiveSystem`. **(Internal use only)**
  * `GameObjectInitializationGroup` - This group contains all systems used to link GameObjects to SpatialOS entities.
* `SpatialOSUpdateGroup` - Most systems which use SpatialOS components should run in this group.
* `SpatialOSSendGroup` - This group contains all systems related to sending data to SpatialOS .
  * `InternalSpatialOSSendGroup` - This group is used by the `SpatialOSSendSystem` to handle sending all replicated components to SpatialOS. **(Internal use only)**
  * `CustomSpatialOSSendGroup` -  This group contains all systems running custom replication logic. If you create a [custom replication system]({{urlRoot}}/content/ecs/custom-replication-system), add your system to this group.
  * `InternalSpatialOSCleanGroup` - This group is used by the `CleanReactiveComponentsSystem` to remove all [reactive]({{urlRoot}}/content/ecs/reactive-components) and [temporary components]({{urlRoot}}/content/ecs/temporary-components) at the end of each update loop. **(Internal use only)**

Here's a diagram of the update order:

![Update order]({{assetRoot}}assets/update-order.png)

## Update vs FixedUpdate

All of these groups run relative to `PlayerLoop.Update`, but sometimes you might want to run a system on `PlayerLoop.FixedUpdate`. An example of this is moving a GameObject at a fixed speed. You should do this in `PlayerLoop.FixedUpdate` because that will tick at regular intervals. You can find more information on `Update` and `FixedUpdate` [here (Unity documentation)](https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate).

> Reactive components may not be available in FixedUpdate. To access the data in the components reliably, you will need to manually cache the data in a system that runs on Update.
