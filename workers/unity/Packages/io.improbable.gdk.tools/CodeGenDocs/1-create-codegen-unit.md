# How to configure a codegen unit

As mentioned in the overview, the GDK is responsible for finding and linking relevant codegen units to the `CodeGen` project. This means that configuring a new codegen unit is as simple as:

1. Creating a `.codegen` directory inside your package.
1. Creating a codegen job class that inherits from `CodegenJob`.
1. Hitting `Generate code` to update links in the `CodeGen` project.
1. Adding input file dependencies and job targets to your codegen job.
1. Testing your codegen job!

## 1. Create a `.codegen` directory

To find each unit easily, all modular codegen units must be defined inside a `.codegen` directory of a package in the Unity project.

For example:

```text
├── unity-project-root/
    ├── Assets/
    ├── Packages/
        ├── your-package-here/
            ├── .codegen/
                ├── Partials
                ├── Source
            ...
        ...
    ...
```

> Note: Neither `Partials` nor `Source` are mandatory, but the code generator will not look for files at other paths.

### Partials

All partials must be defined inside the `.codegen/Partial` directory.

```text
├── .codegen/
    ├── Partials
```

Partials are code snippets that are injected verbatim into generated code where a type's qualified name matches the file name of the partial.

For example, the `Improbable.Coordinates` partial defined in the `io.improbable.gdk.core` codegen unit defines a set of operators and extension methods for the `Improbable.Coordinates` type.

### Source

All code generation logic must be defined inside the `.codegen/Source` directory.

```text
├── .codegen/
    ├── Source
```

This is where any logic should be defined for a codegen unit. This is the directory where you should define your generators and the codegen job that calls them.

> Think carefully about how to structure `Source/`. The `CodeGen.sln` solution coalesces all contents and directories within each codegen unit's `Source/` directory into the `CodeGen.csproj` project.

Click [here](2-write-a-generator.md) for documentation on writing a generator.

## 2. Create a codegen job

When performing code generation, the `CodeGen` entry point:

1. Parses schema and stores their details.
1. Finds all `CodegenJob` classes in the `CodeGen` project.
1. Uses reflection to instantiate each `CodegenJob` that was found.
1. Checks which jobs are dirty.
1. Runs all jobs marked as dirty.

The base `CodegenJob`, defined in the `CodeGenerationLib`, contains methods to:

* Add files to depend on for dirty checks.
* Run dirty checks for the job.
* Add job targets.
* Clean up the job.
* Run the job.

> [FAQ: How does `CodeGen` figure out whether a `CodegenJob` is dirty?](faq.md)

Therefore, create a class that inherits from `CodegenJob` as below:

```csharp
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Your.Own.CodeGenerator
{
    public class YourCodegenJob : CodegenJob
    {
        public YourCodegenJob(string outputDir, IFileSystem fileSystem, DetailsStore store, bool force)
            : base(outputDir, fileSystem, store, force)
        {
            // Add job targets here
        }
    }
}
```

Ensure this is placed inside the `.codegen/Source` directory created in the previous step:

```text
├── unity-project-root/
    ├── Assets/
    ├── Packages/
        ├── your-package-here/
            ├── .codegen/
                ├── Source/
                    ├── YourCodegenJob.cs
                ...
            ...
        ...
    ...
```

### Open `CodeGen.sln`

Navigate to the `<projectroot>/build/codegen` directory and load `CodeGen.sln` in an IDE of your choice.

> Your codegen job isn't visible, because `GenerateCode.cs` hasn't had the chance to link it to the `CodeGen` project yet.

## 3. Update `CodeGen`

After creating a skeleton `CodegenJob`, select `SpatialOS` > `Generate code` from the Unity Editor. This will update `CodegenJob.csproj` with references to your newly-created codegen unit.

> You will know this has worked when your codegen job is visible in your IDE. Now that `CodeGen` is linked with your new codegen unit, you can iterate from within the IDE without having to switch to the Unity Editor.

