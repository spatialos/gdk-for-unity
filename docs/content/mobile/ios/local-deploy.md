# Connect to a local deployment

This page describes how to connect an iOS client to a local deployment of SpatialOS. You can do this using either; just your[Unity Editor](#in-editor), the [Unity Remote](#unity-remote) app, the [iOS Simulator](#ios-simulator), or your own [iOS device](#ios-device).

Before reading this document, make sure you are familiar with:

  * [Setting up iOS Support for the GDK]({{urlRoot}}/content/mobile/ios/setup)
  * [Ways to test your iOS client]({{urlRoot}}/content/mobile/ios/ways-to-test)

## Unity Editor{#in-editor}
1. In the Unity Editor, select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In your Unity Editor, open the `FPS-Development` Scene and select the Play button.<br/>

## Unity Remote{#unity-remote}
You need the Unity Remote app installed on your iOS device in order to follow steps below. See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for details.

1. Open your project in the Unity Editor.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. Connect your iOS device to your computer using a USB cable. Accept the **Trust This Computer** alert if it appears.
1. Open the project that you want to deploy with the Unity Editor and go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window.
1. In the **Unity Remote** section, select **Device** > **Any iOS Device**.
1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
1. In the Unity Editor, Select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. Open the Scene that starts both your [client-workers]({{urlRoot}}/content/glossary#client-worker) and [server-workers]({{urlRoot}}/content/glossary#server-worker). In the FPS Starter Project this is `FPS-Development.scene`.
1. In the Editor’s Game view, select **Play**.

    > **TIP:** You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

1. You should now see your Unity Editor Game view mirrored on your iOS device.
1. When you're done, select **Play** to stop your client and, in the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.

## iOS Simulator{#ios-simulator}

1. Open your project in the Unity Editor.
1. In the Unity Editor, select **File** > **Build Settings**, and ensure that iOS is selected. Selection is indicated by a Unity logo that appears next to the name of the selected platform.<br>
If iOS is not selected, select it and then select **Switch Platform**.
1. In the Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens **PlayerSettings** in the Inspector window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Simulator SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. In the Unity Editor, select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In Finder, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.<br>
	Open it in Xcode.
1. Still in Xcode, click the Play button in the top left of the window.
1. Once the game is deployed and started on the Simulator, you see an empty text field and a **Connect** button: Select **Connect**.<br>
Note: You don’t need to enter anything in the text field.
1. Play the game on the Simulator.
1. When you're done, select **Play** to stop your client and, in the terminal window that's running the SpatialOS process, enter **Ctrl+C** to stop the process.

## iOS device{#ios-device}

1. Make sure your computer and your mobile device are both connected to the same network.
1. Connect the mobile device to your computer using a USB cable.
1. In your Unity Editor, build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. You need to know the local IP address of your computer to connect. [This page](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address) (on the Lifehacker website)  describes how you can find your external and local IP address.
1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch --runtime_ip=<your-local-ip>`. (Where `<your-local-ip>` is the IP address you just located.)<br>
You cannot use **SpatialOS** > **Local launch** in your Unity Editor as you would normally, because you need to specify the runtime IP.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.

1. In the Unity Editor, navigate to **Edit** > **Project Settings** > **Player**. This opens **PlayerSettings** in the Inspector window.
1. In the Inspector window, navigate to **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Device SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. In the Unity Editor, navigate to **File** > **Build Settings**, and ensure that **iOS** is selected. Selection is indicated by a Unity logo that appears next to the name of the selected platform.<br>
If iOS is not selected, select it and then select **Switch Platform**.
1. Select **Build**. This prompts you to choose where to save the XCode project that Unity generates. Select a directory and Unity generates the XCode project.
1. After the build has finished, Unity opens the folder containing the project in Finder. You must then open the project directory and then open the `.xcodeproj` file in XCode.
1. In XCode, in the Navigation Area, select the **Project root**.
1. Still in Xcode, now in the Editor Area, go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
1. Still in the Editor Area, select **General** > **Signing** and sign the project.<br>
	For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).

    > **TIP:**  If you choose **Build and Run** instead of **Build** Unity generates the XCode project, automatically opens it for you and starts the build to install the game on the connected device. This will most likely fail upon your first attempt, because you need to sign the application as described above prior to running it.
    >
    > For subsequent runs you will be prompted to pick an XCode project directory again (with the one you chose previously pre-selected). You can either generate a new project, append or replace an existing one. In subsequent runs, if you've set up provisioning and choose to append an existing project, you can use **Build and Run** to trigger a project run automatically once the XCode project has been generated.

1. Still in Xcode, click the Play button in the top left of the window.
1. Once the game is running on your device, you see an empty text field and a **Connect** button: enter the local IP address of your computer in the text field and select **Connect**.
1. Play the game on your mobile device.
