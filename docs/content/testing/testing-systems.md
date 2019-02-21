<%(TOC)%>
# Testing GDK Systems

We can test systems functionality like this:

* Construct a world
* Add a system
* (If necessary) Assert that the system is set up correctly
* Execute the system
* Assert the system behaves correctly
* Dispose

See Unite Berlin talk: https://youtu.be/BW9qSy6ZB0A?t=9048 - 2:30:48 to 2:31:10.

See this example test:

```cs
[TestFixture]
public class TickSystemTests
{
    [Test]
    public void GlobalTick_gets_incremented_in_OnUpdate()
    {
        // Construct a world
        using (var world = new World("test-world"))
        {
            // Add a system and assert the set up
            var tickSystem = world.GetOrCreateManager<TickSystem>();
            Assert.AreEqual(0, tickSystem.GlobalTick);

            // Execute the system and assert the behaviour
            tickSystem.Update();
            Assert.AreEqual(1, tickSystem.GlobalTick);
        }

        // Disposal happens automatically after the "using" block has finished.
    }
}
```

## Systems which depend on workers

Some systems assume the world they are in belongs to a worker.
For example, in their `OnCreateManager` method, they get a reference of the worker using `WorkerRegistry.GetWorkerForWorld`. For these systems, you can create an instance of `UnityTestWorker` in your `[SetUp]` and dispose of it in `[TearDown]`.

You can access the `UnityTestWorker` by adding a reference to
 `Improbable.Gdk.TestUtils` assembly in your test assembly definition file.

For example:

```cs
[TestFixture]
public class CleanReactiveComponentsSystemTests
{
    private UnityTestWorker worker;

    [SetUp]
    public void Setup()
    {
        // Need to use a different worker instance for every test because the
        // system's world needs to be re-created.
        worker = new UnityTestWorker("worker-id", new Vector3());
    }

    [TearDown]
    public void TearDown()
    {
        worker.Dispose();
    }

    [Test]
    public void SomeTest() 
    {
        // The world will be disposed in the [TearDown] above.
        var world = worker.World;

        // The CleanReactiveComponentsSystem can now be created and it will be
        // able to find the worker through the world.
        world.GetOrCreateManager<CleanReactiveComponentsSystem>();
        // ...
    }

    [Test]
    public void SomeOtherTest() {
        // New worker, new world, since [SetUp] and [TearDown] 
        // happen for each test.
        var world = worker.World;
        world.GetOrCreateManager<CleanReactiveComponentsSystem>();
        // ...
    }
}
```

## Hybrid Systems

Your system is not pure and is hybrid if it `[Inject]`s a struct that has any one of these types as fields:

* `ComponentArray<>`
* `GameObjectArray`
* `TransformAccessArray`

In order to be able to test Hybrid Systems, you will need to ensure that your
 fixture extends `HybridGdkSystemTestBase`. The `HybridGdkSystemTestBase` class
 ensures that ECS injection hooks are prepared for the above types.

Otherwise, you will see an exception like this:

```text
System.ArgumentException : ExampleHybridSystem:testDataToPrepare [Inject] may only be used on ComponentDataArray<>, ComponentArray<>, TransformAccessArray, EntityArray, and int Length.
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
            var entity = entityManager.CreateEntity(typeof(Rigidbody));
            var testSystem = world.GetOrCreateManager<ExampleHybridSystem>();
            // ...
        }
    }
}
```
