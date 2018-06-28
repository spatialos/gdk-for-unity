**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Running tests

Run the following commands in your terminal from the root of the repository.

For the first run, you need these commands:

```bash
./prepare-workspace.sh
./ci/build-test.sh
```

If you have modified anything in the code generator or docs linter tool;
 to re-test these you need to re-run this command:

```bash
./ci/build-test.sh
```

If you have modified anything in the Unity Project; to re-test this you can
 re-run this command:

```bash
./ci/test.sh
```

## Unity tests

For Unity tests, you can also run tests from the Unity Test Runner Window.

### 1. Open the Unity project in Unity 

The unity project can be found in this directory within the repository: `/worker/unity`

### 2. Open test runner window

From the toolbar at the top of the Unity Editor, select "Window" > "Test Runner".

### 3. Run tests

You can press the "run all" button to run all tests. These will include the
 tests that come with the Unity Entities packages.

As each test is being run, the status of it will be displayed next to the test,
 with a green tick if it passes, or a red cross if it fails.

The tests for the GDK can be found in the assemblies that start with
 `Improbable.Gdk.`.

You can click on the arrows on the left of the assemblies to expand or collapse
 them.

Double click any item in the hierarchy to run the tests within that entry.

You can also right-click to either run or to open the source code for a test.

For any test method, if you select it, the panel at the bottom shows more
 information on what went wrong, and if there were any log messages during
 the test.