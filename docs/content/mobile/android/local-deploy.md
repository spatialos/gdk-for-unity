# Connect to a local deployment

Before reading this document, make sure you are familiar with:

* [Setting up Android Support for the GDK]({{urlRoot}}/content/mobile/android/setup)
* [Ways to test your Android client]({{urlRoot}}/content/mobile/android/ways-to-test)

## Prepare your project to connect to a local deployment

To connect your Android device to a local deployment, you need to prepare; follow the steps below to ensure any local deployment you launch via the Unity Editor is correctly set up.

1. Open your project in the Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Runtime IP for local deployment** field, enter your local machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Select **Save** and close the window.

## Use Unity Remote

You need the Unity Remote app for this. See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for details.

1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. In the Unity Editor, open the project that you want to deploy and go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window.
1. Open the Scene that starts both your [client-]({{urlRoot}}/content/glossary#client-worker) and [server-workers]({{urlRoot}}/content/glossary#server-worker).
1. In **Editor Settings** window's **Unity Remote** section, set the **Device** setting to **Any Android Device**.
1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
1. In the Unity Editor, select **SpatialOS** > **Local launch** to start your local deployment.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Unity Editor’s Game view, select **Play**.

    > You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

1. You should now see your Unity Editor's game view mirrored on your Android device.

## Use your Android emulator

1. [Start your Android Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds).

    > Ensure you choose the same CPU architecture for your virtual machine as your development computer. If you don’t, you will get warning messages as mismatched CPU architecture affects performance.
1. [Set up your local deployment](#prepare-your-project-to-connect-to-a-local-deployment).
1. [Build your server-workers]({{urlRoot}}/content/build).
1. In the Unity Editor, select **SpatialOS** > **Local launch** to start your local deployment

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. Still in the Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your Android worker, and wait for the build to complete.
1. Select **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on the Emulator.

## Use your Android device

1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Make sure your computer and your mobile device are both connected to the same wireless network.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. [Build your server-workers]({{urlRoot}}/content/build).
1. [Set up your local deployment](#prepare-your-project-to-connect-to-a-local-deployment).
1. In the Unity Editor, select **SpatialOS** > **Local launch** to start your local deployment.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. Still in the Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your Android worker, and wait for the build to complete.
1. Select **SpatialOS** > **Launch mobile client** > **Android Device** to start your Android client.
1. Play the game on your device.
