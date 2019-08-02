<%( TOC )%>

# Require inspector extension

The Require inspector extension is a debugging extension for the Unity inspector, which helps with visualizing the use of `[Require]` in MonoBehaviour components.

## Usage

To use the extension, ensure the package is added to your project and enter Play mode.

1. Select any SpatialOS Gameobject in your Hierarchy.
1. Open the Inspector.
1. Browse to a MonoBehaviour that you want to inspect.
1. Select the ▶ button next to `SpatialOS`

<img src="{{assetRoot}}assets/modules/debug/inspector.png" style="margin: 0 auto; width: auto; display: block;" />

## Explanation

The list of types displayed in the inspector, are the types that the MonoBehaviour requires through its `[Require]` attributes. All of these requirements have to be available, before the MonoBehaviour will be enabled.
The dot in front of each type indicates the current state of this requirement. Green means it is available, and grey means it is not.

If you have a grey indicator on a `Reader` type it can be due to one the following reasons:

* The entity does not have the Component present.
* The Worker does not have read access on the component.

A grey indicator for a `Writer` type is either due to the above reasons, or due to the worker not having authority over the component at that moment.
