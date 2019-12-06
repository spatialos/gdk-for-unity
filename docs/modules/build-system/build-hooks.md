# Build hooks

During a build, you can use the Unity provided callbacks to add pre-processing and/or post-processing to your worker build.
The Build System Feature Module provides a static readonly variable, `WorkerBuilder.CurrentContext`, which contains information such as the worker type and build options.

This context is only valid during a build.

```csharp
public class BuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
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

> In order to access `WorkerBuilder.CurrentContext`, you must ensure that your Editor assembly definition references `Improbable.Gdk.BuildSystem`.
