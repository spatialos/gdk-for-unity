<%( TOC )%>

# Manage deployments

Open the Deployment Launcher window by selecting **SpatialOS** > **Deployment Launcher** from the Unity Editor menu.

## UI walkthrough

The section of the Deployment Launcher window that you use to manage live deployments can be found under the **Live Deployments** label.

Initially, the Deployment Launcher does not know of any deployments.

<img src="{{assetRoot}}assets/modules/deployment-launcher/manage-deployments-empty.png" style="margin: 0 auto; width: auto; display: block;" />

| Button name | Description |
| --- | --- |
| Refresh | When pressed, the Deployment Launcher retrieves a list of cloud deployments currently running in the SpatialOS project. |

### Listed deployments

When there are live cloud deployments running, this section is updated with a list of these live deployments and some metadata about them. Deployment metadata is **read-only**.

<img src="{{assetRoot}}assets/modules/deployment-launcher/manage-deployments.png" style="margin: 0 auto; width: auto; display: block;" />

| Metadata | Example |
| --- | --- |
| Deployment name | The name of the given cloud deployment. For example, `testmobile13052019`. |
| Start Time | The local time that the deployment was started. |
| Region | The geographical region that the deployment is running in. |
| Connected Workers | A list of worker types and how many are connected to the deployment. |
| Tags | A list of metadata tags added to the deployment. |

| Button name | Description |
| --- | --- |
| 🌐 | When pressed, the Console page for the selected cloud deployment opens in your browser. |
| Stop deployment | When pressed, this begins to stop a deployment chosen from the list of running cloud deployments. |

## Expected behaviour

### Listing deployments

When you press the `Refresh` button, the Deployment Launcher gathers a list of running cloud deployments within the SpatialOS project.

You should see a notification similar to the following at the bottom of the deployment launcher window:

```text
Listing deployments in project "<project_name>".
Assembly reloading locked.
```

The standard output and standard error from the command is forwarded to the Unity Console.

<%(Callout message="
A list of deployments has been retrieved successfully when:

* The notification at the bottom of the Deployment Launcher window has disappeared.
")%>

### Stopping deployments

When you press the `Stop deployment` button, the Deployment Launcher sends a request to stop the deployment chosen from the adjacent drop-down option.

<img src="{{assetRoot}}assets/modules/deployment-launcher/stop-deployments-choice.png" style="margin: 0 auto; width: auto; display: block;" />

<%(Callout type="warn" message="

The list of deployments and corresponding metadata are **not** automatically refreshed. Ensure that you have refreshed the list manually before performing any action.

")%>

You should see a notification similar to the following at the bottom of the deployment launcher window:

```text
Stopping deployment "<deployment_name>".
Assembly reloading locked.
```

The standard output and standard error from the command is forwarded to the Unity Console.

<%(Callout message="
The deployment has stopped successfully when:

* You see the following message in your Unity Editor Console window: `Stopped deployment: <deployment_name> successfully.`
* The stopped deployment has been removed from the list of live deployments.
* The notification at the bottom of the Deployment Launcher window has disappeared.
")%>
