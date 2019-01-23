# Get the dependencies



To start using the SpatialOS GDK for Unity you'll need to create a SpatialOS account, install some dependencies and clone the GDK repository.

<%(Include file="content/initial-set-up.md")%>

### Clone the SpatialOS GDK for Unity repository

To run the GDK for Unity, you need to download the source code. To do this, you need to clone the GDK repository.

#### Using a terminal:

Clone the SpacialOS GDK for Unity repository using one of the following terminal commands:

|     |     |
| --- | --- |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |

#### Using GitHub Desktop

To clone the repository using GitHub desktop: navigate to **File > Clone Repository**, click the URL tab and enter `https://github.com/spatialos/gdk-for-unity` into the URL or username/repository field, then click Clone. 

#### Check out the latest release

Once you have cloned the repository, you need to checkout the latest release by using the following terminal command: 

`git checkout 0.1.3`

The GDK repository is a SpatialOS project called `gdk-for-unity`. It contains:

  * a Unity project at `gdk-for-unity/workers/unity`, which you need to open in the Unity editor in order to use the GDK
  * SpatialOS features, such as the schema and snapshot files

