# Write a generator with the CodeWriter API

### _"It's lambdas all the way down!"_

## Set up a generator

Create a static class in your code generator's namespace that implements a `Generate` method, returning a `CodeWriter`.

For example:

```csharp
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Your.Own.CodeGenerator
{
    public static class YourFirstGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate()
        {
            // Create and return a CodeWriter
        }
    }
}
```

> Note: `NLog` is optional, but recommended for code generator logging.

### Making use of `AddGenerators`

To take advantage of the `AddGenerators` method on the `CodegenJob`, your function must match one of the following signatures:

```csharp
public static CodeWriter Generate(UnityComponentDetails details);
public static CodeWriter Generate(UnityTypeDetails details);
public static CodeWriter Generate(UnityEnumDetails details);
```

> Note: the methods do not _have_ to be called `Generate`, but they must return a `CodeWriter` and consume a type of `GeneratorInputDetails`. They also do not have to be public: accessibility should depend on where you are calling them from.

### Returning raw text

If you are preparing your own `string` to return and do not intend to take advantage of the `CodeWriter` API, you can either:

* Use `CodeWriter.Raw(string text)` to define a verbatim `CodeWriter`.
* Directly return `string` from the `Generate` method.

To take advantage of the `AddGenerators` method on the `CodegenJob`, your function must match one of the following signatures:

```csharp
public static string Generate(UnityComponentDetails details);
public static string Generate(UnityTypeDetails details);
public static string Generate(UnityEnumDetails details);
```

> Note: the methods do not _have_ to be called `Generate`, but they must return a `CodeWriter` and consume a type of `GeneratorInputDetails`. They also do not have to be public: accessibility should depend on where you are calling them from.

## CodeWriter: Introduction

To create a new CodeWriter, use the static `Populate` method. Use the provided APIs to define the contents of your file.

> See [here](3-using-the-codewriter.md) for an in-depth guide to using the `CodeWriter` API.

Here's an example:

```csharp
public static CodeWriter Generate()
{
    return CodeWriter.Populate(cw =>
    {
        cw.UsingDirectives(
            "System",
            "System.Collections.Generic"
        );

        cw.Namespace("Your.Own.Namespace", ns =>
        {
            //define namespace contents
        });
    });
}
```

## CodeWriter: Namespaces

Use the `Namespace` method as above to start defining a namespace:

## CodeWriter: Structs and classes

Structs and classes can be defined using the `Type` method where available.

## Enums

Enums can be defined with the `Enum` method where available.

## CodeWriter: Other scoped bodies

A scoped body is a generic body of code that is either a method or can lie within a method scope. Constructs of this kind are:

* `CustomScope`
* `Method`
* `Loop`

> Note: the API available within scoped bodies is not exposed at top level types/namespaces/enums as they can only be used inside method bodies.

### Custom scope

A custom scope is a a scope that can be defined with or without a declaration. For example:

```csharp
using (var thing = new SomethingDisposable())
{
    //scope with a declaration
}

{
    //scope without declaration
}
```

You can use this to define any un-annotated code construct, but it is recommended for constructs that don't already have a helper API.

### Methods

Methods are a special type of custom scope block that can also be annotated.

### Loops

Loops are a special type of custom scope block that also expose `Continue` and `Break` methods.

The following examples would all return identical generated code:

```csharp
m.Loop("foreach (var x in xs)", e =>
{
    e.If("x == 0", then =>
    {
        then.Continue();
    })
    .ElseIf("x > 5", then =>
    {
        then.Break();
    });

    //logic
});
```

```csharp
m.Loop("foreach (var x in xs)", e =>
{
    e.If("x == 0", then =>
    {
        then.Line("continue;");
    })
    .ElseIf("x > 5", then =>
    {
        then.Line("break;");
    });

    //logic
});
```

```csharp
m.Loop("foreach (var x in xs)", e =>
{
    e.If("x == 0", () => new[]
    {
        "continue;"
    })
    .ElseIf("x > 5", () => new[]
    {
        "break;"
    });

    //logic
});
```

## `Text` and `Scope` static constructors

The `Text` and `Scope` static classes exist for cases where you would like to define code constructs without using a top-level `CodeWriter`.

This is especially useful when splitting your generator logic across multiple methods, for example:

```csharp
public static CodeWriter Generate()
{
    return CodeWriter.Populate(cw =>
    {
        cw.UsingDirectives(...);

        cw.Namespace("Your.Own.Namespace", ns =>
        {
            ns.Text(GenerateSampleText());
            ns.Type(GenerateSampleType());
        });
    });
}

private static Text GenerateSampleText()
{
    return Text.New("//example text");
}

private static TypeBlock GenerateSampleType()
{
    return Scope.Type("public class ExampleClass", t =>
    {
        //define class here
    });
}
```
