# Setting up iOS support for the GDK

## Get the dependencies for developing iOS games

  1. Follow the steps in [Get the dependencies]({{urlRoot}}/setup-and-installing) and additionally install **iOS build support** for Unity.
  1. Install iOS prerequisites
    * [Xcode](https://developer.apple.com/xcode/)
      * Once installed, open Xcode. It might prompt you to accept user agreements.
    * (Optional) [Unity Remote](https://itunes.apple.com/gb/app/unity-remote-5/id871767552?mt=8) - this is Unity’s solution for faster development iteration times.
    * (Optional) [ideviceinstaller](https://github.com/libimobiledevice/ideviceinstaller) and [idevicedebug](https://helpmanual.io/help/idevicedebug/) - iOS device command line interaction tools. They will allow you to connect to local deployment directly from Unity.

## Set up your Unity Editor
After installing these dependencies, open the Unity project in `<path-to-your-project>/workers/unity`.

  1. In the Unity Editor, go to **File** > **Build Settings**. Select **iOS** and then click on **Switch Platform**.
