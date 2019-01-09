[//]: # (TODO - get rid of mobile_launch.json mention and explain it differently)

# Connecting to a local deployment

Before reading this document, make sure you are familiar with:

  * [Setting up iOS Support for the GDK]({{urlRoot}}/content/mobile/ios/setup)
  * [Ways to test your iOS client]({{urlRoot}}/content/mobile/ios/ways-to-test)

## Prepare your project to connect to a local deployment

To connect your iOS device or simulator to a local deployment, follow these steps:

1. Open your project in the Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Runtime IP for local deployment** field, enter your local machine's IP address. (You can find how to do this on the [Lifehacker website](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address).)
1. Select **Save**, and close the window.

This ensures that any local deployment that is launched via the Unity Editor is set up correctly to connect to your iOS device or simulator.

## Connecting your iOS device to a local deployment using Unity Remote
You need the Unity Remote app for this. See the [Unity documentation](https://docs.unity3d.com/Manual/UnityRemote5.html) for details.

  1. Connect the mobile device to your computer using a USB cable.
  1. Open the project that you want to deploy with the Unity Editor and go to **Edit** > **Project Settings** > **Editor** to bring up the **Editor Settings** window.
  1. Open the scene that starts both your [client-]({{urlRoot}}/content/glossary#client-worker) and [server-workers]({{urlRoot}}/content/glossary#server-worker).
  1. In the **Unity Remote** section, click on the drop-down menu beside the **Device** option and select **Any iOS Device**.
  1. On your mobile device, open the **Unity Remote** app. Make sure you allow it permissions for location and camera.
  1. In the Unity Editor, Select **SpatialOS** > **Local launch**.

    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

  1. In the Editor’s Game view, select **Play**.

    > You can change the resolution of the Game view in your Unity Editor to make sure it does not appear stretched on your mobile device. Choose the resolution that’s identical to your mobile device to produce the best results.

  1. You should now see your Unity Editor game view mirrored on your iOS device.

## Connecting your iOS simulator to a local deployment

  1. [Build your server-workers.]({{urlRoot}}/content/build)
  1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch mobile_launch.json`.

    > You cannot use **SpatialOS** > **Local launch** in your Unity Editor, because you need to specify a different launch configuration.
    >
    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

  1. In the Unity Editor, navigate to **Edit** > **Project Settings** > **Player** > **Settings for iOS** > **Other Settings** > **Configuration** > **Target SDK** and choose **Simulator SDK**.
  1. In the Unity Editor, navigate to **File** > **Build Settings** and click **Build and Run**. This prompts you to choose where to save the XCode project that Unity generates. After you've selected the directory, Unity generates the XCode project, opens it in XCode and starts the build. If the build succeeds, XCode starts a Simulator and installs the game on it.
    * If you choose **Build**, instead of **Build and Run**, Unity generates a XCode project and opens the folder containing the project.
  1. Once the game is deployed and started on the Simulator, you see an empty text field and a **Connect** button: Select **Connect**.

    > You don’t need to enter anything in the text field.

  1. Play the game on the Simulator.

## Connecting your iOS device to a local deployment

  1. Set up [Code signing and provisioning](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7).
  1. Make sure your computer and your mobile device are both connected to the same wireless network.
  1. Connect the mobile device to your computer using a USB cable.
  1. [Build your server-workers.]({{urlRoot}}/content/build)
  1. You need to know the local IP address of your computer to connect. [This page](https://lifehacker.com/5833108/how-to-find-your-local-and-external-ip-address) (on the Lifehacker website)  describes how you can find your external and local IP address.
  1. In a terminal window from the root folder of your SpatialOS project,  run: `spatial local launch mobile_launch.json --runtime_ip=<your-local-ip>`. (Where `<your-local-ip>` is the IP address you just located.)

    > You cannot use **SpatialOS** > **Local launch** in your Unity Editor, because you need to specify the runtime IP.
    >
    > **It’s done when:** You see the following message in the terminal: `SpatialOS ready. Access the Inspector at http://localhost:21000/inspector`

  1. In the Unity Editor, navigate to **Edit** > **Project Settings** > **Player** > **Settings for iOS** > **Other Settings** > **Configuration** > **Target SDK** and choose **Device SDK**.
  1. In the Unity Editor, navigate to **File** > **Build Settings** and click **Build**. This prompts you to choose where to save the XCode project that Unity generates. Select a directory and Unity generates the XCode project. After the build has finished, Unity opens the folder containing the project. Open the project in XCode, select the Project root, go to **General** > **Signing** and sign the project.

    > If you choose **Build and Run** instead of **Build** Unity generates the XCode project, automatically opens it for you and starts the build to install the game on the connected device. This will most likely fail, because you need to first sign the application as described in the previous step.
    >
    > For subsequent runs you will be prompted to pick XCode project directory again (with the one used previously preselected). You can generate a new project or append/replace existing one. In subsequent runs, if you've set up provisioning and use Append, you can use **Build and Run** to trigger project run automatically after XCode project was generated.

  1. Once the game is running on your device, you need to connect using the local IP address.<br/>Either: 
  
      * On your device, note the empty text field and a **Connect** button: enter the local IP address of your computer in the text field and click **Connect**.
      
      Or:
  
      * In your Unity Editor, navigate to: **SpatialOS** > **Launch Mobile Client** > **iOS Client** to launch the iOS client with the IP address field prefilled. (It's prefilled with the value you entered in the GDK Tools Configuration Window. You can access this by navigating to **SpatialOS** > **GDK Tools Configuration** > **Runtime IP for local deployment** in the Unity Editor.)
  
      > To use this workflow you need to have [ideviceinstaller](https://github.com/libimobiledevice/ideviceinstaller) and [idevicedebug](https://helpmanual.io/help/idevicedebug/) tools installed.
      >
      > To start the iOS client from the Unity Editor, you need to build the application as a `.ipa` archive to the `workers\unity\build` directory.

  1. Play the game on your mobile device.
