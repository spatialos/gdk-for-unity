<%(TOC max="2")%>

# 1.2 - Project walkthrough

The Blank Project is a barebones SpatialOS project containing the minimal functionality required to begin development with the GDK. It includes:

* A basic Unity project.
* Local and cloud launch configurations.
* A default snapshot containing only a single PlayerCreator entity.
* Client-worker and server-worker configurations.
* Worker build configuration.
* Unity Scenes to use in development.

## Clone the project

Before exploring the contents of the Blank Project, we recommend you first download the source code. There are two ways you can do this: _either_ get the source code as one zip file download _or_ clone the repository using [Git](https://try.github.io).

<%(Callout message="We recommend using Git, as Git's version control makes it easier for you to get updates in the future.")%>

<%(#Expandable title="Zip file download")%>

 While we recommend using Git, if you prefer to, you can get the source code for the Blank Project by downloading one zip file <a href="https://github.com/spatialos/gdk-for-unity-blank-project/releases" target="_blank">here</a>. Please download the latest release, the file should be called something like `gdk-for-unity-blank-project-x.y.z.zip`.

<%(/Expandable)%>

<%(#Expandable title="Clone the repository using Git")%>

If you haven't downloaded the zip file, you need the Blank Project repository.

Clone the Blank Project using one of the following commands:

|       |                                                                          |
| ----- | ------------------------------------------------------------------------ |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity-blank-project.git` |
| SSH   | `git clone git@github.com:spatialos/gdk-for-unity-blank-project.git`     |

> You can only clone via SSH if you have already [set up SSH keys (GitHub)](https://help.github.com/articles/connecting-to-github-with-ssh/) with your GitHub account.

<%(/Expandable)%>

## Configurations

At the root of the project you’ll find two launch configurations: one for local deployments and one for cloud deployments.

<pre>

    gdk-for-unity-blank-project/
        ├── snapshots/
        ├── workers/
        ├── <b><font color="white">cloud_launch.json</font></b>
        ├── <b><font color="white">default_launch.json</font></b>
        ├── spatialos.json

</pre>

These configurations are used to determine what template your deployment uses, the world size, the load balancing policies, and some worker-specific configuration parameters.

The snapshots directory contains a default snapshot defining the initial state of your game world.

<pre>

    gdk-for-unity-blank-project/
        ├── snapshots/
                ├── <b><font color="white">default.snapshot</font></b>
        ...

</pre>

You can find the root of the Unity project in `workers/unity/`.

<pre>

    gdk-for-unity-blank-project/
        ...
        ├── workers/
            ├── <b><font color="white">unity/</font></b>
                ├── <b><font color="white">Assets/</font></b>
                ├── <b><font color="white">Packages/</font></b>
                ├── <b><font color="white">ProjectSettings/</font></b>
                ├── <b><font color="white">spatialos.UnityClient.worker.json</font></b>
                ├── <b><font color="white">spatialos.UnityGameLogic.worker.json</font></b>
        ...

</pre>

Here, you will find the client-worker and server-worker configurations. They are used to configure how a worker should be launched and specify particular Runtime settings for the given worker type.

Under `Assets/Config/` of the Unity Project, there is a `BuildConfiguration` asset.

<pre>

    Assets/
        ├── Config/
            ├── <b><font color="white">BuildConfiguration.asset</font></b>
            ...

</pre>

This asset defines for each worker type the target platforms to build and which Scenes to include. By default the GDK builds both Windows and MacOS UnityClients for cloud deployments, but only build a single platform (Windows or MacOS depending on your machine) for local deployments. This is because you will only ever run a client for local deployments on your machine, but may want to distribute your game to both Windows and MacOS users when running a cloud deployment.

See below to learn more about:

* [SpatialOS launch configurations](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/launch-config)
* [SpatialOS worker configurations](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/introduction)
* [Snapshots]({{urlRoot}}/reference/concepts/snapshots)
* [GDK worker build configurations]({{urlRoot}}/modules/build-system/build-config)

## Opening the project

Open the Unity Project found at `workers/unity/`. The first time you open the project, Unity will retrieve all the packages defined in the `manifest.json` and cache them on your machine. After resolving the packages, you’ll see a popup that says `Generating code`.

<img src="{{assetRoot}}assets/blank/tutorial/1/generating-code.png" style="margin: 0 auto; width: 50%; display: block;" />

## Scenes

You may have noticed that there are a set of Scenes present at `Assets/Scenes/`.

<img src="{{assetRoot}}assets/blank/tutorial/1/scenes-list.png" style="margin: 0 auto; width: 25%; display: block;" />

The `ClientScene` contains a `ClientWorker` prefab to represent a client-worker and similarly the `GameLogicScene` contains a `GameLogicWorker` prefab to represent a server-side worker. By playing either of these Scenes, these prefabs will try to connect their respective client-worker or server-worker to your SpatialOS deployment.

```text

    ClientScene
        ├── ClientWorker

    DevelopmentScene
        ├── ClientWorker
        ├── GameLogicWorker

    GameLogicScene
        ├── GameLogicWorker

```

The GDK also allows you to run more than one worker in your Unity Editor. The `DevelopmentScene` contains both `ClientWorker` and `GameLogicWorker` prefabs, which in turn runs both a client-worker and a server-worker side-by-side in your Editor. By running both workers you don’t need to build out workers with every new change. This greatly speeds up local iteration times, as you’ll discover later on in this tutorial.

## Packages and assembly definitions

Within your Unity Project there is a `Packages/manifest.json` file, which defines all the package dependencies of your project. The Blank Project includes dependencies to all Feature Modules, to make it easier to adopt and include them in your user code.

<pre>

    unity/
        ├── Assets/
            ├── <b><font color="white">BlankProject.asmdef</font></b>
            ...
        ├── Packages/
            ├── <b><font color="white">manifest.json</font></b>
        ...

</pre>

There is also a BlankProject assembly definition in your project’s Assets folder. Although the project `manifest.json` defines dependencies to all GDK Feature Modules, only the Core and Player Lifecycle modules are referenced in the assembly definition. This is because the Blank Project has minimal game code, and only makes use of the Player Lifecycle module. You can read more about assembly definitions [here](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html).

#### Next: [What the Blank Project does]({{urlRoot}}/projects/blank/tutorial/1/what-it-does)
