<%(TOC)%>
# Setting up iOS support for the GDK

## Get the dependencies for developing iOS games

> **Note:** iOS development is only supported on macOS.

  1. Follow the steps in [Setup & installation]({{urlRoot}}/machine-setup) and be sure to select the **iOS build support** component during your Unity installation.
  1. Install iOS development prerequisites:
    * [XCode](https://developer.apple.com/xcode/)
    * (Optional) [Unity Remote](https://itunes.apple.com/gb/app/unity-remote-5/id871767552?mt=8) - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor

Most of your interactions with the GDK happen inside your Unity Editor. To get started:

1. Open your Unity Editor.
1. It should automatically detect your project. If it doesn't, select **Open**, navigate to `<path-to-your-project>/workers/unity` and select **Open**.<br>
If you don’t have a SpatialOS Unity project you can use the [FPS Starter Project]({{urlRoot}}/content/get-started/get-started) or the [Blank Starter Project]({{urlRoot}}/projects/blank/overview) to get started. If you are using one of these projects, please ensure that you've completed the [setup]({{urlRoot}}/content/get-started/set-up) steps for those projects before continuing these steps.
1. In your Unity Editor, go to **File** > **Build Settings** to bring up the Build Settings window. 
1. In the window, select **iOS** and then **Switch Platform**.
