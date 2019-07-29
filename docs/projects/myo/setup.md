<%(TOC max="2")%>

# Project setup

_Make sure you have followed the [Setup & installation]({{urlRoot}}/machine-setup) before following this guide._

<br/>

To use the SpatialOS GDK for Unity in a new project, you need to:

1. Setup the [SpatialOS project structure](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/project-layout/files-and-directories).
1. Add the GDK packages to your Unity project.

## Setup a SpatialOS project

A SpatialOS project needs to have a specific directory layout and configuration files in order to function properly.

**Step 1.** Setup the directory layout.

In the root of your SpatialOS project, you need to create the following directories: `schema` and `workers`.

* `schema`: This directory contains your `*.schema` files.
* `workers`: This directory contains your worker code and configuration.

<%(#Expandable title="What should my project directory look like when I'm done?")%>

```text
  <project_root>
    ├── schema/
    ├── workers/
```

<%(/Expandable)%>

**Step 2.** Create the required configuration files.

In the root of your SpatialOS project, create two files:

* `spatialos.json`, copy and paste in the following content:

```json
{
    "name": "my_project",
    "project_version": "0.0.1",
    "sdk_version": "13.7.1-gdk-for-unity",
    "dependencies": [
        {"name": "standard_library", "version": "13.7.1-gdk-for-unity"}
    ]
}
```

> Note to replace the value of the `name` field with your **own** project name, which you can find in the [SpatialOS Console](https://console.improbable.io/projects).

* `default_launch.json`, copy and paste in the following content:

```json
{
  "template": "w2_r0500_e5",
  "world": {
    "chunkEdgeLengthMeters": 50,
    "snapshots": {
      "snapshotWritePeriodSeconds": 0
    },
    "dimensions": {
      "xMeters": 1000,
      "zMeters": 1000
    }
  },
  "load_balancing": {
    "layer_configurations": [
      {
        "layer": "UnityGameLogic",
        "points_of_interest": {
          "num_workers": 1,
          "points": [
            {
              "x": 0,
              "z": 0
            }
          ]
        },
        "options": {
          "manual_worker_connection_only": true
        }
      }
    ]
  },
  "workers": [
    {
      "worker_type": "UnityGameLogic",
      "permissions": [
        {
          "all": {}
        }
      ]
    },
    {
      "worker_type": "UnityClient",
      "permissions": [
        {
          "all": {}
        }
      ]
    }
  ]
}
```

<%(#Expandable title="What should my project directory look like when I'm done?")%>

```text
  <project_root>
    ├── schema/
    ├── workers/
    ├── spatialos.json
    ├── default_launch.json
```

<%(/Expandable)%>

**Step 3.** Create a new Unity project.

You need to put the new Unity project in the `workers` directory. For example `workers/my-unity-project/`.

<%(#Expandable title="What should my project directory look like when I'm done?")%>

```text
  <project_root>
    ├── schema/
    ├── workers/
        ├── my-unity-project/
    ├── spatialos.json
    ├── default_launch.json
```

<%(/Expandable)%>

**Step 4.** Add worker configurations.

For a basic set up of two worker types, an `UnityGameLogic` and `UnityClient`, we recommend you to reuse these files:

* [spatialos.UnityGameLogic.worker.json](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/spatialos.UnityGameLogic.worker.json)
* [spatialos.UnityClient.worker.json](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/spatialos.UnityClient.worker.json)

Copy these files into `workers/my-unity-project/`.

<%(#Expandable title="What should my project directory look like when I'm done?")%>

```text
  <project_root>
    ├── schema/
    ├── workers/
        ├── my-unity-project/
            ├── spatialos.UnityGameLogic.worker.json
            ├── spatialos.UnityClient.worker.json
    ├── spatialos.json
    ├── default_launch.json
```

<%(/Expandable)%>

## Add the GDK packages

**Step 1.** Create a `asmdef` for generated code.

In order to ensure that the generated code can access its dependencies, you will need to create an `asmdef` for the generated code. We recommend that you reuse the following file:

* [Improbable.Gdk.Generated.asmdef](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Assets/Generated/Improbable.Gdk.Generated.asmdef)

Copy this file into `workers/my-unity-project/Assets/Generated/Improbable.Gdk.Generated.asmdef`.

> **Note:** You will need to create the `Generated` folder.

**Step 2.** Add package references to your `manifest.json`.

Unity loads the packages that are declared in the `manifest.json` file into your Unity project.

Open the `manifest.json` in the `workers/my-unity-project/Packages/` directory and add the following:

```json
{
  "dependencies": {
    "io.improbable.gdk.core": "<%(Var key="current_version")%>",
    "io.improbable.gdk.buildsystem": "<%(Var key="current_version")%>"
  },
  "scopedRegistries": [
    {
      "name": "Improbable",
      "url": "https://npm.improbable.io/gdk-for-unity/",
      "scopes": [
        "io.improbable"
      ]
    }
  ]
}
```

See our [FPS Starter Project](https://github.com/spatialos/gdk-for-unity-fps-starter-project/blob/master/workers/unity/Packages/manifest.json) for an example.

> **Note:** There may already be some dependencies listed in your `manifest.json`. If there are, do not remove them - just add to the list.

<%(Callout message="The packages listed above are just the **minimum** set required to get started with the GDK.<br/><br/>You can add additional feature module packages by referencing them the same way.")%>

**Step 3.** Create a GDK tools configuration file.

The GDK for Unity uses a configuration file when generating code. We recommend that you reuse this file:

* [`GdKToolsConfiguration.json`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Assets/Config/GdkToolsConfiguration.json)

Copy this file into `workers/my-unity-project/Assets/Config/GdkToolsConfiguration.json`

> **Note:** You will need to create the `Assets/Config` folder.

**Step 4.** Open your Unity project.

Open your Unity project located at `workers/my-unity-project`. This triggers code generation for your project.

> **Note:** Unity generates code from the [schema]({{urlRoot}}/reference/glossary#schema) files defined in your SpatialOS project.

<br/>

<%(#Expandable title="What should my project directory look like when I'm done?")%>

```text
  <project_root>
    ├── schema/
    ├── workers/
        ├── my-unity-project/
            ├── Assets/
                ├── Config/
                    ├── GdkToolsConfiguration.json
                ├── Generated/
                    ├── Improbable.Gdk.Generated.asmdef
                    ├── ...
                ├── ...
            ├── spatialos.UnityGameLogic.worker.json
            ├── spatialos.UnityClient.worker.json
            ├── ...
    ├── spatialos.json
    ├── default_launch.json
```

<%(/Expandable)%>
