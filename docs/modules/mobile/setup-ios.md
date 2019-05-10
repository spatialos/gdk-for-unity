<%(TOC)%>

# Setting up iOS support for the GDK

## Get the dependencies for developing iOS games

> **Note:** iOS development is only supported on macOS.

1. Install **iOS build support** for Unity
    * If you have not used the GDK for Unity before, please follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and additionally select **iOS build support** when installing Unity.
    * If you have Unity already installed and followed the [Setup & installation]({{urlRoot}}/machine-setup), open your Unity Hub and add the **iOS build support** to your existing installation.
1. Install [XCode](https://developer.apple.com/xcode/).
1. (Optional) Install the [Unity Remote](https://itunes.apple.com/gb/app/unity-remote-5/id871767552?mt=8) app on your physical device - this is Unity’s solution for faster development iteration times.

## Prepare your physical device

If you want to launch your game on a physical device, you need to:

1. Connect your iOS device to your computer using a USB cable. Accept the **Trust This Computer** alert if it appears.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.

## Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your project inside your Unity Editor.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select:
    * **Device SDK**, if you want to run your application on a physical device.
    * **Simulator SDK**, if you want to run your application on an iOS Simulator.
1. (Optional, if you want to use Unity Remote) Go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window and navigate to the **Unity Remote** section. Set the **Device** setting to **Any iOS Device**.
