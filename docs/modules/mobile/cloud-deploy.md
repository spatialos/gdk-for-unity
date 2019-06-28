<%(TOC)%>

# Connect to a cloud deployment

<%(Callout message="
Before reading this document, make sure you have read:

* [Setting up Android support for the GDK]({{urlRoot}}/modules/mobile/setup-android)
* [Setting up iOS support for the GDK]({{urlRoot}}/modules/mobile/setup-ios)
* [Development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
")%>

To connect your mobile device to a cloud deployment, you need to authenticate with our services.

This guide describes how to authenticate using the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication) which we provide for the early stages of game development.

## Prepare your project to connect to a cloud deployment{#prepare-deployment}

To connect your mobile device to a cloud deployment:

1. Open your project in your Unity Editor.
1. Select **SpatialOS** > **Mobile Launcher** to open the launcher window.
1. Uncheck the **Connect locally** option.

## Choose how to run your mobile client-worker

See [this page]({{urlRoot}}/modules/mobile/run-client) for more information on the different ways to run your client.

### Unity Editor or Unity Remote{#in-editor}

1. In your Unity Editor, open the scene that contains your mobile client-worker.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your Android device that is connected to your development machine.
1. Click the Play button.

### Android device or Android emulator{#android-device}

1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds) or connect your Android device to your development machine.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your mobile worker, and wait for the build to complete.
1. Select **SpatialOS** > **Mobile Launcher** to open the Mobile Launcher window.
1. Select **Launch Android app** to start your Android client.
1. Play the game on your device or emulator.

> As soon as you have built your Android app once, you are able to launch your app for either local or cloud deployments.

### iOS Simulator or iOS device{#ios-device}

> **Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS Simulator.

1. Ensure you have set a valid **Bundle Identifier** for your project. (This can be found in the Player Settings.)
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your mobile worker, and wait for the build to complete.
1. Navigate to your server-worker Scene and start it via the Editor.
1. Select **SpatialOS** > **Mobile Launcher** to open the Mobile Launcher window.
1. Fill in your **Development Team ID**. (There are instructions on where to find it on this [Stackoverflow topic](https://stackoverflow.com/a/47732584).)
1. Select **Build XCode project** and wait for the build to complete.
1. Select your device or simulator from the relevant dropdown.
   * If your device or simulator does not show up, you can select the button to the right of the dropdown to refresh the list.
1. Select the **Launch iOS app on Simulator** or **Launch iOS app on Device**
   * Ensure the app isn't already running on the device, as this will cause the app to crash.
   * You may need to mark your Developer account as trusted on the target device.
1. Play the game on your device or Simulator.
