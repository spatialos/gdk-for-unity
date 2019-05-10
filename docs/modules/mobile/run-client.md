<%(TOC)%>

# Run your mobile client

<%(Callout message="
Before reading this document, make sure you have read:

  * [The SpatialOS GDK for Unity]({{urlRoot}}/reference/overview)
  * [Mobile support overview]({{urlRoot}}/modules/mobile/overview)
")%>

Unity provides multiple ways to test your mobile [client-worker]({{urlRoot}}/reference/glossary#client-worker). We integrated them all to work with [SpatialOS]({{urlRoot}}/reference/glossary#spatialos-runtime). Below we describe the benefits and drawbacks of each option.

## In the Editor

For standard workflows and minor changes, we recommend that you run your game in the Editor to speed up your iteration time. You can set your build platform to **Android** or **iOS**. This allows Unity to compile and execute sections of code marked with the [platform #define directive](https://docs.unity3d.com/Manual/PlatformDependentCompilation.html) `#if UNITY_ANDROID` or `#if UNITY_IOS`.

## Unity Remote

With the Unity Remote app, you don’t have to spend time building and deploying your game, reducing development iteration time. It mirrors your Unity Editor’s Game view on your mobile device, giving you quick feedback on how the game looks on a mobile device. However, it does not provide the full native capabilities of the game running on a device.

For more information, see the following documentation:

  * [Unity documentation on Unity Remote](https://docs.unity3d.com/Manual/UnityRemote5.html)

## Android Emulator / iOS Simulator

Use an emulator or simulator to test your game on a variety of devices and target versions without needing a physical device.

For more information, see the following documentation:

  * [The Android Developers documentation](https://developer.android.com/studio/run/emulator)
  * [The Apple Developer documentation](https://developer.apple.com/library/archive/documentation/IDEs/Conceptual/simulator_help_topics/Chapter/Chapter.html)

## Android device

This option provides the full native capabilities of deploying the game to a device and allows you to identify any potential issues you may encounter on a real device.