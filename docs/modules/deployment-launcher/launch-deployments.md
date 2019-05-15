<nav id="pageToc" class="page-toc">
    <ul>
        <li>
            <a href="#ui-walkthrough">UI walkthrough</a>
            <ul>
                <li>
                    <a href="#deployment-configuration">Deployment configuration</a>
                    <a href="#simulated-player-deployment-configuration">Simulated player deployment configuration</a>
                    <a href="#launch-deployments">Launch deployments</a>
                </li>
            </ul>
            <a href="#expected-behaviour">Expected behaviour</a>
            <ul>
                <li>
                    <a href="#launching-a-deployment">Launching a deployment</a>
                </li>
            </ul>
        </li>
    </ul>
</nav>

# Launch deployments

Open the Deployment Launcher window by selecting **SpatialOS** > **Deployment Launcher** from the Unity Editor menu.

## UI walkthrough

The section of the Deployment Launcher window that you use to launch deployments can be found under the **Deployment Configurations** label.

<%(#Expandable title="Full deployment configuration from the FPS Starter Project.")%>

<img src="{{assetRoot}}assets/modules/deployment-launcher/deployment-configurations-label.png" style="margin: 0 auto; width: auto; display: block;" />

<%(/Expandable)%>

### Deployment configuration

A deployment configuration is used to describe what parameters a deployment should be launched with.

<img src="{{assetRoot}}assets/modules/deployment-launcher/dpl-configs.png" style="margin: 0 auto; width: auto; display: block;" />

<%(#Expandable title="Fields")%>

| Field | Required? | Description |
| --- | --- | --- |
| Assembly Name | ✔️ | The identifier of an assembly you have uploaded. |
| Deployment Name | ✔️ | The name you wish to give to the deployment. |
| Snapshot Path | ❌ | Path to the [snapshot](https://docs.improbable.io/reference/latest/shared/glossary#snapshot) you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Launch Config | ✔️ | Path to the launch configuration you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Region | ✔️ | The geographical region code that the deployment is running in. |
| Tags | ❌ | Metadata that can be added to a deployment. Some tags have built-in functionality. For example, the `dev_login` tag is used to connect into deployment through the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication). |
| Simulated Player Deployments | ❌ | A set of child deployments responsible for running [simulated players]({{urlRoot}}/reference/glossary#simulated-player) that connect into the parent deployment. These deployments are launched after the parent deployment has successfully started. |

<%(/Expandable)%>

<%(#Expandable title="Buttons")%>

| Button name | Description |
| --- | --- |
| Add new deployment configuration | When pressed, this creates a new deployment configuration. |
| Add simulated player deployment | When pressed, this creates a new [simulated player]({{urlRoot}}/reference/glossary#simulated-player) deployment configuration as a child of a parent deployment. |
| Remove (➖) | When pressed, this removes a deployment from the list of configurations. |

<%(/Expandable)%>

<%(Callout message="

The remove button (➖) is used to either:

* Remove a full deployment configuration.
* Remove a [simulated player]({{urlRoot}}/reference/glossary#simulated-player) deployment from its parent deployment configuration.

![]({{assetRoot}}assets/modules/deployment-launcher/remove-simplayer-dpl-config.png)

")%>

### Simulated player deployment configuration

Simulated player deployments inherit the following fields from their parent deployment:

* Assembly name
* Region

<img src="{{assetRoot}}assets/modules/deployment-launcher/dpl-configs-sim.png" style="margin: 0 auto; width: auto; display: block;" />

In addition, each simulated player deployment is assigned a deployment name based on its parent. For example, given a `parent_deployment_name`, a simulated player deployment is given a name of the format:

```text
{parent_deployment_name}_sim{N}

Where N is in range [1, number of simulated player deployments]
```

<%(#Expandable title="Additional fields")%>

| Field | Required? | Description |
| --- | --- | --- |
| Snapshot Path | ❌ | Path to the [snapshot](https://docs.improbable.io/reference/latest/shared/glossary#snapshot) you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Launch Config | ✔️ | Path to the launch configuration you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Region | ✔️ | The geographical region code that the deployment is running in. |
| Tags | ❌ | Metadata that can be added to a deployment. Some tags have built-in functionality. For example, the `dev_login` tag is used to connect into deployment through the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication). |
| Flag Prefix | ✔️ | The prefix to include on worker flags that the Deployment Launcher adds to simulated player deployments. This should follow [worker flag naming conventions](https://docs.improbable.io/reference/latest/shared/worker-configuration/worker-flags#naming-conventions). |
| Worker Type | ✔️ | The worker type of the [simulated player coordinator]({{urlRoot}}/reference/glossary#simulated-player-coordinator), responsible for managing [simulated players]({{urlRoot}}/reference/glossary#simulated-player) in the child deployment and connecting them to the parent deployment. |

<%(/Expandable)%>

### Launch deployments

In the deployment configuration panel, select which deployment you wish to launch from the drop-down list. To start your chosen deployment, press **Launch deployment**.

<img src="{{assetRoot}}assets/modules/deployment-launcher/choose-launch-config.png" style="margin: 0 auto; width: auto; display: block;" />

## Expected behaviour

<%(#Expandable title="<b>Input validation</b>")%>

The deployment launcher validates your deployment configurations as you edit them in the Unity Editor. If any field fails validation, errors similar to those highlighted below are shown and the **Launch deployment** button is disabled until you resolve all the errors.

#### Assembly name

Your assembly name:

* Must contain at least 5 characters.
* Must contain at most 64 characters.
* Can contain alphanumeric characters.
* Can contain the following special characters: `_`, `.`, and `-`.

If validation fails, an error similar to below is shown:

```text
Assembly Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-zA-Z0-9_.-]{5,64}
```

#### Deployment name

Your deployment name:

* Must contain at least 2 characters.
* Must contain at most 32 characters.
* Must be in lowercase.
* Can contain alphanumeric characters.
* Can contain underscores.

If validation fails, an error similar to below is shown:

```text
Deployment Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-z0-9_]{2,32}$
```

#### Snapshot path

If a snapshot does not exist at the file path you have specified, an error similar to below is shown:

```text
Snapshot file at "<snapshot_path>" cannot be found.
```

#### Launch config

If your launch configuration is an empty file or does not exist at the specified file path, you will see an error similar to one of the below:

```text
Launch Config cannot be empty.
```

```text
Launch Config file at {filePath} cannot be found.
```

#### Region

At present, you can select either `EU` or `US` as the region to launch your deployment in.

#### Tags

Each tag:

* Must begin with an alphanumeric character.
* Must contain at least 2 characters.
* Must contain at most 32 characters.
* Can contain underscores after the first character.

If validation fails, an error similar to below is shown:

```text
Tag "<tag>" invalid. Must conform to the regex: ^[A-Za-z0-9][A-Za-z0-9_]{2,32}$
```

#### Flag prefix

A flag prefix is not allowed to be empty and cannot contain full stops or spaces.

If validation fails, an error similar to below is shown:

```text
Flag Prefix cannot be empty.

Flag Prefix cannot contain full stops.

Flag Prefix cannot contain spaces.
```

#### Worker Type

The simulated player coordinator worker type can be chosen from a drop-down list of workers in the project.

<%(/Expandable)%>

### Launching a deployment

When you press the **Launch deployment** button, the Deployment Launcher starts launching the deployments specified in the configuration chosen in the adjacent drop-down menu.

<img src="{{assetRoot}}assets/modules/deployment-launcher/choose-launch-config.png" style="margin: 0 auto; width: auto; display: block;" />

For each deployment launched, you should see a notification similar to the following at the bottom of the deployment launcher window:

```text
Launching deployment "<deployment_name>" in project "<project_name>".
Assembly reloading locked.
```

<%(Callout message="
When a deployment successfully launches:

* The [Console](https://docs.improbable.io/reference/latest/shared/glossary#console) page for the deployment automatically opens in your browser.
* The notification at the bottom of the Deployment Launcher window disappears.
")%>

<%(#Expandable title="Cancel the deployment launching operation")%>

When a deployment is being launched, you can cancel the operation by pressing the **Cancel** button shown next to the notification at the bottom of your Deployment Launcher window.

<%(Callout type="warn" message="
Cancelling the operation does not prevent the deployment from launching, and may result in a new deployment starting with an unknown state.
")%>

<%(/Expandable)%>
