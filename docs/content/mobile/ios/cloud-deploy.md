# Connect to a cloud deployment

Before reading this document, make sure you are familiar with:

* [Setting up iOS Support for the GDK]({{urlRoot}}/content/mobile/ios/setup)
* [Ways to test your iOS client]({{urlRoot}}/content/mobile/ios/ways-to-test)
* [Development Authentication Flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
* [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/content/gameobject/creating-workers-with-workerconnector)

To connect your mobile application to a cloud deployment, you need to authenticate against our services.
This guide describes how to authenticate using the development authentication flow which we provide for early stages in development.
Alternatively, if you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk).

## iOS Simulator{#ios-simulator}

1. Open your project in your Unity Editor.
1. In your Unity Editor, select **File** > **Build Settings**, and ensure that **iOS** is selected. Selection is indicated by a Unity logo that appears next to the name of the selected platform.<br>
If **iOS** is not selected, select it and then select **Switch Platform**.
1. In the Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens **PlayerSettings** in the Inspector window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Simulator SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. Build your workers from the SpatialOS menu by selecting **Build for cloud** > **All workers**.
1. Upload your server-workers.<br>
	To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/default.snapshot <assembly name> <launch configuration>.json <deployment name>`.
1. In your [SpatialOS Console](https://console.improbable.io), tag your cloud deployment with the tag `dev_login`. <br/>
To do this:
  *  In your SpatialOS Console, select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token (SpatialOS documentation).](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance)
1. Create a MonoBehaviour script which inherits from the [`MobileWorkerConnector`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.mobile/Worker/MobileWorkerConnector.cs) and includes the functionality you want. In your Unity Editor, add this script it to your iOS client-worker GameObject.
1. The `MobileWorkerConnector` provides a `DevelopmentAuthToken` field. Still in your Unity Editor, make sure your iOS client-worker GameObject is selected and in the Inspector, locate the script you just added to it. 
1. In the Inspector, in the script’s drop-down window, there is a field to add the authentication token that you created. 
1. In the same drop-down window, ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your iOS client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Select **SpatialOS** > **Launch mobile client** > **iOS Device**.
1. In your file manager, navigate to `/workers/unity/build/worker/` and locate the open the generated XCode project file (ending in `.xcodeproj`) that corresponds to your iOS client-worker. (The XCode file has a similar name to your client-worker and may be in a sub-folder.)<br>
	1. Open the `.xcodeproj` file in Xcode.
1. Still in XCode, select the **Play** button in the top left of the window.
1. Once the game is deployed and started on the Simulator, you see an empty text field and a **Connect** button: Select **Connect**.<br>
Note: You don’t need to enter anything in the text field.
1. Play the game on the Simulator.
1. When you're done, exit the Simulator:<br>
	* In XCode, select the **Play** button to stop your the game running.<br>
	* In your SpatialOS Console, select **Stop** to halt your cloud deployment.

## iOS device{#ios-device}

1. Make sure your computer and your mobile device are both connected to the same network.
1. Connect the mobile device to your computer using a USB cable.
1. Open your project in your Unity Editor.
1. In your Unity Editor, select **File** > **Build Settings**, and ensure that **iOS** is selected. Selection is indicated by a Unity logo that appears next to the name of the selected platform.<br>
If **iOS** is not selected, select it and then select **Switch Platform**.
1. In the Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens **PlayerSettings** in the Inspector window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Device SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. Build your workers from the SpatialOS menu by selecting **Build for cloud** > **All workers**.
1. Upload your server-workers.<br>
	To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/default.snapshot <assembly name> <launch configuration>.json <deployment name>`.
1. In your [SpatialOS Console](https://console.improbable.io), tag your cloud deployment with the tag `dev_login`. <br/>
To do this:
  *  In your SpatialOS Console, select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token (SpatialOS documentation).](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance)
1. Create a MonoBehaviour script which inherits from the [`MobileWorkerConnector`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.mobile/Worker/MobileWorkerConnector.cs) and includes the functionality you want. In your Unity Editor, add this script it to your iOS client-worker GameObject.
1. The `MobileWorkerConnector` provides a `DevelopmentAuthToken` field. Still in your Unity Editor, make sure your iOS client-worker GameObject is selected and in the Inspector, locate the script you just added to it. 
1. In the Inspector, in the script’s drop-down window, there is a field to add the authentication token that you created. 
1. In the same drop-down window, ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your iOS client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Select **SpatialOS** > **Launch mobile client** > **iOS Device**.
1. In Finder, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.<br>
	Open it in Xcode.
1. In XCode, in the Navigation Area, select the **Project root**.
1. Still in XCode, now in the Editor Area, go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
1. Still in the Editor Area, select **General** > **Signing** and sign the project.<br>
	For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Still in XCode, select the **Play** button in the top left of the window.
1. Once the game is deployed and started on your device, you see an empty text field and a **Connect** button: Select **Connect**.<br>
Note: You don’t need to enter anything in the text field.
1. Play the game on the Simulator.
1. When you're done, exit the app on your device.<br>
	In XCode, select the **Play** button to stop your the game running.<br>
	In your SpatialOS Console, select **Stop** to halt your cloud deployment.
