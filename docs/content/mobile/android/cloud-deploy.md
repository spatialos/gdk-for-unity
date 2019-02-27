# Connect to a cloud deployment

Before reading this document, make sure you are familiar with:

* [Setting up Android Support for the GDK]({{urlRoot}}/content/mobile/android/setup)
* [Ways to test your Android client]({{urlRoot}}/content/mobile/android/ways-to-test)
* [Development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
* [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/content/gameobject/creating-workers-with-workerconnector)

## Prepare your project to connect to a cloud deployment {#prepare}

To connect your Android device to a cloud deployment, you need a mobile connector script.

**Note:** If you are using one of our [Starter Projects]({{urlRoot}}/content/glossary#starter-project), you can skip the **Create a mobile connector script** section below, as you already have one in your project.

<%(#Expandable title="Create a mobile connector script")%>

If you [added the GDK]({{urlRoot}}/content/set-up-new-project) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your Android client-worker GameObject. To do this:

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

1. [Build your server-workers.]({{urlRoot}}/content/build)
1. Upload your server-workers. To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/<your snapshot> <assembly name> <launch configuration>.json <deployment name>`.
1. In the SpatialOS Console, tag your cloud deployment with the tag `dev_login`. <BR/>
To do this:
  *  From the [SpatialOS Console](https://console.improbable.io/projects), select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token (SpatialOS documentation)](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance).
1. In your Unity Editor, select the prefab containing the MonoBehaviour script which inherits from the [`MobileWorkerConnector`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.mobile/Worker/MobileWorkerConnector.cs).<br>
In the FPS Starter Project this is located at: **Assets** > **Fps** > **Prefabs** > **AndroidClientWorker**.
1. Still in your Unity Editor, in the Inspector window, enter the Development Authentication Token you created in the `DevelopmentAuthToken` field.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your Android client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Select **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on your Android device or emulator.
