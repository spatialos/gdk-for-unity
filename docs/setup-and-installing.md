# Get the dependencies



To get set you'll need to create a SpatialOS account, install some dependencies and clone the GDK repository.

<%(Include file="content/initial-set-up.md")%>

### Clone the SpatialOS GDK for Unity repository

Clone the SpacialOS GDK for Unity repository using one of the following commands:

|     |     |
| --- | --- |
| SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |
| HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |

Once you have cloned the repository, you need to checkout the latest release by using the following command: 

```
git checkout <version>
```

The GDK repository is a SpatialOS project called `gdk-for-unity`. It contains:

  * a Unity project at `gdk-for-unity/workers/unity`, which you need to open in the Unity editor in order to use the GDK
  * SpatialOS features, such as the schema and snapshot files

