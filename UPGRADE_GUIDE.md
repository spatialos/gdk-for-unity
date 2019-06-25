# Upgrade Guide

## From `0.2.3` to `Unreleased`

- When constructing a `ForwardingDispatcher`, you must now provide a `UnityEngine.LogType` instead of a `Improbable.Worker.CInterop.LogLevel`.
- You must no longer use the `TransformInternal.Snapshot` constructor. Instead, use the `CreateTransformSnapshot` method provided by the `TransformUtils` class. This is to ensure that the contents of an entity's `TransformInternal` component are compressed using the Transform Synchronization module's new compression scheme.
    - Users of the `AddTransformSynchronizationComponents` method in the `TransformSynchronizationHelper` class do not need to update code.
- If any entities in your snapshot require the Transform Synchronization feature module, you must regenerate your snapshot.
