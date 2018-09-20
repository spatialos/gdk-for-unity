
**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----

## Workers

While SpatialOS manages the current state of your game, [workers](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing) execute the logic necessary to simulate your game.

In the SpatialOS GDK for Unity (GDK), a worker is responsible for its connection to the SpatialOS Runtime and its [ECS World](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#world)). When you create a worker in the GDK using `Worker.CreateWorkerAsync`, the worker creates the ECS world and populates it with the systems from the Core module. We provide several wrappers around this `Worker` object, depending on whether you want to use the ECS or the MonoBehaviour workflow.

See also documentation on [Accessing information about the worker during runtime](./ecs/accessing-worker-info.md)

## Starting up a worker

### Worker Prefab

In the GDKs [`Playground` project](../../workers/unity/Assets/Playground) we provide an example implementation of the connection logic necessary for the workers to connect to the SpatialOS Runtime. These are stored as prefabs, so that you can use them directly in Scenes. We provide three sample Scenes:

* `SampleScene`: This Scenes contains both the `UnityClient` and the `UnityGameLogic` prefabs and will start both workers as soon as you load the scene.

* `ClientScene`: This Scene contains only the `UnityClient` prefab and can be used to build your client worker for cloud deployments. 

* `GameLogicScene`: This Scene contains only the `UnityGameLogic` prefab and can be used to build your server worker for cloud deployments.  

The position of these prefabs in the Scene define the `Origin` of the worker, which is used to translate all entities that have been added to this worker. This ensures that entities checked out by different workers don't interfere with each other when running multiple workers in the same scene.

----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).

