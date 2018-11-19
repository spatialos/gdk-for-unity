[//]: # (TODO - tech writer pass)

# Troubleshooting

<%(TOC)%>

<%(Callout type="tip" message="We also maintain a [known issues]({{urlRoot}}/known-issues) page for any open bugs in the SpatialOS GDK for Unity.")%>

## Errors

#### Error building player because build target was unsupported

When building workers by selecting **Build For Cloud** > **my_worker_name** the following error may occur:
**_Error building player because build target was unsupported_**</br>

**Cause**<br/>
 You don't have the correct Unity build support packages installed.<br/>

**Fix**<br/>
 In the Unity Editor:

* Select **SpatialOS** > **Check build support** and check the Console for errors relating to specific build platforms. <br/>
  * You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.
  * You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users.
  * You need **Android** and/or **iOS** build support if you are developing for those platforms.
  * Unity gives you build support for your development machine (Windows or Mac) by default.
  * _**In addition**_, make sure you have **Linux** build support enabled.<br/>
 You need Linux build support because all server-workers in a cloud deployment run in a Linux environment.
 <br/>Fix this by runing the Unity installer and selecting the appropriate build support options during the instalation. See [Setup and installing]({{urlRoot}}/setup-and-installing#set-up-your-machine) for more information.
 
 **Note:** When building your project do not change the `UnityGameLogic Cloud Environment` field in your `BuildConfiguration.asset` from Linux. This can cause further build errors.

#### Could not discover location for dotnet.exe

**Cause**<br/>
Either you don't have the .NET Core SDK (x64) installed or the directory containing the dotnet executable is not in your PATH.

**Fix**<br/>

1. Ensure that you have the correct version of [.NET Core SDK (x64)(Microsoft documentation)](https://www.microsoft.com/net/download/dotnet-core/) installed. Our supported versions are listed on the [setup page]({{urlRoot}}/setup-and-installing#set-up-your-machine)).
2. Ensure that the dotnet executable is added to your PATH environment variable.
3. Restart your computer after making the above changes.

<br/>

## Exceptions

#### `<name of a type>` is an IComponentData, and thus must be blittable

**Cause**<br/>

The struct you defined implements [`IComponentData`](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/132f511a0f36d2bb422fc807cb3a808ea18d7df5/Documentation/content/ecs_in_detail.md#icomponentdata) and contains [non-blittable](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types) fields. Types that implement
`IComponentData` may only contain blittable fields.

The most common causes of this exception are `bool` and `System.Boolean`, both of which are not blittable.

**Fix**<br/>

 Instead of `bool` and `System.Boolean`, please use `Improbable.Gdk.Core.BlittableBool`, a blittable alternative implementation we provide as a workaround for this limitation.

<br/>

#### `Runtime\bcl.exe` did not run properly!

**Cause**<br/>

This is a benign exception that is thrown when building a worker while burst compilation is turned on. Your worker was successfully built despite this error. This occurs becayse Unity's burst compiler doesn't yet fully support cross compilation (Windows to Linux ,for example).

**Workaround**<br/>

To remove the error message, disable Burst compilation by unchecking `Jobs > Enable Burst Compilation` in the Unity Editor.

## Warnings

#### Several warnings when opening the project for the first time

**Cause**<br/>
These warnings originate from code written by Unity, `com.unity.mathematics` or `com.unity.entities` for example. These warnings are benign so you can ignore them.

#### Several warnings when deleting a GameObject or ECS entity that represents a SpatialOS entity

**Cause**<br/>
These warnings occur when a [GameObject]({{urlRoot}}/content/glossary#gameobject) or [ECS entity]({{urlRoot}}/content/glossary#unity-ecs-entity) that represents a SpatialOS entity is deleted before its corresponding SpatialOS entity leaves the worker's view. The GDK is designed so that the GameObjects and ECS entities that represent SpatialOS entities should only be deleted after the SpatialOS entity they represent has left that worker's view. ECS entities are deleted automatically by the GDK when their corresponding SpatialOS entities leaves that worker's view. In the case of GameObjects, the SpatialOS Runtime sends a message to the Unity worker when a SpatialOS entity leaves that worker's view, the GDK passes this message onto you. It's your responsibility to act upon these commands in your code.

**Fix**<br/>
When you want to delete a GameObject or ECS entity that represent SpatialOS entity, send a [DeleteEntity world command]({{urlRoot}}/content/gameobject/world-commands.md) to delete the entity on the SpatialOS side instead.

## Problems

#### A Reader/Writer or CommandSender/Handler in my code is null

**Causes and fixes**<br/>

This can be caused by multiple problems. Follow the checklist below to discover the cause and fix:

  * Ensure that your field is declared in a [MonoBehaviour]({{urlRoot}}/content/glossary#monobehaviour). They are not supported in ECS systems.
  * Ensure that the field is decorated with a [Require] attribute.
  * Ensure that the GameObject containing your MonoBehaviour is associated with a SpatialOS entity. You can verify this by examining whether a [`SpatialOSComponent`]({{urlRoot}}/content/glossary#spatialos-component) MonoBehaviour is added to your GameObject at runtime.
  * Ensure that you only access Requirables in a MonoBehaviour while the MonoBehaviour is enabled. Requirables are supposed to be null while the MonoBehaviour is disabled. Note that certain Unity event methods like `OnTriggerEnter` or `OnCollisionEnter` may be invoked even if a MonoBehaviour is disabled.

#### A MonoBehaviour containing fields using the Require attribute remains disabled when I expect it to be enabled

**Causes and fixes**<br/>

This can be caused by multiple problems. Follow the checklist below to discover the cause and fix:

  * Ensure that `GameObjectRepresentationHelper.AddSystems` is called when initializing your [`Worker`]({{urlRoot}}/content/glossary#worker).
  * Ensure that the GameObject containing your MonoBehaviour is associated with a SpatialOS entity. You can verify this be examining whether a `SpatialOSComponent` MonoBehaviour was added to your GameObject at runtime.
  * Ensure that the [requirements]({{urlRoot}}/content/gameobject/interact-spatialos-monobehaviours) for all your fields in your MonoBehaviour are satisfied. You can verify this by examining the SpatialOS entity associated with your GameObject in the [Inspector]({{urlRoot}}/content/glossary#inspector). In the Inspector, ensure that all relevant components are attached to your SpatialOS entity and that read and write access is delegated correctly for your worker.

#### When opening a Unity GDK solution in Visial Studio 2017, the file has no matching compilation unit

**Cause**<br/>
You don't have the correct compilation units installed.

**Fix**<br/>

Restart your computer. We've noticed that this sometimes resolves the issue. If the issue persists:

1. Follow the Visual Studio instalation steps on our [setup page]({{urlRoot}}/setup-and-installing#set-up-your-machine)).
2. Restart your computer.
