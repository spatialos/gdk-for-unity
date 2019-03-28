<%(TOC)%>
# Run your iOS client

Before reading this document, make sure you are familiar with:

  * [The SpatialOS GDK for Unity]({{urlRoot}}/reference/overview)
  * [Mobile support overview]({{urlRoot}}/reference/mobile/overview)
  * [Setting up iOS support for the GDK]({{urlRoot}}/reference/mobile/ios/setup)

Unity provides multiple ways to test your iOS [client-worker]({{urlRoot}}/reference/glossary#client-worker). We integrated them all to work with [SpatialOS]({{urlRoot}}/reference/glossary#spatialos-runtime). This documentation describes the benefits of the different options.

## In the Editor
For standard workflows and for minor changes, we recommend that you run your game in the Editor. Now that your build platform is set to **iOS**, Unity compiles and executes sections of code marked with the [platform #define directive](https://docs.unity3d.com/Manual/PlatformDependentCompilation.html); `#if UNITY_IOS`. This means that you have the full capabilities and ease of use of your Unity Editor, while still executing code that would otherwise only run on an iOS device.

For more information, see the following documentation:

  * [Connect to a local deployment in the Editor]({{urlRoot}}/reference/mobile/ios/local-deploy#in-editor)

## Unity Remote

With the Unity Remote app, you don’t have to spend time building and deploying your game, reducing development iteration times. It mirrors your Unity Editor’s Game view on your mobile device, giving you quick feedback on how the game looks on a mobile device. However, it does not provide the full native capabilities of the game running on a device.

For more information, see the following documentation:

  * [Unity documentation on Unity Remote](https://docs.unity3d.com/Manual/UnityRemote5.html)
  * [Connect to a local deployment using Unity Remote]({{urlRoot}}/reference/mobile/ios/local-deploy#unity-remote)

## iOS Simulator

The iOS Simulator simulates iOS devices on your development computer so that you can test your game on a variety of devices and iOS versions without needing a physical device. You need to build and deploy your game to use the Simulator.

For more information, see the following documentation:

  * [The Apple Developer documentation](https://developer.apple.com/library/archive/documentation/IDEs/Conceptual/simulator_help_topics/Chapter/Chapter.html)
  * [Connect to a local deployment using iOS Simulator]({{urlRoot}}/reference/mobile/ios/local-deploy#ios-simulator)

## iOS device

While it takes time to build and deploy, this option provides the full native capabilities of deploying the game to a device; good picture quality, instant feedback, and snappy controls.

For more information, see the following documentation:

  * [Connect to a local deployment using your iOS device]({{urlRoot}}/reference/mobile/ios/local-deploy#ios-device)
