# Setting up Android support for the GDK

## Get the dependencies for developing Android games
  1. Follow the steps in [Get the dependencies]({{urlRoot}}/setup-and-installing) and additionally install **Android build support** for Unity.
  1. Install Android prerequisites:
    * [Android Studio](https://developer.android.com/studio/)
       * At the Choose Components stage of the installation, select **Android Virtual Device**.
       * Once the installation is complete, open Android Studio and select **Configure** > **SDK Manager**.<br>
         Select the version you intend to develop your game for.<br>
         Select **Apply** to download and install the matching **Android SDK** version.
    * [Java Platform, Standard Edition Development Kit (JDK)](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html) - download and install the latest version.
  1. (Optional) Download and unzip [Android NDK r16b](https://developer.android.com/ndk/downloads/older_releases), you only need this if you want to build for Android using [IL2CPP](https://docs.unity3d.com/Manual/IL2CPP.html). Extract it to a directory of your choice. Note down this file path as you need to specify the path in your Unity Editor later.
  1. (Optional) [Unity Remote](https://play.google.com/store/apps/details?id=com.unity3d.genericremote) - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your Unity Editor.
1. It should automatically detect your project. If it doesn't, select **Open**, navigate to `<path-to-your-project>/workers/unity` and select **Open**.<br>
If you don’t have a SpatialOS Unity project you can use the [FPS Starter Project]({{urlRoot}}/content/get-started/get-started) or the [Blank Starter Project]({{urlRoot}}/projects/blank/overview) to get started. If you are using one of these projects, please ensure that you've completed the [setup]({{urlRoot}}/content/get-started/set-up) steps for those projects before continuing these steps.
1. In your Unity Editor, go to **File** > **Build Settings**. Select **Android** and then **Switch Platform**.
1. Still in your Unity Editor, add the file paths Unity needs via the **External Tools** window. To do this:
    * on Windows, go to **Edit** > **Preferences** 
    * on MacOS, go to **Unity** > **Preferences**.

    In the **Android** section of the **External Tools** window,  input the paths to the SDK, JDK and NDK. The easiest way to ensure that Unity reads the file path correctly is to use the browse option:

| Field | How to find the path |
|-------|------|
| SDK  |  You can find the SDK location by opening Android Studio and selecting **Configure** then **SDK Manager**. |
| JDK  |  **Windows:** default path is `C:/Program Files/Java` <br/>**Mac:** run `which java` to retrieve the path. |
| NDK  |  (Optional) Use the directory path that you noted down earlier when extracting the Android NDK.|
