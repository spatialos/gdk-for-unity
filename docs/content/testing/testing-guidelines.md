**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Testing guidelines

The tests should:

- Set up some initial state
- Execute a method that acts upon this state and results in a new state
- Make assertions in the new state against the expected changes
- Clean up

NUnit allows this to be done through the use of fixtures and special method annotations.

## Module directory structure

The Unity GDK project's assets are divided into modules, such as `Core`, `Physics`, `PlayerLifecycle`.

Each module is in its own directory within `workers\unity\Assets\Gdk`.

Each module has its own tests that cover the functionality of that module.

Unity Test Runner also allows separate Playmode and Editmode tests. For more information on this division, please refer to the [Unity Test Runner manual page](https://docs.unity3d.com/Manual/testing-editortestsrunner.html).

Within the root of the module, the `Tests` directory should contain `Editmode`
 and `Playmode` subdirectories with their own assembly definition files.

Test assembly definition files must match this pattern:
> `Improbable.Gdk.<ModuleName>.<TestMode>Tests.asmdef`

### Editmode vs. Playmode tests

As many unit tests as possible should be Editmode tests. This is because Editmode tests are faster, since they do not require scenes and Gameobjects to be set up.

Playmode-specific features should be mocked as much as possible, unless it’s impractical to do so. For example, if a function depends on some time passing, it should be using a mockable time interface instead of the `UnityEngine.Time` class directly.

If it is impossible to do a test as an editor test, e.g. a particular kind of functionality that responds to a physics callback, then they can be Playmode tests.

Another justification for a Playmode test is to check that certain functionality will still be present when a game is being played, or for more complicated integration testing that may depend on multiple systems interacting with each other.

### Test Folder Structure

Within each test folder, the folder structure should match the non-test code structure.

If any class file path can be broken down into the pattern `<module path>/<path within module>.cs`, the Editmode tests for this class should be in `<module path>/Tests/Editmode/<path within module>Tests.cs`.

For a more concrete example, for the class  `workers/unity/Assets/Gdk/Core/Utility/CommandLineUtility.cs` the test fixtures should be within `workers/unity/Assets/Gdk/Core/Tests/Editmode/Utility/CommandLineUtilityTests.cs`.

If a class has both Editmode and Playmode tests, you can name the test filenames as `<ClassName>EditmodeTests.cs` and `<ClassName>PlaymodeTests.cs` within their respective assemblies.

## File structure

### Namespaces

The tests should be grouped under this namespace pattern:

`Improbable.Gdk.<ModuleName>.<TestMode>Tests`

Where:
- `<ModuleName>` is the GDK module being tested (e.g. `Core`)
- `<TestMode>` is one of `Editmode` or `Playmode`

### Fixtures and methods

NUnit allows us to use test fixtures to set up common properties for tests for a particular part of a feature, and test cases within fixtures that will each test a single functionality.

You can have multiple fixtures within a test file. These fixtures set up and clean up different base conditions that can be tested. For example, if a class has both static and instance methods, this can be tested within two fixtures:
- `<ClassName>StaticTests`
- `<ClassName>InstanceTests`

The `StaticTests` may not need to set anything up, but the `InstanceTests` may need to create an instance of the class to be able to verify the instance methods.

There are other reasons to create multiple fixtures, for example if you would like to test different pre-conditions.

In most cases, each test file will contain one fixture. In this case, this fixture can be named `<ClassName>Tests`.

The test methods should match this pattern:

> `<method>_should_<action>_when_<conditions>`.

### Setup and teardown

The following snippet shows the recommended order of the NUnit set up and tear
 down annotations:

```cs
[TestFixture]
public class ExampleTests
{
    // Proposed structure:
    // - one time set up and tear down
    // - per test set up and tear down
    // - each test one after another

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // Set up resources, variables and parameters that may not change per test
        // E.g. load a scene that all tests in this fixture will depend on
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Clean up the resources, variables and parameters that may not change per test
        // E.g. unload the scene
    }

    [SetUp]
    public void Setup()
    {
        // Set up resources, variables and parameters that may change within each test
        // E.g. create a gameobject
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the resources, variables and parameters that may change within each test
        // E.g. delete the created gameobject
    }

    [Test]
    public void X_should_Y_when_Z()
    {
        // Prepare case, call X, assert that Y happens
    }

    [Test]
    public void P_should_Q_when_R()
    {
        // Prepare case, call P, assert that Q happens
    }
}
```

### Assertions

For NUnit, you can use the `Assert` class to make assertions.

Please pay attention to parameter names, for example, for `Assert.AreEqual`,
 the first parameter is the expected value, and the second parameter is the
 value we are testing.

```cs
  var sumResult = Add(2, 3);
  Assert.AreEqual(5, sumResult);
```

#### Assertions for exceptions

By default, tests will fail if any code within throws an exception.

Sometimes you may want to test that an exception will be thrown in certain
 situations.

You can do this using the `Assert.Throws` method. This method returns the caught
 exception, and you can do further assertions against that.

```cs
        [Test]
        public void Validate_should_return_false_when_LocatorHost_is_empty()
        {
            var config = GetDefaultWorkingConfig();
            config.LocatorHost = "";

            var exception = Assert.Throws<System.ArgumentException>(() => config.Validate());
            Assert.IsTrue(exception.Message.Contains("locatorHost"));
        }
```

### Disposables vs setup and teardown

If you have a fixture that has only one test, and the setup is simple, then you
 can use the `using` keyword within the test.

e.g.:
```cs
[Test]
public void World_should_have_an_EntityManager()
{
    using (var world = new World("test-world"))
    {
        Assert.IsNotNull(world.GetOrCreateManager<EntityManager>());
    }
}
```

However, if you will be adding a similar test to the same
 fixture, e.g. you need to create another `world` to assert something else about
 the `world`, move the creation and disposal of the instance into the `[SetUp]`
 and `[TearDown]` functions:

```cs
private World world;

[SetUp]
public void SetUp()
{
    world = new World("test-world")
}

[TearDown]
public void TearDown()
{
    world.Dispose();
}

[Test]
public void World_should_have_an_EntityManager()
{
    Assert.IsNotNull(world.GetOrCreateManager<EntityManager>());
}

[Test]
public void World_should_not_have_behaviour_managers_when_created()
{
    Assert.AreEqual(0, world.BehaviourManagers.Count());
}
```

### Handling Unity logs

By default, any error logs will fail tests.

You can choose to expect these errors or ignore them using the `LogAssert`
 class. Please refer to the [official documentation by Unity](https://docs.unity3d.com/ScriptReference/TestTools.LogAssert.html)
 for usage instructions.

It is recommended to:
- expect any error logging that the Unity GDK outputs.
- expect any error logging that is relevant to the functionality that you are
 testing.
- ignore irrelevant ones as last resort.

Do not forget to reset `LogAssert.ignoreFailingMessages` to false if you had
 modified it for a test.
