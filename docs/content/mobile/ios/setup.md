# Setting up iOS support for the GDK

## Get the dependencies for developing iOS games

> **Note:** iOS development is only supported on macOS.

  1. Follow the steps in [Get the dependencies]({{urlRoot}}/setup-and-installing) and additionally install **iOS build support** for Unity.
  1. Install iOS prerequisites
    * [Xcode](https://developer.apple.com/xcode/)
      * Once installed, open Xcode. It might prompt you to accept user agreements.
    * (Optional) [Unity Remote](https://itunes.apple.com/gb/app/unity-remote-5/id871767552?mt=8) - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor

Most of your interactions with the GDK will happen inside the Unity Editor. To get started:

1. Open the Unity Editor.
1. Launch the Unity Editor. It should automatically detect your project but if it doesn't, select **Open** and then select `<path-to-your-project>/workers/unity`. If you don’t have a SpatialOS Unity project you can use the [FPS Starter Project]({{urlRoot}}/content/get-started/get-started) or the [Blank Starter Project]({{urlRoot}}/projects/blank/overview). If you are using one of these projects, please ensure that you've completed the setup steps for those projects before continuing these steps.
1. In the Unity Editor, go to **File** > **Build Settings**. Select **iOS** and then click on **Switch Platform**.
