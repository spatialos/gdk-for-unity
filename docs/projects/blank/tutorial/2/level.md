<%(TOC max="2")%>

# 2.2 - Instantiate a level

Before you can start spawning gameobjects for Player entities, you should ensure there is a simple level for them to spawn on. This level will become the “ground” of our game world.

You need to make sure that each worker initialises its level when it starts, before any other GameObject spawning can begin. To do this, you need to update the worker connectors.

## Worker connectors

A worker connector is a MonoBehaviour that allows you to connect workers to your deployment. The GDK provides an abstract WorkerConnector MonoBehaviour class, which has methods for connecting and managing the lifecycle of your worker-instance.

The Blank Project provides both `UnityClientConnector` and `UnityGameLogicConnector` classes that inherit from the abstract WorkerConnector class. Each class sets up a SpatialOS connection of their respective worker type.

A worker _prefab_ is a prefab which contains a worker connector script. In the Blank Project, the `ClientWorker` prefab contains the `UnityClientConnector` script, and the `GameLogicWorker` prefab contains the `UnityGameLogicConnector` script.

> The `ClientScene`, `GameLogicScene` and `DevelopmentScene` in the project contain different worker prefabs, corresponding to what worker types to connect when a scene is played. As the `DevelopmentScene` contains both `ClientWorker` and `GameLogicWorker` prefabs, starting this scene connects both a client-worker and a server-worker.

Both worker connectors configure and initialise their connection flow within the `Start()` method. At the end of `Start()`, the base class `Connect` method is called with the configured connection parameters.

The `HandleWorkerConnectionEstablished` method is triggered after a worker connects successfully to SpatialOS. Currently, both worker connectors use this method to enable either the respective client-side or server-side functionality of the PlayerLifecycle feature module.

