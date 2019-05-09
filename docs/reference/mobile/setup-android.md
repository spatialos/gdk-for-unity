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
1. Open Android Studio and select **Configure** > **SDK Manager**.
1. Select the version you intend to develop your game for.
1. Select **Apply** to download and install the matching **Android SDK** version.
1. (Optional) Create an Android emulator by going to **Tools** > **AVD Manager** > **Create Virtual Device**. Ensure you choose the same CPU architecture for your virtual machine as your development machine to get the best performance.
1. (Optional) Download and unzip [Android NDK r16b](https://developer.android.com/ndk/downloads/older_releases).<br>

    > This is only needed, if you want to build Android using [IL2CPP](https://docs.unity3d.com/Manual/IL2CPP.html). Extract it to a directory of your choice. Note down this file path as you need to specify the path in your Unity Editor later.

1. (Optional) Install the [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) app on your physical device - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your project inside the Unity Editor.
1. In your Unity Editor, go to the **External Tools** window. To do this:
    * on Windows, go to **Edit** > **Preferences** > **External Tools**
    * on MacOS, go to **Unity** > **Preferences** > **External Tools**

1. In the **Android** section of the **External Tools** window, tick the **Use embedded JDK** checkbox to use the JDK that Unity provides. 
1. Input the following paths into the **Android Section**:
    * **SDK:** You can find the SDK location by opening Android Studio and going to **Configure** > **SDK Manager**.
    * **NDK:** (Optional) Use the directory path that you noted down earlier when extracting the Android NDK.

1. (Optional, if you want to use Unity Remote) Go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window and navigate to the **Unity Remote** section. Set the **Device** setting to **Any Android Device**.

## Set up your project

> If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip the **Create a mobile connector script** section, as you already have one in your project.

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your mobile client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`DefaultMobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) and include the functionality you want. This scripts contains support for both Android and iOS. You can base your implementation on [the one](https://github.com/spatialos/gdk-for-unity-blank-project/blob/develop/workers/unity/Assets/Scripts/Workers/MobileClientWorkerConnector.cs) in our Blank Starter Project.
1. In your Unity Editor, add the MonoBehaviour script to your mobile client-worker GameObject.

If you want to launch your game on a physical device, you need to 

1. Ensure you have USB debugging enabled on it. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.
