<%(TOC)%>
# Connect to a cloud deployment

Before reading this document, make sure you are familiar with:

* [Setting up Android Support for the GDK](\{\{urlRoot\}\}/reference/mobile/android/setup)
* [Ways to test your Android client](\{\{urlRoot\}\}/reference/mobile/android/run-client)
* [Development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
* [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/reference/workflows/monobehaviour/creating-workers)

## Prepare your project to connect to a cloud deployment {#prepare}

To connect your Android device to a cloud deployment, you need a mobile connector script.

**Note:** If you are using one of our [Starter Projects](\{\{urlRoot\}\}/reference/glossary#starter-project), you can skip the **Create a mobile connector script** section below, as you already have one in your project.

<%(#Expandable title="Create a mobile connector script")%>

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your Android client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`MobileWorkerConnector`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.mobile/Worker/MobileWorkerConnector.cs) and include the functionality you want. You can base your implementation on [the one](https://github.com/spatialos/gdk-for-unity-blank-project/blob/master/workers/unity/Assets/Scripts/Workers/AndroidClientWorkerConnector.cs) in our Blank Starter Project.
1. In your Unity Editor, add the MonoBehaviour script to your Android client-worker GameObject.
1. In your Unity Editor, navigate to your Android client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is **not** checked in the script's drop-down window of the Inspector window.

<%(/Expandable)%>

## Android device or Android emulator

To connect your mobile application to a cloud deployment, you need to authenticate against our services.
This guide describes how to authenticate using the development authentication flow which we provide for the early stages of game development.
Altenatively, if you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk).

1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds) or connect your Android device to your development computer.

    > If you start an emulator, ensure you choose the same CPU architecture for your virtual machine as your development computer. Using a different architecture might affect the performance of your emulator.

1. [Build your server-workers.]({{urlRoot}}/projects/myo/build)
1. Upload your server-workers. To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/<your snapshot> <assembly name> <launch configuration>.json <deployment name>`.
1. In the SpatialOS Console, tag your cloud deployment with the tag `dev_login`. <BR/>
To do this:
  *  From the [SpatialOS Console](https://console.improbable.io/projects), select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance).<br>
Be sure to note down the `id` that is output when you create this, you will need it in a moment.
1. In your Unity Editor, locate the mobile connector script which inherits from the [`MobileWorkerConnector`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.mobile/Worker/MobileWorkerConnector.cs).<br>
If you're using the FPS Starter Project, you can locate this script in `Assets/FPS/Prefabs/AndroidClientWorker`.<br>
If you added the GDK to an existing project, then you created this script in the **Create a mobile connector script** section [above](#prepare).<br>
1. Still in your Unity Editor, in the Inspector, in the `Android Worker Connector` section, there is a **Development Auth Token** field.<br>
Enter the `id` that you noted down earlier.
1. In the same drop-down window, ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your Android client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Select **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on your Android device or emulator.
