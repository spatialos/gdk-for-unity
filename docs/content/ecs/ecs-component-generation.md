[//]: # (Doc of docs reference 31)
[//]: # (TODO - Tech writer review)
[//]: # (TODO - use discussions about content in here https://docs.google.com/document/d/1IGblyE-pvA4ZyJIjN8PcD1Ct6pE4FNhtlXRdp_Sy97o/edit)
#  (ECS) ECS component generation
 _This document relates to the [ECS workflow]({{urlRoot}}/content/intro-workflows-spos-entities.md)._

The [code generator]({{urlRoot}}/content/code-generator) uses `.schema` files to generate components that the Unity ECS can understand. See the [schemalang guide (SpatialOS documentation)](https://docs.improbable.io/reference/latest/shared/schema/introduction#schema-introduction) for details on how to create schema components.

> Note that code generation runs when you open the Unity Editor or when you select **SpatialOS** > **Generate Code** from the Editor menu.

## Overview

A `struct`, which implements `Unity.Entities.IComponentData` and `Improbable.Gdk.Core.ISpatialComponentData`, is generated for each SpatialOS component. It has the same name as the corresponding schema component.
The generation process creates a namespace for each struct according to the relevant schemalang component name: `{name of schema component}.Component`
These structs only contain the defined schema data fields. They do *not* contain any fields or methods relating to [commands]({{urlRoot}}/content/ecs/sending-receiving-component-commands) or [events (SpatialOS documentation)](https://docs.improbable.io/reference/latest/shared/glossary#event) defined on that component.

## Generation details
Each struct contains the following fields:

  * the public property `uint ComponentId` to read the component ID of this component as defined in schemalang
  * the public property `BlittableBool DirtyBit` used internally to identify whether a component update needs to be sent to the SpatialOS Runtime

Additionally, for each field defined in your schema file, the generated C# struct creates:

  * a private field corresponding to the field defined in schemalang
  * a public property that can be used for reading and writing the value of this field. Changing the value of this property makes sure that `DirtyBit` is set to `true`.

The struct also contains the following method:
```csharp
public static Improbable.Worker.Core.ComponentData CreateSchemaComponentData({arguments: the fields defined in schemalang})
```

This method can be used to add his component to your [entity template]({{urlRoot}}/content/entity-templates).

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
| `EntityId`                     | `Improbable.Worker.EntityId` |

Note that, for the moment, schemalang `bool` corresponds to a `BlittableBool` which is required to make the components blittable. This allows you to represent any schema component as a `struct` inheriting from `IComponentData` so that it can be used by Unity’s ECS.

#### Collection types
Schemalang has three collection types:

| Schemalang collection | SpatialOS GDK collection                          |
| --------------------- | :-----------------------------------------------: |
| `map<K, V>`           | `System.Collections.Generic.Dictionary<K, V>`     |
| `list<T>`             | `System.Collections.Generic.List<T>`              |
| `option<T>`           | `Improbable.Gdk.Core.Option<T>`                              |


### Custom types
For every custom data type in schema, a `struct` is generated defining this type in C# and providing additional serialization methods that are used internally.

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

  public static class Serialization
  {
    // methods to serialize / deserialize this specific type
  }
}
```

### Enums
For every schemalang enum, a C# enum will be generated.
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
