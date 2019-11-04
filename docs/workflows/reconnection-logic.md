<%(TOC)%>

# Handling disconnections

Online games rely on all the clients and the server being able to connect and stay connected during the game. However, a client may disconnect due to an unstable connection or when the application is put into a background state by the OS. In this page, we will dive a bit deeper into the causes of a disconnect and discuss how you can handle it in your game.

## Why do disconnects happen

### Unstable connection 

You need to ensure that [client-workers]({{urlRoot}}/reference/glossary#client-worker) are able to try to connect even when the connect might be more unstable. If the connection becomes too weak or unstable, there is a high risk of them disconnecting soon. It is important to include logic in your game that detects and handles disconnects and provides ways of reconnecting your clients.

### Paused application

When pausing an application or putting it into the background, there are at least two possible scenarios that can cause a disconnect:

#### 1. The device OS closes the application.

If the application has been closed, the user has to restart the application. The user will be brought back to the start screen the next time the application is opened and from there on, the application can connect and load any data necessary.

#### 2. The client stops sending data. 

This scenario is a bit more tricky. The application is still alive, however the OS may decide to not run the game or send any data while the application is in the background. [SpatialOS]({{urlRoot}}/reference/glossary#spatialos-runtime) will close the connection if it doesn't receive any messages from a client-worker for a period of time. You need add a way to reconnect clients to your game to handle this scenario. 

## Types of disconnections in the GDK

[Heartbeating]({{urlRoot}}/modules/player-lifecycle/heartbeating) is a technique used to verify that your client is still connected. If there are too many failed heartbeats, there are two ways a client-worker may be seen as disconnected:

### 1. Heartbeating fails in the SpatialOS Runtime

To ensure that a worker is still connected, the worker has to send messages to SpatialOS at a set interval. This indicates that the worker is still alive. If SpatialOS doesn't receive any messages for a while, it will close the connection to that worker. 

Whenever the connection gets closed on a worker, that worker receives a `DisconnectOp` object. It contains the reason behind the disconnection and triggers a disconnect event in the GDK.

#### How to handle a disconnect

The GDK provides you with a callback that is triggered when the worker receives a `DisconnectOp` object. You can register to this callback to perform your disconnection logic.

```csharp
namespace YourGame
{
    public class ClientWorkerConnector : WorkerConnector
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



### 2. Heartbeating fails in the player lifecycle module 

Our [Player Lifecycle module]({{urlRoot}}/modules/player-lifecycle/overview) allows you to easily create players and uses the heartbeating technique to detect and delete the player entity of any unresponsive client-workers. In mobile applications, this can result in entities being destroyed even though the worker is still connected to SpatialOS. If this happens, that client-worker won't receive a `DisconnectOp`.

#### How to handle a disconnect

When the player entity gets deleted, the client-worker might end up with no entities to be authoritative over and therefore unable to check out any entities. This leaves the client-worker in a bad state. The safest option to handle this is to disconnect at this point and go back to your start screen to attempt a reconnect.

```csharp
namespace YourGame
{
    public class YourMonoBehaviour : MonoBehaviour
    {
        // Make sure to store a reference to the client worker connector
        private WorkerConnector workerConnector;

        public void OnDestroy()
        {
            if (workerConnector.Worker.IsConnected)
            {
                // the player lifecycle module deleted your player entity even though you are still connected.
                // Destroying the worker connector will trigger a disconnect:
                Destroy(workerConnector);
                // Go back to your start screen to allow the client to reconnect
            } 
        }
    }
}
```
