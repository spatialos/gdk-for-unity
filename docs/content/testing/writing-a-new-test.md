**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Writing a new test

This document walks through creating tests for a made-up class to explain how
 the parts of the Unity GDK can be tested.

## Prerequisites

Please read through and understand the [Testing Overview](./testing-overview.md)
 and the [Testing Guidelines](./testing-guidelines.md) documents first.

## Using NUnit

To help get started with testing, we recommend these documents which help cover
 the basics:

- [Introduction to NUnit](https://www.youtube.com/watch?v=1TPZetHaZ-A):
  - Skip the part between 0:40 - 2:49
    - Project setup
  - Skip the part between 5:25 - 7:57
    - It explains how to use the NUnit Runner
    - We’ll use Unity Test Runner instead
- [Unit Testing Using NUnit](https://www.codeproject.com/articles/178635/unit-testing-using-nunit)

Unity specific testing guidelines:
- [Writing and executing tests in Unity Test Runner](https://docs.unity3d.com/Manual/PlaymodeTestFramework.html)

## Example new test

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
            // Pretend to set up some state
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
            // Pretend to clean up some state
        }
    }
}
```

And this class exists at this path:

`workers/unity/Assets/Gdk/Core/Utility/MyClass.cs`.

### Test folder and filename

The module for this class is:

`workers/unity/Assets/Gdk/Core/`.

The path within the module, is:

`Utility/MyClass.cs`.

Following the [module directory structure instructions](./testing-guidelines.md#module-directory-structure), the test filename should be: 

`workers/unity/Assets/Gdk/Core/Tests/Editmode/Utility/MyClassTests.cs`

This assumes that the `workers/unity/Assets/Gdk/Core/Tests/` directory exists.
 If it does not exist, please refer to the
 [Creating a New Test Folder and Assembly](#creating-a-new-test-folder-and-assembly)
 heading.

### Test fixtures

Test Fixtures allow test methods within them to share similar setups.

Since this class has both static and non-static methods, they can be tested
 within two separate fixtures.

The setup/teardown logic are done through these annotations:

- `[SetUp]` and `[TearDown]` methods are performed before and after each test.
- `[OneTimeSetUp]` and `[OneTimeTearDown]` methods are peformed before and
 after all tests.

For each fixture, the execution happens in this order:

Before any test has run:
- `[OneTimeSetUp]`

Then, e.g. if you have two tests:
- `[SetUp]`
- Test 1
- `[TearDown]`
- `[SetUp]`
- Test 2
- `[TearDown]`

And after all tests have been run:
- `[OneTimeTearDown]`

The namespace for the fixture should be
 `Improbable.Gdk.Core.EditmodeTests.Utility`, because the class you are testing
 is in the `Improbable.Gdk.Core.Utility` namespace.

```cs
using Improbable.Gdk.Core.Utility;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Utility
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

Let's write a few test methods.

The test should ensure that the `AddTwoNumbers` static method returns the sum of the
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

The tests above will not compile.

This is because the test class is in another assembly than the implementation
 class, and the tests are looking at internal fields.

There is a solution. In the `MyClass.cs` file, you can make its internals
 visible to the test assembly:

```
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Improbable.Gdk.Core.EditmodeTests")]
```

### Running the new tests

In the Test Runner window of Unity, you can find the test under:
- Improbable.Gdk.Core.EditmodeTests.dll (this matches the folder name
 `Gdk/Core/Tests/Editmode`)

And you can follow the namespace and fixture name in the hierarchy:

- `Improbable` > `Gdk` > `Core` > `EditmodeTests` > `Utility` > `MyClassStaticTests`
- `Improbable` > `Gdk` > `Core` > `EditmodeTests` > `Utility` > `MyClassInstanceTests`

Double-click the fixture name to run all tests within that fixture.

## Creating a new test folder and assembly

Let's say you want to add tests for the `/workers/unity/Assets/Gdk/Legacy`
 folder within the Unity project's assets.

Create the directory:

`/workers/unity/Assets/Gdk/Legacy/Tests/Editmode`

Right click the directory in the Project window in Unity, and select
 `Create` > `Assembly Definition`.

Name this file to match:

`Improbable.Gdk.Legacy.EditmodeTests`.

Select this file, and change the `name` property in the inspector to match the
 filename:

`Improbable.Gdk.Legacy.EditmodeTests`.

Hit the `Apply` button.

Check the `Test Assemblies` checkbox.

If you're writing Editmode tests:
- Uncheck the `Any Platform` checkbox.
- Scroll to the bottom, and press the `Deselect All` button.
- Check the `Editor` checkbox only.
- Hit the `Apply` button again.

In the references list, add the reference to the module you are testing (in this
 case, `Improbable.Gdk.Legacy`).

Hit the `Apply button` again.

### Adding the entities package references to test assemblies

You need to edit the `.asmdef` file manually to add references to the Entities
 preview package assemblies.

Right click the assembly definition file, and press the `show in explorer`
 action.

Open in your favourite text editor.

Add in these lines to references:

```
  "Unity.Entities",
  "Unity.Entities.Hybrid",
```

In the end, the assembly definition file should look like this in the text
 editor:

```json
{
    "name": "Improbable.Gdk.Legacy.EditmodeTests",
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
 assemblies and namespaces. Additionally, these assemblies will not be
 included within player builds.

For more info, please see [Unity Test Runner manual](https://docs.unity3d.com/Manual/testing-editortestsrunner.html).
