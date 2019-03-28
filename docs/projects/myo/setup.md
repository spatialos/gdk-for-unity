<%(TOC max="2")%>
# Project setup

_Make sure you have followed the [Setup & installation]({{urlRoot}}/machine-setup) before following this guide._

---

To use the SpatialOS GDK for Unity in a new project, you need to:

1. Clone the `gdk-for-unity` repository.
1. Setup the [SpatialOS project structure](https://docs.improbable.io/reference/latest/shared/reference/project-structure).
1. Add the GDK packages to your Unity project.

## Clone the SpatialOS GDK for Unity repository

To run the GDK for Unity, you need to download the source code. To do this, you need to clone the GDK repository.

### Using a terminal

Clone the SpatialOS GDK for Unity repository using one of the following terminal commands:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |

### Using GitHub Desktop

To clone the repository using GitHub desktop: navigate to **File > Clone Repository**, select the URL tab and enter `https://github.com/spatialos/gdk-for-unity` into the URL or username/repository field, then select Clone.

## Setup a SpatialOS project

A SpatialOS project needs to have a specific directory layout and configuration files in order to function properly.

**Step 1.** Setup the directory layout.

In the root of your SpatialOS project, you need to create the following directories: `schema` and `workers`.

* `schema`: This directory contains your `.schema` files.
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
    "sdk_version": "13.6.2-gdk-for-unity",
    "dependencies": [
        {"name": "standard_library", "version": "13.6.2-gdk-for-unity"}
    ]
}
```

> Note to replace the value of the `name` field with your **own** project name, which you can find in the [SpatialOS Console](https://console.improbable.io/projects).

* `default_launch.json`, copy and paste in the following content:

```json
{
  "template": "small",
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

Unity loads the packages that are declared in the `manifest.json` file into your Unity project. We will be side-loading the GDK packages.

Open the `manifest.json` in the `workers/my-unity-project/Packages/` directory and add the following dependencies:

```json
{
  "dependencies": {
    "com.improbable.gdk.core": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.core",
    "com.improbable.gdk.tools": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.tools",
    "com.improbable.gdk.buildsystem": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.buildsystem",
    "com.improbable.gdk.testutils": "file:<path-to-the-gdk>/workers/unity/Packages/com.improbable.gdk.testutils",
  }
}
```

> **Note:** There may already be some dependencies listed in your `manifest.json`. If there are, do not remove them - just add to the list.

<%(Callout message="The packages listed above are just the **minimum** set required to get started with the GDK.<br/><br/>You can add additional feature module packages by referencing them the same way.")%>

<%(#Expandable title="What is sideloading?")%>
Sideloading is the mechanism by which we load packages in the GDK (for now!). 

Unity allows you to reference a package with a file path instead of fetching a versioned package from a registry.

The consequence of this mechanism is that you tend to need two repositories cloned side by side.
<%(/Expandable)%>

**Step 3.** Create a GDK tools configuration file.

Our `com.improbable.gdk.tools` package uses a configuration file when downloading Worker SDK packages and generating code. We recommend that you reuse this file:

* [`GdKToolsConfiguration.json`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Assets/Config/GdkToolsConfiguration.json)

Copy this file into `workers/my-unity-project/Assets/Config/GdkToolsConfiguration.json`

> **Note:** You will need to create the `Config` folder.

**Step 4.** Open your Unity project.

Open your Unity project located at `workers/my-unity-project`. This triggers a few actions:
  
* Unity downloads several required SpatialOS libraries.

> This may result in opening a browser windows prompting you to log in to your SpatialOS account.

* Unity generates code from the [schema]({{urlRoot}}/reference/glossary#schema) files defined in your SpatialOS project.
