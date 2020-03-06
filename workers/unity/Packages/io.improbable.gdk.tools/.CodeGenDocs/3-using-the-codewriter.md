# CodeWriter API

Get started with the CodeWriter API by writing `CodeWriter.Populate`, providing an `Action` on a `CodeWriter`:

```csharp
public static CodeWriter Generate()
{
    return CodeWriter.Populate(cw =>
    {
        //define codewriter contents here
    });
}
```

The output of each `CodeWriter` is intended to be a separate file, therefore each instance exposes the `UsingDirectives` method to define a list of using directives that should be at the top of the file:

```csharp
public static CodeWriter Generate()
{
    return CodeWriter.Populate(cw =>
    {
        cw.UsingDirectives(
            "System",
            "System.Collections.Generic"
        );

        //define file contents here
    });
}
```

> Note: The `CodeWriter` class also exposes a `Format` method, which returns the contents of the class in a properly-indented format as a string.

## Namespaces

The `CodeWriter` entrypoint for writing code generators is the `Namespace` function.

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

This is generated into code that resembles the following:

```csharp
using System;
using System.Collections.Generic;

namespace Your.Own.Namespace
{
    //namespace contents
}
```

## Types

Use the `Type` method to define structs and classes.

### Structs

```csharp
ns.Type("public struct ExampleStruct", t =>
{
    //define code
});
```

Generated code:

```csharp
public struct ExampleStruct
{
    //code
}
```

### Classes

```csharp
ns.Type("public class ExampleClass", t =>
{
    //define code
});
```

Generated code:

```csharp
public class ExampleClass
{
    //code
}
```

## Enums

The `Enum` methods provides handy helpers for defining enums:

- `Member`
- `Members`

### Member

```csharp
ns.Enum("public enum ExampleEnum", e =>
{
    e.Member("MEMBER1");
    e.Member("MEMBER2");
    e.Member("MEMBER3");
});
```

Generated code:

```csharp
public enum ExampleEnum
{
    MEMBER1,
    MEMBER2,
    MEMBER3
}
```

### Members

```csharp
ns.Enum("public enum ExampleEnum", e =>
{
    e.Members(new[]
    {
        "MEMBER1",
        "MEMBER2",
        "MEMBER3"
    });
});
```

Generated code:

```csharp
public enum ExampleEnum
{
    MEMBER1,
    MEMBER2,
    MEMBER3
}
```

## Methods

Use the `Method` function to begin defining a method.

> To follow C# standards, the `Method` function is only available inside types.

```csharp
cw.Namespace("Your.Own.Namespace", ns =>
{
    /*
      invalid code:
          ns.Method("public void InvalidCode", m => {});
    */

    ns.Type("public class TypeName", t =>
    {
        t.Method("public void TestMethod()", m =>
        {
            //define code
        });
    });
});
```

Generated code:

```csharp
namespace Your.Own.Namespace
{
    public class TypeName
    {
        public void TestMethod()
        {
            //code
        }
    }
}
```

## Annotations

### Outside a type

Outside a type (ie at namespace level), the `Annotate` method will only let you create annotated types and enums, as you are not allowed to define methods outside of a type in C#.

```csharp
cw.Namespace("Your.Own.Namespace", ns =>
{
    ns.Annotate("Test").Type("public class TypeName", t =>
    {
        //define code
    });

    ns.Annotate("Test").Enum("public enum EnumName", t =>
    {
        //define members
    });
});
```

Generated code:

```csharp
namespace Your.Own.Namespace
{
    [Test]
    public class TypeName
    {
        //code
    }

    [Test]
    public enum EnumName
    {
        //members
    }
}
```

### Inside a type

Within a type, the `Annotate` method will also let you create annotated methods.

```csharp
cw.Namespace("Your.Own.Namespace", ns =>
{
    ns.Type("public class TypeName", t =>
    {
        t.Annotate("TestAnnotation").Method("public void TestMethod()", m =>
        {
            //define code
        });
    });
});
```

Generated code:

```csharp
namespace Your.Own.Namespace
{
    public class TypeName
    {
        [TestAnnotation]
        public void TestMethod()
        {
            //code
        }
    }
}
```

## Initializer

