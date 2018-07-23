**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----

##  Logging

The Unity GDK uses a custom `ILogDispatcher` interface instead of `UnityEngine.Debug`, which gives more flexibility to handle logs separately in different workers and gives more context when handling the logs. There are two provided implementations of this interface: 

*  `LoggingDispatcher`, which simply logs to the Unity console
*  `ForwardingDispatcher`, which logs to the Unity console and forwards it to the SpatialOS Console

All workers use the `ForwardingDispatcher` by default. To replace it with the `LoggingDispatcher`, see the last step of [Creating and using your own dispatcher](#creating-and-using-your-own-dispatcher).

### Using the dispatcher

You can access the dispatcher through the `MutableView`. The dispatcher provides a single `HandleLog` function, which takes two arguments:

* `LogType` (e.g. `UnityEngine.LogType.Error`)
* `LogEvent`, which stores the message and other context variables in the Data dictionary

There are two log context variables: 

* `LoggingUtils.LoggerName`, which specifies where the log was sent from
* `LoggingUtils.EntityId`, which links the log to a specific entity

These are automatically picked up by the `ForwardingDispatcher` if provided. Other context variables are formatted in a string and sent with the log message.

For example, if you want to use one of the dispatchers in a system, you can use the following code:

```csharp
var worker = WorkerRegistry.GetWorkerForWorld(World);

worker.View.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
        "Custom error message.")
    .WithField(LoggingUtils.LoggerName, LoggerName)
    .WithField(LoggingUtils.EntityId, entityId)
    .WithField("CustomKey", "CustomValue")
```

### Creating and using your own dispatcher

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


If you want a specific worker to use the dispatcher, pass it to the constructor of the `WorkerBase` class instead of the default dispatcher:

```csharp
public class UnityClient : WorkerBase
{
    public UnityClient(string workerId, Vector3 origin) : base(workerId, origin, new MyCustomDispatcher())
    {
    }
}
```