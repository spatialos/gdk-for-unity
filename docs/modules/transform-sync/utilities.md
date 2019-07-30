<%(TOC max="4")%>

# Transform utilities

The transform synchronization module includes a [`TransformUtils`]({{urlRoot}}/api/transform-synchronization/transform-utils) class, populated with a set of static methods that makes it nicer to use the feature module.

## Type conversion methods

The following static methods can be used to construct a `FixedPointVector3` or `CompressedQuaternion` from their respective native Unity types.

| Static Method                                            | Result Type            |
|----------------------------------------------------------|------------------------|
| `FixedPointVector3.FromUnityVector(Vector3 v)`           | `FixedPointVector3`    |
| `CompressedQuaternion.FromUnityQuaternion(Quaternion q)` | `CompressedQuaternion` |

The methods below can be called to easily convert a variable to and from native Unity types.

| Type                   | Method                      | Result Type            |
|------------------------|-----------------------------|------------------------|
| `Vector3`              | `.ToCoordinates()`          | `Coordinates`          |
| `Vector3`              | `.ToFixedPointVector3()`    | `FixedPointVector3`    |
| `FixedPointVector3`    | `.ToUnityVector()`          | `Vector3`              |
| `FixedPointVector3`    | `.ToCoordinates()`          | `Coordinates`          |
| `CompressedQuaternion` | `.ToUnityQuaternion()`      | `Quaternion`           |
| `Quaternion`           | `.ToCompressedQuaternion()` | `CompressedQuaternion` |

## Snapshot constructor

The `CreateTransformSnapshot` method constructs a `TransformInternal` snapshot, given uncompressed position, rotation or velocity as arguments.

```csharp
var coords = new Coordinates(10, 20, 30);
var transformSnapshot = TransformUtils.CreateTransformSnapshot(
    coords.ToUnityVector(),
    Quaternion.identity,
    Vector3.zero);
```

> Note: `ToUnityVector()` casts each element of a `Coordinates` from a double down to a float.
