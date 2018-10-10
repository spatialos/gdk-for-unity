[//]: # (Doc of docs reference 23)
[//]: # (TODO - tech writer pass)

# Troubleshooting

We also maintain a [known issues page]({{urlRoot}}/known-issues) for any open bugs in the SpatialOS GDK for Unity.

**The following exception is thrown: `<name of a type>` is an IComponentData, and thus must be blittable
(No managed object is allowed on the struct).**
<br/>
The struct you defined implements `IComponentData` and contains [non-blittable](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types) fields. Types that implement
`IComponentData` may only contain blittable fields.

> `bool` as well as `System.Boolean` are not blittable. Please use `Improbable.Gdk.Core.BlittableBool`, a blittable alternative
implementation we provide, in that case.

**The following exception is thrown when building a worker: `<path to burst package>\.Runtime\bcl.exe` did not run properly!**
<br/>
This is a benign exception that is thrown when building a worker while burst compilation
is turned on. Your worker was successfully built despite this error. To remove the
error message, disable Burst compilation by unchecking `Jobs > Enable Burst Compilation`
in the Unity Editor.

**The following error is thrown: Could not discover location for dotnet.exe.**
<br/>
Ensure that you have the [.NET Core SDK (x64)(Microsoft documentation)](https://www.microsoft.com/net/download/dotnet-core/2.1) installed and that the directory containing
the dotnet executable is added to your PATH environment variable. Restart your computer
and Unity after installing the .NET Core SDK.

**The Unity editor logs a bunch of warnings when I open the Unity project of the SpatialOS GDK for the first time. The warnings originate from code written by Unity, e.g. from `com.unity.mathematics` or `com.unity.entities`.**
<br/>
These warnings are caused by code maintained by Unity over which we have no control. They are benign and can be ignored.

**Several error messages are logged after I destroyed a GameObject or ECS entity representing a SpatialOS entity.**
<br/>
Do not proactively destroy GameObjects or ECS entities representing SpatialOS entities. The SpatialOS GDK cleans them up automatically when the respective SpatialOS entity is removed from the worker. Send a [DeleteEntity world command]({{urlRoot}}/content/gameobject/world-commands.md) to delete the entity on the SpatialOS side instead.

**A Reader/Writer or CommandSender/Handler in my code is null.**
<br/>

  * Ensure that your field is declared in a MonoBehaviour. They are not supported in ECS systems.
  * Ensure that the field is decorated with a [Require] attribute.
  * Ensure that the GameObject containing your MonoBehaviour is associated with a SpatialOS entity. You can verify this by examining whether a `SpatialOSComponent` MonoBehaviour was added to your GameObject at runtime.
  * Ensure that you only access Requirables in a MonoBehaviour while the MonoBehaviour is enabled. Requirables are supposed to be null while the MonoBehaviour is disabled. Note that certain Unity event methods like `OnTriggerEnter` or `OnCollisionEnter` may be invoked even if a MonoBehaviour is disabled.

**A MonoBehaviour containing fields using the [Require] attribute stays disabled when I expect it to be enabled.**
<br/>

  * Ensure that `GameObjectRepresentationHelper.AddSystems` is called when initializing your [`Worker`]({{urlRoot}}/content/glossary#worker).
  * Ensure that the GameObject containing your MonoBehaviour is associated with a SpatialOS entity. You can verify this be examining whether a `SpatialOSComponent` MonoBehaviour was added to your GameObject at runtime.
  * Ensure that the [requirements]({{urlRoot}}/content/gameobject/interact-spatialos-monobehaviours) for all your fields in your MonoBehaviour are satisfied. You can verify this by examining the SpatialOS entity associated with your GameObject in the Inspector. In the Inspector, ensure that all relevant components are attached to your SpatialOS entity and that read and write access is delegated correctly for your worker.