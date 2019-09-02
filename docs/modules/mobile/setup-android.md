<%(TOC)%>

# Setting up Android support for the GDK

## Get the dependencies for developing Android games

1. Install **Android build support** for Unity:
    * If you have not used the GDK for Unity before, please follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and additionally select **Android build support** and **Android SDK & NDK tools** when installing Unity.
    * If you have Unity already installed and followed the [Setup & installation]({{urlRoot}}/machine-setup), open your Unity Hub and add the **Android build support** and **Android SDK & NDK tools** to your existing installation.
1. Install [Android Studio](https://developer.android.com/studio/). At the Choose Components stage of the installation, be sure to select **Android Virtual Device**.
1. (Optional) Create an Android emulator by going to **Tools** > **AVD Manager** > **Create Virtual Device**. Ensure you choose the same CPU architecture for your virtual machine as your development machine to get the best performance.

## Prepare your physical device

If you want to launch your game on a physical device, you need to:

1. Ensure you have USB debugging enabled on it. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.

## (Optional) Set up Unity Remote

Unity Remote is Unity's solution for faster mobile development iteration times. Check out [Unity's documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for more information and setup instructions.
