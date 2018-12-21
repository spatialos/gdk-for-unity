# Get started: 2 - Open the FPS Starter Project

Launch the Unity Editor. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

<%(Callout type="warning" message="
It will take about 10 minutes for Unity to load the Starter Project for the first time. In the meantime, 
why not take a stroll on our [Games Blog](https://improbable.io/games/blog)?" )%>

**Before you start, apply these two quick Unity bug fixes:**

#### Shaders
There is a [known issue]({{urlRoot}}/known-issues) in the preview version of the [High Definition Render Pipeline](https://blogs.unity3d.com/2018/03/16/the-high-definition-render-pipeline-focused-on-visual-quality/), where shaders do not fully compile and appear visually darker than intended.

There is a quick fix however:

1. Open the FPS Starter Project in the Unity Editor located in `workers/unity`.
1. In the Project window, navigate to **Assets** > **Fps** > **Art** > **Materials**.
1. Right click on the `Source_Shaders` directory and select Reimport.

<img src="{{assetRoot}}assets/shader-fix.png" style="margin: 0 auto; display: block;" />

#### Bake Navmesh
There is a [known issue]({{urlRoot}}/known-issues) in the Unity Editor regarding importing a navmesh. The navmseh for the `FPS-SimulatedPlayerCoordinator` is not imported correctly when you open the project for the first time. To fix this, you need to rebake the navmesh for this Scene. To do this:

1. In the [Project window (Unity documentation)](https://docs.unity3d.com/Manual/ProjectView.html), open the `FPS-SimulatedPlayerCoordinator` Scene located at `Assets/Fps/Scenes`.
1. In the [Hierarchy window (Unity documentation)](https://docs.unity3d.com/Manual/Hierarchy.html), click on the `FPS-Start_Large` object to see it in the [Inspector window (Unity documentation)](https://docs.unity3d.com/Manual/UsingTheInspector.html), and enable the object by clicking the checkbox next to its name.
1. Open the **Navigation** window (Unity Editor menu: **Windows** > **AI** > **Navigation**).
1. Select the **Bake** tab, and then the **Bake** button.

You can verify that the NavMesh has been baked correctly by navigating to **Assets** > **Fps** > **Scenes** > **FPS-SimulatedPlayerCoordinator**, and checking that Unity displays the correct icon.
<img src="{{assetRoot}}assets/navmesh-fixed.png" style="margin: 0 auto; display: block;" />


<br/>
#### Next: [Build your workers]({{urlRoot}}/content/get-started/build-workers.md)

