# SpatialOS GDK for Unity FAQs

[2018-Aug-02]

Since the SpatialOS GDK for Unity entered pre-alpha in June 2018, we've had a number of questions come up regularly so we've gathered them together, with their answers, in an FAQ to make them easy to find. If the answer to your question is not here, or if you have any follow-up questions, please don’t hesitate to raise an [issue](https://github.com/spatialos/UnityGDK/issues) or a post in [our Discord](https://discordapp.com/invite/SCZTCYm) (use the **#unity** channel to make sure we see it).

## 1. When can I use the GDK for Unity to develop a new game?

We aim to release the GDK in [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) in October 2018. It’s at this point that we recommend you start prototyping any new Unity SpatialOS games on the GDK, rather than on our existing [SDK](https://github.com/spatialos/UnitySDK). The GDK will be the best choice then, despite its relative immaturity, because it’s both the future of Unity and SpatialOS, and it has a very active dev team: the dev team will be able to respond quickly to user feedback and feature requests. 

<br/>
The alpha release will include:

* The ability to write your game code using either of our two first class workflows: MonoBehaviours or the ECS.

* A Starter Project that demonstrates best practices and on which you can build your game.

* Git tag versioning and release notes.

* A dev team hungry for your feedback.

<br/>
The alpha release will work with existing tools in the SpatialOS ecosystem, including:

* Tools to iterate quickly on your build, such as the [Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector) and its [Log view](https://docs.improbable.io/reference/latest/shared/operate/logs#cloud-deployments). 

* Live services such as [Deploying to the cloud](https://docs.improbable.io/reference/latest/shared/deploy/deploy-cloud) and the [Inspector Metrics View](https://docs.improbable.io/reference/latest/shared/operate/metrics#metrics).

<br/>
The alpha release will not include:

* Any guarantee that APIs will not change!

* A documented Unity SDK to GDK migration path - this is planned for later releases.

* Production-ready performance or optimizations.


## 2. What are the biggest development experience wins with the GDK’s Core?

The biggest wins are fast iteration, native workflows, high performance and customizability. Check out the short [Summary of key features](key-features.md).

## 3. What is the Unity ECS and do I have to use it?

Unity describe their Entity Component System (ECS) as "*a new model for writing high-performance code by default*". 

It is a means of separating data (Entities and Components) from logic that works on this data (Systems). This allows you to allocate and process data more efficiently, as well as build logic systems to handle this data separately.

With our GDK for Unity, you have two workflow options to build your gameplay features:

1. A MonoBehaviour-centric workflow: takes advantage of Unity’s fully developed MonoBehaviour tooling, workflow and APIs. 

2. An ECS workflow: takes advantage of the ECS development paradigm and associated performance improvements.

Planned for our alpha release, our MonoBehaviour workflow supports an API similar to the [Readers and Writers API](https://github.com/spatialos/UnitySDK/blob/master/docs/interact-with-world/interact-components.md#example-of-monobehaviours-component-readers-and-writers) in the [SpatialOS SDK for Unity](https://github.com/spatialos/UnitySDK). Follow [this Trello card](https://trello.com/c/ytJ6fZZT/10-monobehaviour-workflow) to keep track of our development progress. 

 

If you are interested in finding out more about our ECS workflow, these topics in our documentation are a good place to start: [Component updates](../component-data.md), [Events](../events.md), [Commands](../commands.md) and [Reactive components](../reactive-components.md).

For more information on Unity ECS, see Unity’s page on the [Unity ECS](https://unity3d.com/unity/features/job-system-ECS) and their [vision](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_principles_and_vision.md) for it going forward.

## 4. How will Feature Modules be distributed? How customizable will they be?

Feature Modules will be available as source, and you can modify or extend them as much as you like. 

As for distribution, we’re still experimenting! We’re currently leaning towards Unity Packages via the [Unity Package Manager](https://blogs.unity3d.com/2018/05/04/project-management-is-evolving-unity-package-manager-overview/). However, based on user feedback, we’re exploring other options, such as [Asset Packages](https://docs.unity3d.com/Manual/AssetPackages.html) via the Unity Asset Store.

## 5. Is the GDK for Unity on Windows only or also on MacOS X? 

The GDK supports both Windows and MacOS X for development. We have experienced some stability issues with the Unity ECS on Mac OS, however this should be resolved by Unity in the near future.

