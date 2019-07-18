# Query-based Interest Helper Module

The Query-based Interest (QBI) Helper Module provides methods to easily define the `Interest` component used by QBI.

The module includes functionality to:

* Add, replace and clear queries from an `Interest` component.
* Define a query's constraints and which components it should return.
* Construct query constraints with less boilerplate code.

You can find more information about the underlying APIs provided in our [API reference docs]({{urlRoot}}/api/query-based-interest-index).

## Installation

### 1. Add the package

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.1/manual/index.html#installing-a-new-package).

Or add it to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "io.improbable.gdk.querybasedinteresthelper": "<%(Var key="current_version")%>"
  },
}
```

### 2. Reference the assemblies

The Query-based Interest Helper Module contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs depending on whether you have an assembly definition file in your own project or not.

**I have an assembly definition file**

Open your assembly definition file and add `Improbable.Gdk.QueryBasedInterestHelper` to the reference list.

**I don't have an assembly definition file**

If you don't have an assembly definition file in your project, Unity will automatically reference the `Improbable.Gdk.QueryBasedInterestHelper` assembly for you.
