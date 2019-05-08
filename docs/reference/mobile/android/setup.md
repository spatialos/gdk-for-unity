<%(TOC)%>

# Setting up Android support for the GDK

<%(Callout message="
Before reading this document, make sure you have read:

  * [Mobile support overview]({{urlRoot}}/reference/mobile/overview)
  * [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/reference/workflows/monobehaviour/creating-workers)
")%>

## Get the dependencies for developing Android games

1. Follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and be sure to select the **Android build support** component during your Unity installation.
1. Install [Android Studio](https://developer.android.com/studio/):<br>
  At the Choose Components stage of the installation, be sure to select **Android Virtual Device**.
1. Install the Android SDK:<br>
Once the Android Studio installation is complete, open Android Studio and select **Configure** > **SDK Manager**.<br>
Select the version you intend to develop your game for.<br>
Select **Apply** to download and install the matching **Android SDK** version.
1. (Optional) Download and unzip [Android NDK r16b](https://developer.android.com/ndk/downloads/older_releases).<br>

    > This is only needed, if you want to build Android using [IL2CPP](https://docs.unity3d.com/Manual/IL2CPP.html). Extract it to a directory of your choice. Note down this file path as you need to specify the path in your Unity Editor later.

1. (Optional) Install the [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) app on your physical device - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your SpatialOS project inside the Unity Editor. It is located inside `<path-to-your-project>/workers/unity`.

    > If you don’t have a SpatialOS Unity project, you can use the [FPS Starter Project]({{urlRoot}}/projects/fps/get-started/get-started) or the [Blank Starter Project]({{urlRoot}}/projects/blank/overview) to get started.

1. In your Unity Editor, go to the **External Tools** window. To do this:
    * on Windows, go to **Edit** > **Preferences** > **External Tools**
    * on MacOS, go to **Unity** > **Preferences** > **External Tools**

1. In the **Android** section of the **External Tools** window, tick the **Use embedded JDK** checkbox to use the JDK that Unity provides. 
1. Input the following paths into the **Android Section**:

	| Field | How to find the path |
	|-------|------|
	| SDK  |  You can find the SDK location by opening Android Studio and selecting **Configure** then **SDK Manager**. |
	| NDK  |  (Optional) Use the directory path that you noted down earlier when extracting the Android NDK.|

1. (Optional, if you want to use Unity Remote) Go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window and navigate to the **Unity Remote** section. Set the **Device** setting to **Any Android Device**.

## Set up your project

If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip the **Create a mobile connector script** section, as you already have one in your project.

<%(#Expandable title="Create a mobile connector script")%>

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your Android client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`DefaultMobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) and include the functionality you want. You can base your implementation on [the one](https://github.com/spatialos/gdk-for-unity-blank-project/blob/develop/workers/unity/Assets/Scripts/Workers/MobileClientWorkerConnector.cs) in our Blank Starter Project.
1. In your Unity Editor, add the MonoBehaviour script to your Android client-worker GameObject.

<%(/Expandable)%>

If you want to launch your application on a physical device, you need to 

1. Ensure you have USB debugging enabled on it. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. If you want to connect it to a local deployment, ensure your computer and your mobile device are connected to the same network.

### Prepare your project to connect to a local deployment

To connect your Android device to a local deployment, you need to prepare its configuration. Follow the steps below to ensure your project is set up correctly:

1. Open your project in your Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Runtime IP for local deployment** field, enter your local machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Select **Save** and close the window.

### Prepare your project to connect to a cloud deployment

To connect your Android device to a cloud deployment, you need to authenticate against our services. This guide describes how to authenticate using the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication) which we provide for the early stages of game development. The GDK for Unity provides tooling around it to help you iterate faster on your mobile application.

1. Open your project in your Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Dev Auth Token Settings** specify the lifetime of the token and the path to the [Resources folder](https://unity3d.com/learn/tutorials/topics/best-practices/resources-folder) that you would like to store the generated token in.
1. Select **Save** and close the window.
1. Go to **SpatialOS** > **Generate Dev Authentication Token**. This generates a `DevAuthToken.txt` asset in the folder you specified in the configuration window. If your worker connector inherits from the `DefaultMobileWorkerConnector` script, it will automatically read that token when running the application and authenticate against our services.

  > Your token expires after the amount of days that you specified in the configuration window. Regenerate the token whenever that happens.

If you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk).
