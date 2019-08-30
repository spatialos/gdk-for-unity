<%( TOC )%>

# Requirable Inspector

The Requirable Inspector is a debugging extension for the Unity Inspector, which helps visualize the use of [`[Require]`]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/lifecycle) in MonoBehaviour components.

## Usage

To use the extension, ensure the package is added to your project and enter Play mode.

1. Select any Unity GameObject that is linked to a SpatialOS entity in your Hierarchy.
1. Open the Unity Inspector.
1. Browse to a MonoBehaviour that you want to inspect.
1. Select the ▶ button next to `SpatialOS`

<img src="{{assetRoot}}assets/modules/debug/inspector.png" style="margin: 0 auto; width: auto; display: block;" />

## Explanation

The extension lists all objects that are required through the [`[Require]`]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/lifecycle) attributes. The dot in front of each type indicates whether an object is available:

* Green: The object is available.
* Grey: The object is not available.

Each object that you require may have requirements that need to be fulfilled, before the GDK is able to make the object available and enable this MonoBehaviour.

If you have a grey indicator on a [`Reader`]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/overview) it can be due to one the following reasons:

* The [entity]({{urlRoot}}/reference/glossary#spatialos-entity) does not have the [component]({{urlRoot}}/reference/glossary#spatialos-component) present.
* The [worker instance]({{urlRoot}}/reference/glossary#worker) does not have [read access]({{urlRoot}}/reference/glossary#read-access) on the [component]({{urlRoot}}/reference/glossary#spatialos-component).

A grey indicator for a [`Writer`]({{urlRoot}}/workflows/monobehaviour/interaction/reader-writers/overview) or [`CommandReceiver`]({{urlRoot}}/workflows/monobehaviour/interaction/commands/component-commands) can be due to:

* The above `Reader` reasons.
* The [worker instance]({{urlRoot}}/reference/glossary#worker) does not have [authority]({{urlRoot}}/reference/glossary#authority) over the [component]({{urlRoot}}/reference/glossary#spatialos-component).
