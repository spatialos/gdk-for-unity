<%(TOC)%>

# Connect to a local deployment

<%(Callout message="
Before reading this document, make sure you have read:

* [Setting up iOS Support for the GDK]({{urlRoot}}/reference/mobile/ios/setup)
")%>

## Start a local deployment

Before connecting your Android client-worker you need to start a local deployment.<br/>
In your Unity Editor, select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.

## Unity Editor or Unity Remote


1. In your Unity Editor, open the scene that contains your mobile client worker and your server-workers.
1. Navigate to your mobile client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is checked in the script’s drop-down window of the Inspector window.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your iOS device that is connected to your development machine.
1. Click the Play button.

## iOS Simulator or iOS device

> **Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS simulator.

1. Open your project in your Unity Editor.
1. In your Unity Editor, go to your mobile client game object
    * Enter your local IP address in the **IP Address** field.
    * Ensure that the **Should Connect Locally** checkbox is checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your mobile worker, and wait for the build to complete.<br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. In Finder, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.
1. Open the project in XCode.
1. If you want to run it on a physical device, you need to follow these additional steps:
    1. Connect your device to your development machine
    1. Go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
    1. Still in the Editor Area, select **General** > **Signing** and sign the project.<br>
    For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Still in XCode, select the Play button in the top left of the window. This builds and install the game on your device or simulator.
1. Play the game on your iOS device or Simulator.
