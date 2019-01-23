# Get started: 1 - Set up
There are three parts to this step: 

* Sign up for a SpatialOS account (or make sure you are logged in)
* Set up your machine
* Get the GDK and the FPS Starter Project source code

(This step is the longest of the 6 steps - the others are much quicker.)
<br/>
<br/>

<%(Include file="../initial-set-up.md")%>

## Get the GDK and the FPS Starter Project source code
To run the GDK and the FPS Starter project, you need to download the source code. There are two ways you can do this: _either_ get both sets of source code as one zip file download _or_ clone the two repositories separately using Git. (To find out more about Git, see [github.io](https://try.github.io)).

**NOTE:** We recommend using Git, as Git's version control makes it easier for you to get updates in the future.

### Zip file download

 While we recommend using Git, if you prefer to, you can get the source code for both the GDK and FPS Starter Project by downloading one zip file <a href="https://github.com/spatialos/gdk-for-unity-fps-starter-project/releases/tag/0.1.3" data-track-link="Starter Project Zip Clicked|product=Docs" target="_blank">here</a>. 

**NOTE:**
If you have downloaded the source code via a zip file, skip the rest of this page and move on to the next step: [Open the FPS Starter Project]({{urlRoot}}/content/get-started/open-project.md).

### Clone the two repositories using Git

If you haven't downloaded the zip file, you need to clone two repositories; the FPS Starter Project and the GDK for Unity.

#### 1. Clone the FPS Starter Project repository

Clone the FPS Starter Project using one of the following commands:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity-fps-starter-project.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity-fps-starter-project.git` |

**NOTE:**
You can only clone via SSH if you have already [set up SSH keys (GitHub help)](https://help.github.com/articles/connecting-to-github-with-ssh/) with your GitHub account.

Then navigate to the root folder of the FPS starter project and run the following command: `git checkout 0.1.3`

This ensures that you are on the alpha release version of the FPS starter project.

#### 1. Clone the GDK for Unity repository and checkout the latest release

You can use scripts to automatically do this or follow manual instructions.

* To use the scripts:<br/>
From the root of the `gdk-for-unity-fps-starter-project` repository:
    * If you are using Windows run: `powershell scripts/powershell/setup.ps1`
    * If you are using Mac run: `bash scripts/shell/setup.sh`
* To follow manual instructions, see below:

<%(#Expandable title="Manually clone the GDK for Unity")%>

2. Clone the [GDK for Unity](https://github.com/spatialos/gdk-for-unity) repository alongside the FPS Starter Project so that they sit side-by-side:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |

  > The two repositories should share a common parent, like the example below:

```text
  <common_parent_directory>
      ├── gdk-for-unity-fps-starter-project
      ├── gdk-for-unity
```

2. Navigate to the `gdk-for-unity` directory and checkout the pinned version which you can find in the `gdk.pinned` file, in the root of the `gdk-for-unity-fps-starter-project` directory.
  * `git checkout <pinned_version>`

<%(/Expandable)%>

<br/>

#### Next: [Open the FPS Starter Project]({{urlRoot}}/content/get-started/open-project.md)
