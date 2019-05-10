<%( TOC )%>

# Launch deployments

Open the Deployment Launcher window by selecting **SpatialOS** > **Deployment Launcher** from the Unity Editor menu.

## UI walkthrough

The section of the Deployment Launcher window that you use to launch deployments can be found under the **Deployment Configurations** label.

<%(#Expandable title="Full deployment configuration from the FPS Starter Project.")%>

<img src="{{assetRoot}}assets/modules/deployment-launcher/deployment-configurations-label.png" style="margin: 0 auto; width: auto; display: block;" />

<%(/Expandable)%>

### Deployment configuration

<%(#Expandable title="Example deployment configuration from the FPS Starter Project.")%>

<img src="{{assetRoot}}assets/modules/deployment-launcher/dpl-configs.png" style="margin: 0 auto; width: auto; display: block;" />

<%(/Expandable)%>

A deployment configuration is used to describe what parameters a deployment should be launched with.

Each deployment configuration contains 7 settings that you can edit:

| Setting | Required? | Description |
| --- | --- | --- |
| Assembly Name | ✔️ | The identifier of an assembly you have uploaded. |
| Deployment Name | ✔️ | The name you wish to give to the deployment. |
| Snapshot Path | ❌ | Path to the snapshot you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Launch Config | ✔️ | Path to the launch configuration you wish to start the deployment with, relative from the root of your SpatialOS project. |
| Region | ✔️ | The geographical location in which your deployment will launch. |
| Tags | ❌ | Metadata that can be added to a deployment. Some tags have built-in functionality. For example, the `dev_login` tag is used to connect into deployment through the [development authentication flow](https://docs.improbable.io/reference/latest/shared/auth/development-authentication). |
| Simulated Player Deployments | ❌ | A set of child deployments responsible for running simulated players that connect into the parent deployment. These deployments are launched after the parent deployment has successfully started. |

> Use the `Add simulated player deployment` button to create a new simulated player deployment configuration to add to your current set of child configurations.

### Simulated player deployment configuration

<%(#Expandable title="Example simulated player deployment configuration from the FPS Starter Project.")%>

<img src="{{assetRoot}}assets/modules/deployment-launcher/dpl-configs-sim.png" style="margin: 0 auto; width: auto; display: block;" />

<%(/Expandable)%>

Simulated player deployments inherit the following settings with their parent deployment:

* Assembly name
* Region

In addition, each simulated player deployment is assigned a deployment name based on its parent. For example, given a `parent_deployment_name`, a simulated player deployment will be given a name of the format:

```text
{parent_deployment_name}_sim{N}

Where N is in range [1, number of simulated player deployments].
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

At the bottom of the deployment configurations panel, there is a `Launch deployment` button.

Use the option to select which deployment configuration you wish to launch.

## Expected behaviour

### Input validation

The deployment launcher validates your deployment configurations as you edit them in the Unity editor.

#### Assembly name

If your chosen assembly name fails validation, you will see an error similar to below and the **Launch deployment** button will be disabled:

```text
Assembly Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-zA-Z0-9_.-]{5,64}
```

This means that your assembly name must:

* Contain at least 5 characters.
* Contain at most 64 characters.
* Not contain invalid characters (acceptable characters are alphanumeric as well as "\_", ".", and "-").

#### Deployment name

```text
Deployment Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-z0-9_]{2,32}$
```

#### Snapshot path

```text
Snapshot file at "<snapshot_path>" cannot be found.
```

#### Launch config

```text
Launch Config cannot be empty.
```

```text
Launch Config file at {filePath} cannot be found.
```

#### Tags

```text
Tag "<tag>" invalid. Must conform to the regex: ^[A-Za-z0-9][A-Za-z0-9_]{2,32}$
```

#### Flag prefix

```text
Flag Prefix cannot be empty.
```

```text
Flag Prefix cannot contain full stops.
```

```text
Flag Prefix cannot contain spaces.
```

#### Worker type

```text
Worker Type cannot be empty.
```

### Launching a deployment

When you press the `Launch deployment` button, the Deployment Launcher {{does something under the hood that you need to explain}}. You should see a notification similar to the following at the bottom of the deployment launcher window:

<img src="{{assetRoot}}assets/modules/deployment-launcher/upload-progress.png" style="margin: 0 auto; width: 100%; display: block;" />

The standard output and standard error from the command is forwarded to the Unity Console.

<%(Callout message="
A deployment has launched when:

* You see the following message in your Unity Editor Console window: `{{add the error here pls}}`
* The notification at the bottom of the Deployment Launcher window has disappeared.
* {{need to talk about multiple notifications coming up - one for the parent dpl and one for each child}}
")%>
