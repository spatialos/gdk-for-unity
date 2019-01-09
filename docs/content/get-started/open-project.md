# Get started: 2 - Open the FPS Starter Project

Launch the Unity Editor. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

>**TIP:** The first time you open the Starter Project in your Unity Editor, It takes about 10 minutes; it's much quicker to open after this. (While you are waiting, you could look at our [Games Blog](https://improbable.io/games/blog).)

**Before you start, apply these two quick Unity bug fixes:**

#### Shaders
There is a [known issue]({{urlRoot}}/known-issues) in the preview version of the [High Definition Render Pipeline](https://blogs.unity3d.com/2018/03/16/the-high-definition-render-pipeline-focused-on-visual-quality/), where shaders do not fully compile and appear visually darker than intended.

There is a quick fix however:

1. In your Unity Editor, open the FPS Starter Project; it's located in `workers/unity`.
1. In your Editor's Project window, navigate to **Assets** > **Fps** > **Art** > **Materials**.
1. Right-click on the `Source_Shaders` directory and select **Reimport**.
<br/>
<br/>
![]({{assetRoot}}assets/shader-fix.png)<br/>
_Unity Editor menu to reimport shaders_

#### Bake Navmesh
There is a [known issue]({{urlRoot}}/known-issues) in the Unity Editor regarding importing NavMesh. The Unity Editor does not import the NavMesh for the `FPS-SimulatedPlayerCoordinator` correctly when you open the project for the first time. To fix this, you need to rebake the NavMesh for this Scene. To do this:

1. In the [Project window (Unity documentation)](https://docs.unity3d.com/Manual/ProjectView.html), open the `FPS-SimulatedPlayerCoordinator` Scene located at `Assets/Fps/Scenes`.
1. In the [Hierarchy window (Unity documentation)](https://docs.unity3d.com/Manual/Hierarchy.html), click on the `FPS-Start_Large` object to see it in the [Inspector window (Unity documentation)](https://docs.unity3d.com/Manual/UsingTheInspector.html), and enable the object by clicking the checkbox next to its name.
1. Open the **Navigation** window (Unity Editor menu: **Windows** > **AI** > **Navigation**).
1. Select the **Bake** tab, and then the **Bake** button.
1. You can verify that the NavMesh is baked correctly by navigating to **Assets** > **Fps** > **Scenes** > **FPS-SimulatedPlayerCoordinator** and checking that there's a NavMesh object listed. The NavMesh object is usually represented by the icon below. 
<br/>
<br/>
![]({{assetRoot}}assets/navmesh-fixed.png)
<br/>
_NavMesh image showing it's baked correctly_

> **TIP:** You may get a number of warnings displayed in the Unity Editor Console window. You can ignore the warnings at this stage; use the message icons on the right-hand side of the Console window to set it to display only info and error messages.


<br/>
#### Next: [Build your workers]({{urlRoot}}/content/get-started/build-workers.md)

