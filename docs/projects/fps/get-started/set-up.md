# Set up

There are four parts to this step:

1. Sign up for a SpatialOS account (or make sure you are logged in)
2. Set up your machine
3. Get the GDK and the FPS Starter Project source code
4. Open the FPS Starter Project in your Unity Editor

(This page is the longest of the get started guide - the others are much quicker.)

<br/>

<%(Include file="../../../fragments/machine-setup.md")%>

<br/>

### 3. Get the GDK and the FPS Starter Project source code

To run the GDK and the FPS Starter Project, you need to download the source code. There are two ways you can do this: _either_ get both sets of source code as one zip file download _or_ clone the two repositories separately using Git. (To find out more about Git, see [github.io](https://try.github.io)).

<%(Callout message="We recommend using Git, as Git's version control makes it easier for you to get updates in the future.")%>

<%(#Expandable title="Zip file download")%>

 While we recommend using Git, if you prefer to, you can get the source code for both the GDK and FPS Starter Project by downloading one zip file <a href="https://github.com/spatialos/gdk-for-unity-fps-starter-project/releases" data-track-link="Starter Project Zip Clicked|product=Docs" target="_blank">here</a>. Please download the latest release, the file should be called something like `gdk-for-unity-fps-starter-project-x.y.z.zip`.

<%(Callout message="If you have downloaded the source code via a zip file move on to the next section of this page: [Open the FPS Starter Project in your Unity Editor](#4-open-the-fps-starter-project-in-your-unity-editor).")%>

<%(/Expandable)%>

<%(#Expandable title="Clone the two repositories using Git")%>

If you haven't downloaded the zip file, you need to clone two repositories; the FPS Starter Project and the GDK for Unity.

###### Clone the FPS Starter Project repository

Clone the FPS Starter Project using one of the following commands:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity-fps-starter-project.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity-fps-starter-project.git` |

> You can only clone via SSH if you have already [set up SSH keys (GitHub)](https://help.github.com/articles/connecting-to-github-with-ssh/) with your GitHub account.

###### Clone the GDK for Unity repository

You can use scripts to automatically do this or follow manual instructions.

**Scripts**<br/>
From the root of the `gdk-for-unity-fps-starter-project` repository:

  - If you are using Windows run: `powershell scripts/powershell/setup.ps1`
  - If you are using Mac run: `bash scripts/shell/setup.sh`

<br/>

**Manual Instructions**<br/>

Clone the [GDK for Unity](https://github.com/spatialos/gdk-for-unity) repository alongside the FPS Starter Project so that they sit side-by-side:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |

The two repositories should share a common parent, like the example below:

```text
  <common_parent_directory>
      ├── gdk-for-unity-fps-starter-project
      ├── gdk-for-unity
```

<%(/Expandable)%>

<br/>

### 4. Open the FPS Starter Project in your Unity Editor

Launch your Unity Editor. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

>**TIP:** The first time you open the Starter Project in your Unity Editor, It takes about 10 minutes; it's much quicker to open after this. (While you are waiting, you could look at our [blog](https://improbable.io/blog).)

<br/>

#### Next: [Build your workers]({{urlRoot}}/projects/fps/get-started/build-workers.md)