An initializer shares similar traits to a custom scope and enums, but the key difference is that it must end with a semi-colon. The `Initializer` API exists to cater for the difference.

### Example: initialize by hand

```csharp
t.Initializer("private static readonly int[] CountToFive = new int[]", () => new[]
{
    "0",
    "1",
    "2",
    "3",
    "4",
    "5"
});
```

Generated code:

```csharp
private static readonly int[] CountToFive = new int[]
{
    0,
    1,
    2,
    3,
    4,
    5
}
```

### Example: initialize from list

```csharp
var zeroToFive = new List<string> { "0", "1", "2", "3", "4", "5" };

...

t.Initializer("private static readonly int[] CountToFive = new int[]", () => zeroToFive);
```

Generated code:

```csharp
private static readonly int[] CountToFive = new int[]
{
    0,
    1,
    2,
    3,
    4,
    5
}
```

## Scoped bodies

A scoped body is used to describe the content of scopes that can contain some actual logic. For example, a `Namespace` does not have logic directly within it - you'd have to define logic inside a `Method` within a `Type`.

A `Method` is an example of a scoped body, because you can define arbitrary logic inside it. Examples of scoped bodies include:

* methods (as mentioned)
* loops
* each scope within an `if-elseif-else` statement
* each scope within a `try-catch-finally` statement
* custom scopes

> In the code generator, an abstract `ScopeBody` class exists to generalise the API available for types of scoped bodies.

The API described in the sections below is avaiable from all scoped bodies.

### Loops

The `Loop` method is a special type of custom scope body that also offers `Continue` and `Break` methods.

#### Example: basic loop

```csharp
m.Loop("foreach (var x in xs)", e =>
{
    //define code
});
```

Generated code:

```csharp
foreach (var x in xs)
{
    //code
}
```

#### Example: continue & break

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

    //define more code
});
```

Generated code:

```csharp
foreach (var x in xs)
{
    if (x == 0)
    {
        continue;
    }
    else if (x > 5)
    {
        break;
    }

    //more code
}
```

### If/else statements

The `If` method is an entry point for defining `If/ElseIf/Else` statements.

#### Example: if-else

```csharp
m.If("condition", then =>
{
    //define code
});
```

Generated code:

```csharp
if (condition)
{
    //code
}
```

#### Example: if-elseif-else

```csharp
m.If("condition1", then =>
{
    //define code
})
.ElseIf("condition2", then =>
{
    //define code
}).Else(then =>
{
    //define code
});
```

Generated code:

```csharp
if (condition1)
{
    //code
}
else if (condition2)
{
    //code
}
else
{
    //code
}
```

### Try/catch statements

The `Try` method is an entry point for defining `Try/Catch/Finally` statements.

Note that you **must** define at least one `Catch` or `Finally` scope, otherwise an `InvalidOperationException` will be thrown at format-time.

#### Example: try-catch-finally

```csharp
m.Try(t =>
{
    //define code
})
.Catch("InvalidOperationException e", e =>
{
    //define code
})
.Catch("Exception e", e =>
{
    //define code
})
.Finally(f =>
{
    //define code
});
```

Generated code:

```csharp
try
{
    //code
}
catch(InvalidOperationException e)
{
    //code
}
catch(Exception e)
{
    //code
}
finally
{
    //code
}
```

#### Example: try-finally

```csharp
m.Try(t =>
{
    //define code
})
.Finally(f =>
{
    //define code
});
```


Generated code:

```csharp
try
{
    //code
}
finally
{
    //code
}
```

### Custom scopes

A custom scope can be declared with or without a declaration.

#### Example: without declaration

```csharp
m.CustomScope(cs =>
{
    //define code
});
```

Generated code:

```csharp
{
    //code
}
```

#### Example: with declaration

```csharp
m.CustomScope("using (var x = Some.RandomThing())", cs =>
{
    //define code
});
```

Generated code:

```csharp
using (var x = Some.RandomThing())
{
    //code
}
```

## Profiler sampling

The `ProfilerStart` and `ProfilerEnd` methods are wrappers for adding profile markers to generated code.

```csharp
t.Method("public void ExampleMethod()", m =>
{
    m.ProfilerStart("Sample name");

    //define code

    m.ProfilerEnd();
});
```

Generated code:

```csharp
public void ExampleMethod()
{
    Profiler.BeginSample("Sample name");

    //code

    Profiler.EndSample();
}
```

## Return statements

There are two wrappers for writing `Return` statements in generated code:

- `Return`
- `Return(string toReturn)`

### Void return

```csharp
t.Method("public void ExampleMethod()", m =>
{
    //define code

    m.Return();
});
```

Generated code:

```csharp
public void ExampleMethod()
{
    //code

    return;
}
```

### Non-void return

```csharp
t.Method("public bool ExampleMethod()", m =>
{
    //define code

    m.Return("true");
});
```

Generated code:

```csharp
public bool ExampleMethod()
{
    //code

    return true;
}
```

## Lines of code

The `Line` and `TextList` methods exist to define individual lines of code. To ensure they work in a similar way, the base `Text` construct exists.

### Text

A block of `Text` is one string. When the call to `Format` is made, each `Text` is separated by a newline:

```csharp
ns.Text(Text.New("//some comment"));
ns.Text(Text.New("//some comment"));
ns.Text(Text.New("//some other comment"));
```

Generated code:

```csharp
//some comment

