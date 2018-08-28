**Warning:** The pre-alpha release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

# Set up and get started with the SpatialOS Unity GDK for Android

## Setting up your machine

1. Before setting up for Android:
    * Follow the steps in [Setup and install GDK](../../../docs/setup-and-installing.md). 
    * Make sure you use the Unity Editor specified in the *Setup and install GDK* documentation.

2. Install Android prerequisites
    1. [Android Studio](https://developer.android.com/studio/)
        * Once installed, open Android Studio. It prompts you to install:
            * The Android SDK
            * The Android Studio emulator
    2. [Android NDK r13b](https://developer.android.com/ndk/downloads/older_releases)
    3. [JDK 8](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)
    4. [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) (Optional - this is Unity's solution for faster development iteration times.)]

3. Set up your Unity Editor
    * In the Unity Editor, go to **File > Build Settings**. Select Android and click on **Switch Platform**.
    * Go to **Edit > Preferences**. In the **External Tools** window, add the file paths to the tools you have just downloaded and installed: Android SDK, JDK and Android NDK.
    * From the **Improbable** menu in the Editor toolbar, select **Build UnityGameLogic for Local**.


## Choosing the right way to test your game

There are four ways to test your game for Android devices.

### In the Editor
For standard workflows and for minor changes, run the game in the Editor. When the build platform is set for Android, it executes code that is in preprocessor `#if UNITY_ANDROID` clauses. This way you have the full capabilities and ease of use of the Unity Editor while still testing code that would otherwise only run on a mobile device.  

### Unity Remote
With the Unity Remote, you don’t have to spend time building and deploying your game. It provides an alternative for reducing iteration times. It mirrors the Unity Editor’s Game view on your mobile device, giving you quick feedback on how the game looks on a phone. However, this does not provide the full native capabilities of deploying the game to a device. 
See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for more information. See also [Run your game with Unity Remote](#run-your-game-with-unity-remote).

### Android Emulator
The Android Emulator from Android Studio simulates Android devices on your development computer so that you can test your game on a variety of devices and Android APIs without needing to have each physical device. You need to build and deploy your game to use the Emulator.
See the [Android Developers documentation](https://developer.android.com/studio/run/emulator) for more information. See also [Deploy your game with Android Emulator](#deploy-your-game-to-the-android-emulator).

### Android device
While it takes time to build and deploy, this option provides the full native capabilities of deploying the game to a device; good picture quality, instant feedback, and snappy controls. 
See [Deploy your game to an Android device](#deploy-your-game-to-an-android-device).

## How to:

### Run your game with Unity Remote

1. In the Editor go to **Edit > Project Settings > Editor** to bring up the Editor Settings window.
2. In the **Unity Remote** section, clock on the drop-down menu beside the **Device** option and select **Any Android Device**.
3. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
4. Open a terminal window and from your project directory run `spatial local launch`.
5. In the Editor's Game view, click **Play**. 
    > You can change the resolution of the game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that's identical to your mobile device to produce the best results.
6. You should now see your Editor screen mirrored on your Android device.

### Deploy your game to the Android Emulator

1. [Start the Emulator in Android Studio](https://developer.android.com/studio/run/managing-avds)
    > Make sure you choose the same CPU architecture for your virtual machine as your development computer. If you don't, you will get warning messages as mis-matched CPU architecture affects performance.
2. In the Editor, go to **File > Build Settings**. Choose the virtual device from the drop-down menu. The device is likely to be called **Google Android SDK built for x86 (emulator-XXXX)**. Click **Build and Run**. 
    > For subsequent runs, you can just select **File > Build and Run**.
3. In the Editor select **Improbable > Run SpatialOS for Android**. This opens a command line window. In here SpatialOS starts locally with the appropriate settings.
    > **Warning:** Starting the game from the Editor at this point might lead to unexpected behavior!
4. Once the game is deployed and started on the Emulator, click on **Connect**. You don't need to enter anything in the text field.
5. Play the game on the Emulator.

### Deploy your game to an Android device

1. Enable USB debugging on your mobile device before deploying. See the [Android developer documentation](https://developer.android.com/studio/debug/dev-options#enable) for guidance.
2. Make sure your computer and your mobile device are both connected to the same wireless network.
3. Connect the mobile device to your computer using a USB cable. You might get a pop-up window on the device asking you to allow USB debugging. Select **Yes**.
4. In the Editor, go to **File > Build Settings**. Choose the type of mobile device from the drop-down menu and click **Build and Run**.
    > For subsequent runs you can just select **File > Build and Run**.
5. In the Editor, select **Improbable > Run SpatialOS for Android**. This opens a command line window. In here SpatialOS starts locally with the appropriate settings. It also prints a message in the Editor Console with the IP address of your machine. Wait until you see the message `SpatialOS is ready` and `UnityGameLogic worker has connected`.
    > **Warning:** Starting the game from the Editor at this point might lead to unexpected behavior!
6. After the game runs on your device, enter the IP address you saw in 5 above and click **Connect**.
7. Play the game on the mobile device.

----
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../../README.md#give-us-feedback).