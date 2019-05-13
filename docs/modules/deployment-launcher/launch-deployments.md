<%( TOC )%>

# Launch deployments

Open the Deployment Launcher window by selecting **SpatialOS** > **Deployment Launcher** from the Unity Editor menu.

## UI walkthrough

The section of the Deployment Launcher window that you use to launch deployments can be found under the **Deployment Configurations** label.

<%(#Expandable title="Full deployment configuration from the FPS Starter Project.")%>

<img src="{{assetRoot}}assets/modules/deployment-launcher/deployment-configurations-label.png" style="margin: 0 auto; width: auto; display: block;" />

<%(/Expandable)%>

### Deployment configuration

A deployment configuration is used to describe what parameters a deployment should be launched with.

Each deployment configuration contains 7 settings that you can edit:

<img src="{{assetRoot}}assets/modules/deployment-launcher/dpl-configs.png" style="margin: 0 auto; width: 100%; display: block;" />

| Setting | Required? | Description |
| --- | --- | --- |
| Assembly Name | ✔️ | The identifier of an assembly you have uploaded. |
| Deployment Name | ✔️ | The name you wish to give to the deployment. |
| Snapshot Path | ❌ | Path to the snapshot you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Launch Config | ✔️ | Path to the launch configuration you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Region | ✔️ | The geographical location in which your deployment will launch. |
| Tags | ❌ | Metadata that can be added to a deployment. Some tags have built-in functionality. For example, the `dev_login` tag is used to connect into deployment through the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication). |
| Simulated Player Deployments | ❌ | A set of child deployments responsible for running simulated players that connect into the parent deployment. These deployments are launched after the parent deployment has successfully started. |

| Button name | Description |
| --- | --- |
| Add new deployment configuration | When pressed, this creates a new deployment configuration. |
| Add simulated player deployment | When pressed, this creates a new simulated player deployment configuration as a child of a parent deployment. |
| `-` | When pressed, this removes a deployment from the list of configurations. |

<%(Callout message="

The remove button (`-`) is used to either:

* Remove a full deployment configuration.
* Remove a simulated player deployment from its parent deployment configuration.

![]({{assetRoot}}assets/modules/deployment-launcher/remove-simplayer-dpl-config.png)

")%>

### Simulated player deployment configuration

<img src="{{assetRoot}}assets/modules/deployment-launcher/dpl-configs-sim.png" style="margin: 0 auto; width: 100%; display: block;" />

Simulated player deployments inherit the following settings from their parent deployment:

* Assembly name
* Region

In addition, each simulated player deployment is assigned a deployment name based on its parent. For example, given a `parent_deployment_name`, a simulated player deployment will be given a name of the format:

```text
{parent_deployment_name}_sim{N}

Where N is in range [1, number of simulated player deployments]
```

A simulated player deployment configuration exposes 6 additional settings:

| Setting | Required? | Description |
| --- | --- | --- |
| Snapshot Path | ❌ | Path to the snapshot you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Launch Config | ✔️ | Path to the launch configuration you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Region | ✔️ | The geographical location in which your deployment will launch. |
| Tags | ❌ | Metadata that can be added to a deployment. Some tags have built-in functionality. For example, the `dev_login` tag is used to connect into deployment through the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication). |
| Flag Prefix | ✔️ |  |
| Worker Type | ✔️ |  |

### Launch deployments

At the bottom of the deployment configurations panel, there is a `Launch deployment` button. Use the option to select which deployment configuration you wish to launch.

<img src="{{assetRoot}}assets/modules/deployment-launcher/choose-launch-config.png" style="margin: 0 auto; width: 100%; display: block;" />

## Expected behaviour

### Input validation

The deployment launcher validates your deployment configurations as you edit them in the Unity editor. If any field fails validation, you will see errors similar to those highlighted below and the **Launch deployment** button will be disabled.

#### Assembly name

Your assembly name must:

* Must contain at least 5 characters.
* Must contain at most 64 characters.
* Not contain invalid characters.
  * `_`, `.`, and `-` are accepted characters.

If validation fails, you will see an error similar to below:

```text
Assembly Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-zA-Z0-9_.-]{5,64}
```

#### Deployment name

Your deployment name:

* Must contain at least 2 characters.
* Must contain at most 32 characters.
* Must be in lowercase.
* Can contain underscores.

If validation fails, you will see an error similar to below:

```text
Deployment Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-z0-9_]{2,32}$
```

#### Snapshot path

If a snapshot does not exist at the file path you have specified, you will see an error similar to below:

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

* Must contain at least 2 characters.
* Must contain at most 32 characters.
* Can contain underscores after the first character.
  * The first character **must** be alphanumeric.

If validation fails, you will see an error similar to below:

```text
Tag "<tag>" invalid. Must conform to the regex: ^[A-Za-z0-9][A-Za-z0-9_]{2,32}$
```

#### Flag prefix

A flag prefix is not allowed to be empty and cannot contain full stops or spaces.

If validation fails, you will see an error similar to one of the below:

```text
Flag Prefix cannot be empty.
```

```text
Flag Prefix cannot contain full stops.
```

```text
Flag Prefix cannot contain spaces.
```

#### Worker Type

The simulated player worker type can be chosen from a drop-down list of workers in the project.

### Launching a deployment

When you press the `Launch deployment` button, the Deployment Launcher starts launching the deployments specified in the configuration chosen in the adjacent drop-down menu.

<img src="{{assetRoot}}assets/modules/deployment-launcher/choose-launch-config.png" style="margin: 0 auto; width: 100%; display: block;" />

For each deployment launched, you should see a notification similar to the following at the bottom of the deployment launcher window:

```text
Launching deployment "<deployment_name>" in project "<project_name>".
Assembly reloading locked.
```

The standard output and standard error from the command is forwarded to the Unity Console.

<%(Callout message="
When a deployment successfully launches:

* The Console page for the deployment will automatically open in your browser.
* The notification at the bottom of the Deployment Launcher window will disappear.
")%>
