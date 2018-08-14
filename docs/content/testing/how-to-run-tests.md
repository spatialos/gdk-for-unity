[google-docs-link]: https://docs.google.com/document/d/1cNB-1CS-m3-28tZfVyi9ljWPiVwjihkphNN4Q9x_3EI/edit (Please place reviews as comments into this document here)

**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# How to run tests

This document describes how to run tests for the two parts of the SpatialOS GDK for Unity:
* the tools (which includes the document linter)
* the Unity project

Note that each part of the GDK project has its own separate testing.

## Prepare the workspace
To run any test, you need to prepare your workspace. To do this, open up a terminal window and, from the root directory of the GDK repository you cloned, run:

```bash
./prepare-workspace.sh
```


**Note:**<br/>
Before you start using the Spatialos GDK for Unity for any purpose, you will need to run `./prepare-workspace.sh` at least once.

## How to run all tests
To run tests on all elements of the SpatialOS GDK for Unity (the tools and the Unity project which forms part of the SpatialOS GDK for Unity), open a terminal window and, from the root directory of the GDK repository, run the following command:

```bash
./ci/test.sh
```

## Test success or failure
* A successful test run displays this message: `All tests passed!`
* A failed test run displays this message: `Tests failed! See above for more information.`

**How to test the SpatialOS GDK’s Unity project only**<br/>
In addition to the `test.sh` script mentioned above, you can use the Test Runner Window of Unity Editor to test the Unity Engine integration specific parts of the SpatialOS GDK for Unity.
The tests for the GDK are the assemblies that start with `Improbable.Gdk.`.<br>
For more information on how to use the Unity Test Runner, see Unity’s [Unity Test Runner manual page](https://docs.unity3d.com/Manual/testing-editortestsrunner.html).

[//]: # (Editorial review status: Full review 2018-07-13)
