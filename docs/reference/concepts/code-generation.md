<%(TOC max="2")%>

# Code generator

The code generator uses `.schema` files to generate C# code that you can use to interact with SpatialOS in Unity. See the [schemalang reference](https://docs.improbable.io/reference/latest/shared/schema/introduction#schema-introduction) for details on how to create schema components.

The code generator runs when you do one of the following:

* Open your Unity Editor
* Select **SpatialOS** > **Generate Code** from the Unity Editor menu
* Select **SpatialOS** > **Generate Code (force)** from the Unity Editor menu

## Type mappings

The code generator maps from the [schema primitive types](https://docs.improbable.io/reference/latest/shared/schema/reference#primitive-types) to C# types according to the following table:

| Schemalang type                | C# type      |
| ------------------------------ | :---------------------: |
| `int32` / `sint32` / `fixed32` | `int`                   |
| `uint32`                       | `uint`                  |
| `int64` / `sint64`/ `fixed64`  | `long`                  |
| `uint64`                       | `ulong`                 |
| `float`                        | `float`                 |
| `bool`                         | `BlittableBool`         |
| `string`                       | `string`                |
| `bytes`                        | `byte[]`                |
| `EntityId`                     | `Improbable.Gdk.Core.EntityId` |

The code generator also maps [schema collection types](https://docs.improbable.io/reference/latest/shared/schema/reference#collection-types) to C# collections according to the following table:

| Schemalang collection | C# collection                         |
| --------------------- | :-----------------------------------------------: |
| `map<K, V>`           | `System.Collections.Generic.Dictionary<K, V>`     |
| `list<T>`             | `System.Collections.Generic.List<T>`              |
| `option<T>`           | `Improbable.Gdk.Core.Option<T>`                   |

## User defined types

The code generator generates a C# struct for each [user defined type in schema](https://docs.improbable.io/reference/latest/shared/schema/reference#user-defined-types). The generated struct is annotated with the [`System.Serializable` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html) and has a constructor with a parameter per schema field.

## Enums

The code generator generates a C# enum for each [enum in schema](https://docs.improbable.io/reference/latest/shared/schema/reference#enumerations). The generated enum is annotated with the [`System.Serializable` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

> The `uint` values defined for the generated C# enum are not guaranteed to be the same as the defined schemalang field IDs.

## Components

The code generator generates two C# structs for each [SpatialOS component defined in schema](https://docs.improbable.io/reference/latest/shared/schema/reference#components): a **snapshot** struct and a **component** struct.

### Snapshot

The snapshot struct implements the [`Improbable.Gdk.Core.ISpatialComponentSnapshot` interface]({{urlRoot}}/api/core/i-spatial-component-snapshot) and is annotated with the [`System.Serializable` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

The struct has one public field for each field defined in the schema component and a constructor with a parameter per schema field.

### Component

The component struct implements the [`Improbable.Gdk.Core.ISpatialComponentData`]({{urlRoot}}/api/core/i-spatial-component-data) and [`Improbable.Gdk.Core.ISnapshottable<T>`]({{urlRoot}}/api/core/i-snapshottable) interfaces.

> **Note:** The `T` parameter in the `Improbable.Gdk.Core.ISnapshottable<T>` is the snapshot struct for the same component.

The struct has one public property for each field defined in the schema component.
