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

## How to run tests

Using your terminal, in the repository directory:

For the first run:

```bash
./prepare-workspace.sh
./ci/build-test.sh
```

If you have modified anything in code generator or docs linter tool; to re-test you need to re-run:

```bash
./ci/build-test.sh
```

If you have modified anything in the Unity Project; to re-test you can run:

```bash
./ci/test.sh
```

For Unity tests, you can also follow these steps:

1. Prepare workspace
1. Open the Unity project in Unity Editor
1. Open test runner window
1. Run tests

For more detail, please see the [Running Tests](./running-tests.md) document.

## Where to find tests code

- For the tools tests, `tools\DocsLinter\Tests`
- For the code generator tests, `code_generator\src\Tests`
- For the Unity tests, within each module, the `Tests` directory.
  - e.g. `workers\unity\Assets\Gdk\Core\Tests`
  - e.g. `workers\unity\Assets\Gdk\PlayerLifecycle\Tests`

## Testing Guidelines

For the Unity tests, please see the [Testing Guidelines](./testing-guidelines.md) document.

## How to write tests

For the Unity tests, please see the [Writing a New Test](./writing-a-new-test.md) document.
