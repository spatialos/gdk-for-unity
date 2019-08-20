# Prepare your project

<%(Callout message="
Before reading this document, make sure you have read:

* [Setting up Android support for the GDK]({{urlRoot}}/modules/mobile/setup-android)
* [Setting up iOS Support for the GDK]({{urlRoot}}/modules/mobile/setup-ios)
* [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/reference/workflows/monobehaviour/worker-connectors)
* [Development authentication flow](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/auth/development-authentication)
")%>

## Create your worker connector script

If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip this section, as you already have one in your project.

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you need to create and add a MonoBehaviour script to your mobile client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector). This scripts contains support for both Android and iOS. You can base your implementation on the one in our [Blank Starter Project](https://github.com/spatialos/gdk-for-unity-blank-project/blob/master/workers/unity/Assets/BlankProject/Scripts/Workers/MobileClientWorkerConnector.cs).
1. In your Unity Editor, add the MonoBehaviour script to your mobile client-worker GameObject.

## Create your development authentication token

To connect to a cloud deployment using your mobile device, you need to use the [development authentication flow](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/auth/development-authentication) which we provide for the early stages of game development. The GDK for Unity provides tooling around the development authentication flow to help you iterate faster on your mobile application.

1. Open your project in your Unity Editor.
1. Navigate to **SpatialOS** > **GDK Tools configuration** to open the configuration window.
1. In the **Dev Auth Token Settings** specify the lifetime of the token and the path to the [Resources folder](https://unity3d.com/learn/tutorials/topics/best-practices/resources-folder) that you would like to store the generated token in.

    > Your token expires after the amount of days that you specified in the configuration window. Regenerate the token whenever that happens.
1. (for iOS) Ensure that **Save token to file** is checked.
1. Select **Save** and close the window.
1. Select **SpatialOS** > **Dev Authentication Token** > **Generate Token**. This stores the dev auth token inside the Unity Player Preferences. If **Save token to file** inside the configuration window is checked, it will also generate a `DevAuthToken.txt` asset in the folder you specified in the configuration window. This token is used to authenticate when connecting to a cloud deployment.

    > If your worker connector inherits from the `DefaultMobileWorkerConnector` script, it will automatically read the content inside `DevAuthToken.txt` when running the application and authenticate against our services. See the section above to learn how to create a mobile worker connector.

If you want to create your own authentication server, follow [this guide](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/auth/integrate-authentication-platform-sdk).
