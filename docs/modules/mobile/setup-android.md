<%(TOC)%>

# Setting up Android support for the GDK

<%(Callout message="
Before reading this document, make sure you have read:

  * [Mobile support overview]({{urlRoot}}/modules/mobile/overview)
")%>

## Get the dependencies for developing Android games

1. Install **Android build support** for Unity:
    * If you have not used the GDK for Unity before, please follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and additionally select **Android build support** when installing Unity.
    * If you have Unity already installed and followed the [Setup & installation]({{urlRoot}}/machine-setup), open your Unity Hub and add the **Android build support** to your existing installation.
1. Install [Android Studio](https://developer.android.com/studio/). At the Choose Components stage of the installation, be sure to select **Android Virtual Device**.
1. Open Android Studio and select **Configure** > **SDK Manager**.
1. Select the version you intend to develop your game for.
1. Select **Apply** to download and install the matching **Android SDK** version.
1. (Optional) Create an Android emulator by going to **Tools** > **AVD Manager** > **Create Virtual Device**. Ensure you choose the same CPU architecture for your virtual machine as your development machine to get the best performance.
1. (Optional) Download and unzip [Android NDK r16b](https://developer.android.com/ndk/downloads/older_releases). Our supported Unity version requires you to use NDK r16b. Older or newer versions of the NDK are not supported.

    > **Note:** This is only needed if you want to build Android using [IL2CPP](https://docs.unity3d.com/Manual/IL2CPP.html). Extract it to a directory of your choice. Note down the directory path as you must specify the path in your Unity Editor later.

1. (Optional) Install the [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) app on your physical device - this is Unity’s solution for faster development iteration times.

## Prepare your physical device

If you want to launch your game on a physical device, you need to:

1. Ensure you have USB debugging enabled on it. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.

## Set up your Unity Editor

> If you don’t have a SpatialOS Unity project, you can get started with the [FPS Starter Project]({{urlRoot}}/projects/fps/get-started/get-started), the [Blank Starter Project]({{urlRoot}}/projects/blank/overview), or [make your own project]({{urlRoot}}/projects/myo/setup).

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your project inside your Unity Editor.
1. In your Unity Editor, go to the **External Tools** window. To do this:
    * On Windows, go to **Edit** > **Preferences** > **External Tools**.
    * On MacOS, go to **Unity** > **Preferences** > **External Tools**.

1. In the **Android** section of the **External Tools** window, tick the **Use embedded JDK** checkbox to use the JDK that Unity provides. 
1. Input the following paths into the **Android Section**:
    * **SDK:** You can find the SDK location by opening Android Studio and going to **Configure** > **SDK Manager**.
    * **NDK:** (Optional) Use the directory path that you noted down earlier when extracting the Android NDK.

1. (Optional, if you want to use Unity Remote) Go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window and navigate to the **Unity Remote** section. Set the **Device** setting to **Any Android Device**.
