<%(TOC)%>
# Player Lifecycle Feature Module

The Player Lifecycle Feature Module provides you with an implementation of player creation and simple player lifecycle management. You can find the Feature Module in the GDK repository [here](https://github.com/spatialos/gdk-for-unity/tree/master/workers/unity/Packages/com.improbable.gdk.playerlifecycle).

The module consists of:

* Systems to send player creation requests and handle responses.
* Systems to send and acknowledge player heartbeats. [What's a heartbeat?]({{urlRoot}}/modules/player-lifecycle/heartbeating)
* A static [`PlayerLifecycleConfig`]({{urlRoot}}/api/player-lifecycle/player-lifecycle-config) class to configure variables used by the module.
* A static [`PlayerLifecycleHelper`]({{urlRoot}}/api/player-lifecycle/player-lifecycle-helper) class containing helper methods to more easily set-up and use the Feature Module.

You can find more information about the available systems and the APIs they offer in our [API reference docs]({{urlRoot}}/api/player-lifecycle-index).
