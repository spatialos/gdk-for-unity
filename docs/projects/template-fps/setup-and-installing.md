# Set up the FPS Starter Project

1. Follow the [machine setup guide]({{urlRoot}}/setup-and-installing#set-up-your-machine).
2. Clone the [GDK for Unity FPS Starter Project](https://github.com/spatialos/gdk-for-unity-fps-starter-project) repository:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/gdk-for-unity-fps-starter-project.git` |
    | HTTPS | `git clone https://github.com/spatialos/gdk-for-unity-fps-starter-project.git` |
3. Setup dependencies by either:
    - Running the bundled scripts in the `gdk-for-unity-fps-starter-project` repository: `powershell scripts/powershell/setup.ps1` (Windows) or `bash scripts/shell/setup.sh` (Mac).
    - Manually by following the instructions below.

<%(#Expandable title="Manually setup dependencies")%>

1. Clone the [GDK for Unity](https://github.com/spatialos/gdk-for-unity) repository alongside the FPS Starter Project so that they sit side-by-side:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/gdk-for-unity.git` |
    | HTTPS | `git clone https://github.com/spatialos/gdk-for-unity.git` |
    > The two repositories should share a common parent like the following:
    ```text
    <common_parent_directory>
        ├── gdk-for-unity-fps-starter-project
        ├── gdk-for-unity
    ```

2. Navigate to the `gdk-for-unity` directory and checkout the pinned version which can be found in the `gdk.pinned` file in the root of the `gdk-for-unity-fps-starter-project` directory.
    - `git checkout <pinned_version>`

<%(/Expandable)%>

## Next Steps

To get started with running a deployment with the FPS Starter Project, check out [our guide for building and deploying]({{urlRoot}}/get-started#building-workers).
