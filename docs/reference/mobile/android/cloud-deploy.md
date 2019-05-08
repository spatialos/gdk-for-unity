<%(TOC)%>

# Connect to a cloud deployment

<%(Callout message="
Before reading this document, make sure you have read:

* [Setting up Android Support for the GDK]({{urlRoot}}/reference/mobile/android/setup)
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

## Unity Editor or Unity Remote{#in-editor}

1. In your Unity Editor, open the scene that contains your mobile client-worker.
1. Navigate to your mobile client-worker GameObject and ensure the `ShouldConnectLocally` checkbox is **not** checked in the script’s drop-down window of the Inspector window.
1. (Optional) If you want to use Unity Remote, open the Unity Remote app on your Android device that is connected to your development machine.
1. Click the Play button.

## Android device or Android emulator

1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds) or connect your Android device to your development machine.

    > If you start an emulator, ensure you choose the same CPU architecture for your virtual machine as your development computer. Using a different architecture might affect the performance of your emulator.


1. Select **SpatialOS** > **Launch mobile client** > **Android for cloud**.<br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Play the game on your Android device or emulator.

    > Once you have built your Android client-worker once, you can launch your application for either local or cloud deployments without having to re-build it.
