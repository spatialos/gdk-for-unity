<%(TOC)%>

# Connect to a local deployment

This page describes how to connect an iOS client to a local deployment of SpatialOS. You can do this using either; just your[Unity Editor](#in-editor), the [Unity Remote](#unity-remote) app, the [iOS Simulator](#ios-simulator), or your own [iOS device](#ios-device).

Before reading this document, make sure you are familiar with:

  * [Setting up iOS Support for the GDK]({{urlRoot}}/reference/mobile/ios/setup)
  * [Ways to test your iOS client]({{urlRoot}}/reference/mobile/ios/run-client)

## Prepare your project to connect to a local deployment {#prepare}

To connect your iOS device to a local deployment, you need to prepare its configuration. Follow the steps below to ensure any local deployment you launch via your Unity Editor is correctly set up.

1. Open your project in your Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Runtime IP for local deployment** field, enter your local machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Select **Save** and close the window.

**Note:** If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip the **Create a mobile connector script** section below, as you already have one in your project.

<%(#Expandable title="Create a mobile connector script")%>

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your iOS client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`MobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) and include the functionality you want. You can base your implementation on [the one](https://github.com/spatialos/gdk-for-unity-blank-project/blob/master/workers/unity/Assets/Scripts/Workers/iOSClientWorkerConnector.cs) in our Blank Starter Project.
1. In your Unity Editor, add the MonoBehaviour script to your iOS client-worker GameObject.
1. In your Unity Editor, navigate to your iOS client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is checked in the script's drop-down window of the Inspector window.

<%(/Expandable)%>

## Unity Editor{#in-editor}
1. In your Unity Editor, select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In your Unity Editor, open the `FPS-Development` Scene and select the Play button.<br/>

## Unity Remote{#unity-remote}
You need the Unity Remote app installed on your iOS device in order to follow steps below. See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for details.

1. Open your project in your Unity Editor.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. Connect your iOS device to your computer using a USB cable. Accept the **Trust This Computer** alert if it appears.
1. Open the project that you want to deploy with your Unity Editor and go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window.
1. In the **Unity Remote** section, select **Device** > **Any iOS Device**.
1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
1. In your Unity Editor, Select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. Open the Scene that starts both your [client-workers]({{urlRoot}}/reference/glossary#client-worker) and [server-workers]({{urlRoot}}/reference/glossary#server-worker). In the FPS Starter Project this is `FPS-Development.scene`.
1. In the Editor’s Game view, select **Play**.

    > **TIP:** You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

1. You should now see your Unity Editor Game view mirrored on your iOS device.
1. When you're done:<br>
	* Select **Play** to stop your client.
	* In the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.

## iOS Simulator{#ios-simulator}

**Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS simulator.

1. Open your project in your Unity Editor.
1. In your Unity Editor, select **File** > **Build Settings**, and ensure that **iOS** is selected. (The Unity logo next to a platform name indicates it's selected.)<br>
If **iOS** is not selected, select it and then select **Switch Platform**.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Simulator SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. In your Unity Editor, select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In your file manager, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.<br>
	Open it in Xcode.
1. Still in XCode, select the **Play** button in the top left of the window.
1. Once the game is deployed and started on the Simulator, you see an empty text field and a **Connect** button: Select **Connect**.<br>
Note: You don’t need to enter anything in the text field.
1. Play the game on the Simulator.
1. When you're done:<br>
	* Exit the Simulator
	* In XCode, select the **Play** button to stop your the game running.<br>
	* In the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.

## iOS device{#ios-device}

1. Make sure your computer and your mobile device are both connected to the same network.
1. Connect the mobile device to your computer using a USB cable.
1. Open your project in your Unity Editor.
1. In your Unity Editor, select **File** > **Build Settings**, and ensure that **iOS** is selected. (The Unity logo next to a platform name indicates it's selected.)<br>
If **iOS** is not selected, select it and then select **Switch Platform**.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Device SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. In your Unity Editor, select **Assets** > **Fps** > **Prefabs** > **iOSClientWorker**.
1. In the Inspector pane, enter your local IP address into the **IOS Worker Connector (Script)** > **Forced Ip Address** field.
1. Still in the Inspector pane, ensure that the **should connect locally** checkbox is checked.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. You need to know the local IP address of your computer to connect. [This page](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address) (on the Lifehacker website)  describes how you can find your external and local IP address.
1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch --runtime_ip=<your-local-ip>`. (Where `<your-local-ip>` is the IP address you just located.)<br>
You cannot use **SpatialOS** > **Local launch** in your Unity Editor as you would normally, because you need to specify the runtime IP.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In Finder, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.<br>
	Open it in Xcode.
1. In XCode, in the Navigation Area, select the **Project root**.
1. Still in XCode, now in the Editor Area, go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
1. Still in the Editor Area, select **General** > **Signing** and sign the project.<br>
	For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Still in XCode, select the Play button in the top left of the window. This will install the game as an app on your device and start it.
1. Once the game is running on your device, you see an empty text field and a **Connect** button: enter the local IP address of your computer in the text field and select **Connect**.
1. Play the game on your mobile device.
1. When you're done:<br>
	* Exit the app on your device.
	* In XCode, select the **Play** button to stop your the game running.
	* In the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.
