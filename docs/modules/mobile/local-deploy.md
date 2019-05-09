<%(TOC)%>

# Connect to a local deployment

<%(Callout message="
Before starting with mobile development, make sure you have read:

* [Setting up Android support for the GDK]({{urlRoot}}/modules/mobile/setup-android)
* [Setting up iOS Support for the GDK]({{urlRoot}}/modules/mobile/setup-ios)
")%>


## Prepare your project to connect to a local deployment

To connect your mobile device to a local deployment, you need to configure the Runtime IP parameter:

1. Open your project in your Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Runtime IP for local deployment** field, enter your local machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Select **Save** and close the window.

## Start a local deployment

Before connecting your mobile client-worker you need to start a local deployment. In your Unity Editor, select **SpatialOS** > **Local launch**. Your deployment is ready when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.

## Choose how to run your mobile client-worker

See [How to run your mobile client-worker]({{urlRoot}}/modules/mobile/run-client) for more information.

### Unity Editor or Unity Remote{#in-editor}

1. In your Unity Editor, open the Scene that contains your mobile client worker and your server-workers.
1. Navigate to your mobile client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is checked in the script’s drop-down window of the Inspector window.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your mobile device that is connected to your development machine.
1. Click the Play button.

### Android emulator or device{#android-device}

1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds) or connect your Android device to your development machine.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your mobile worker, and wait for the build to complete.
1. Navigate to your server-worker Scene and start it via the Editor.
1. Select **SpatialOS** > **Launch mobile client** > **Android for local** to start your Android client.
1. Play the game on your device or emulator.

### iOS Simulator or iOS device

> **Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS simulator.

1. In your Unity Editor, go to your mobile client game object
    * Enter your local IP address in the **IP Address** field.
    * Ensure that the **Should Connect Locally** checkbox is checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your mobile worker, and wait for the build to complete.
1. In Finder, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.
1. Open the project in XCode.
1. If you want to run it on a physical device, you need to follow these additional steps:
    * Connect your device to your development machine.
    * Go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
    * Still in the Editor Area, select **General** > **Signing** and sign the project. For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Still in XCode, select the Play button in the top left of the window. This builds and install the game on your device or Simulator.
1. Play the game on your iOS device or Simulator.