[//]: # (TODO - get rid of mobile_launch.json mention and explain it differently)

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

  1. [Build your server-workers.]({{urlRoot}}/content/build)
  1. You need to know the local IP address of your computer to connect. [This page](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address) (on the Lifehacker website)  describes how you can find your external and local IP address.
  1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch mobile_launch.json --runtime_ip=<your-local-ip>`.  (Where `<your-local-ip>` is the IP address you just located.)

    > You cannot use **SpatialOS** > **Local launch** in your Unity Editor, because you need to specify the runtime IP and a different launch configuration.
    >
    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

  1. In the Unity Editor, navigate to **File** > **Build Settings**. Select **Android** as your Build Platform and choose the virtual device from the drop-down menu in the **Run Device** field. The device is likely to be called **Google Android SDK built for x86 (emulator-XXXX)**.
  1. In the Unity Editor, navigate to **File** > **Build Settings** and click **Build and Run**. (For subsequent runs using the same Emulator, you can just select **File** > **Build and Run**.)
  1. Once the game is deployed and started on the Emulator, you see an empty text field and a **Connect** button: Select **Connect**.

    > You don’t need to enter anything in the text field.

  1. Play the game on the Emulator.

## Connecting your Android device to a local deployment

  1. Enable USB debugging on your mobile device. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
  1. Make sure your computer and your mobile device are both connected to the same wireless network.
  1. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
  1. [Build your server-workers.]({{urlRoot}}/content/build)
  1. You need to know the local IP address of your computer to connect. [This page (on the Lifehacker website)](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address) describes how you can find your local and external IP address.
  1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch mobile_launch.json --runtime_ip=<your-local-ip>`. (Where `<your-local-ip>` is the IP address you just located.)

    > You cannot use **SpatialOS** > **Local launch** in your Unity Editor, because you need to specify the runtime IP and a different launch configuration.
    >
    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

  1. In the Unity Editor, go to **File** > **Build Settings**. Choose the type of mobile device from the drop-down menu and click **Build and Run**. (For subsequent runs you can just select **File** > **Build and Run**.)
  1. Once the game is running on your device, you see an empty text field and a **Connect** button: enter the local IP address of your computer in the text field and click **Connect**.
  1. Play the game on your mobile device.
