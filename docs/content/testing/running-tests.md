**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Running Tests

You can run tests from the command line, or using the Unity Editor, through
 the [Unity Test Runner window](https://docs.unity3d.com/Manual/testing-editortestsrunner.html).

## 1. Prepare Workspace

You need to do this once after you check out the repository, or if anything in
 the project setup has changed.

Open the repository directory in your terminal, for example using Git Bash.

```bash
$ ./prepare-workspace.sh
```

You also need to build the tools that will be used for testing.

```bash
$ ./ci/build.sh
```

## 2. (Optional) Run All Tests From The Command Line

From the repository directory:

```bash
./ci/test.sh
```

## 3. Open the Unity Project in Unity Editor

The unity project can be found in this directory:

> repository/worker/unity

## 3. Open Test Runner Window

From the toolbar at the top of the Unity Editor, select "Window" > "Test Runner".

## 4. Run Tests

You can press the "run all" button to run all tests. These will include the tests that come with the Unity Entities packages.

As each test is being run, the status of it will be displayed next to the test,
 with a green tick if it passes, or a red cross if it fails.

The tests for the GDK can be found in the assemblies that start with "Improbable.Gdk.".

You can click on the arrows on the left of the assemblies to expand or collapse them.

Double click any item in the hierarchy to run the tests within that entry.

You can also right-click to either run or to open the source code for a test.

For any test method, if you select it, the panel at the bottom shows more
 information on what went wrong, and if there were any log messages during
 the test.