# Connect to a cloud deployment

Before reading this document, make sure you are familiar with:

* [Setting up iOS Support for the GDK]({{urlRoot}}/content/mobile/ios/setup)
* [Ways to test your iOS client]({{urlRoot}}/content/mobile/ios/ways-to-test)
* [Development Authentication Flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication)
* [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/content/gameobject/creating-workers-with-workerconnector)

To connect your mobile application to a cloud deployment, you need to authenticate against our services.
This guide describes how to authenticate using the development authentication flow which we provide for early stages in development.
Alternatively, if you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/latest/shared/auth/integrate-authentication-platform-sdk).

## Connecting your iOS device or simulator to a cloud deployment

1. [Build your server-worker types.]({{urlRoot}}/content/build)
1. Upload your server-worker types. To do this, open a terminal window and from the root directory of your SpatialOS project, enter `spatial cloud upload <assembly name>`.
1. In the same directory, start your cloud deployment using `spatial cloud launch --snapshot=snapshots/default.snapshot my_assembly <launch configuration>.json <deployment name>`.
1. In the SpatialOS Console, tag your cloud deployment with the tag `dev_login`. <BR/>
To do this:
  *  From the [SpatialOS Console](https://console.improbable.io/projects), select your deployment name to display the project **OVERVIEW** screen.
  * In the **OVERVIEW** screen, there’s a **Tag** field, add `dev_login` to the field.
1. [Create a Development Authentication Token (SpatialOS documentation).](https://docs.improbable.io/reference/latest/shared/auth/development-authentication#developmentauthenticationtoken-maintenance)
1. Create a MonoBehaviour script which inherits from the [`MobileWorkerConnector`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.mobile/Worker/MobileWorkerConnector.cs) and includes the functionality you want. In your Unity Editor, add this script it to your iOS client-worker GameObject.
1. The `MobileWorkerConnector` provides a `DevelopmentAuthToken` field. Still in your Unity Editor, make sure your iOS client-worker GameObject is selected and in the Inspector, locate the script you just added to it. 
1. In the Inspector, in the script’s drop-down window, there is a field to add the authentication token that you created. 
1. In the same drop-down window, ensure that the checkbox `ShouldConnectLocally` is not checked.
1. In your Unity Editor, navigate to **SpatialOS** > **Build for cloud**. Select your iOS client-worker, and wait for the build to complete. <br/>
You know it’s complete when it says `Completed build for Cloud target` in your Unity Editor’s Console window.
1. Select **SpatialOS** > **Launch mobile client** > **iOS Device**.
1. Go to your file manager and open the generated XCode project in the `workers/unity/build/iOSClient@iOS` directory.
1. Code sign the project and build it - see the _Building an XCode project using Unity_ section in the [Unity documentation](https://unity3d.com/learn/tutorials/topics/mobile-touch/building-your-unity-game-ios-device-testing).
1. Play the game on your iOS device or simulator.
