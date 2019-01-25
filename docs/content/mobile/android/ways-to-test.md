# Ways to test your Android client

Before reading this document, make sure you are familiar with:

  * [The SpatialOS GDK for Unity]({{urlRoot}}/content/intro-reference)
  * [Mobile support overview]({{urlRoot}}/content/mobile/overview)
  * [Setting up iOS support for the GDK]({{urlRoot}}/content/mobile/android/setup)

Unity provides multiple ways to test your Android [client-worker]({{urlRoot}}/content/glossary#client-worker). We integrated them all to work with [SpatialOS]({{urlRoot}}/content/glossary#spatialos-runtime). This documentation describes the benefits of the different options.

## In the Editor
For standard workflows and for minor changes, run your game in the Editor. When the build platform is set for Android, it executes code that is in preprocessor `#if UNITY_ANDROID` clauses. This way, you have the full capabilities and ease of use of the Unity Editor, while still testing code that would otherwise only run on a mobile device.

## Unity Remote

With the Unity Remote, you don’t have to spend time building and deploying your game, reducing development iteration times. It mirrors the Unity Editor’s Game view on your mobile device, giving you quick feedback on how the game looks on a mobile device. However, it does not provide the full native capabilities of the game running on a device.

For more information, see the following documentation:

  * [The Unity documentation on Unity Remote](https://docs.unity3d.com/Manual/UnityRemote5.html)
  * [Run your game with Unity Remote]({{urlRoot}}/content/mobile/android/local-deploy#remote)

## Android Emulator

The Android Emulator from Android Studio emulates Android devices on your development computer so that you can test your game on a variety of devices and Android APIs without needing a physical device. You need to build and deploy your game to use the Emulator.

For more information, see the following documentation:

  * [The Android Developers documentation](https://developer.android.com/studio/run/emulator)
  * [Deploy your game with the Android Emulator]({{urlRoot}}/content/mobile/android/local-deploy#emulator)

## Android device

While it takes time to build and deploy, this option provides the full native capabilities of deploying the game to a device; good picture quality, instant feedback, and snappy controls.

For more information, see the following documentation:

  * [Deploy your game to an Android device]({{urlRoot}}/content/mobile/android/local-deploy#device)
