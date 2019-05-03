<%(TOC)%>

# Connect to a cloud deployment

Before reading this document, make sure you are familiar with:

* [Setting up iOS Support for the GDK]({{urlRoot}}/reference/mobile/ios/setup)
* [Ways to test your iOS client]({{urlRoot}}/reference/mobile/ios/run-client)
* [Development Authentication Flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
* [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/reference/workflows/monobehaviour/creating-workers)

## Prepare your project to connect to a cloud deployment {#prepare}

To connect your iOS device to a cloud deployment, you need a mobile connector script.

**Note:** If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip the **Create a mobile connector script** section below, as you already have one in your project.

<%(#Expandable title="Create a mobile connector script")%>

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you also need to create and add a MonoBehaviour script to your iOS client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`MobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) and include the functionality you want. You can base your implementation on [the one](https://github.com/spatialos/gdk-for-unity-blank-project/blob/master/workers/unity/Assets/Scripts/Workers/iOSClientWorkerConnector.cs) in our Blank Starter Project.
1. In your Unity Editor, add the MonoBehaviour script to your iOS client-worker GameObject.
1. In your Unity Editor, navigate to your iOS client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is **not** checked in the script's drop-down window of the Inspector window.

<%(/Expandable)%>

## iOS Simulator{#ios-simulator}

**Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS simulator.

To connect your mobile application to a cloud deployment, you need to authenticate against our services. This guide describes how to authenticate using the development authentication flow which we provide for early stages in development.

Alternatively, if you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk).

1. Open your project in your Unity Editor.
1. In your Unity Editor, select **File** > **Build Settings**, and ensure that **iOS** is selected. (The Unity logo next to a platform name indicates it's selected.)<br>
If **iOS** is not selected, select it and then select **Switch Platform**.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Simulator SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. Build your workers from the SpatialOS menu by selecting **Build for cloud** > **All workers**.
1. Upload your server-workers.<br>
	To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/<your snapshot> <assembly name> <launch configuration>.json <deployment name>`.
1. In your [SpatialOS Console](https://console.improbable.io), tag your cloud deployment with the tag `dev_login`. <br/>
To do this:
  *  In your SpatialOS Console, select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance).<br>
Be sure to note down the `id` that is output when you create this, you will need it in a moment.
1. In your Unity Editor, locate the mobile connector script which inherits from the [`MobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector).<br>
If you're using the FPS Starter Project, you can locate this script in `Assets/FPS/Prefabs/iOSClientWorker`.<br>
If you added the GDK to an existing project, then you created this script in the **Create a mobile connector script** section [above](#prepare).<br>
1. Still in your Unity Editor, in the Inspector, in the `iOS Worker Connector` section, there is a **Development Auth Token** field.<br>
Enter the `id` that you noted down earlier.
1. In the same drop-down window, ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your iOS client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. In your file manager, from the root of your SpatialOS project, navigate to `/workers/unity/build/worker/iOSClient@iOS`.<br>
Locate the generated XCode project file (ending in `.xcodeproj`) that corresponds to your iOS client-worker.<br>
1. Open the `.xcodeproj` file in Xcode.
1. Still in XCode, select the **Play** button in the top left of the window.
1. Once the game is deployed and started on the Simulator, you see an empty text field and a **Connect** button: Select **Connect**.<br>
Note: You don’t need to enter anything in the text field.
1. Play the game on the Simulator.
1. When you're done, exit the Simulator:<br>
	* In XCode, select the **Play** button to stop your the game running.<br>
	* In your SpatialOS Console, select **Stop** to halt your cloud deployment.

## iOS device{#ios-device}

To connect your mobile application to a cloud deployment, you need to authenticate against our services. This guide describes how to authenticate using the development authentication flow which we provide for early stages in development.

Alternatively, if you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk).

1. Make sure your computer and your mobile device are both connected to the same network.
1. Connect the mobile device to your computer using a USB cable.
1. Open your project in your Unity Editor.
1. In your Unity Editor, select **File** > **Build Settings**, and ensure that **iOS** is selected. (The Unity logo next to a platform name indicates it's selected.)<br>
If **iOS** is not selected, select it and then select **Switch Platform**.
1. In your Unity Editor, select **Edit** > **Project Settings** > **Player**. This opens the **Project Settings** window.
1. In the Inspector window, select **Settings for iOS (the iPhone icon)** > **Other Settings**.
1. In the **Configuration** section of the Inspector window, locate **Target SDK** and select **Device SDK**.
1. Still in the **Configuration** section, locate **Target minimum iOS version** and input `10.0`.
1. Build your workers from the SpatialOS menu by selecting **Build for cloud** > **All workers**.
1. Upload your server-workers.<br>
	To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/<your snapshot> <assembly name> <launch configuration>.json <deployment name>`.
1. In your [SpatialOS Console](https://console.improbable.io), tag your cloud deployment with the tag `dev_login`. <br/>
To do this:
  *  In your SpatialOS Console, select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance).<br>
Be sure to note down the `id` that is output when you create this, you will need it in a moment.
1. In your Unity Editor, locate the mobile connector script which inherits from the [`MobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector).<br>
If you're using the FPS Starter Project, you can locate this script in `Assets/FPS/Prefabs/iOSClientWorker`.<br>
If you added the GDK to an existing project, then you created this script in the **Create a mobile connector script** section [above](#prepare).<br>
1. Still in your Unity Editor, in the Inspector, in the `iOS Worker Connector` section, there is a **Development Auth Token** field.<br>
Enter the `id` that you noted down earlier.
1. In the same drop-down window, ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your iOS client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. In your file manager, from the root of your SpatialOS project, navigate to `/workers/unity/build/worker/iOSClient@iOS`.<br>
Locate the generated XCode project file (ending in `.xcodeproj`) that corresponds to your iOS client-worker.<br>
1. Open the `.xcodeproj` file in Xcode.
1. In XCode, in the Navigation Area, select the **Project root**.
1. Still in XCode, now in the Editor Area, go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
1. Still in the Editor Area, select **General** > **Signing** and sign the project.<br>
	For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Still in XCode, select the **Play** button in the top left of the window.
1. Once the game is deployed and started on your device, you see an empty text field and a **Connect** button: Select **Connect**.<br>
Note: You don’t need to enter anything in the text field.
1. Play the game on the Simulator.
1. When you're done:<br>
	* Exit the app on your device.
	* In XCode, select the **Play** button to stop your the game running.
	* In your SpatialOS Console, select **Stop** to halt your cloud deployment.
