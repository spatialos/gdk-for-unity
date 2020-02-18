# Set up

<%(Include file="../../../fragments/machine-setup.md")%>

### 5. Get the FPS Starter Project source code

To run the the FPS Starter Project, you need to download the source code. There are two ways you can do this: _either_ get the source code as one zip file download _or_ clone the repository using Git. (To find out more about Git, see [github.io](https://try.github.io)).

<%(Callout message="We recommend using Git, as Git's version control makes it easier for you to get updates in the future.")%>

<%(#Expandable title="Zip file download")%>

 While we recommend using Git, if you prefer to, you can get the source code for the FPS Starter Project by downloading a zip file <a href="https://github.com/spatialos/gdk-for-unity-fps-starter-project/releases" data-track-link="Starter Project Zip Clicked|product=Docs" target="_blank">here</a>. Please download the latest release, the file should be called `Source code`.

<%(Callout message="If you have downloaded the source code via a zip file move on to the next section of this page: [Open the FPS Starter Project in your Unity Editor](#open-in-editor).")%>

<%(/Expandable)%>

<%(#Expandable title="Clone the repository  using Git")%>

If you haven't downloaded the zip file, you need the the FPS Starter Project repository.

###### Clone the FPS Starter Project repository

Clone the FPS Starter Project using one of the following commands:

|     |     |
| --- | --- |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity-fps-starter-project.git` |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity-fps-starter-project.git` |

> You can only clone via SSH if you have already [set up SSH keys (GitHub)](https://help.github.com/articles/connecting-to-github-with-ssh/) with your GitHub account.

<%(/Expandable)%>

### 6. Open the FPS Starter Project in your Unity Editor{#open-in-editor}

Launch your Unity Editor. It should automatically detect the project but if it doesn't, select **Open** and then select `gdk-for-unity-fps-starter-project/workers/unity`.

>**TIP:** The first time you open the Starter Project in your Unity Editor, It takes about 10 minutes; it's much quicker to open after this. (While you are waiting, you could look at our [blog](https://improbable.io/blog).)

<br/>

#### Next: [Build your workers]({{urlRoot}}/projects/fps/get-started/build-workers)
