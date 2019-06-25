# Upgrade Guide

## From `0.2.3` to `Unreleased`

### General changes

When constructing a `ForwardingDispatcher`, you must now provide a `UnityEngine.LogType` instead of a `Improbable.Worker.CInterop.LogLevel`.

### Transform Synchronization changes

Several utility methods in the `TransformUtils` class have been made `internal` to the Transform Synchronization package. We highly recommend that **if you were using the `TransformInternal` component in your logic, please use `UnityEngine.Transform` instead.**

You must no longer use the `TransformInternal.Snapshot` constructor.

```csharp
// Sample of INVALID code - the `Location` and `Quaternion` schema types have been removed.
var transform = new TransformInternal.Snapshot
{
    Location = new Location((float) coords.X, (float) coords.Y, (float) coords.Z),
    Rotation = new Quaternion(1, 0, 0, 0),
    TicksPerSecond = 1f / Time.fixedDeltaTime
};
```

Instead, you must use the `CreateTransformSnapshot` method provided by the `TransformUtils` class. This is to ensure that the contents of an entity's `TransformInternal` component are compressed using the Transform Synchronization module's new compression scheme.

```csharp
// This is the correct way of initialising a `TransformInternal` snapshot manually.
var transform = TransformUtils.CreateTransformSnapshot(Coordinates.Zero, Quaternion.identity, Vector3.forward)
```

If you use the `AddTransformSynchronizationComponents` method in the `TransformSynchronizationHelper` class, you do not need to update code.

**You must regenerate any snapshots which contain entities that make use of the Transform Synchronization feature module.**
