# Setting up Android support for the GDK

## Get the dependencies for developing Android games
  1. Follow the steps in [Get the dependencies]({{urlRoot}}/setup-and-installing) and additionally install **Android build support** for Unity.
  1. Install Android prerequisites:
    * [Android Studio](https://developer.android.com/studio/) -  Once installed, open Android Studio. Ensure to install:
       * The Android SDK
       * The Android Studio emulator
    * [Android NDK r13b](https://developer.android.com/ndk/downloads/older_releases) - Extract it to a directory of your choice. Note down this directory path as you will be needing it in the following steps.
    * [Java Platform, Standard Edition Development Kit (JDK)](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html) - download and install the latest version.
  * (Optional) [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor
After installing these dependencies, open the Unity project in `<path-to-your-project>/workers/unity`.

  1. In the Unity Editor, go to **File** > **Build Settings**. Select **Android** and then click on **Switch Platform**.
  1. In the Unity Editor, go to **Edit** > **Preferences**. In the **External Tools** window, add the file paths to the tools in the **Android** section:

| Field | How to find the path |
|-------|------|
| SDK  |  You can find the SDK location by opening Android Studio and selecting **Configure** then **SDK Manager** from the launcher. |
| JDK  |  **Windows:** default path is `C:\Program Files\Java` <br/>**Mac:** run `which java` to retrieve the path. |
| NDK  |  Use the directory path that you noted down earlier when extracting the Android NDK.|
