# Setting up iOS support for the GDK

## Get the dependencies for developing iOS games

  1. Follow the steps in [Get the dependencies]({{urlRoot}}/setup-and-installing) and additionally install **iOS build support** for Unity.
  1. Install iOS prerequisites
    * [Xcode](https://developer.apple.com/xcode/)
      * Once installed, open Xcode. It might prompt you to accept user agreements.
    * (Optional) [Unity Remote](https://itunes.apple.com/gb/app/unity-remote-5/id871767552?mt=8) - this is Unity’s solution for faster development iteration times.

## Set up your Unity Editor
Most of your interactions with the SpatialOS GDK will be from within the Unity Editor. To get started, from your Unity Editor’s file browser, open the `workers/unity` directory inside of your SpatialOS Unity project. If you don’t have a SpatialOS Unity project you can use the [FPS Starter Project](https://docs.improbable.io/unity/latest/projects/fps/overview) or the [Blank Starter Project](https://docs.improbable.io/unity/latest/projects/blank/overview).

  1. In the Unity Editor, go to **File** > **Build Settings**. Select **iOS** and then click on **Switch Platform**.
