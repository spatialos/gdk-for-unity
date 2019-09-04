<%(TOC)%>

# Set up iOS support

> **Note:** iOS development is only supported on macOS.

## 1. Install the build dependencies

The build dependencies for developing for iOS can be installed directly through the Unity Hub.

![]({{assetRoot}}assets/modules/mobile/add-ios-dependencies.png)

### I have an existing Unity installation

1. Open the Unity Hub.
2. Select Installs.
3. Find the Editor you want to add Android build support to.
4. Click the three dots to the right of the version label, then select Add Modules.
5. In the Add Modules dialog, locate the **iOS Build Support** module and tick its checkbox.
6. When you have selected all the modules to add, select Done.

> **Note:** If you didn’t install the Editor via the Hub, you will not see the option to **Add Modules**. To enable this option, install the Editor via the Hub.

### I don't have an existing Unity installation

Please follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and additionally select the **iOS Build Support** module when installing Unity.

## 2. Install Xcode and the command line tools

Install [Xcode](https://developer.apple.com/xcode/) through the [Mac App Store](https://apps.apple.com/us/app/xcode/id497799835?mt=12).

Once Xcode is installed, download and install the Command Line Tools for Xcode via the [download archive](https://developer.apple.com/download/more/?=xcode).

## 3. Prepare your physical device

If you want to launch your game on a physical device, you need to:

1. Connect your iOS device to your computer using a USB cable. Accept the **Trust This Computer** alert if it appears.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.

## 4. Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your project inside your Unity Editor.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select:
    * **Device SDK**, if you want to run your application on a physical device.
    * **Simulator SDK**, if you want to run your application on an iOS Simulator.
1. (Optional, if you want to use Unity Remote) Go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window and navigate to the **Unity Remote** section. Set the **Device** setting to **Any iOS Device**.

## (Optional) Set up Unity Remote

Unity Remote is Unity's solution for faster mobile development iteration times. Check out [Unity's documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for more information and setup instructions.
