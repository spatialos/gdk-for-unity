[//]: # (TODO: Move this doc from the ecs folder)
<%(TOC)%>
#  Logs
 _This document relates to both the [MonoBehaviour and ECS workflows]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

The SpatialOS GDK for Unity uses a custom `ILogDispatcher` interface instead of `UnityEngine.Debug`, which gives more flexibility to handle logs separately in different workers and gives more context when handling the logs. There are two provided implementations of this interface:

*  `LoggingDispatcher`, which simply logs to the Unity console
*  `ForwardingDispatcher`, which logs to the Unity console and sends it to the SpatialOS Console
    * Note: By default, messages with the log level `LogType.Log` are not sent to SpatialOS. This can be changed by instantiating the `ForwardingDispatcher` with a different `minimumLogLevel` parameter.

All workers use the `ForwardingDispatcher` by default in the Playground. If you want to use the `LoggingDispatcher`, see the last step of [Creating and using your own dispatcher]({{urlRoot}}/content/ecs/logging#creating-and-using-your-own-dispatcher).

## Using the ILogDispatcher

You can access the dispatcher through the `WorkerSystem`. The dispatcher provides a single `HandleLog` function, which takes two arguments:

* `LogType` (e.g. `UnityEngine.LogType.Error`)
* `LogEvent`, which stores the message and other context variables in the Data dictionary

There are two log context variables:

* `LoggingUtils.LoggerName`, which specifies where the log was sent from.
* `LoggingUtils.EntityId`, which links the log to a specific entity. When running the game in the cloud using the `ForwardingDispatcher`, this lets you filter for a particular entity's logs using the Inspector and Logger.

These are automatically picked up by the `ForwardingDispatcher` if provided. Other context variables are formatted in a string and sent with the log message.

For example, if you want to use one of the dispatchers in a system, you can use the following code:

```csharp
var workerSystem = World.GetExistingManager<WorkerSystem>();

workerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
        "Custom error message.")
    .WithField(LoggingUtils.LoggerName, LoggerName)
    .WithField(LoggingUtils.EntityId, entityId)
    .WithField("CustomKey", "CustomValue"));
```

The `LogEvent` class allows construction of enhanced log messages.

* Use the `WithField` method to set additional information to be displayed with the log message.
* Use the `WithContext(UnityEngine.Object)` method to add a context object to be passed into the Unity console.

**Note**: For `LogType.Exception`, add a relevant exception to the `LogEvent` class using the `WithException(Exception)` method. The `WithException` method will be ignored for other `LogType` values.

## Creating and using your own dispatcher

To create your own dispatcher, make a new class which implements `ILogDispatcher`:

```csharp
public class MyCustomDispatcher: ILogDispatcher
{
    public void HandleLog(LogType type, LogEvent logEvent)
    {
        // Handle Log
    }
}
```


If you want a specific worker to use the dispatcher, pass it to the `Connect` method of the `WorkerConnectorBase` class instead of the default dispatcher when connecting to SpatialOS:

```csharp
public class ClientWorkerConnector : WorkerConnectorBase
{
    private async void Start()
    {
        /* ... */
        await Connect(WorkerUtils.UnityClient, new MyCustomDispatcher()).ConfigureAwait(false);
    }
}
```
