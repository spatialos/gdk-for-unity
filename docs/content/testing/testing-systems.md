**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

-----

# Testing GDK Systems

We can test systems functionality like this:

- Construct a world
- Add systems
- Execute systems
- Check state
- Dispose

See Unite Berlin talk: https://youtu.be/BW9qSy6ZB0A?t=9048

See this example test that does not need the `[SetUp]` or `[TearDown]`
methods:

```cs
[TestFixture]
public class TickSystemTests
{
    [Test]
    public void GlobalTick_gets_incremented_in_OnUpdate()
    {
        // Construct a world
        // The "using" ensures the world will get destroyed (along with its systems)
        using (var world = new World("test-world"))
        {
            // Add system
            var tickSystem = world.GetOrCreateManager<TickSystem>();

            // Make assertion against initial state of system
            Assert.AreEqual(0, tickSystem.GlobalTick);

            // Execute system
            tickSystem.Update();

            // Assert the system behaves correctly
            Assert.AreEqual(1, tickSystem.GlobalTick);

            tickSystem.Update();

            Assert.AreEqual(2, tickSystem.GlobalTick);
        }
    }
}
```

## Systems which depend on workers
There are some systems which assume that the world they are in belongs to a worker.
For example, in their `OnCreateManager` method, they get a handle of the worker
using `WorkerRegistry.GetWorkerForWorld`. For these systems, you can create an
 instance of `UnityTestWorker` in your `[SetUp]` and dispose of it in `[TearDown]`.

You can access the `UnityTestWorker` by adding a reference to `Improbable.Gdk.TestUtils` assembly.

For example:

```cs
[TestFixture]
public class CleanReactiveComponentsSystemTests
{
    private UnityTestWorker worker;

    [SetUp]
    public void Setup()
    {
        // Need to use a different worker instance for every test because the system's world needs to be re-created.
        worker = new UnityTestWorker("worker-id", new Vector3());
    }

    [TearDown]
    public void TearDown()
    {
        worker.Dispose();
    }

    [Test]
    public void SomeTest() {
      // the world will be disposed in the [TearDown] above.
      var world = worker.World;
      
      // safe to do this
      world.GetOrCreateManager<CleanReactiveComponentsSystem>();
      // ...
    }

    [Test]
    public void SomeOtherTest() {
      // new worker, new world, since [SetUp] and [TearDown] happen for each test.
      var world = worker.World;

      world.GetOrCreateManager<CleanReactiveComponentsSystem>();
      // ...
    }
}
```

## Hybrid Systems
Your system is not pure and is hybrid if it  `[Inject]`s a struct that has any one of these types as fields:
- `ComponentArray<>`,
- `GameObjectArray`,
- `TransformAccessArray`.

In order to be able to test Hybrid Systems, you will need to ensure that your fixture extends `HybridGdkSystemTestBase`. The `HybridGdkSystemTestBas` class ensures that ECS injection hooks are prepared for the above types.

Otherwise, you will see an exception like this:

```
System.ArgumentException : ExampleHybridSystem:testDataToPrepare [Inject] may only be used on ComponentDataArray<>, ComponentArray<>, TransformAccessArray, EntityArray,  and int Length.
```

For example:
```cs
[TestFixture]
private class FixtureImplementingHybridTestBase : HybridGdkSystemTestBase
{
    [Test]
    public static void ExampleHybridSystem_can_be_created_and_updated()
    {
        using (var world = new World("test-world"))
        {
            var entityManager = world.GetOrCreateManager<EntityManager>();

            var entity = entityManager.CreateEntity(
                typeof(Rigidbody)
            );

            var testSystem = world.GetOrCreateManager<ExampleHybridSystem>();
            // ...
        }
    }
}
```