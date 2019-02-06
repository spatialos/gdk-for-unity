# Setting up Android support for the GDK

## Get the dependencies for developing Android games
  1. Follow the steps in [Get the dependencies]({{urlRoot}}/setup-and-installing) and additionally install **Android build support** for Unity.
  1. Install Android prerequisites:
    * [Android Studio](https://developer.android.com/studio/) -  Once installed, open Android Studio. Ensure to install:
       * The Android SDK
       * The Android Studio emulator
    * [Java Platform, Standard Edition Development Kit (JDK)](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html) - download and install the latest version.
  1. (Optional) Download and unzip [Android NDK r13b](https://developer.android.com/ndk/downloads/older_releases), you only need this if you want to build for Android using [IL2CPP](https://docs.unity3d.com/Manual/IL2CPP.html). Extract it to a directory of your choice. Note down this directory path as you will be need to input the path into Unity later.
  1. (Optional) [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor
After installing these dependencies, open the Unity project in `<path-to-your-project>/workers/unity`.

  1. In your Unity Editor, go to **File** > **Build Settings**. Select **Android** and then select **Switch Platform**.
  1. In your Unity Editor, go to **Edit** > **Preferences** on Windows or **Unity** > **Preferences** on macOS. In the **External Tools** window, in the **Android** section, input the paths to the SDK, JDK and NDK. The easiest way to ensure that Unity reads the filepath correctly is to use the browse option:

| Field | How to find the path |
|-------|------|
| SDK  |  You can find the SDK location by opening Android Studio and selecting **Configure** then **SDK Manager** from the launcher. |
| JDK  |  **Windows:** default path is `C:/Program Files/Java` <br/>**Mac:** run `which java` to retrieve the path. |
| NDK  |  (Optional) Use the directory path that you noted down earlier when extracting the Android NDK.|
