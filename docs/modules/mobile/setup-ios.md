<%(TOC)%>

# Setting up iOS support for the GDK

<%(Callout message="
Before reading this document, make sure you have read:

  * [Mobile support overview]({{urlRoot}}/modules/mobile/overview)
  * [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/reference/workflows/monobehaviour/creating-workers)
")%>

## Get the dependencies for developing iOS games

> **Note:** iOS development is only supported on macOS.

1. Follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and be sure to select the **iOS build support** component during your Unity installation.
1. Install [XCode](https://developer.apple.com/xcode/).
1. (Optional) Install the [Unity Remote](https://itunes.apple.com/gb/app/unity-remote-5/id871767552?mt=8) app on your physical device - this is Unity’s solution for faster development iteration times.

## Prepare your physical device

If you want to launch your game on a physical device, you need to 

1. Connect your iOS device to your computer using a USB cable. Accept the **Trust This Computer** alert if it appears.
1. If you want to connect your device to a local deployment, ensure your computer and your mobile device are connected to the same network.

## Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your project inside the Unity Editor.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select:
    * **Device SDK**, if you want to run your application on a physical device.
    * **Simulator SDK**, if you want to run your application on an iOS Simulator.
1. (Optional, if you want to use Unity Remote) Go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window and navigate to the **Unity Remote** section. Set the **Device** setting to **Any iOS Device**.

## Set up your project

> If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip the **Create a mobile connector script** section, as you already have one in your project.

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your mobile client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`DefaultMobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) and include the functionality you want. This scripts contains support for both Android and iOS. You can base your implementation on the one in our [Blank Starter Project](https://github.com/spatialos/gdk-for-unity-blank-project/blob/develop/workers/unity/Assets/Scripts/Workers/MobileClientWorkerConnector.cs).
1. In your Unity Editor, add the MonoBehaviour script to your mobile client-worker GameObject.

