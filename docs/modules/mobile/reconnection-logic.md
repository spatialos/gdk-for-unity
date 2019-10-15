<%(TOC)%>

# Reconnection logic

Mobile applications present new challenges in terms of handling client connections. A client may disconnect due to an unstable connection or when the application is put into a background state by the OS. We want to dive a bit deeper into the causes of a disconnect and how you can handle it in your game.

## Why clients disconnect

### Unstable connection 

When creating a mobile application, you need to ensure that [client-workers]({{urlRoot}}/reference/glossary#client-worker) are able to run the game even with an unstable connection. To achieve this, you can implement connection handling to ensure that those client-workers are able to reconnect.

If a client-worker's connection is too unstable, they might not be able to connect to your game or might disconnect at any time. You should provide a connection flow that allows them to gracefully disconnect and handle reconnection when they have a more stable connection.

### Pausing of applications

When pausing an application or putting it into the background, there are two possible scenarios that can cause a disconnect:

#### 1. The device OS closes the application.

If the application has been closed, the user has to restart the application. The user will be brought back to the start screen the next time the application is opened and from there on, the application can connect and load any data necessary.

#### 2. The client stops sending data. 

This scenario is a bit more tricky. The application is still alive, however the OS may decide to not run the game or send any data while the application is in the background. [SpatialOS]({{urlRoot}}/reference/glossary#spatialos-runtime) will close the connection if it doesn't receive any messages from a client-worker for a period of time. You need to add additional reconnection logic to your game to handle this scenario. 

In the end, this is a very game-specific question that depends entirely on what you want to offer to your users. For example, the FPS Starter Project implements the simplest solution: a disconnect takes you back to the start screen, from where you can reconnect to the game.

## How to detect a disconnect

[Heartbeating]({{urlRoot}}/modules/player-lifecycle/heartbeating) is a technique used to verify that your client is still connected. If there are too many failed heartbeats, there are two ways a client-worker may be seen as disconnected:

### 1. SpatialOS believes you disconnected

To ensure that a worker is still connected, the worker has to send messages to SpatialOS at a set interval. This indicates that the worker is still alive. If SpatialOS doesn't receive any message for a while, it will close the connection to that worker. 

Whenever the connection gets closed on a worker, that worker receives a `DisconnectOp` object. It contains the reason behind the disconnection and triggers a disconnect event in the GDK.

You can listen for this event using the `Worker` object inside the `WorkerConnector` class in order to perform your disconnection logic.

```csharp
namespace YourGame
{
    public class MobileClientWorkerConnector : WorkerConnector
    {
    	...

        protected override void HandleWorkerConnectionEstablished()
        {
             Worker.OnDisconnect += OnDisconnected;
        }

        private void OnDisconnected(string reason)
        {
            Worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent($"Worker disconnected")
                .WithField("WorkerId", Worker.WorkerId)
                .WithField("Reason", reason));
        }
    }
}
```

### 2. The player lifecycle module believes you disconnected 

Our [Player Lifecycle module]({{urlRoot}}/modules/player-lifecycle/overview) allows you to easily create players and uses the heartbeating technique to detect and delete the player entity of any unresponsive client-workers. In mobile applications, this can result in entities being destroyed even though the worker is still connected to SpatialOS. If this happens, that client-worker won't receive a `DisconnectOp`.

When the player entity gets deleted, the client-worker might end up with no entities to be authoritative over and therefore unable to check out any entities. This leaves the client-worker in a bad state. There are two ways to recover from this: 

1. Requesting a new player entity.
1. Reconnecting the client-worker.
