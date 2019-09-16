<%( TOC )%>

# Network Analyzer

The Network Analyzer is a Unity Editor window which displays live bandwidth usage for a worker broken down by component type. This window can assist you with finding networking bottlenecks in your game.

<%(Callout type="info" message="
Note that the bandwidth reported in the Network Analyzer is calculated from the serialized data before it is put on the wire. This does not include any overhead from the network protocol.

This can be used to indicate how changes will affect bandwidth, but should not be relied upon for precise measurement or cost.

Please use the [cloud metrics](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/metrics) if you wish to have more precise measurements.
")%>

## Usage

To open the window, ensure the `Debug` package is added to your project and select **SpatialOS** > **Window** > **Network Analyzer** from the Unity Editor menu.

Enter Play mode to start the data collection.

## Explanation

The window lists all SpatialOS components in your project and displays bandwidth usage for component update ops for each component. The data shown is collected over 60 frames and then normalized to show an approximate reading per second.

<img src="{{assetRoot}}assets/modules/debug/network-analyzer.png" style="margin: 0 auto; width: auto; display: block;" />

### Metrics

| Metric     | Description                                                                                             |
|------------|---------------------------------------------------------------------------------------------------------|
| `Op/s in`  | This metric describes the number of component update ops that this worker is receiving per second.      |
| `KB/s in`  | This metric describes the bandwidth used by component updates that this worker is receiving per second. |
| `Op/s out` | This metric describes the number of component update ops that this worker is sending per second.        |
| `KB/s out` | This metric describes the bandwidth used by component updates that this worker is sending per second.   |

> **Note:** Component updates are used for both component field changes as well as component events.

### Worker selection

The Network Analyzer only shows the bandwidth usage of a single worker at a time. If you have multiple workers running in the Editor, you can select which worker you wish to see data for by using the dropdown at the top of the window.

<img src="{{assetRoot}}assets/modules/debug/network-analyzer-worker-selection.png" style="margin: 0 auto; width: auto; display: block;" />

### Component update loopback

The GDK for Unity has component update loopback turned **on** by default. This means that every update that is sent is also received by that worker. One effect of this is that you will see these loopbacked updates in the bandwidth inspector, which artificially inflates the incoming bandwidth usage. You should keep this in mind when interpreting the data presented in the Network Analyzer.
