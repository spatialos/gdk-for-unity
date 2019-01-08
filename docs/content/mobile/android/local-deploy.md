[//]: # (TODO - get rid of mobile_launch.json mention and explain it differently)

# Prepare your project to connect to a local deployment

To connect your Android device to a local deployment, follow these steps:

1. Open your project in the Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Runtime IP for local deployment** field, enter your local machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Select **Save**, and close the window.

This ensures that any local deployment that is launched via the Unity Editor is set up correctly to connect to your Android device.

# Connecting to a local deployment

Before reading this document, make sure you are familiar with:

* [Setting up Android Support for the GDK]({{urlRoot}}/content/mobile/android/setup)
* [Ways to test your Android client]({{urlRoot}}/content/mobile/android/ways-to-test)

## Connecting your Android device to a local deployment using Unity Remote

You need the Unity Remote app for this. See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for details.

1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. Open the project that you want to deploy with the Unity Editor and go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window.
1. Open the scene that starts both your [client-]({{urlRoot}}/content/glossary#client-worker) and [server-workers]({{urlRoot}}/content/glossary#server-worker).
1. In the **Unity Remote** section, select the drop-down menu beside the **Device** option and select **Any Android Device**.
1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
1. In the Unity Editor, Select **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Editor’s Game view, select **Play**.

    > You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

1. You should now see your Unity Editor game view mirrored on your Android device.

## Connecting your Android emulator to a local deployment

1. [Start your Android Emulator in Android Studio.](https://developer.android.com/studio/run/managing-avds)

    > Ensure you choose the same CPU architecture for your virtual machine as your development computer. If you don’t, you will get warning messages as mismatched CPU architecture affects performance.
1. [Set up your local deployment.](#setup-local-deployment-for-mobile-phone-connections)
1. [Build your server-workers.]({{urlRoot}}/content/build)
1. Start your local deployment from the Unity Editor's menu: **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your android worker, and wait for the build to complete.
1. Now select **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on the Emulator.

## Connecting your Android device to a local deployment

1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Make sure your computer and your mobile device are both connected to the same wireless network.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. [Set up your local deployment](#setup-local-deployment-for-mobile-phone-connections)
1. [Build your server-workers.]({{urlRoot}}/content/build)
1. Start your local deployment from the Unity Editor's menu: **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your android worker, and wait for the build to complete.
1. Now select **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on your device.
