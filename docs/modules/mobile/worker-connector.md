# Create a mobile connector script

<%(Callout message="
Before reading this document, make sure you have read:

  * [Mobile support overview]({{urlRoot}}/modules/mobile/overview)
  * [Creating workers with WorkerConnector](https://docs.improbable.io/unity/alpha/reference/workflows/monobehaviour/creating-workers)
")%>

If you are using one of our [Starter Projects]({{urlRoot}}/reference/glossary#starter-project), you can skip this section, as you already have one in your project.

If you [added the GDK]({{urlRoot}}/projects/myo/setup) to an existing Unity project rather than using a Starter Project, then you need to create and add a MonoBehaviour script to your mobile client-worker GameObject. To do this:

1. Create a MonoBehaviour script which inherits from the [`DefaultMobileWorkerConnector`]({{urlRoot}}/api/mobile/mobile-worker-connector) and include the functionality you want. This scripts contains support for both Android and iOS. You can base your implementation on the one in our [Blank Starter Project](https://github.com/spatialos/gdk-for-unity-blank-project/blob/develop/workers/unity/Assets/Scripts/Workers/MobileClientWorkerConnector.cs).
1. In your Unity Editor, add the MonoBehaviour script to your mobile client-worker GameObject.
