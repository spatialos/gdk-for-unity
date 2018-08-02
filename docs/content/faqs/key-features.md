
# SpatialOS GDK for Unity key features

[2018-Aug-02]

The SpatialOS GDK for Unity enables you to create online games that were previously impossible without specialized staff and skill sets. Its key features are based around fast development iteration, Unity-native workflows, high performance and customizability. In addition, we develop in the open, our community shaping what we do.

## Fast development iteration

Development iteration speed is critically important in creating games; fast iteration means you can focus on ***finding the fun*** in your game, so we prioritised this when designing the GDK.

#### Test client and server(s) in one Editor

The GIF below shows the game client and server running simultaneously in the same Unity Editor. (Without the SpatialOS GDK, you need to run one Unity Editor for the client, and one Unity Editor for the server simultaneously.) Running both in one Editor enables you to iterate on development of the entirety of your project, client *and* server code, in one Editor instance.
<br/>
<br/>

![The game client and server running simultaneously in the same Unity Editor](../../assets/key-features-page-client-server.gif)
*The game client and server running simultaneously in the same Unity Editor*

In the single Editor instance, you can configure the *spatial offset* between the client and server, placing them adjacent to one another, or even occupying the same space. Unity is currently investigating the ability to run separate physics scenes in the same Editor. When they introduce this, we plan to simulate the client and server in separate physics scenes.

In the near future, we aim to support multiple servers in the same Editor, allowing fast, local testing of gameplay across server boundaries.

#### Iterate on game code without rebuilding executables.

With the SpatialOS GDK, you’ll be able to iterate on your game code with a Unity refresh, instead of a full rebuild of the executables. In the GIF below, we made changes to the movement direction of the cubes. 

In our earlier product, the SpatialOS SDK for Unity, this would have required a full rebuild, and a relaunch of the game client and server.
<br/>
<br/>
![Code changes to the movement direction of cubes are reflected in the Scene windows with a refresh (no rebuild necessary](../../assets/key-features-page-refresh.gif)
*Code changes to the movement direction of cubes are reflected in the Scene windows with a refresh (no rebuild necessary)*

#### SpatialOS schema auto-generation

In future releases, you will be able to save time on writing out classes and structs by annotating the C# classes and structs you use. The [schema](https://docs.improbable.io/reference/latest/shared/glossary#schema) will be automatically generated for you in a standardised format. 

## Native workflows

We know that game developers are deeply familiar with and experienced in using Unity and the Unity Editor, so we are building features within Unity itself as much as possible, allowing you to experience a Unity-native workflow. 

#### MonoBehaviour workflow and callbacks

You can easily inject components into MonoBehaviour and get method callbacks when values change. You can use this to run code in reaction to changes in component values, events fired or commands received. Alternatively, you can get the latest value on each frame, if that’s your preference. 

#### Automatic state synchronization

When you update ECS component values, those changes are automatically synchronized across the network. We recognise that in some instances, you’ll want to use custom update logic, so we’ve made it easy to override the default behavior.

## High performance

We want to the SpatialOS GDK for Unity’s performance to be as fast as possible. A more efficient integration means more resources available to both the client and server; your game uses less resources so you can develop more content.

We are using low allocation APIs, optional GameObjects and the Unity ECS to accomplish this. 

#### Near-zero allocation for component updates

In our older SpatialOS **SDK** for Unity, each component update allocates memory in managed C#. This was problematic as its garbage collection caused loading spikes, manifesting as dropped frames. 

The new SpatialOS **GDK** for Unity directly deserializes networked data into ECS data types (which are largely structs), creating extremely low allocations for networked operations. (This functionality is currently in R&D and is something we’re treating as high priority.)

#### GameObjects are optional

You only use GameObjects when you want to. You have the option to do computation on entity data without the overhead of MonoBehaviours. Built-in functionality, such as the spawning pipeline, makes use of this.   

## Customizability

#### Full source access

At Improbable, we’ve always believed that everyone benefits from open development and that code gets better, faster, when it’s public. We’re developing the SpatialOS GDK for Unity in the open so that you can better understand it, and directly impact its development. We welcome [issue reports](https://github.com/spatialos/UnityGDK/issues) and [feature requests](https://github.com/spatialos/UnityGDK#give-us-feedback) right now, and at later releases we’ll be accepting pull requests directly from the community.

## Publicly-available experimental features

#### Mobile game development

To enable mobile game development support for SpatialOS, we have an experimental branch (`experimental/mobile-support`) for Android on the SpatialOS GDK. We plan to have iOS support for the GDK soon.

