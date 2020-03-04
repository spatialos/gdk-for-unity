# Frequently Asked Questions

## How does `CodeGen` figure out whether a `CodegenJob` is dirty?

A `CodegenJob` is considered "dirty" if it meets at least one of the following conditions:

- has no expected input schema files.
- has no expected output files.
- any expected output files are missing.
- any schema file was written to after the last output file was written.

> The code generator also contains a `--force` flag to force all jobs to be marked as dirty.

---

## When should I run `Generate code (force)` instead of `Generate code`?

You should run `Generate code (force)` if any of the below apply to you:

- you have added, modified or removed schema source directories.
- you want to clean and reset the build directory's `CodeGen` solution.

---

## How does `CodegenJob` execute its job targets?

A `CodegenJob` keeps a `List<JobTarget>` to track all the jobs it has to do. Each `JobTarget` keeps track of the target filepath and a `Func` to generate the code for that file.

> See the `JobTarget` implementation [here](../.CodeGenTemplate/CodeGenerationLib/Jobs/JobTarget.cs#L5-L37).

The code generator `EntryPoint` finds classes inheriting from `CodegenJob` and instantiates them. Each class's constructor makes calls to `AddJobTarget` or `AddGenerators`, which adds to the list like so:

- `AddJobTarget`: create one `JobTarget`
- `AddGenerators`: create one `JobTarget` per `GeneratorInputDetails` per generator

The `EntryPoint` then calls the `JobRunner` to:

1. Check which jobs are dirty.
1. Run the dirty jobs.

The base `CodegenJob` calls the `Generate()` method on each `JobTarget` in the list. It then proceeds to write the generated code to the target filepath, creating the output directory if it has to.

---

## When does `CodeWriter` formatting work?

All generated code, whether written with the ergonomic API or not, have line endings fixed by the `JobTarget`:

```csharp
public string Format()
{
    return generatedContent.Format()
        .Replace("\r\n", "\n")
        .Replace("\n", Environment.NewLine);
}
```

The return value of this method is used as the contents for the generated code written to disk.

### Ok, what happens with the ergonomic API?

The `CodeWriter` system of writing code generators makes use of classes that implement `ICodeBlock`.

```csharp
namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    /// <summary>
    /// A unit of code that can be returned as an indented string.
    /// </summary>
    public interface ICodeBlock
    {
        string Format(int indentLevel = 0);
    }
}
```

The base `CodeWriter` provides a `Namespace` method to add a `NamespaceBlock` (which implements `ICodeBlock`). The `NamespaceBlock` provides further methods to add other types of `ICodeBlock`, with each type of `ICodeBlock` providing its own methods for adding other code blocks.

The `Format` method is called recursively on each `ICodeBlock`, starting from the top-level `CodeWriter` at an `indentLevel` of `0`. The `indentLevel` is increased each layer down.

Click [here](../.CodeGenTemplate/CodeGenerationLib/CodeWriter) to browse the `CodeWriter` directory within the `CodeGenerationLib` to understand more about how it is implemented.
