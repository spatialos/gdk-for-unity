<%(TOC)%>
# Test overview

We use tests to validate functionality and to ensure that the SpatialOS GDK for Unity (GDK) and the framework as a whole are resilient under different conditions.

## Test categories

We have two main test categories:

* Tools tests, such as testing the linter
* Unity tests - these test the Unity project which forms part of the GDK

The tests in both of these categories use the NUnit testing framework. NUnit is the open source library that’s included with Unity; it forms the basis of the Unity Test Runner. See the [NUnit’s documentation](https://github.com/nunit/docs/wiki/NUnit-Documentation) and Unity’s User Manual [Test Runner documentation](https://docs.unity3d.com/Manual/testing-editortestsrunner.html) for further information.

## Where to find test code

* The documentation linter tests are in `tools\DocsLinter\Tests`.
* The code generator tests are in `workers\unity\Packages\com.improbable.gdk.tools\.CodeGenerator\src\Tests`.
* For the Unity tests, each GDK Module has its own `Tests` directory.
  * For example: `workers\unity\Packages\com.improbable.gdk.core\Tests`

Find out more about the GDK Unity project test directory structure and file names in [Testing guidelines](\{\{urlRoot\}\}/reference/testing/testing-guidelines#test-directory-structure-and-file-names).

## Further information

* [How to run tests](\{\{urlRoot\}\}/reference/testing/how-to-run-tests)
* [Unity testing guidelines](\{\{urlRoot\}\}/reference/testing/testing-guidelines)
* [Writing a new Unity unit test](\{\{urlRoot\}\}/reference/testing/writing-a-new-unit-test)

[//]: # (Editorial review status: Full review 2018-07-13)
