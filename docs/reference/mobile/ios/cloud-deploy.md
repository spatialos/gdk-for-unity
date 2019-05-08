<%(TOC)%>

# Connect to a cloud deployment

<%(Callout message="
Before reading this document, make sure you have read:

* [Setting up iOS Support for the GDK]({{urlRoot}}/reference/mobile/ios/setup)
* [Development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
")%>


## Start a cloud deployment

1. Go to **SpatialOS** > **Build for cloud** > **All workers** to build all your workers. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Upload your workers and start the cloud deployment by running the following commands in the terminal:
```
spatial cloud upload <assembly name>
spatial cloud launch --tags=dev_login --snapshot==<path to your snapshot> <assembly name> <path to your launch configuration> <deployment name>
```
    > The tag `dev_login` is necessary to be able to use the development authentication flow.

## Unity Editor or Unity Remote

1. In your Unity Editor, open the scene that contains your mobile client-worker.
1. Navigate to your mobile client-worker GameObject and ensure the **Should Connect Locally** checkbox is **not** checked in the script’s drop-down window of the Inspector window.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your iOS device that is connected to your development machine.
1. Click the Play button.

## iOS Simulator or iOS deivce

> **Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS simulator.

1. Open your project in your Unity Editor.
1. In your Unity Editor, go to your mobile client game object and ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your mobile client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. In Finder, navigate to `/workers/unity/build/worker/` and locate the `.xcodeproj` that corresponds to your iOS client-worker, it may be in a sub-folder.
1. Open the project in XCode
1. If you want to run it on a physical device, you need to follow these additional steps:
    1. Connect your device to your development machine
    1. Go to **Build Settings** > **Packaging** > **Project Bundle Identifier** and input a unique string.
    1. Still in the Editor Area, select **General** > **Signing** and sign the project.<br>
    For more information, see [Code signing and provisioning [Apple Documentation]](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
1. Still in XCode, select the **Play** button in the top left of the window.
1. Play the game on your iOS device or Simulator.
