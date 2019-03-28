[//]: # (Doc of docs reference 31)
[//]: # (TODO - Tech writer review)
[//]: # (TODO - Move this doc from ECS to generic - and change its title to not be ECS)
[//]: # (TODO - use discussions about content in here https://docs.google.com/document/d/1IGblyE-pvA4ZyJIjN8PcD1Ct6pE4FNhtlXRdp_Sy97o/edit)
<%(TOC)%>
# Code generation
 _This document relates to both the [MonoBehaviour and ECS workflows](\{\{urlRoot\}\}/reference/intro-workflows-spatialos-entities)._

> Note this document refers to ECS components but it is also relevant if you are using the MonoBehaviour workflow. You don't need to know about ECS to use component generation.

The code generator uses `.schema` files to generate components that the Unity ECS can understand. See the [schemalang guide](https://docs.improbable.io/reference/latest/shared/schema/introduction#schema-introduction) for details on how to create schema components.

 Code generation runs when you open your Unity Editor or when you select **SpatialOS** > **Generate Code** from the Editor menu.

## Overview

The code generator generates two structs for each SpatialOS component defined in schema. The generation process places these structs in a namespace according to the SpatialOS component name: `{schema package}.{name of schema component}`. The two structs it generates are:

  * A **snapshot** `struct` which implements `Improbable.Gdk.Core.ISpatialComponentSnapshot`. Its fully qualified name is: `{schema package}.{name of schema component}.Snapshot`.
  * A **component** `struct` which implements `Unity.Entities.IComponentData`, `Improbable.Gdk.Core.ISpatialComponentData`, and `Improbable.Gdk.Core.ISnapshottable<{schema package}.{name of schema component}.Snapshot>`. Its fully qualified name is: `{schema package}.{name of schema component}.Component`.

These structs only contain the defined schema data fields. They do *not* contain any fields or methods relating to [commands](\{\{urlRoot\}\}/reference/ecs/sending-receiving-component-commands) or [events](https://docs.improbable.io/reference/latest/shared/glossary#event) defined on that component.

## Generation details

### Snapshot

This struct contains the following fields:

  * the public property `uint ComponentId` to read the component ID of this component as defined in [schemalang](https://docs.improbable.io/reference/latest/shared/glossary#schemalang).

Additionally, for each field defined in your schema file, the generated C# struct contains:
  
  * a public field corresponding to the field defined in schemalang.

If there are any fields defined in your schema file, the generated C# struct contains:

  * a public constructor with one parameter per field

This struct also has the [`[System.Serializable]` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

### Component

This struct contains the following fields:

  * the public property `uint ComponentId` to read the component ID of this component as defined in schemalang.
  * the private fields `byte dirtyBits{i}`, which represents a bitmask used internally to identify whether a component field needs to be replicated to the [SpatialOS Runtime](\{\{urlRoot\}\}/reference/glossary#spatialos-runtime).

Additionally, for each field defined in your [schema](\{\{urlRoot\}\}/reference/glossary#schema) file, the generated C# struct contains:

  * a private field corresponding to the field defined in schemalang.
  * a public property that represents the value of this field. If you change the value of this property, the corresponding `dirtyBit` is set to true.

The struct also contains the following method:

```csharp
public Snapshot ToComponentSnapshot(Unity.Entities.World world);
```

Use this method to convert this component into the corresponding `ISpatialComponentSnapshot`.

### Primitive types

Each primitive type in schemalang corresponds to a type in the SpatialOS GDK for Unity (GDK).

| Schemalang type                | SpatialOS GDK type      |
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

Note that, for the moment, schemalang `bool` corresponds to a `BlittableBool` which is required to make the components blittable. This allows you to represent any schema component as a `struct` inheriting from `IComponentData` so that it can be used by Unity’s ECS.

#### Collection types

Schemalang has three collection types:

| Schemalang collection | SpatialOS GDK collection                          |
| --------------------- | :-----------------------------------------------: |
| `map<K, V>`           | `System.Collections.Generic.Dictionary<K, V>`     |
| `list<T>`             | `System.Collections.Generic.List<T>`              |
| `option<T>`           | `Improbable.Gdk.Core.Option<T>`                              |


### Custom types

A `struct` is generated for every custom data type in schema. The generated struct has the [`[System.Serializable]` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

**Schemalang**
```
type SomeData {
  int32 value = 1;
}
```

**Generated C#**
```	csharp
public struct SomeData
{
  public int Value;

  public SomeData(int value)
  {
    Value = value;
  }
}
```

### Enums

A C# enum is generated for every schemalang enum. The generated enum has the [`[System.Serializable]` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

> The `uint` values defined for the generated C# enum are not guaranteed to be the same as the defined schemalang field IDs.

**Schemalang**
```
enum Color {
    YELLOW = 0;
    GREEN = 1;
    BLUE = 2;
    RED = 3;
}

```
**Generated C#**
```csharp
public enum Color : uint
{
    YELLOW = 0,
    GREEN = 1,
    BLUE = 2,
    RED = 3,
}
```
