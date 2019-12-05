# Build hooks

During a build, you can use the Unity provided callbacks to add pre- and post-processing to your worker build.
The build system feature module provides `WorkerBuilder.CurrentContext` containing information such as worker type and build options.

This context is only valid during a build.

```csharp
public class BuildPreProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        var workerType = WorkerBuilder.CurrentContext.WorkerType;
        Debug.Log($"Pre-processing build with worker type {workerType}");
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        var workerType = WorkerBuilder.CurrentContext.WorkerType;
        Debug.Log($"Post-processing build with worker type {workerType}");
    }
}
```

> Ensure that your Editor assembly definition references `Improbable.Gdk.BuildSystem`.