//some comment

//some other comment
```

### Line

The `Line` method is a wrapper for creating blocks of `Text`. For example, the following code generates identical code to the previous example:

```csharp
ns.Line("//some comment");
ns.Line("//some comment");
ns.Line("//some other comment");
```

Generated code:

```csharp
//some comment

//some comment

//some other comment
```

### TextList

A `TextList` is a sequence of `Text` blocks, with the ability to customise the string that separates each `Text`. As each `Text` is a single `string`, you construct a `TextList` by providing an `IEnumerable<string>`.

```csharp
ns.TextList(TextList.New(new[]
{
    "//some comment",
    "//some comment",
    "//some other comment"
}));
```

*By default, each `Text` element is generated on a new line:*

```csharp
//some comment
//some comment
//some other comment
```

#### Example: ", " as separator

```csharp
ns.TextList(TextList.New(", ", new[]
{
    "//some comment",
    "//some comment",
    "//some other comment"
}));
```

Generated code:

```csharp
//some comment, //some comment, //some other comment
```

#### Example: ",\n" as separator

```csharp
ns.TextList(TextList.New(",\n", new[]
{
    "//some comment",
    "//some comment",
    "//some other comment"
}));
```

Generated code:

```csharp
//some comment,
//some comment,
//some other comment
```

## Scopes with strings

In addition to the API described above, the following methods also provide an override that take a `Func<IEnumerable<string>>`:

- `Enum`
- `Initializer`
- `Method`
- `CustomScope`
- `Loop`
- `If`/`ElseIf`/`Else`
- `Try`/`Catch`/`Finally`

This override is effectively a wrapper for defining the above scopes with a `TextList`, therefore the formatted scope will have each element of the `IEnumerable<string>` placed on a different line.

### Comparisons

As you can see in the snippet below, the `Func<IEnumerable<string>>` override provides a cleaner way to define scope bodies with strings.

```csharp
vs.Method("public void LineExample(int input)", m =>
{
    m.Line("var x = 5;");
    m.Line("var y = SomeFunc(input);");
    m.Line("Debug.Log(x*y);");
});

vs.Method("public void TextListExample(int input)", m =>
{
    m.TextList(TextList.New(new[]
    {
        "var x = 5;",
        "var y = SomeFunc(input);",
        "Debug.Log(x*y)"
    }));
});

// Less boilerplate than `Line` or `TextList`
vs.Method("public void FuncExample(int, input)", () => new[]
{
    "var x = 5;",
    "var y = SomeFunc(input);",
    "Debug.Log(x*y)"
});
```

Generated code:

```csharp
// Each `Line` is spaced out
public void LineExample(int input)
{
    var x = 5;

    var y = SomeFunc(input);

    Debug.Log(x*y);
}

// Default `TextList` formatting
public void TextListExample(int input)
{
    var x = 5;
    var y = SomeFunc(input);
    Debug.Log(x*y);
}

// Identical formatting to default `TextList`
public void FuncExample(int input)
{
    var x = 5;
    var y = SomeFunc(input);
    Debug.Log(x*y);
}
```
