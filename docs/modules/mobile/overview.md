<%(TOC)%>

# Mobile Feature Module

<%(Callout type="warn" message=" Mobile support is in [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages). Please follow our [Roadmap](https://github.com/spatialos/gdk-for-unity/projects/1) to learn more about updates to this in future releases.")%>

<%(Callout message="
Before starting with mobile development, make sure you have read:

  * [The SpatialOS GDK for Unity]({{urlRoot}}/reference/overview)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

The Mobile Feature Module enables you to develop games for Android and iOS. SpatialOS games are cross-platform by default, so PC and mobile users can play together in the same deployment.

## Installation

### 1. Add the package

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html#specifying-a-local-package-location).

The Mobile Feature Module `package.json` can be found in the [`gdk-for-unity` repository](https://github.com/spatialos/gdk-for-unity) at:

```text
workers/unity/Packages/com.improbable.gdk.mobile/package.json
```

### 2. Reference the assemblies

The Mobile Feature Module contains a single [assembly definition file](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) which you must reference. This process differs depending on whether you have an assembly definition file in your own project or not.

**I have an assembly definition file**

Open your assembly definition file and add `Improbable.Gdk.Mobile` to the reference list.

**I don't have an assembly definition file**

If you don't have an assembly definition file in your project, Unity will automatically reference the `Improbable.Gdk.Mobile` assembly for you.

## Getting started with your mobile client-worker

  * [Ways to run your client]({{urlRoot}}/modules/mobile/run-client)
  * [Set up Android support for the GDK]({{urlRoot}}/modules/mobile/setup-android)
  * [Set up iOS support for the GDK]({{urlRoot}}/modules/mobile/setup-ios)
  * [Prepare your project]({{urlRoot}}/modules/mobile/prepare-project)
  * [Connect to a local deployment]({{urlRoot}}/modules/mobile/local-deploy)
  * [Connect to a cloud deployment]({{urlRoot}}/modules/mobile/cloud-deploy)