<%(#Expandable title="What if the worker fails to connect?")%>

The `WorkerConnector` class contains a `HandleWorkerConnectionFailure` method, which takes the error message as an argument. This method is called when the worker connector is unable to connect to SpatialOS.

<%(/Expandable)%>

## Update the worker connectors

Since you would want the level to be initialised _before_ the game loop begins, you need to update the `HandleWorkerConnectionEstablished` method.

First, open UnityClientConnector.cs and create a private serialized field for the Level prefab. You’ll be able to set this through the Unity Editor later.

```csharp
[SerializeField] private GameObject level;
```

Next, create a private field so the worker connector can track the instance of the Level it creates. This allows each worker connector to destroy its Level upon disconnection.

```csharp
private GameObject levelInstance;
```

At the end of `HandleWorkerConnectionEstablished()`, `Instantiate` the Level prefab if there isn’t already an instance. You should make use of the worker connector’s transform to ensure the level is spawned with the position and rotation of the worker prefabs in your scenes.

```csharp
if (level == null)
{
    return;
}

levelInstance = Instantiate(level, transform.position, transform.rotation);
```

Lastly, override the public `Dispose()` function to destroy the level instance. **Don’t forget to call the base Dispose() method at the end!**

```csharp
public override void Dispose()
{
    if (levelInstance != null)
    {
        Destroy(levelInstance);
    }

    base.Dispose();
}
```

Now that you’re done with the UnityClientConnector, repeat the above steps for the UnityGameLogicConnector class.

<%(#Expandable title="What should the UnityClientConnector look like when I'm done?")%>

```csharp
using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace BlankProject
{
    public class UnityClientConnector : WorkerConnector
    {
        public const string WorkerType = "UnityClient";

        [SerializeField] private GameObject level;
        private GameObject levelInstance;

        private async void Start()
        {
            var connParams = CreateConnectionParameters(WorkerType);
            connParams.Network.ConnectionType = NetworkConnectionType.Kcp;

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (!Application.isEditor)
            {
                var initializer = new CommandLineConnectionFlowInitializer();
                switch (initializer.GetConnectionService())
                {
                    case ConnectionService.Receptionist:
                        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType), initializer));
                        break;
                    case ConnectionService.Locator:
                        builder.SetConnectionFlow(new LocatorFlow(initializer));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType)));
            }

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            PlayerLifecycleHelper.AddClientSystems(Worker.World);

            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        public override void Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }

            base.Dispose();
        }
    }
}
```

<%(/Expandable)%>

<%(#Expandable title="What should the UnityGameLogicConnector look like when I'm done?")%>

```csharp
using BlankProject.Scripts.Config;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace BlankProject
{
    public class UnityGameLogicConnector : WorkerConnector
    {
        public const string WorkerType = "UnityGameLogic";

        [SerializeField] private GameObject level;
        private GameObject levelInstance;

        private async void Start()
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = EntityTemplates.CreatePlayerEntityTemplate;

            IConnectionFlow flow;
            ConnectionParameters connectionParameters;

            if (Application.isEditor)
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerType));
                connectionParameters = CreateConnectionParameters(WorkerType);
            }
            else
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerType),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(WorkerType,
                    new CommandLineConnectionParameterInitializer());
            }

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionFlow(flow)
                .SetConnectionParameters(connectionParameters);

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            Worker.World.GetOrCreateSystem<MetricSendSystem>();
            PlayerLifecycleHelper.AddServerSystems(Worker.World);

            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        public override void Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }

            base.Dispose();
        }
    }
}
```

<%(/Expandable)%>

## Create a Level prefab

Create a `Common` folder within the `Assets/Resources/Prefabs` directory of your Unity project.

<pre>

    gdk-for-unity-blank-project/workers/unity
        ├── Assets/
            ├── Resources/
                ├── Prefabs/
                    ├── <b><font color="white">Common/</font></b>
        ...

</pre>

Right-click in your scene and select **3D Object** > **Plane** to create a plane with scale (10, 1, 10) whose position is (0, 0, 0). Drag it into the `Assets/Resources/Prefabs/Common` folder. This will be our level, so rename it to “Level”.

<img src="{{assetRoot}}assets/blank/tutorial/2/level-prefab-inspector.png" style="margin: 0 auto; width: 50%; display: block;" />

Once the Level prefab is created, you can delete the plane from the current scene.

## Update worker prefabs

Now that the Level prefab exists, you need to update the worker prefabs to reference it. You can find these prefabs in the **Resources** > **Prefabs** > **Worker** folder.

For both `ClientWorker` and `GameLogicWorker` prefabs, click the Level field on the worker connector and select the Level GameObject you just created.

Let’s test this out!

If you don’t have a local deployment running, hit `Ctrl+L`/`Cmd+L` inside the Unity Editor to start one up. When it’s ready, hit play in the Unity Editor. You should observe that there’s not one - but **two** levels side-by-side!

[image of two levels side by side]

Since both worker connectors were updated to instantiate a level - both of these worker types are in the development scenes - you can expect two levels to be created when you hit the play button.

However, in the SpatialOS Inspector you’ll see that the `UnityClient` and `UnityGameLogic` worker share the same position. Why do two workers with the same SpatialOS position instantiate their levels at different positions in the Unity Editor?

## Worker offsets

When there are two different workers operating in the same Unity world space, there’s a higher chance you’ll see side-effects with unwanted interaction. Since the DevelopmentScene connects two workers to your deployment, you’ll need to figure out how to make the workers run independently.

With workers overlapping in the same scene, you no longer have the guarantee that the only objects on each worker’s level are the ones you would expect in a single-worker-to-scene mapping. This introduces the risk of objects from different workers interacting with each other.

An effective and simple way of removing this risk is to place each worker at different positions in the scene. As long as the offsets are larger than the maximum level size that you expect to need for local development, the two levels will not overlap.

## Update the worker offsets

Stop your Editor and look through the hierarchy of the `DevelopmentScene`. You should notice that the `ClientWorker` prefab is placed at (0, 0, 0) and the `GameLogicWorker` prefab is placed at (100, 0, 0). Given the level is a square 100 units wide, the current offsets would place the levels visually side-by-side as you saw earlier.

Now, update the position of the `GameLogicWorker` in your `DevelopmentScene` to (150, 0, 0). This should spawn the two levels side-by-side, but with a 50 unit gap. Ensure that this change only affects the `GameLogicWorker` _object_ in the scene, and not the `GameLogicWorker` prefab itself.

With your local deployment still running, hit play again. You should now observe a gap between the client-worker’s level instance and the server-worker’s level instance.

[image showing gap between the two levels]

#### Next: [GameObject creation]({{urlRoot}}/projects/blank/tutorial/2/gameobject-creation)
