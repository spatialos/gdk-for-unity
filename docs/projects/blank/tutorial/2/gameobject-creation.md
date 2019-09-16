<%(TOC max="2")%>

# 2.3 - GameObject creation

The GameObject Creation module contains a default implementation of spawning GameObjects for your SpatialOS entities and offers the ability to customise that process.

We’ll cover how this works and how you can set it up to create GameObjects linked to the player entities in your world.

## How GameObject creation works

The GameObject Creation module contains the `IEntityGameObjectCreator` interface. Any class that implements this interface is known as a _GameObject creator_. This interface contains a method called when a SpatialOS entity _enters_ a [worker’s view]({{urlRoot}}/reference/glossary#worker-s-view) and another method called when a SpatialOS entity _leaves_ a [worker’s view]({{urlRoot}}/reference/glossary#worker-s-view).

The default GameObject creator is the `GameObjectCreatorFromMetadata` class, which has a reference to the type of worker that initialized it. It uses an entity’s `Metadata` component in conjunction with the worker type to determine what prefab to create GameObjects from.

The default GameObject creator requires that any prefab it could use be located inside `Resources/Prefabs/Common` or a specific `Resources/Prefabs/worker_type` folder. It first checks inside the worker-specific folder before using the `Common` directory as backup. If neither directory contains a prefab whose name matches an entity’s `Metadata` component, no GameObject is created.

## Plan your entity representations

You’ll be adding more player functionality in later chapters of this tutorial series, so it is important to plan your client-side and server player representations.

Future chapters in this tutorial series will add more client-side behaviour to players, so you’ll want a Player’s client-side and server-side representations to be different. This means you should create worker-specific prefabs for `UnityClient` and `UnityGameLogic` worker types.

The `Player` GameObject on a `UnityClient` worker-instance could either be owned by the client-worker or be just another player that has entered the client-worker’s view. This means that while we should still be able to see all the Player GameObjects, some behaviour is only enabled on an authoritative client-worker.

The client-side `Player` object owned by a given client-worker will need to:

* Rotate the camera using mouse input.
* Move the player using keyboard input.
* Send requests to spawn and interact with spheres.

The server-side representation only needs to receive and apply movement updates.

## Create the Player prefabs

Create a `UnityClient` and `UnityGameLogic` folder under `Assets/Resources/Prefabs`.

<pre>

    gdk-for-unity-blank-project/workers/unity
        ├── Assets/
            ├── Resources/
                ├── Prefabs/
                    ├── Common/
                    ├── <b><font color="white">UnityClient/</font></b>
                    ├── <b><font color="white">UnityGameLogic/</font></b>
        ...

</pre>

Now, create a capsule in your scene hierarchy and drag it into the newly created `UnityClient` folder to make a new prefab. Remove the prefab’s capsule collider and rename it to “Player” to match the metadata defined in the Player entity template.

Make a duplicate of this prefab, and move the duplicate to `Assets/Resources/Prefabs/UnityGameLogic`. The duplicate will have a `(1)` at the end, so ensure that you rename it to “Player”.

Once you have created both Player prefabs, delete the remaining capsule GameObject from the scene hierarchy.

## Update assembly definition

Since the GameObject Creation module is already referenced in the blank project’s `manifest.json`, you can update your project’s assembly definition to include `Improbable.Gdk.GameObjectCreation`.

Open the assembly definition located at the root of your project’s `Assets/` folder.

Select the `add` button and choose `Improbable.Gdk.GameObjectCreation` from the available options.

Hit `Apply` to save your changes.

## Update worker connectors

You now need to update the worker connectors to initialise the default GameObject creator.

In both `UnityClientConnector` and `UnityGameLogicConnector` classes, add a `using Improbable.Gdk.GameObjectCreation;` directive to the top of the file.

Inside the `HandleWorkerConnectionEstablished` method, add `GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);` after the PlayerLifecycle systems have been added.

<%(#Expandable title="What should HandleWorkerConnectionEstablished look like in the UnityClientConnector when I’m done?")%>

```csharp
protected override void HandleWorkerConnectionEstablished()
{
    PlayerLifecycleHelper.AddServerSystems(Worker.World);
    GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);

    if (level == null)
    {
        return;
    }

    levelInstance = Instantiate(level, transform.position, transform.rotation);
}
```

<%(/Expandable)%>

<%(#Expandable title="What should HandleWorkerConnectionEstablished look like in the UnityGameLogicConnector when I’m done?")%>

```csharp
protected override void HandleWorkerConnectionEstablished()
{
    Worker.World.GetOrCreateSystem<MetricSendSystem>();
    PlayerLifecycleHelper.AddServerSystems(Worker.World);
    GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);

    if (level == null)
    {
        return;
    }

    levelInstance = Instantiate(level, transform.position, transform.rotation);
}
```

<%(/Expandable)%>

To test that everything works, ensure a local deployment is running and hit the play button. In the “Scene” view, you should notice that a Player prefab is created on both client-worker and server-worker levels!

## Add some color

To better distinguish between client-side and server-side GameObjects in the development scene, it would be good to also add some color to your GameObjects.

Create a `Color` folder under `Assets/BlankProject/Scripts/`. Inside this directory, create a file called `ObjectColor.cs`.

<pre>

    gdk-for-unity-blank-project/workers/unity
        ├── Assets/
            ├── BlankProject/
                ├── Scripts/
                    ├── <b><font color="white">Color/</font></b>
                        ├── <b><font color="white">ObjectColor.cs</font></b>
        ...

</pre>

Copy the code below into the file:

```csharp
using BlankProject;
using Improbable;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Scripts.Color
{
    [WorkerType(UnityClientConnector.WorkerType)]
    public class ObjectColor : MonoBehaviour
    {
        public Color color;
        private Renderer renderer;

        private void OnEnable()
        {
            renderer = GetComponent<Renderer>();
        }

        private void OnUpdate()
        {
            renderer.material.color = color;
        }
    }
}
```

In the Unity Editor, add the `ObjectColor` behaviour to the Player prefab for the UnityClient only. This is so that only the client-side representations of a Player are given color. Choose a color for the player that makes it easy to distinguish from a plain object.

To test this change, hit the play button in the Editor.

You should observe that, as expected, the player object on the client-level has the color you chose, but the player object on the other level does not.

<img src="{{assetRoot}}assets/blank/tutorial/2/player-with-color-annotated.png" style="margin: 0 auto; width: 75%; display: block;" />

This validates that the default GameObject creator makes use of a worker’s worker type to instantiate the correct representation of a Player.

#### Next: [Summary]({{urlRoot}}/projects/blank/tutorial/2/summary)
