# 3. Build your workers

As you will be launching a cloud deployment, you need to build out the code executables which will be run by SpatialOS servers - these are called [workers](https://docs.improbable.io/reference/latest/shared/concepts/workers-load-balancing).

In the Unity Editor, you first need to make sure Burst compilation is **disabled** from **Jobs** > **Enable Burst Compilation**. Then you can build your workers from the SpatialOS menu by clicking **Build for cloud** > **All workers**.

<%(Callout type="info" message="Building workers for the first time may take while. Why not make yourself a cup of tea while you wait?")%>

![SpatialOS menu in Unity]({{assetRoot}}assets/unity-spatialos-menu.jpg)

> **It has finished building when:** You see the following message in the Unity Editor's Console window: `Completed build for Cloud target`

<%(Callout type="alert" message="If you encounter build errors, you might not have selected the build supports your game needs during your Unity setup. <br/><br/>
* You need **Linux** build support. This is because server-workers in a cloud deployment run in a Linux environment. In the `Assets/Fps/Config/BuildConfiguration`, do not change the `UnityGameLogic Cloud Environment` from Linux.<br/> <br/>
* You need **Mac** build support if you are developing on a Windows PC and want to share your game with Mac users.<br/>
* You need **Windows** build support if you are developing on a Mac and want to share your game with Windows PC users. <br/>
* Unity gives you build support for your development machine (Windows or Mac) by default.")%>

After the build has successfully finished, the `gdk-for-unity-fps-starter-project/build/assembly` folder should contain:
```text
  worker
      ├── SimulatedPlayerCoordinator@Linux.zip
      ├── UnityClient@Mac.zip
      ├── UnityClient@Windows.zip
      ├── UnityGameLogic@Linux.zip
```

<%(Callout type="info" message="Note that when you are developing locally with the GDK you can skip building these workers, since both of your workers can run in the editor. To do this press `Ctrl+L` to start a local deployment, wait until you see a message that SpatialOS is ready, and then play the `FPS-Development` scene.")%>

**[[1]]({{urlRoot}}/content/get-started/get-started1.md)[[2]]({{urlRoot}}/content/get-started/get-started2.md)[[3]]({{urlRoot}}/content/get-started/get-started3.md) < Back - Next > [4: Build your workers]({{urlRoot}}/content/get-started/get-started4.md) [5] [6]**

