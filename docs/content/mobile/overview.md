<%(Callout type="alert" message="
Mobile support is in [pre-alpha (SpatialOS documentation)](https://docs.improbable.io/reference/13.3/shared/release-policy#maturity-stages). Currently, you can only connect your mobile [client-workers](https://docs.improbable.io/reference/latest/shared/glossary#client-worker) to a local deployment: you **cannot** connect mobile client-workers to a cloud deployment. Please follow our [Roadmap](https://github.com/spatialos/gdk-for-unity/projects/1) to learn more about updates to this in future releases.")%>

# Mobile Support Overview

Before starting with mobile development, make sure you are familiar with

  * [The SpatialOS GDK for Unity]({{urlRoot}}/content/intro-reference)
  * [The different workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)
  * [Workers in the GDK]({{urlRoot}}/content/workers/workers-in-the-gdk)

## Developing SpatialOS games for Android and iOS

The SpatialOS GDK for Unity contains a [mobile feature module]({{urlRoot}}/content/modules/core-and-feature-module-overview#mobile-support-module) which enables you to develop games for Android and iOS. All feature modules work with mobile devices.

Because your [SpatialOS World](https://docs.improbable.io/reference/latest/shared/glossary#spatialos-world), the canonical source of truth about your game, is distinct from the view of that world that your client-workers visualise, you can create cross-platform multiplayer games with ease.

## Getting started with your Android client-worker

  * [Setting up Android support for the GDK]({{urlRoot}}/content/mobile/android/setup)
  * [Choose the right way to test your Android client-worker]({{urlRoot}}/content/mobile/android/ways-to-test)
  * [Connect your Android client-worker to a local deployment]({{urlRoot}}/content/mobile/android/local-deploy)
  * [Connect your Android client-worker to a cloud deployment]({{urlRoot}}/content/mobile/android/cloud-deploy)

## Getting started with your iOS client-worker

  * [Setting up iOS support for the GDK]({{urlRoot}}/content/mobile/ios/setup)
  * [Choose the right way to test your iOS client-worker]({{urlRoot}}/content/mobile/ios/ways-to-test)
  * [Connect your iOS client-worker to a local deployment]({{urlRoot}}/content/mobile/ios/local-deploy)
  * [Connect your iOS client-worker to a cloud deployment]({{urlRoot}}/content/mobile/ios/cloud-deploy)
