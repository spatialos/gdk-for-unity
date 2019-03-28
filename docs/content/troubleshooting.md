# Troubleshooting

> **Note**: We also maintain a [known issues](https://github.com/spatialos/gdk-for-unity/projects/2) page for any open bugs in the SpatialOS GDK for Unity.

## Errors
<br/>
#### <li> <b>Error building player because build target was unsupported</b>
When building workers by selecting **Build For Cloud** > **my_worker_name** the following error may occur:
**_Error building player because build target was unsupported_**</br>

**Cause**<br/>
You don't have the correct Unity build support packages installed.<br/>

**Fix**<br/>
In your Unity Editor:

* Select **SpatialOS** > **Check build support** and check the Console for errors relating to specific build platforms. <br/>
  * You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.
  * You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users.
  * You need **Android** and/or **iOS** build support if you are developing for those platforms.
  * Unity gives you build support for your development machine (Windows or Mac) by default.
  * _**In addition**_, make sure you have **Linux** build support enabled. You need Linux build support because all server-workers in a cloud deployment run in a Linux environment.

Fix this by runing the Unity installer and selecting the appropriate build support options during the installation. See [Setup and installing]({{urlRoot}}/setup-and-installing) for more information.

 **Note:** When building your project do not change the `UnityGameLogic Cloud Environment` field in your `BuildConfiguration.asset` from Linux. This can cause further build errors.

<br/>
#### <li> <b>Could not discover location for `dotnet.exe`</b>

**Cause**<br/>
Either you don't have the .NET Core SDK (x64) installed or the directory containing the dotnet executable is not in your PATH.

**Fix**<br/>

1. Ensure that you have the correct version of [.NET Core SDK (x64)(Microsoft documentation)](https://www.microsoft.com/net/download/dotnet-core/) installed. Our supported versions are listed on the [setup page]({{urlRoot}}/setup-and-installing)).
1. Ensure that the dotnet executable is added to your PATH environment variable.
1. Restart your computer after making the above changes.

<br/>

## Exceptions
<br/>
#### <li> <b>'name-of-a-type' is an IComponentData, and thus must be blittable</b>

**Cause**<br/>

The struct you defined implements [`IComponentData`](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/132f511a0f36d2bb422fc807cb3a808ea18d7df5/Documentation/content/ecs_in_detail.md#icomponentdata) and contains [non-blittable](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types) fields. Types that implement
`IComponentData` may only contain blittable fields.

The most common causes of this exception are `bool` and `System.Boolean`, both of which are not blittable.

**Fix**<br/>

 Instead of `bool` and `System.Boolean`, please use `Improbable.Gdk.Core.BlittableBool`, a blittable alternative implementation we provide as a workaround for this limitation.

<br/>
#### <li> <b>`Runtime\bcl.exe` did not run properly!</b>

**Cause**<br/>

This is a benign exception that is thrown when building a worker while burst compilation is turned on. Your worker was successfully built despite this error. This occurs because Unity's burst compiler doesn't yet fully support cross compilation (Windows to Linux, for example).

**Workaround**<br/>

To remove the error message, disable burst compilation by unchecking **Jobs** > **Enable Burst Compilation** in your Unity Editor.

<br/>
## Warnings
<br/>
#### <li> <b>I get several warnings when opening the project for the first time</b>

<br/>
**Cause**<br/>
These warnings originate from code written by Unity, for example; `com.unity.mathematics` or `com.unity.entities`. 

**Fix**<br/>
These warnings are benign so you can ignore them.

<br/>
#### <li> <b> I get several warnings when deleting a GameObject or ECS entity that represents a SpatialOS entity</b>

**Cause**<br/>
These warnings occur when a [GameObject]({{urlRoot}}/content/glossary#gameobject) or [ECS entity]({{urlRoot}}/content/glossary#unity-ecs-entity) that represents a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) is deleted before its corresponding SpatialOS entity leaves the [worker's view]({{urlRoot}}/content/glossary#worker-s-view). 

 The GDK expects that a GameObject or ECS entity that represents a SpatialOS entity is only deleted after the SpatialOS entity it represents has left that worker's view: 

* The GDK automatically deletes an ECS entity when its corresponding SpatialOS entity leaves that worker's view. 
* For GameObjects, you need to delete the GameObject via your code. The SpatialOS [Runtime]({{urlRoot}}/content/glossary#spatialos-runtime) sends a message to the server-worker when a SpatialOS entity leaves that worker's view, and the GDK passes this message on to you; it's your responsibility to act upon these commands at the right time in your code.

**Fix**<br/>
When you want to delete a GameObject that represents a SpatialOS entity, send a [DeleteEntity world command]({{urlRoot}}/content/gameobject/world-commands.md) to delete the entity on the SpatialOS side instead.
[comment]: <> "TODO: make this fix clearer and what it means by "passing the message on to you" JIRA: https://improbableio.atlassian.net/browse/UTY-1573 and https://improbableio.atlassian.net/browse/TC-168."

<br/>

## Problems

<br/>
#### <li> <b> A Reader/Writer or CommandSender/Receiver in my code is null </b>

**Causes and fixes**<br/>

This can be caused by multiple problems. Follow the checklist below to discover the cause and fix:

  * Ensure that your field is declared in a [MonoBehaviour]({{urlRoot}}/content/glossary#monobehaviour). They are not supported in ECS systems.
  * Ensure that the field is decorated with a `[Require]` attribute (see [How to interact with SpatialOS using MonoBehaviours]({{urlRoot}}/content/gameobject/interact-spatialos-monobehaviours)).
   * Ensure that the GameObject containing your MonoBehaviour is associated with a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity). You can verify this by examining whether a `LinkedEntityComponent` is attached to the GameObject after it is created.
[comment]: <> "TODO: make this clearer - what is meant by "SpatialOS component" here - do we mean a GameObject component called SpatialOS component? And how do we check whether this is added to the MonoBehaviour? . JIRA:https://improbableio.atlassian.net/browse/UTY-1575 and https://improbableio.atlassian.net/browse/TC-169."
  * Ensure that you only access Requirables in a MonoBehaviour while the MonoBehaviour is enabled. Requirables are supposed to be null while the MonoBehaviour is disabled. Note that certain Unity event methods like `OnTriggerEnter` or `OnCollisionEnter` may be invoked, even if a MonoBehaviour is disabled.
[comment]: <> "TODO: make this fix clearer - what is meant by "Requireables" - we haven't defined this. JIRA: https://improbableio.atlassian.net/browse/UTY-1575 and https://improbableio.atlassian.net/browse/TC-169."

<br/>
#### <li> <b> A MonoBehaviour containing fields using the `Require` attribute remains disabled when I expect it to be enabled </b>

**Causes and fixes**<br/>

This can be caused by multiple problems. Follow the checklist below to discover the cause and fix:

  * Ensure that `GameObjectRepresentationHelper.AddSystems` is called when initializing your [worker]({{urlRoot}}/content/glossary#worker).
   * Ensure that the GameObject containing your MonoBehaviour is associated with a [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity). You can verify this by examining whether a `LinkedEntityComponent` is attached to the GameObject after it is created.
  * Ensure that the [requirements]({{urlRoot}}/content/gameobject/interact-spatialos-monobehaviours) for all your fields in your MonoBehaviour are satisfied. You can verify this by examining the [SpatialOS entity]({{urlRoot}}/content/glossary#spatialos-entity) associated with your GameObject in the [SpatialOS Inspector]({{urlRoot}}/content/glossary#inspector). In the SpatialOS Inspector, ensure that all relevant SpatialOS components are attached to your SpatialOS entity and that read and write access is delegated correctly for your worker.

<br/>
#### <li> <b> When opening a GDK for Unity solution in Visual Studio 2017, the file has no matching compilation unit </b>

**Cause**<br/>
You don't have the correct compilation units installed.

**Fix**<br/>

Restart your computer. We've noticed that this sometimes resolves the issue. If the issue persists:

1. Follow the Visual Studio installation steps on our [setup page]({{urlRoot}}/setup-and-installing).
1. Restart your computer.
