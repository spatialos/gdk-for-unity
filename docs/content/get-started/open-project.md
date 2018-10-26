# Get started: 2 - Open the FPS Starter Project

Launch the Unity Editor. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

**Before you start, apply these two quick Unity bug fixes:**

#### Shaders
There is a [known issue]({{urlRoot}}/known-issues) in the preview version of the [High Definition Render Pipeline](https://blogs.unity3d.com/2018/03/16/the-high-definition-render-pipeline-focused-on-visual-quality/), where shaders do not fully compile and appear visually darker than intended.

There is a quick fix however:

1. Open the FPS Starter Project in the Unity Editor located in `workers/unity`.
2. In the Project window, navigate to **Assets** > **Fps** > **Art** > **Materials**.
3. Right click on the `Source_Shaders` directory and select Reimport.

<img src="{{assetRoot}}assets/shader-fix.png" style="margin: 0 auto; display: block;" />

#### Bake Navmesh
There is a bug where the Unity Editor does not import the navmesh for the `FPS-SimulatedPlayerCoordinator` correctly when opening a project for the first time. To fix this, you need to rebake the navmesh for this scene.

To do this:

1. Open the `FPS-SimulatedPlayerCoordinator` scene located at `Assets/Fps/Scenes`.
2. Click on the `FPS-Start_Large` object in the [Unity Hierarchy window](https://docs.unity3d.com/Manual/Hierarchy.html), and enable the object.
3. Open the **Navigation** pane by clicking on **Windows** > **AI** > **Navigation**.
4. Navigate to the **Bake** tab and click on the **Bake** button.

You can verify that the NavMesh has been baked correctly by navigating to **Assets** > **Fps** > **Scenes** > **FPS-SimulatedPlayerCoordinator**, and checking that Unity displays the correct icon.
<img src="{{assetRoot}}assets/navmesh-fixed.png" style="margin: 0 auto; display: block;" />


<br/>
#### Next: [Build your workers]({{urlRoot}}/content/get-started/build-workers.md)

