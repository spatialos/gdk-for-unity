<%(TOC)%>

# Mobile Feature Module

<%(Callout type="warn" message=" Mobile support is in [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages). Please follow our [Roadmap](https://github.com/spatialos/gdk-for-unity/projects/1) to learn more about updates to this in future releases.")%>

<%(Callout message="
Before starting with mobile development, make sure you have read:

  * [The SpatialOS GDK for Unity]({{urlRoot}}/reference/overview)
  * [Workers in the GDK]({{urlRoot}}/reference/concepts/worker)
")%>

## Developing SpatialOS games for Android and iOS

This feature module enables you to develop games for Android and iOS. SpatialOS games are cross-platform by default, so PC and mobile users can play together in the same deployment.

## How to enable mobile support

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html#specifying-a-local-package-location).

The Mobile Feature Module `package.json` can be found in the [`gdk-for-unity` repository](https://github.com/spatialos/gdk-for-unity) at:

```text
workers/unity/Packages/com.improbable.gdk.mobile/package.json
```

## Getting started with your mobile client-worker

  * [Choose how you want to run your mobile client-worker]({{urlRoot}}/modules/mobile/run-client)
  * [Set up Android support for the GDK]({{urlRoot}}/modules/mobile/setup-android)
  * [Set up iOS support for the GDK]({{urlRoot}}/modules/mobile/setup-ios)
  * [Create a mobile worker connector]({{urlRoot}}/modules/mobile/worker-connector)
  * [Connect your mobile client-worker to a local deployment]({{urlRoot}}/modules/mobile/local-deploy)
  * [Connect your mobile client-worker to a cloud deployment]({{urlRoot}}/modules/mobile/cloud-deploy)
