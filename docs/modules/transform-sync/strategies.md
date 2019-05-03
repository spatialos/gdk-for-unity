<%(TOC max="4")%>

# Transform synchronization strategies

An instance of each of the following strategies can be created from the **Assets** > **Create** > **SpatialOS** > **Transform** menu in your Unity Editor.

## Send Strategies

#### RateLimitedSendStrategy

This strategy sends a transform and position update whenever a period of time (configurable) has elapsed, provided that the entity has moved.

| Parameter | Description | Effect |
|---|---|---|
| `MaxTransformSendRateHz` | This is the maxiumum frequency at which transform updates are sent, provided the entity has moved since the previous update. | Increasing this can result in better looking movement on the client, but also cause increased worker load and bandwidth usage. |
| `MaxPositionSendRateHz` | This is the maximum frequency at which position updates are sent, provided the entity has moved since the previous update. | Increasing this can result in a faster load balancer response to entity movement, but can cause increased load balancer and worker load as well as increased bandwidth usage. |

## Receive Strategies

#### InterpolationReceiveStrategy

This strategy stores a buffer of received transform updates and creates intermediate transform values that interpolate between the updates and places them on the buffer. This sacrifices some latency to achieve visual smoothness.

| Parameter | Description | Effect |
|---|---|---|
| `TargetBufferSize` | The target size of the buffer of the received updates. | Reducing this decreases latency between the authoritative worker and the client, but may cause hitches in the movement. This should be increased when reducing the number of updates sent per second. |
| `MaxBufferSize` | The maximum size of the buffer of the received updates. | Increasing this increases the maximum latency between the authoritative worker and the client before the buffer is reset. Reducing this limits the maximum latency.<br/><br/>This should _always_ be larger than the `TargetBufferSize`. The buffer size tends to increase when the authoritative worker-instance is overloaded, so this should be increased if the server tick rate is unstable. |

#### DirectReceiveStrategy

This strategy applies updates as they are received. This sacrifices visual smoothness for lower latency.
