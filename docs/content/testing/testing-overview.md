**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Testing overview

We use testing to validate functionality and to ensure that the parts of the Unity GDK and the framework as a whole is resilient under different conditions.

## Test categories

We have three main test categories:
- Tools tests
- Code Generator tests
- Unity tests

All three of these categories are built using the NUnit testing framework.

## Where to find tests code

The documentation linter tests can be found in `tools\DocsLinter\Tests`.

The code generator tests can be found in `code_generator\src\Tests`.

For the Unity tests, within each module, the `Tests` directory contains the
 tests for that module.

For example:
  - `workers\unity\Assets\Gdk\Core\Tests`
  - `workers\unity\Assets\Gdk\PlayerLifecycle\Tests`

## How to run tests

Please see the [Running Tests](./running-tests.md) document.

## Testing guidelines

For the Unity tests, please see the [Testing Guidelines](./testing-guidelines.md)
 document.

## How to write tests

For the Unity tests, please see the [Writing a New Test](./writing-a-new-test.md)
 document.
