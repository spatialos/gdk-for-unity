# Modular code generator overview

The modular code generator is responsible for finding schema across a project's packages, and executing code generation jobs defined for that schema.

In addition to schema sources explicitly defined in the GDK tools configuration, the code generator finds all `.schema` directories within packages used by your Unity project.

Aside from schema, the modular code generator has several key elements:

* [Code generation units](#codegen-units).
* [`CodeGen` C# project](#codegen).
* [`CodeGenerationLib` C# project](#codegenerationlib).
* [`GenerateCode.cs` script](#generatecodecs).

The base `CodeGen` template and `CodeGenerationLib` projects are found inside the `.CodeGenTemplate` directory within the `io.improbable.gdk.tools` package.

## Codegen units

A codegen unit is defined in a `.codegen` directory at the root of a package that the Unity project depends on. You would write your codegen logic inside this directory.

For example, the following GDK packages all have codegen units:

* `io.improbable.gdk.buildsystem`
* `io.improbable.gdk.core`
* `io.improbable.gdk.gameobjectcreation`
* `io.improbable.gdk.transformsynchronization`

> The `io.improbable.gdk.dependencytest` package inside the `test-project` also contains a codegen unit.

## `CodeGen`

The base `CodeGen` template is the entry point for the code generator, and therefore responsible for:

* Parsing command line arguments.
* Invoking the schema compiler to produce a schema bundle.
* Using `CodeGenerationLib` to parse and store important schema details.
* Finding code generation jobs in linked codegen units.
* Providing schema details to each code generation job it runs.

Once copied into the project build directory, `CodeGen.csproj` is updated with references to any modular code generation units found in the packages that your Unity project depends on. The code generator will run all code generation jobs defined in each referenced modular codegen unit.

> The linking step is done by the `GenerateCode.cs` script on each call to the code generator from the Unity Editor.

## `CodeGenerationLib`

The `CodeGenerationLib` project is a library used by the base `CodeGen` project and linked codegen units to:

* Parse schema bundles.
* Store schema details in a structured format.
* Let developers use an ergonomic CodeWriter API to write code generators.
* Define code generation jobs based on stored schema detais.
* Write generated code to disk.

## `GenerateCode.cs`

When your project is first opened in Unity, the `GenerateCode.cs` script is responsible for both setting up and subsequently calling the code generator with the correct arguments.

This script also exposes the `Generate code` and `Generate code (force)` options under the `SpatialOS` menu in the Unity Editor.

### Setup project

When opening up your project in Unity, the contents of `.CodeGenTemplate/` are first copied from the `io.improbable.gdk.tools` package into the `<projectroot>/build/codegen/` directory. This ensures that the base `CodeGen` template and the `CodeGenerationLib` it depends on are ready for consumption in the project's build directory.

Once this is done, the GDK generates run configurations for the code generator. This step searches all project packages for `.schema` directories, combines it with the schema source directories defined the GDK tools configuration, and sets this information in the run configurations for the code generator to use.

These configurations are used to ensure the code generator is always* called with the correct, validated arguments across Visual Studio, JetBrains Rider, and the dotnet CLI.

> Note: this enables you to iterate on code generation modules within an IDE without having to switch back and forth with the Unity Editor.

_\*unless a user tinkers with things they shouldn't be._

### Generate code

When you click the `Generate code` button, the GDK:

1. Finds all `.codegen` directories in the packages your Unity project depends on.
1. Updates the build directory's `CodeGen.csproj` with references to all the codegen units that were found.
1. Calls `dotnet run` on the updated code generator.

> All standard output from running the code generator is processed and logged at the appropriate level in the Unity Editor.

#### Generate code (force)

The `Generate code (force)` option effectively "resets" the code generator before running it. The directory containing generated code and the previously copied code generator template are both deleted. The code generator template in the `io.improbable.gdk.tools` package is then copied across again before a freshly set up code generator is run.
