# Connect to a local deployment

This page explains how to connect an iOS client to a local deployment of SpatialOS. You can do this from inside your [Unity Editor](#in-editor), using the [Unity Remote](#unity-remote) app, using the [iOS Simulator](#ios-simulator) or using your own [iOS device](#ios-device).

Before reading this document, make sure you are familiar with:

  * [Setting up iOS Support for the GDK]({{urlRoot}}/content/mobile/ios/setup)
  * [Ways to test your iOS client]({{urlRoot}}/content/mobile/ios/ways-to-test)

## Unity Editor{#in-editor}
1. In the Unity Editor, Select **SpatialOS** > **Local launch**. It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In your Unity Editor, open the `FPS-Development` Scene and select the Play button.<br/>

## Unity Remote{#unity-remote}
You need the Unity Remote app installed on your iOS device in order to follow steps below. See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for details.

1. Open your project in the Unity Editor.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. Connect your iOS device to your computer using a USB cable. Accept the **Trust This Computer** alert if it appears.
1. Open the project that you want to deploy with the Unity Editor and go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window.
1. In the **Unity Remote** section, select **Device** > **Any iOS Device**.
1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
1. In the Unity Editor, Select **SpatialOS** > **Local launch**. It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. Open the scene that starts both your [client-workers]({{urlRoot}}/content/glossary#client-worker) and [server-workers]({{urlRoot}}/content/glossary#server-worker). In the FPS Starter Project this is `FPS-Development.scene`.
1. In the Editor’s Game view, select **Play**.

    > You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

1. You should now see your Unity Editor game view mirrored on your iOS device.
1. When you're done, select **Play** to stop your client and input **Ctrl-C** in the terminal that's running the SpatialOS process to stop it.

## iOS Simulator{#ios-simulator}

1. Open your project in the Unity Editor.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. In the Unity Editor, Select **SpatialOS** > **Local launch**. It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In the Unity Editor, navigate to **Edit** > **Project Settings** > **Player**. This should open **PlayerSettings** in the Inspector window.
1. In the Inspector window, navigate to **Settings for iOS (the iPhone icon)** > **Other Settings** > **Configuration** > **Target SDK** and choose **Simulator SDK**.
1. Also in the **Configuration** section of the Inspector window, select **Targer minimum iOS version** and input `10.0`.
1. In the Unity Editor, navigate to **File** > **Build Settings**, and ensure that iOS is selected. Selection is indicated by a Unity logo that appears next to the name of the selected platform. If iOS is not selected, select it and then select **Switch Platform**.
1. Select **Build and Run**. This prompts you to choose where to save the XCode project that Unity generates. After you've selected the directory, Unity generates the XCode project, opens it in XCode and starts the build. If the build succeeds, XCode starts a Simulator and installs the game on it.
  * If you choose **Build**, instead of **Build and Run**, Unity generates a XCode project and opens the folder containing the project.
1. Once the game is deployed and started on the Simulator, you see an empty text field and a **Connect** button: Select **Connect**.

    > You don’t need to enter anything in the text field.

1. Play the game on the Simulator.
1. When you're done, select **Play** to stop your client and input **Ctrl-C** in the terminal that's running the SpatialOS process to stop it.

## iOS device{#ios-device}

1. Set up [Code signing and provisioning](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Make sure your computer and your mobile device are both connected to the same wireless network.
1. Connect the mobile device to your computer using a USB cable.
1. Build your workers from the SpatialOS menu by selecting **Build for local** > **All workers**.
1. You need to know the local IP address of your computer to connect. [This page](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address) (on the Lifehacker website)  describes how you can find your external and local IP address.
1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch --runtime_ip=<your-local-ip>`. (Where `<your-local-ip>` is the IP address you just located.) You cannot use **SpatialOS** > **Local launch** in your Unity Editor as you would normally, because you need to specify the runtime IP. It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.
1. In the Unity Editor, navigate to **Edit** > **Project Settings** > **Player**. This should open **PlayerSettings** in the Inspector window.
1. In the Inspector window, navigate to **Settings for iOS (the iPhone icon)** > **Other Settings** > **Configuration** > **Target SDK** and choose **Device SDK**.
1. Also in the **Configuration** section of the Inspector window, select **Targer minimum iOS version** and input `10.0`.
1. In the Unity Editor, navigate to **File** > **Build Settings**, and ensure that iOS is selected. Selection is indicated by a Unity logo that appears next to the name of the selected platform. If iOS is not selected, select it and then select **Switch Platform**.
1. Select **Build**. This prompts you to choose where to save the XCode project that Unity generates. Select a directory and Unity generates the XCode project.
1. After the build has finished, Unity opens the folder containing the project. Open the project in XCode, select the Project root, go to **General** > **Signing** and sign the project.

    > If you choose **Build and Run** instead of **Build** Unity generates the XCode project, automatically opens it for you and starts the build to install the game on the connected device. This will most likely fail, because you need to first sign the application as described in the previous step.
    >
    > For subsequent runs you will be prompted to pick XCode project directory again (with the one used previously preselected). You can generate a new project or append/replace existing one. In subsequent runs, if you've set up provisioning and use Append, you can use **Build and Run** to trigger project run automatically after XCode project was generated.

1. Once the game is running on your device, you see an empty text field and a **Connect** button: enter the local IP address of your computer in the text field and select **Connect**.
1. Play the game on your mobile device.
