[google-docs-link]: https://docs.google.com/document/d/1TfVlBR0_zENPniIKNDjXYlHJPnFB-_ltqPoHnK-6Bng/edit (Please place reviews as comments into this document here)

**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Test guidelines

This document provides guidelines on testing the Unity editor project contained within the SpatialOS GDK for Unity.

You can set up your own tests to validate that your functionality is resilient. Every test should always:

- Set up some initial state.
- Execute a method that acts upon this state and results in a new state.
- Make assertions in the new state against the expected changes.
- Clean up.

With [NUnit](https://github.com/nunit/docs/wiki/NUnit-Documentation), you can set up your test using the [TestFixture attribute](https://github.com/nunit/docs/wiki/TestFixture-Attribute)  method annotations.

## Test directory structure and file names


**Test directory location**<br/>
The project's assets are divided into modules, such as `Core`, `TransformSynchronization`, `PlayerLifecycle`. In the GDK repository, each module is in its own directory within `workers/unity/Packages`. In the future, each module will have its own tests which cover the functionality of that module only. Each module can have tests for Unity Editmode and Playmode using the Unity Test Runner (see the Unity User Manual [Test Runner documentation for the difference between EditMode and PlayMode tests](https://docs.unity3d.com/Manual/testing-editortestsrunner.html). Currently, there are only tests for the Core.

[//]: # (TODO: Update document when new Feature Module tests added.) 

The tests have to be located so that each module’s directory has a test directory containing tests about that module; the test directory must have separate subdirectories for EditMode and PlayMode testing if a module needs both of these tests. This split between modes is a requirement of Unity because EditMode and PlayMode tests need to be in different assemblies, and currently Unity `.asmdef` files affect all files within the same directory.

The EditMode and PlayMode directories must contain their own assembly definition files. For example, for the `Core` module, the module files are within
 `workers/unity/Packages/com.improbable.gdk.core`. The EditMode tests should be in
 `workers/unity/Packages/com.improbable.gdk.core/Tests/Editmode`.


**File names**<br/>
Test assembly definition files must match this pattern:
> `Improbable.Gdk.<ModuleName>.<TestMode>Tests.asmdef`

If you want to test a class with both EditMode and PlayMode tests, give the test file names as `<ClassName>EditModeTests.cs` and `<ClassName>PlayModeTests.cs` within their respective assemblies.


**Test directory organisation**<br/>
Within each test directory, the directory structure should match the non-test code structure.


For example, for the class `workers/unity/Packages/com.improbable.gdk.core/Utility/CommandLineUtility.cs` the test fixtures should be within `workers/unity/Packages/com.improbable.gdk.core/Tests/Editmode/Utility/CommandLineUtilityTests.cs`.

### Unity tests: EditMode vs. PlayMode tests

Choose to test in EditMode if you can: To run PlayMode tests, you need to set up Scenes. However, you don’t need to set these up to run EditMode tests, so it’s more time-efficient to run as many unit tests as possible as EditMode tests.

To help debug test failures, exclude functionality which you are not testing as this will make debugging test failures easier. When you are testing in EditMode, wherever it’s practical, mock up as many PlayMode-specific features as possible. For example, if a function depends on some time passing, use a mocked-up time interface instead of the `UnityEngine.Time` class directly.

It’s often impossible to do all tests in EditMode so you might have to test in PlayMode - for example when a particular kind of functionality responds to a physics callback. A PlayMode test is useful to check that certain functionality will still be present when a user is playing your game, or for more complicated integration testing that may depend on multiple systems interacting with each other.

## File structure

### Namespaces

Use the following naming convention for test namespaces:

`Improbable.Gdk.<ModuleName>.<TestMode>Tests`

Where:
- `<ModuleName>` is the GDK module being tested (for example, `Core`)
- `<TestMode>` is either `EditMode` or `PlayMode`

### Fixtures and methods

Using [NUnit](https://github.com/nunit/docs/wiki/NUnit-Documentation), you can add a `TestFixture` attribute to classes. These classes with `TestFixture` attributes are also known as ‘fixtures’ or ‘test fixtures’. You can use these fixtures to set up common properties for tests of a particular part of a feature.
(See the [NUnit documentation on TestFixture](https://github.com/nunit/docs/wiki/TestFixture-Attribute) for guidance.)

You can have multiple fixtures within a test file. These fixtures set up and clean up the different base conditions that you want to test. For example:

* If you would like to test different pre-conditions.

* If a class has both static and instance methods, you can test this within two fixtures, shown below:
    *  `<ClassName>StaticTests`
    * `<ClassName>InstanceTests`

Note: The `StaticTests` may not need to set anything up, but the `InstanceTests` may need to create an instance of the class to be able to verify the instance methods.

In most cases, each test file contains one fixture. In this case, name this fixture `<ClassName>Tests`.

The test methods should match this pattern:

 `<method>_should_<action>_when_<conditions>`.

### Setup and teardown

The following snippet shows the recommended order of the NUnit setup and tear
 down annotations:

```cs
[TestFixture]
public class ExampleTests
{
    // Proposed structure:
    // - one time setup and teardown
    // - per test setup and teardown
    // - each test one after another

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // Set up resources, variables and parameters that do not change per test.
        //For example; load a Scene that all tests in this fixture depend on.
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Clean up the resources, variables and parameters that do not change per test
        // For example; unload the Scene.
    }

    [SetUp]
    public void Setup()
    {
        // Set up resources, variables and parameters that change within each test
        // For example; create a GameObject.
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the resources, variables and parameters that change within each test
        // For example; delete the created GameObject.
    }

    [Test]
    public void X_should_Y_when_Z()
    {
        // Prepare case with condition Z, call X, assert that Y happens.
    }

    [Test]
    public void P_should_Q_when_R()
    {
        // Prepare case with condition Z, call P, assert that Q happens.
    }
}
```

### Assertions

For NUnit, you can use the `Assert` class to make assertions.

Please pay attention to parameter names, for example, for `Assert.AreEqual`,
 the first parameter is the expected value, and the second parameter is the
 value we are testing. (For an example, see the code snippet below.)

```cs
  var sumResult = Add(2, 3);
  Assert.AreEqual(5, sumResult);
```

#### Assertions for exceptions

By default, tests will fail if any code within it throws an exception.

Sometimes, you may want to test for conditions which throw exceptions. You can do this using the `Assert.Throws` method. This method returns the caught exception, and you can test further assertions against that. (For an example, see the code snippet below.)
```cs
        [Test]
        public void Validate_should_return_false_when_LocatorHost_is_empty()
        {
            var config = GetDefaultWorkingConfig();
            config.LocatorHost = "";

            var exception = Assert.Throws<System.ArgumentException>(config.Validate);
            Assert.IsTrue(exception.Message.Contains("locatorHost"));
        }
```

### Disposables vs setup and teardown

If you have a fixture that has only one test and the setup is simple, use the `using` keyword within the test. (For an example, see the code snippet below.)


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

However, if you are going to  add a similar test to the same
 fixture (for example, you need to create another `World` to assert something else about
 the `World`) move the creation and disposal of the instance into the `[SetUp]`
 and `[TearDown]` functions, as in the code snippet below.

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

By default, any error logs will fail your test.

You can choose to expect these errors or ignore them using the `LogAssert`
 class. Please refer to the [Unity  documentation](https://docs.unity3d.com/ScriptReference/TestTools.LogAssert.html)
 for use details.

We recommend that you:
* Expect any error logging that the modules of the Spatialos GDK for Unity outputs.
* Expect any error logging that comes from other parts of the Unity engine, or third parties that are relevant to the functionality that you are testing.
* Ignore irrelevant error logs as last resort.

Do not forget to reset `LogAssert.ignoreFailingMessages` to false every time you modify it for a test.


[//]: # (Editorial review status: Full review 2018-07-13)
[//]: # (Questions to deal with \(but not limited to\):)
[//]: # (TODO: 1. Update document when new Feature Module tests added.)
