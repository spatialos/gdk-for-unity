# Set up the FPS Starter Project

1. Follow the [machine setup guide]({{urlRoot}}/setup-and-installing#set-up-your-machine).
2. Clone the `GDK-for-Unity-FPS-Starter-Project` repository:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/GDK-for-Unity-FPS-Starter-Project.git` |
    | HTTPS | `git clone https://github.com/spatialos/GDK-for-Unity-FPS-Starter-Project.git` |
3. Setup dependencies by either:
    - Running the bundled scripts: `scripts/powershell/setup.ps1` (Windows) or `scripts/shell/setup.sh` (Mac).
    - Manually by following the instructions below.

<%(#Expandable title="Manually setup dependencies")%>
1. Clone the `GDK-for-Unity` repository alongside the `GDK-for-Unity-FPS-Starter-Project` so that they sit side-by-side:

    |     |     |
    | --- | --- |
    | SSH | `git clone git@github.com:spatialos/GDK-for-Unity.git` |
    | HTTPS | `git clone https://github.com/spatialos/GDK-for-Unity.git` |
2. Navigate to the `GDK-for-Unity` directory and checkout the pinned version which can be found in the `gdk.pinned` file in the root of the `GDK-for-Unity-FPS-Starter-Project` directory.
    - `git checkout <pinned_version>`
<%(/Expandable)%>

## Next Steps

To get started with running a deployment with the FPS Starter Project, check out [Fake players in the cloud]({{urlRoot}}/projects/template-fps/cloud-fake-clients).