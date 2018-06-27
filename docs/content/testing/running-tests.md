**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Running Tests

Using your terminal, in the repository directory:

For the first run:

```bash
./prepare-workspace.sh
./ci/build-test.sh
```

If you have modified anything in the code generator or docs linter tool;
 to re-test these you need to re-run:

```bash
./ci/build-test.sh
```

If you have modified anything in the Unity Project; to re-test this you can
 re-run:

```bash
./ci/test.sh
```

For Unity tests, you can also follow these steps after preparing workspace:

1. Open the Unity project in Unity Editor
1. Open test runner window
1. Run tests

## 1. Open the Unity Project in Unity Editor

The unity project can be found in this directory within the repository:

> /worker/unity

## 2. Open Test Runner Window

From the toolbar at the top of the Unity Editor, select "Window" > "Test Runner".

## 3. Run Tests

You can press the "run all" button to run all tests. These will include the
 tests that come with the Unity Entities packages.

As each test is being run, the status of it will be displayed next to the test,
 with a green tick if it passes, or a red cross if it fails.

The tests for the GDK can be found in the assemblies that start with
 "Improbable.Gdk.".

You can click on the arrows on the left of the assemblies to expand or collapse
 them.

Double click any item in the hierarchy to run the tests within that entry.

You can also right-click to either run or to open the source code for a test.

For any test method, if you select it, the panel at the bottom shows more
 information on what went wrong, and if there were any log messages during
 the test.