## 4. Define what your codegen job should do

A codegen job consists of some schema files that the job depends on, and a set of targets that the job is expected to generate when it runs.

### Input files

These are used when performing a job's `IsDirty` check. A job can be marked as dirty if changes were made to any of the input files.

> Note: if no input files are given, a job will always be marked as dirty.

The `CodegenJob` class exposes two methods for adding input files:

* `AddInputFile`
* `AddInputFiles`

### Job targets

A `JobTarget` consists of a function to generate code for one file, as well as the target file path that this code should be written to. The code generation function must return either a `CodeWriter` or a `string`.

> A `CodeWriter` is the class used to access the ergonomic API for writing code generators.

There are two key ways to add job targets to a codegen job:

* Adding a single job target.
* Adding multiple job targets that operate on the same input.

#### `AddJobTarget`

This is a primitive method for adding one `JobTarget` at a time. Here you must specify:

* a target file path, **relative to the codegen job's output directory**.
* a `Func<string>` or `Func<CodeWriter>` for generating.

> Unless you specify otherwise, a codegen job's base output directory is the one set via the `native-output-dir=` command line argument. By default this is set to `<projectroot>/Assets/Generated/Source/`.

The `WorkerGenerationJob` from the `io.improbable.gdk.buildsystem` module contains good examples of this method:

```csharp
// Example that passes arguments into the generator.
AddJobTarget(Path.Combine(relativeEditorPath, WorkerFileName),
    () => UnityWorkerMenuGenerator.Generate(workerTypesToGenerate));

// Example with a generator that requires no arguments.
AddJobTarget(Path.Combine(relativeEditorPath, BuildSystemFileName),
    () => BuildSystemAssemblyGenerator.Generate());

// Example of returning a string.
AddJobTarget(Path.Combine(relativeOutputPath, WorkerListFileName),
    () => string.Join(Environment.NewLine, workerTypesToGenerate));
```

#### `AddGenerators`

Often when writing codegen jobs, you will have a set of `GeneratorInputDetails` that you wish to provide as arguments to multiple generators.

> `UnityTypeDetails`, `UnityEnumDetails` and `UnityComponentDetails` all inherit from `GeneratorInputDetails`.

Instead of manually iterating over these elements to add job targets:

```csharp
// I will cry if you do this.
foreach(var c in store.Components.Values)
{
    AddJobTarget(Path.Combine(c.NamespacePath, $"{c.Name}Example1.cs"), Example1Generator.Generate(c));
    AddJobTarget(Path.Combine(c.NamespacePath, $"{c.Name}Example2.cs"), Example2Generator.Generate(c));
    AddJobTarget(Path.Combine(c.NamespacePath, $"{c.Name}Example3.cs"), Example3Generator.Generate(c));
    ...
}
```

You could pass in lambdas that define a target file path and code generation function:

```csharp
AddGenerators(store.Components.Values,
    c => ($"{c.Name}Example1.cs", Example1Generator.Generate),
    c => ($"{c.Name}Example2.cs", Example2Generator.Generate),
    c => ($"{c.Name}Example3.cs", Example3Generator.Generate),
    ...);
```

When using `AddGenerators`, file paths must be relative to the `NamespacePath` directory of a given `GeneratorInputDetails`.

> The full target file path is combined inside `CodegenJob` like so:
>
> `Path.Combine(OutputDirectory, detail.NamespacePath, filePath)`.

All given code generation functions must take a single `GeneratorInputDetails` as an argument, returning either a `CodeWriter` or a `string`.

> You must ensure each generation method given to a single call of `AddGenerators` returns the same type.

## 5. Run code generation

Hit the play button in your IDE and watch as code generation happens!

Once complete, check the logs to find your codegen job. It should have run after being marked as dirty.

> Tip: you can force each codegen job to be dirty by adding the `-f` program argument.

If you've done this correctly, you should find that the generated code you expect is present in the `Assets/Generated/Source` directory of your Unity project!
