# SpatialOS GDK for Unity C# coding standards

**Contributions**: We are not currently taking public contributions - see our [contributions](README.md#contributions) policy. However, we are accepting issues and we do want your [feedback](README.md#give-us-feedback).

## Table of contents

* SpatialOS GDK for Unity C# coding standards
    * [Table of contents](#table-of-contents)
    * [Introduction](#introduction)
    * [ReSharper and formatting](#resharper-and-formatting)
    * [General](#general)
    * [Casing](#casing)
    * [Deprecation](#deprecation)
    * [Tests](#tests)
    * [Unity specific](#unity-specific)

## Introduction

Generally, we use [Microsoft's C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions). See also the [Framework Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/index). Anything noted below overrides these conventions.

## ReSharper and formatting

(Optional) Install [ReSharper](https://www.jetbrains.com/resharper/) for Visual Studio.

Please format any code files that you've touched (normally `Ctrl+Alt+F`) before committing changes.

## General

* Avoid interfaces with fewer than two implementations.
* Avoid extension methods. They can introduce magic and make implementation hard to find.
* Avoid Linq. It often introduces unnecessary allocation.
* Non-obvious constant-value arguments (e.g. - `42` and `true`) should use named parameters:
    * `SomeFunction(answer: 42)`
* Callbacks passed to native code need to be static methods marked with the `[MonoPInvokeCallback]` attribute. `GCHandle` should be used for dynamic object storage and lookup.
* Disposable objects should throw `ObjectDisposedException` when methods are called on them, where appropriate.
* Use interpolation strings, `string.Format`, or `Log*Format` rather than string concatenation.
    * :white_check_mark: `var message = Console.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");`
    * :white_check_mark: `var message = String.Format("The current price is {0} per ounce.", pricePerOunce)`
    * :x: `var message = "The current price is " + pricePerOunce + " per ounce."`
* Avoid lines longer than 120 characters.
* Use braces for any kind of conditional or loop statements.
* Add an empty line at the end of the file.
* Avoid any kind of redundant whitespace.
* Use spaces instead of tabs for indentation.

## Casing

* PascalCase for class and method names.
* PascalCase for public variables and private const variables, camelCase for private variables.
* Acronyms have only their first letter capitalised in variable or method names. In comments, acronyms are fully-capitalised.
    * :white_check_mark: `EntityAcl GetEntityAcl();`
    * :x: `EntityACL GetEntityACL();`
    * :white_check_mark: `var entityAcl = GetEntityAcl(); // Gets the Entity ACL.`
    * :x: `var entityAcl = GetEntityAcl(); //Gets the Entity Acl.`

## Deprecation

* Annotate deprecated functions with `[System.Obsolete(<string>)]` where `<string>` is a short summary of the deprecation reason and either a guide on how to upgrade or a link to a guide on how to upgrade.

## Tests

* Name test fixtures as `<class>Tests`
* Name test methods as `<method>_should_<action>_when_<conditions>`

## Unity specific

* We are using the `.NET 4.x` support in the SpatialOS GDK for Unity (mandated by use of the Unity ECS). This equates to `C# 7` so newer language features are supported.
* Implement logic in a `ComponentSystem` instead of a `MonoBehaviour` wherever possible.
* Make sure you remove all `Debug.Log` statements before opening a PR.
* Avoid running `foreach` over an `IEnumerable<T>` because it allocates excessively. See [this StackOverflow question](https://stackoverflow.com/questions/19689328/why-ienumerable-slow-and-list-is-fast) for an explanation of why `IEnumerable<T>` allocates.
* Be aware of the possible allocations [when using collections](https://jacksondunstan.com/articles/3805) and avoid doing so where the volume would impact performance.
* When using structs as keys dictionaries, sets or in comparisons, ensure to implement a custom hash code function and the `IEquatable<>` interface to avoid a performance drop.
* Avoid using enums as dictionary keys. This leads to extra allocations due to boxing in the Mono runtime. The boxing can be avoided by implementing `EqualityComparer<MyEnum>` for your enum as described [here](https://stackoverflow.com/a/26281533).
* When writing Unity code that's not compatible with all supported versions of Unity, use `ifdef`s:
    * Write all of them in a forward-compatible way.
    * Write `if (!(UNITY_2018_0 | UNITY_2018_1))` instead of `if (UNITY_2018_2)` if you want to specify `2018.2` or newer.
    * Be careful with defines such as `UNITY_2018_1_OR_NEWER` - they might not be available in all versions of Unity 2018.1.
* Use `== null` when testing `GameObject`s and `MonoBehaviour`s even though they overload the `operator!`.
* Avoid using custom threads. They complicate things and can easily cause crashes.
