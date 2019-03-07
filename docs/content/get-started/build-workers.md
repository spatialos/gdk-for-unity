<%(TOC)%>
# Get started: 2 - Build your workers

As you will be launching a cloud deployment, you need to build out the code executables which will be run by SpatialOS servers - these are called [workers]({{urlRoot}}/content/glossary#worker).

1\. First, make sure burst compilation is disabled; from your Unity Editor menu, select **Jobs** and in the drop-down menu, make sure **Enable Burst Compilation** is unchecked. (If it is checked, select it to remove the check mark and disable it.)

2\.  Now build your workers from the Unity Editor's SpatialOS menu by selecting **SpatialOS** > **Build for cloud** > **All workers**. 
  (Shown below.) 
  <br/>
  <br/>![SpatialOS menu in Unity]({{assetRoot}}assets/unity-spatialos-menu.png)
  <br/>_The SpatialOS menu in the Unity Editor_
  <br/>

 Building workers for the first time may take a while (about 10 minutes). Why not make yourself a cup of tea or check out [our Youtube channel](https://www.youtube.com/channel/UC7BE8B2yUeQxPvZytk47NYw/videos) while you wait?

3\.  **Your workers have finished building when:** You see the following message in your Unity Editor's Console window: `Completed build for Cloud target`.

**After the build has successfully finished:** 
<br/>Your `gdk-for-unity-fps-starter-project/build/assembly/worker` folder should contain:

```text
    ├── SimulatedPlayerCoordinator@Linux.zip
    ├── UnityClient@Mac.zip
    ├── UnityClient@Windows.zip
    ├── UnityGameLogic@Linux.zip
```

You may get a number of warnings displayed in your Unity Editor Console window. You can ignore the warnings at this stage.

If your build succeeded, you can now [upload and launch your game]({{urlRoot}}/content/get-started/upload-launch). If you got build errors in Unity Editor Console window, check the **Common build errors** section below.

<%(#Expandable title="Common build errors")%>

#### You can ignore iOS and Android errors if you're not developing for mobile platforms

When you build your workers you may see the following errors in the Unity console: 
<br/>`The worker "iOSClient" cannot be built for a Cloud deployment: your Unity Editor is missing build support for iOS. Please add the missing build support options to your Unity Editor.`
<br/><br/>`The worker "AndroidClient" cannot be built for a Cloud deployment: your Unity Editor is missing build support for Android.Please add the missing build support options to your Unity Editor.`<br/>
<br/>You can ignore these errors if you are not developing a game for Android or iOS
<br/><br/>Mobile support is in pre-alpha. If you are developing a game for Android or iOS, refer to our GDK for Unity [mobile support documentation]({{urlRoot}}/content/mobile/overview)
<br/>

You might not have selected the build support modules that your game needs during your Unity setup.

#### You need the correct build support components

* You need **Linux** build support. This is because server-workers in a cloud deployment always run in a Linux environment. In the `Assets/Fps/Config/BuildConfiguration`, do not change the `UnityGameLogic Cloud Environment` from Linux.
* You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.<br/>
* You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users. <br/>
* Unity gives you build support for your development machine (Windows or Mac) by default.

You can also check our [Known Issues]({{urlRoot}}/known-issues) for other error messages.

<%(/Expandable)%>

>**TIP: Speed up development iteration with worker quick-run** 
<br/> When you are developing with the GDK, you don't need to build out workers all the time, you can use quick-run to run multiple workers in your Unity Editor using Ctrl+L (Windows) or Cmd+L (Mac).
<br/>
<br/>This tutorial takes you through the steps to set up a cloud deployment. When you are developing with the GDK, you use a local deployment rather than a cloud deployment. In a local deployment, you can either build your workers to run locally or use quick-run. With quick-run you can run multiple workers in your Unity Editor, so you don't have to keep building out workers during development iteration. 
<br/>
<br/>
 To use quick-run with the FPS Starter Project:<br/>
 <br/>
1. With your project open in your Unity Editor, on your computer’s keyboard, input Ctrl+L (Windows) or Cmd+L (Mac).<br/>
1. Wait until you see a message in the Editor’s Console window that SpatialOS is ready. The message is: SpatialOS ready. Access the inspector at http://localhost:21000/inspector.<br/>
1. In your Unity Editor, play the `FPS-Development` Scene.<br/>

<br/>
#### Next: [Upload and launch your game]({{urlRoot}}/content/get-started/upload-launch.md)

