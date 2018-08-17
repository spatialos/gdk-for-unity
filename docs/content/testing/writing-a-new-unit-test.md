[google-docs-link]: https://docs.google.com/document/d/14_FY-Chu_illhaCCym-pv6P8NHtyN9PLzVBGSuK0ou4/edit (Please place reviews as comments into this document here)

**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Writing a new unit test

This document covers Unity tests only (that is tests of the Unity project which forms part of the SpatialOS GDK for Unity). It walks through creating tests for a made-up class to explain how you can test parts of the GDK. You need to get to know NUnit and the Unity Test Runner before creating tests. See below for more information on these.

## Prerequisites

Please look through and understand the follow documentation and videos before writing a new test:
* SpatialOS GDK for Unity documentation - [Testing overview](./testing-overview.md)
* SpatialOS GDK for Unity documentation - [How to run tests](./how-to-run-tests.md)
* SptialOS GDK for Unity documentation - [Testing guidelines](./testing-guidelines.md)
* NUnit video - [Introduction to NUnit](https://www.youtube.com/watch?v=1TPZetHaZ-A)<br/>
**Skip** the part between 0:40 - 2:49 as it does not apply to Unity testing. <br/>
**Skip** the part between 5:25 - 7:57 as it explains how to use the NUnit Runner and we’ll use Unity Test Runner instead.
* [Unit Testing Using NUnit](https://www.codeproject.com/articles/178635/unit-testing-using-nunit)
- Unity documentation - [Writing and executing tests in Unity Test Runner](https://docs.unity3d.com/Manual/PlaymodeTestFramework.html)
* Infallible Code video -  [How To Test Unity ECS Code](http://infalliblecode.com/test-unity-ecs/) (Note the code for this is behind a paywall.)

## Example new test

The example below uses a made-up class `MyClass`.

Let's say you have this class:

```cs
using System;

namespace Improbable.Gdk.Core.Utility
{
    public class MyClass : IDisposable
    {
        public int InstanceValue;

        public MyClass()
        {
                // Pretend to set up some state.
                InstanceValue = 0;
        }

        public void DoInstanceOperation()
        {
                InstanceValue++;
        }

        public void DoSomethingElse()
        {
                InstanceValue += 5;
        }

        public static int AddTwoNumbers(int a, int b)
        {
                return a + b;
        }

        public void Dispose()
        {
                // Pretend to clean up some state.
        }
    }
}
```

This class would exist at this path: `workers/Unity/Packages/com.improbable.gdk.core/Utility/MyClass.cs`.

### Test folder and filename

The package for this class would be: `workers/Unity/Packages/com.improbable.gdk.core/`.

The path within the package would be: `Utility/MyClass.cs`.

Following the [package directory structure instructions](./testing-guidelines.md#package-directory-structure), the test filename would be:

`workers/Unity/Assets/Gdk/Core/Tests/EditMode/Utility/MyClassTests.cs`.

(This assumes that the `workers/Unity/Packages/com.improbable.gdk.core/Tests/` directory exists.
 If it does not exist, see the section on 
 [How to  create a new test folder and assembly](#how-to-create-a-new-test-folder-and-assembly).).

### Test fixtures

With NUnit, you can add a `TestFixture` attribute to a class. This makes the class a test. While you create specific tests for methods in classes, you can reuse the tests where methods have similar setups. (A class with a `TestFixture` attribute is also known as “test fixture” and “fixture”.) See NUnit’s documentation on the [TestFixture attribute](https://github.com/nunit/docs/wiki/TestFixture-Attribute) and the [test fixture example class](https://github.com/nunit/docs/wiki/TestFixture-Attribute#example-1) in the the NUnit repository.

As `MyClass` has both static and non-static methods, you can create tests within two separate fixtures.

You can use the following annotations for the setup and teardown logic:

- `[SetUp]` methods run before each test and `[TearDown]` methods run after each test.- `[OneTimeSetUp]` methods run before all tests and `[OneTimeTearDown]` methods after all tests.

For each fixture, the execution happens in this order:

```cs
// Before any test has run:
fixture.OneTimeSetUp();

// Then (for example), if you have two tests:
fixture.SetUp();
fixture.Test1();
fixture.TearDown();

fixture.SetUp();
fixture.Test2();
fixture.TearDown();

// After all tests have run:
fixture.OneTimeTearDown();
```

The namespace for the fixture should be  `Improbable.Gdk.Core.EditModeTests.Utility`, because the class you are testing
 is in the `Improbable.Gdk.Core.Utility` namespace.

The following code snippet is showing the first step to write tests for a class, declaring the fixtures and placing the setup and teardown code.

```cs
using Improbable.Gdk.Core.Utility;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditModeTests.Utility
{
    [TestFixture]
    public class MyClassStaticTests
    {
    }

    [TestFixture]
    public class MyClassInstanceTests
    {
        private MyClass myClassInstance;

        [SetUp]
        public void SetUp()
        {
            myClassInstance = new MyClass();
        }

        [TearDown]
        public void TearDown()
        {
            myClassInstance.Dispose();
        }
    }
}
```

### Test methods

The following are some example test methods. Note that the test should ensure that the `AddTwoNumbers` static method returns the sum of the
 input numbers.

The test name should match this expectation and contain relevant assertions.

```cs
[TestFixture]
public class MyClassStaticTests
{
    [Test]
    public void AddTwoNumbers_should_return_the_sum_of_the_input_numbers()
    {
        Assert.AreEqual(4, MyClass.AddTwoNumbers(1, 3));
        Assert.AreEqual(4, MyClass.AddTwoNumbers(3, 1));
        Assert.AreEqual(7, MyClass.AddTwoNumbers(2, 5));
    }
}
```

For the instance functionality, you can add tests similarly:

```cs
[TestFixture]
public class MyClassInstanceTests
{
    private MyClass myClassInstance;

    [SetUp]
    public void SetUp()
    {
        myClassInstance = new MyClass();
    }

    [TearDown]
    public void TearDown()
    {
        myClassInstance.Dispose();
    }

    [Test]
    public void InstanceValue_is_0_by_default()
    {
        Assert.AreEqual(0, myClassInstance.InstanceValue);
    }

    [Test]
    public void DoInstanceOperation_increases_InstanceValue_by_1()
    {
        myClassInstance.DoInstanceOperation();
        Assert.AreEqual(1, myClassInstance.InstanceValue);

        myClassInstance.DoInstanceOperation();
        Assert.AreEqual(2, myClassInstance.InstanceValue);
    }

    [Test]
    public void DoSomethingElse_increses_InstanceValue_by_5()
    {
        myClassInstance.DoSomethingElse();
        Assert.AreEqual(5, myClassInstance.InstanceValue);

        myClassInstance.DoSomethingElse();
        Assert.AreEqual(10, myClassInstance.InstanceValue);
    }
}
```

### Internal variables and methods

The tests above will not compile. This is because the test class is in a different assembly to the implementation class, and the tests are looking at internal fields.

To solve this; make the internals of the `MyClass.cs` file visible to the test assembly as shown below.

```
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Improbable.Gdk.Core.EditModeTests")]
```

### Running the new tests

In the Test Runner window of Unity, find your test under: **Improbable.Gdk.Core.EditModeTests.dll**. (This matches the folder name
 `Gdk/Core/Tests/EditMode`.)

You can follow the namespace and fixture name in the hierarchy:

- **Improbable** > **Gdk**  > **Core**  > **EditModeTests** > **Utility**  > **MyClassStaticTests** 
- **Improbable**  > **Gdk**  > **Core**  > **EditModeTests**  > **Utility**  > **MyClassInstanceTests** 

Double-click the test fixture name to run all tests within that fixture.

## How to create a new test folder and assembly

To add tests for the `/workers/Unity/Assets/Gdk/Legacy` folder within the GDK Unity project's assets:

1. Create the directory: `/workers/Unity/Assets/Gdk/Legacy/Tests/EditMode`

1. In the Unity Editor’s Project window, right-click the directory in the Project and select **Create** > **Assembly Definition**.

1. Name the new **Assembly Definition** file you have just created to match: `Improbable.Gdk.Legacy.EditModeTests`.

1. Select this file, and change the **name** property in the Unity Inspector to match the filename:`Improbable.Gdk.Legacy.EditModeTests`.

1. Click  **Apply** .

1. Check the **Test Assemblies** checkbox.

1. If you're writing EditMode tests:

    1. Uncheck the **Any Platform** checkbox.

    1. Scroll to the bottom, and select **Deselect All**.

    1. Check the **Editor** checkbox only.

    1. Click on **Apply** again.

1. In the references list, add the reference to the package you are testing (in this
 case, `Improbable.Gdk.Legacy`).

1. Click **Apply** again.

### Add the entities package references to test assemblies

You need to edit the `.asmdef` file manually to add references to the entities preview package assemblies.

1. Right-click the assembly definition file, and select the **Show in Explorer** action.

1. Open in your favourite text editor.

1. Add in these lines to references:

```
  "Unity.Entities",
  "Unity.Entities.Hybrid",
```

The assembly definition file should look like this in your text editor:

```json
{
    "name": "Improbable.Gdk.Legacy.EditModeTests",
    "references": [
        "Improbable.Gdk.Legacy",
        "Unity.Entities",
        "Unity.Entities.Hybrid"
    ],
    "optionalUnityReferences": [
        "TestAssemblies"
    ],
    "includePlatforms": [
        "Editor"
    ],
    "excludePlatforms": [],
    "allowUnsafeCode": false
}
```

From now on, any `.cs` file you create within this directory and its
 subdirectories should be able to refer to `NUnit.*` and `Unity.Entities.*`
 assemblies and namespaces. Additionally, these assemblies are not 
 included within player builds.

For more information, please see the Unity documentation on [Unity Test Runner](https://docs.unity3d.com/Manual/testing-editortestsrunner.html).

[//]: # (Editorial review status: Full review 2018-07-11)
[//]: # (TODO:)
[//]: # (1. Add Mac instructions to this - UTY-640.)
