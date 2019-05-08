<%(TOC)%>

# Connect to a local deployment

Before reading this document, make sure you are familiar with:

* [Setting up Android support for the GDK]({{urlRoot}}/reference/mobile/android/setup)
* [Ways to test your mobile client]({{urlRoot}}/reference/mobile/run-client)

## Start a local deployment

Before connecting your Android client-worker you need to start a local deployment.<br/>
In your Unity Editor, select **SpatialOS** > **Local launch**.<br>
It’s done when you see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`.

## Unity Editor or Unity Remote{#in-editor}


1. In your Unity Editor, open the scene that contains your mobile client worker and your server-workers.
1. Navigate to your mobile client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is checked in the script’s drop-down window of the Inspector window.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your Android device that is connected to your development machine.
1. Click the Play button.

## Android emulator or device{#android-device}

1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds) or connect your Android device to your development machine.

    > If you start an emulator, ensure you choose the same CPU architecture for your virtual machine as your development computer. Using a different architecture might affect the performance of your emulator.

1. In your Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your mobile worker, and wait for the build to complete.
1. In your Unity Editor, navigate to your server-worker Scene and start it via the Editor.
1. Select **SpatialOS** > **Launch mobile client** > **Android for local** to start your Android client.<br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Play the game on your device or emulator.

    > If you have followed the setup guide and have built out your mobile client-worker at least once, you can launch your application for either local or cloud deployments without having to re-build it.