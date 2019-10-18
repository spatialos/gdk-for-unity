<%(TOC)%>

# Connect to a local deployment

<%(Callout message="
Before starting with mobile development, make sure you have read:

* [Setting up Android support for the GDK]({{urlRoot}}/modules/mobile/setup-android)
* [Setting up iOS support for the GDK]({{urlRoot}}/modules/mobile/setup-ios)
")%>

## Prepare your project to connect to a local deployment{#prepare-deployment}

To connect your mobile device to a local deployment, you need to configure the Runtime IP parameter. This ensures that your mobile device is able to connect to local deployments running on your machine.

1. Open your project in your Unity Editor.
1. Select **SpatialOS** > **Mobile Launcher** to open the launcher window.
1. In the **Local runtime IP** field, enter your development machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Check the **Connect locally** option.

## Start a local deployment{#start-deployment}

Before connecting your mobile client-worker you need to start a local deployment. In your Unity Editor, select **SpatialOS** > **Local launch**. Your deployment is ready when you see the following message in the terminal:

```text
SpatialOS ready. Access the Inspector at http://localhost:21000/inspector.
```

## Choose how to run your mobile client-worker

See [Ways to run your client]({{urlRoot}}/modules/mobile/run-client) for more information.

### Unity Editor or Unity Remote{#in-editor}

1. In your Unity Editor, open the Scene that contains your mobile client worker and your server-workers.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your mobile device that is connected to your development machine.
1. Click the Play button.

### Android emulator or device{#android-device}

1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds) or connect your Android device to your development machine.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your mobile worker, and wait for the build to complete.
1. Navigate to your server-worker Scene and start it via the Editor.
1. Select **SpatialOS** > **Mobile Launcher** to open the Mobile Launcher window.
1. Select your device or emulator from the relevant dropdown.
   * If your device or emulator does not show up, you can select the button to the right of the dropdown to refresh the list.
1. Select **Launch app on Android Emulator** or **Launch app on Android Device**.
1. Play the game on your emulator or device.

> As soon as you have built your Android app once, you are able to launch your app for either local or cloud deployments.

### iOS Simulator or iOS device{#ios-device}

> **Note:** You cannot run the [First Person Shooter (FPS) Starter Project]({{urlRoot}}/projects/fps/overview) on the iOS Simulator. This is due to an incompatibility between the [Metal Graphics API](https://developer.apple.com/metal/) used by the project and the iOS Simulator.

1. Ensure you have set a valid **Bundle Identifier** for your project. (This can be found in the Player Settings.)
1. In your Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your mobile worker, and wait for the build to complete.
1. Navigate to your server-worker Scene and start it via the Editor.
1. Select **SpatialOS** > **Mobile Launcher** to open the Mobile Launcher window.
1. Fill in your **Development Team ID**. (There are instructions on where to find it on this [Stackoverflow topic](https://stackoverflow.com/a/47732584).)
1. Select **Build XCode project** and wait for the build to complete.
1. Select your Simulator or device from the relevant dropdown.
   * If your Simulator or device does not show up, you can select the button to the right of the dropdown to refresh the list.
1. Select **Launch iOS app on Simulator** or **Launch iOS app on Device**.
   * Ensure the app isn't already running on the device, as this will cause the app to crash.
   * You may need to mark your Developer account as trusted on the target device.
1. Play the game on Simulator or your device.

> As soon as you have built your iOS app once, you are able to launch your app for either local or cloud deployments.
