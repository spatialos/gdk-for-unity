# Build your workers

As you will be launching a cloud deployment, you need to build out the code executables which will be run by SpatialOS servers - these are called [workers]({{urlRoot}}/content/glossary#worker).

<%(#Expandable title="<b>TIP: Speed up local development iteration</b>")%>
This tutorial takes you through the steps to set up a cloud deployment. However, in a local deployment, you can either build your workers to run locally or run the workers in the Unity Editor. You can run multiple workers in your Unity Editor, so you don't have to keep building out workers during development iteration.
<br/>
<br/>
To run workers in the Unity Editor with the FPS Starter Project:
<br/>
<br/>
1. With your project open in your Unity Editor, on your computer’s keyboard, input Ctrl+L (Windows) or Cmd+L (Mac).<br/>
2. Wait until you see a message in the Editor’s Console window that SpatialOS is ready. The message is: SpatialOS ready. Access the inspector at http://localhost:21000/inspector.<br/>
3. In your Unity Editor, play the `FPS-Development` Scene.<br/>

<%(/Expandable)%>

### 1. Disable Burst compilation

First, make sure Burst compilation is disabled. From your Unity Editor **Jobs** menu, make sure **Enable Burst Compilation** is **unchecked**.

> Leaving Burst compilation enabled will throw errors when cross-compiling (building for Linux on Windows, for example).

<br/>

### 2. Build workers

Now build your workers from the Unity Editor's SpatialOS menu by selecting **SpatialOS** > **Build for cloud** > **All workers**.
  (Shown below.)
  <br/>
  <br/>![SpatialOS menu in Unity]({{assetRoot}}assets/unity-spatialos-menu.png)
  <br/>_The SpatialOS menu in the Unity Editor_
  <br/>
  
Building workers for the first time may take a while (about 10 minutes). Why not make yourself a cup of tea or check out [our Youtube channel](https://www.youtube.com/channel/UC7BE8B2yUeQxPvZytk47NYw/videos) while you wait?

<%(Callout message="Your workers have finished building when you see the following message in your Unity Editor's Console window:<br/><br/>**Completed build for Cloud target**")%>

<p/>

> **NOTE**: You may get a number of warnings displayed in your Unity Editor Console window. You can ignore the warnings at this stage.

If your build succeeded, you can now [upload and launch your game]({{urlRoot}}/content/get-started/upload-launch). 

If you got build errors in Unity Editor Console window, check the Common build errors section below.

<%(#Expandable title="Common build errors")%>

###### Missing build support components

* You need **Linux** build support. This is because server-workers in a cloud deployment always run in a Linux environment. In the `Assets/Fps/Config/BuildConfiguration`, do not change the `UnityGameLogic Cloud Environment` from Linux.
* You may need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.<br/>
* You may need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users. <br/>
* Unity gives you build support for your development machine (Windows or Mac) by default.

You can also check our [Known Issues]({{urlRoot}}/known-issues) for other error messages.

<%(/Expandable)%>

<br/>

#### Next: [Upload and launch your game]({{urlRoot}}/content/get-started/upload-launch.md)
