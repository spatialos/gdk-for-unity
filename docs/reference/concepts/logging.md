<%(TOC max="3")%>

# Logging

The SpatialOS GDK for Unity provides an [`ILogDispatcher`]({{urlRoot}}/api/core/i-log-dispatcher) interface for logging, which provides more flexibility to handle logs based on the worker instance and allows you to attach additional context to your logs.

There are two implementations of this interface in the Core package:

*  The [`LoggingDispatcher`]({{urlRoot}}/api/core/logging-dispatcher), which logs to the Unity console.
*  The [`ForwardingDispatcher`]({{urlRoot}}/api/core/forwarding-dispatcher), which logs to the Unity console and sends it to the SpatialOS Console.

> All workers in the FPS Starter Project and the Blank Project use the [`ForwardingDispatcher`]({{urlRoot}}/api/core/forwarding-dispatcher) by default.

## Setting up a log dispatcher

When you create a worker using the [`WorkerConnector`]({{urlRoot}}/api/core/worker-connector), you pass in an `ILogDispatcher` instance. This associates the `ILogDispatcher` instance with that worker.

```csharp
private async void Start()
{
    var builder = new SpatialOSConnectionHandlerBuilder()
        .SetConnectionParameters(CreateConnectionParameters(WorkerUtils.UnityClient));
        .SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityClient)))

    // Associate a new ForwardingDispatcher instance with this worker.
    await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
}

```

This instance handles all logging from the Core and Feature Modules for that worker and it is available for you to use through the methods described below

## Accessing the log dispatcher

There are a few ways you can access the `ILogDispatcher` instance that is associated with your worker.

### In a MonoBehaviour

If your GameObject is linked to a SpatialOS entity through the [GameObject Creation Feature Modules]({{urlRoot}}/modules/game-object-creation/overview), you can either:

##### 1. Require the `ILogDispatcher`. 

This is injected when the `MonoBehaviour` is enabled:

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [Require] private ILogDispatcher logger;

    private void OnEnable() 
    {
        // The logger is now available.
        logger.HandleLog(...);
    }
}
```

##### 2. Access it through the `LinkedEntityComponent` MonoBehaviour 

This MonoBehaviour will be on the linked GameObject:

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    private ILogDispatcher logger;

    private void OnEnable() 
    {
        logger = GetComponent<LinkedEntityComponent>().WorkerSystem.LogDispatcher;
        logger.HandleLog(...);
    }
}
```

### In the ECS

You can access the dispatcher through the [`WorkerSystem`]({{urlRoot}}/api/core/worker-system):

```csharp
public class MySystem : ComponentSystem 
{
    private ILogDispatcher logger;

    protected override void OnCreate()
    {
        logger = World.GetExistingSystem<WorkerSystem>().LogDispatcher;
        logger.HandleLog(...);
    }

    ...
}
```

## Using the log dispatcher

The dispatcher provides a single `HandleLog` function, which takes two arguments:

1. `LogType`, which specifies the verbosity level of the log (e.g. `UnityEngine.LogType.Error`)
2. `LogEvent`, which stores the message, structured logging data, and the context of the log. See [the API documentation]({{urlRoot}}/api/core/log-event) for usage details.

For example:

```csharp
logger.HandleLog(LogType.Error, 
    new LogEvent("Custom error message.")
        .WithField(LoggingUtils.LoggerName, LoggerName)
        .WithField(LoggingUtils.EntityId, entityId)
        .WithField("CustomKey", "CustomValue"));
```

### The `ForwardingDispatcher`

The `ForwardingDispatcher` converts the Unity `LogType` enum to the SpatialOS `LogLevel` enum using the following table:

| Unity               | SpatialOS          |
| ---                 | ---                |
| `LogType.Exception` | `LogLevel.Error`   |
| `LogType.Error`     | `LogLevel.Error`   |
| `LogType.Assert`    | `LogLevel.Error`   |
| `LogType.Warning`   | `LogLevel.Warning` |
| `LogType.Log`       | `LogLevel.Info`    |

> **Note:** By default, messages with log level `LogType.Log`  and sent using the [`ForwardingDispatcher`]({{urlRoot}}/api/core/forwarding-dispatcher) are not forwarded to SpatialOS. You can change this by instantiating the [`ForwardingDispatcher`]({{urlRoot}}/api/core/forwarding-dispatcher) with a different `minimumLogLevel` parameter.

The `ForwardingDispatcher` recognises two special structured logging keys that can be used with the `LogEvent.WithField(string key, object value)` method:

* `LoggingUtils.LoggerName`, which specifies where the log was sent from.
* `LoggingUtils.EntityId`, which links the log to a specific entity. This lets you filter for a particular entity's logs using the [Logger](https://docs.improbable.io/reference/<%(Var key="worker_sdk_version")%>/shared/operate/logs#cloud-deployments).

Other context variables are formatted into a string and sent with the log message as normal.

## Creating your own log dispatcher

To create your own log dispatcher, create a new class which implements [`ILogDispatcher`]({{urlRoot}}/api/core/i-log-dispatcher):

```csharp
public class MyCustomDispatcher: ILogDispatcher
{
    public Worker Worker { get; set; }

    public string WorkerType { get; set; }

    public void HandleLog(LogType type, LogEvent logEvent)
    {
        // Handle logs however you please
    }
}
```

To use this dispatcher in a specific worker, provide it as an argument when calling `Connect` in your worker connector as [described above](#setting-up-a-log-dispatcher).