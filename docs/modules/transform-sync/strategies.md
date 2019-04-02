<%(TOC max="4")%>
# Transform synchronization strategies

An instance of each of the following strategies can be created from the **Assets** > **Create** > **SpatialOS** > **Transform** menu in your Unity Editor.

## Send Strategies

#### RateLimitedSendStrategy

This strategy sends a transform and position update whenever a period of time (configurable) has elapsed, provided that the entity has moved.

| | | |
|---|---|---|
| **Parameter** | **Description** | **Effect** |
| `MaxTransformSendRateHz` | This is the frequency at which transform updates will be sent, provided the entity has moved since the previous one. | Increasing this can result in better looking movement on the client, but will also cause increased worker load and bandwidth usage. |
| `MaxPositionSendRateHz` | This is the frequency at which position updates will be sent, provided the entity has moved since te previous one. | Increasing this can result in a fast load balancer response to entity movement, but will cause increased load balancer and worker load and bandwidth usage. |

## Receive Strategies

#### InterpolationReceiveStrategy

This strategy stores a buffer of received transform updates and creates virtual transform values that smoothly moves between the updates. This sacrifices some latency to achieve visual smoothness.

| | | |
|---|---|---|
| **Parameter** | **Description** | **Effect** |
| `TargetBufferSize` | The target size of the buffer of the received updates. | Reducing this will decrease latency between the authoritative worker and the client, but may cause hitches in the movement. This should be increased when reducing the number of updates sent per second. |
| `MaxBufferSize` | The maximum size of the buffer of the received updates. | Increasing this will increase the maximum latency between the authoritative worker and the client before the buffer is reset. Reducing this will limit the maximum latency. This should _always_ be larger than the `TargetBufferSize`. The buffer size tends to increase whne the authoritative worker-instance is overloaded, so this should be increased if the server tick rate is unstable. |


#### DirectReceiveStrategy

This strategy applies updates as they are received.