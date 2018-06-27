**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Testing Overview

We use testing to validate functionality and to ensure that the parts of the Unity GDK and the framework as a whole is resilient under different conditions.

## Test Categories

We have three main test categories:
- Tools tests,
- Code Generator tests,
- Unity tests.

All three of these categories are built using the NUnit testing framework.

## Where to find tests code

- For the tools tests, `tools\DocsLinter\Tests`
- For the code generator tests, `code_generator\src\Tests`
- For the Unity tests, within each module, the `Tests` directory.
  - e.g. `workers\unity\Assets\Gdk\Core\Tests`
  - e.g. `workers\unity\Assets\Gdk\PlayerLifecycle\Tests`

## How to run tests

Please see the [Running Tests](./running-tests.md) document.

## Testing Guidelines

For the Unity tests, please see the [Testing Guidelines](./testing-guidelines.md)
 document.

## How to write tests

For the Unity tests, please see the [Writing a New Test](./writing-a-new-test.md)
 document.
