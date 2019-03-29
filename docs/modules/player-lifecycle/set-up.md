<%(TOC)%>
# Setting up the Feature Module

Like most of our other Feature Modules, this module is included in the `gdk-for-unity` repository. To use it, you must ensure that your project depends on this package and is also added to the appropriate assembly definition's references.

If you are building on top of either of these Starter Projects, you can skip reading the instructions on this page - as both the FPS and Blank Starter Projects include the player lifecycle Feature Module as a dependency.

</br>

## Add dependency to manifest.json

First, you must ensure that your `manifest.json` file has a dependency on the player lifecycle package.

```
{
  "dependencies": {
    "com.improbable.gdk.playerlifecycle": "file:../../../../gdk-for-unity/workers/unity/Packages/com.improbable.gdk.playerlifecycle",
    //other package dependencies are typically listed here
  },
  "registry": "https://staging-packages.unity.com"
}
```

</br>

## Add reference in assembly definition

Next, ensure that your project's [assembly definition](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) includes a reference to the player lifecycle package. For example, the Blank Starter Project is set up like so:

<img src="{{assetRoot}}assets/blank-project-asmdef.png"/>
