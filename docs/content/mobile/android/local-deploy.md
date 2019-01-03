[//]: # (TODO - get rid of mobile_launch.json mention and explain it differently)

# Setup local deployment for mobile phone connections

To allow a mobile phone to connect to your local deployment, you will need to make the following configuration steps.

1. Have your project opened in the Unity Editor.
1. Open the configuration window through **SpatialOS** > **GDK Tools configuration**.
1. Fill your local machine's IP into the **Runtime IP for local deployment** field.
1. Click **Save**, and close the window.

From this point on, all local deployments launched through the editor will allow your mobile phone to connect to it.

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
1. In the **Unity Remote** section, click on the drop-down menu beside the **Device** option and select **Any Android Device**.
1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
1. In the Unity Editor, Select **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Editor’s Game view, select **Play**.

    > You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

1. You should now see your Unity Editor game view mirrored on your Android device.

## Connecting your Android emulator to a local deployment

1. [Start your Android Emulator in Android Studio.](https://developer.android.com/studio/run/managing-avds)

    > Ensure to choose the same CPU architecture for your virtual machine as your development computer. If you don’t, you will get warning messages as mis-matched CPU architecture affects performance.
1. [Setup your local deployment.](#setup-local-deployment-for-mobile-phone-connections)
1. [Build your server-workers.]({{urlRoot}}/content/build)
1. Start the local deployment from the Unity Editor though **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your android worker, and wait for the build to complete.
1. Click **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on the Emulator.

## Connecting your Android device to a local deployment

1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
1. Make sure your computer and your mobile device are both connected to the same wireless network.
1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
1. [Setup your local deployment](#setup-local-deployment-for-mobile-phone-connections)
1. [Build your server-workers.]({{urlRoot}}/content/build)
1. Start the local deployment from the Unity Editor though **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

1. In the Unity Editor, navigate to **SpatialOS** > **Build for local**. Select your android worker, and wait for the build to complete.
1. Click **SpatialOS** > **Launch mobile client** > **Android Device**.
1. Play the game on the device.
