# Get started: 3 - Build your workers

As you will be launching a cloud deployment, you need to build out the code executables which will be run by SpatialOS servers - these are called [workers]({{urlRoot}}/content/glossary#worker).

1. You first need to make sure Burst compilation is set to disabled; in your Unity Editor, navigate to **Jobs** > **Enable Burst Compilation** and select **disabled**.
2. Now you can build your workers from the SpatialOS menu by selecting **Build for cloud** > **All workers**. 
(Shown below.) 
<br/>
<br/>![SpatialOS menu in Unity]({{assetRoot}}assets/unity-spatialos-menu.png)
<br/>_The SpatialOS menu in the Unity Editor_
<br/>
<br/>**NOTE:** Building workers for the first time may take a while (about 10 minutes). Why not make yourself a cup of tea or check out [our Youtube channel](https://www.youtube.com/channel/UC7BE8B2yUeQxPvZytk47NYw/videos) while you wait?
<br/>
<br/>
3. **It has finished building when:** You see the following message in the Unity Editor's Console window: `Completed build for Cloud target`. 

>**TIP:** You may get a number of warnings displayed in the Unity Editor Console window. You can ignore the warnings at this stage; use the message icons on the right-hand side of the Console window to set it to display only info and error messages so you can see only the relevant messages.

**If you encounter build errors:** 
<br/>You might not have selected the build supports your game needs during your Unity setup.

* You need **Linux** build support. This is because server-workers in a cloud deployment always run in a Linux environment. In the `Assets/Fps/Config/BuildConfiguration`, do not change the `UnityGameLogic Cloud Environment` from Linux.
* You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.<br/>
* You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users. <br/>
* Unity gives you build support for your development machine (Windows or Mac) by default.


**After the build has successfully finished:** 
<br/>Your `gdk-for-unity-fps-starter-project/build/assembly` folder should contain:
```text
  worker
      ├── SimulatedPlayerCoordinator@Linux.zip
      ├── UnityClient@Mac.zip
      ├── UnityClient@Windows.zip
      ├── UnityGameLogic@Linux.zip
```
<br>

>**TIP: Fast worker build with quick-run** 
<br/>When you are developing with the GDK, you can use a quick-run to build workers using Ctrl+L (Windows) or Cmd+L (Mac).
<br/>
<br/> This tutorial takes you through a cloud deployment. When you are developing with the GDK, you don’t need to always use a cloud deployment; you can use a local deployment instead. In a local deployment, you can either build your workers to run locally or use a quick-run option. With quick-run you can run multiple workers in your Unity Editor, so you don't have to keep building out workers during development iteration. 
<br/>
<br/>
 To use quick-run with the FPS Starter Project:<br/>
 <br/>
1. With your project open in the Unity Editor, on your computer’s keyboard, select Ctrl+L (Windows) or Cmd+L (Mac).<br/>
2. Wait until you see a message in the Editor’s console window that SpatialOS is ready. The message is: SpatialOS ready. Access the inspector at http://localhost:21000/inspector (xx.xs).<br/>
3. In your Unity Editor, play the FPS-Development Scene.<br/>

<br/>
#### Next: [Upload and launch your game]({{urlRoot}}/content/get-started/upload-launch.md)

