<%(Toc max="3")%>

# Troubleshooting

> **Note**: We also maintain a [known issues](https://github.com/spatialos/gdk-for-unity/projects/2) page for any open bugs in the SpatialOS GDK for Unity.

<br/>

## Errors

#### Could not discover location for `dotnet.exe`

**Cause**

Either you don't have the .NET Core SDK (x64) installed or the directory containing the dotnet executable is not in your PATH.

**Fix**

1. Ensure that you have the correct version of [.NET Core SDK (x64)(Microsoft documentation)](https://www.microsoft.com/net/download/dotnet-core/) installed. Our supported versions are listed on the [setup page]({{urlRoot}}/machine-setup)).
1. Ensure that the dotnet executable is added to your PATH environment variable.
1. Restart your computer after making the above changes.

<br/>

## Exceptions

#### `Runtime\bcl.exe` did not run properly

**Cause**

This is a benign exception that is thrown when cross compiling a worker while burst compilation is turned on. Your worker was successfully built despite this error. 

This occurs because Unity's burst compiler doesn't yet fully support cross compilation (Windows to Linux, for example).

**Fix**

To remove the error message, disable burst compilation by unchecking **Jobs** > **Enable Burst Compilation** in your Unity Editor.

<br/>

## Problems

#### A Reader/Writer or CommandSender/Receiver in my code is null

This can be caused by one of multiple problems. Please check the list below for possible fixes:

* Ensure that your field is declared in a [MonoBehaviour]({{urlRoot}}/reference/glossary#monobehaviour). They are **not** supported in ECS systems.
* Ensure that the field is decorated with a `[Require]` attribute.
* Ensure that the GameObject containing your MonoBehaviour is associated with a [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity).
  * You can verify this by examining whether a `LinkedEntityComponent` is attached to the GameObject _after_ it is created at runtime.
* Ensure that you only access Requirables in a MonoBehaviour while the MonoBehaviour is enabled. Requirables are supposed to be null while the MonoBehaviour is disabled. 

> **Note:** certain Unity event methods like `OnTriggerEnter` or `OnCollisionEnter` may be invoked, even if a MonoBehaviour is disabled.


#### A MonoBehaviour containing fields with the `Require` attribute is disabled when I expect it to be enabled

This can be caused by one of multiple problems. Please check the list below for possible fixes:

* Ensure that `GameObjectRepresentationHelper.AddSystems` is called when initializing your [worker]({{urlRoot}}/reference/glossary#worker).
* Ensure that the GameObject containing your MonoBehaviour is associated with a [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity). 
  * You can verify this by examining whether a `LinkedEntityComponent` is attached to the GameObject after it is created.
* Ensure that the [requirements]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/lifecycle) for all your fields in your MonoBehaviour are satisfied. 
  * You can verify this by examining the [SpatialOS entity]({{urlRoot}}/reference/glossary#spatialos-entity) associated with your GameObject in the [SpatialOS Inspector]({{urlRoot}}/reference/glossary#inspector). 
  * In the SpatialOS Inspector, ensure that all relevant SpatialOS components are attached to your SpatialOS entity and that read and write access is delegated correctly for your worker.
