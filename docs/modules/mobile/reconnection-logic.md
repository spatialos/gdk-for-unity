<%(TOC)%>

# Reconnection logic

Mobile applications present new challenges in terms of handling client connections. A client may disconnect due to unstable connections or when the application is put into a background state by the OS. We want to dive a bit deeper into the causes of a disconnect and how you can handle it in your game.

## Why clients disconnect

### Unstable connection 

When creating a mobile application, you need to ensure that clients are able to play the game even with an unstable connection.

If a client's connection is too unstable, they might not be able to connect to your game or might disconnect at any time. You should provide a connection flow that allows them to disconnect properly, and handle reconnection as soon as they obtain better reception.

### Pausing of applications

When pausing an application or putting it into the background, there are two possible scenarios that can cause a disconnect:

#### 1. The device OS closes the application.

If the application has been closed, the user has to restart the application. The user will be brought back to the start screen the next time the application is opened and from there on, the application can connect and load any data necessary.

#### 2. The client stops sending data. 

This scenario is a bit more tricky. The application is still alive, however the next time the user opens the application, SpatialOS will have already closed the connection due to not receiving any data from the client. This can happen due to the OS deciding not to send any data while the application is in the background. You need to add additional reconnection logic to your game to be able to handle this scenario. 

In the end this is a very game-specific question that depends entirely on what you want to offer to your users. The FPS Starter Project implements the simplest solution: a disconnect takes you back to the start screen, from where you can reconnect to the game.

## How to detect a disconnect

[Heartbeating]({{urlRoot}}/modules/player-lifecycle/heartbeating) is a technique used to verify that your client is still connected. If there are too many failed heartbeats, there are two ways your client may be seen as disconnected:

### 1. SpatialOS believes you disconnected

To ensure that your client is still connected, the client has to send a message to SpatialOS. This indicates that the client is still alive. If SpatialOS doesn't receive a message in a while, it will close the connection to your client. 
Whenever the connection gets closed on a worker, you receive a `DisconnectOp`. This object contains the reason behind the disconnection and triggers a disconnect event in the GDK.

You can listen for this event using the `Worker` object inside the `WorkerConnector` class in order to perform any kind of disconnection logic necessary.

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

Our player lifecycle module allows you to easily create players and also implements heartbeating to delete unresponsive players. In moible application, this can result in players being destroyed that are still connected to SpatialOS. If this happens, you won’t receive a `DisconnectOp`.

When this happens, your client might end up with no entities to be authoritative over and therefore unable to check out any entities. The client-worker's world will appear to be empty.

This needs to be handled by either: 

1. Requesting a new player entity.
1. Reconnecting the client-worker.
