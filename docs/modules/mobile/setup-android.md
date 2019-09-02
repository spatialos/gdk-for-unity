<%(TOC)%>

# Setting up Android support for the GDK

## Install the dependencies

The dependencies for developing for Android can be installed directly through the Unity Hub:

![]({{assetRoot}}assets/modules/mobile/add-android-dependencies.png)

### I have an existing installation

1. Open the Unity Hub.
2. Select Installs.
3. Find the Editor you want to add Android build support to.
4. Click the three dots to the right of the version label, then select Add Modules.
5. In the Add Modules dialog, locate the **Android build suport** and **Android SDK & NDK tools** modules and tick its checkbox.
6. When you have selected all the modules to add, select Done.

> Note: If you didn’t install the Editor via the Hub, you will not see the option to **Add Modules**. To enable this option, install the Editor via the Hub.

### I don't have an existing installation

Please follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and additionally select **Android build support** and **Android SDK & NDK tools** when installing Unity.

## Prepare your physical device

If you want to launch your game on a physical device, you need to:

1. Ensure you have USB debugging enabled on it. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.

## (Optional) Set up an emulator

1. Install [Android Studio](https://developer.android.com/studio/). At the Choose Components stage of the installation, be sure to select **Android Virtual Device**.
1. Create an Android emulator by going to **Tools** > **AVD Manager** > **Create Virtual Device**. Ensure you choose the same CPU architecture for your virtual machine as your development machine to get the best performance.

## (Optional) Set up Unity Remote

Unity Remote is Unity's solution for faster mobile development iteration times. Check out [Unity's documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for more information and setup instructions.
