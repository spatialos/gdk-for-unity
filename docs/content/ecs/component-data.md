<%(TOC)%>
# Components and component updates
_This document relates to the [ECS workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

The [code generator]({{urlRoot}}/content/code-generator) uses `.schema` files to generate components that the Unity ECS can understand. See the [schemalang docs](https://docs.improbable.io/reference/latest/shared/schema/introduction#schema-introduction) for details on how to create schema components.

> Note that code generation runs when you open your Unity Editor or when you select **SpatialOS** > **Generate Code** from the Editor menu.

## Overview

A `struct`, which implements `Unity.Entities.IComponentData` and `Improbable.Gdk.Core.ISpatialComponentData`,
is generated for each SpatialOS component.

The generation process names each of these structs according to the relevant schemalang component name, `[schemalang_name].Component`. The structs only contain the schema data fields. They do *not* contain any fields or methods relating to [commands](https://docs.improbable.io/reference/latest/shared/glossary#commands) or [events](https://docs.improbable.io/reference/latest/shared/glossary#event) defined on that component.

For example:

**Schemalang**:

```
component Example {
  id = 1;
  int32 value = 1;
}
```

**Generated component**:

```csharp
public partial class Example
{
    public struct Component : IComponentData, ISpatialComponentData
    {
        public int Value;
    }
}
```

## Reading and writing

When a SpatialOS entity is [checked out]({{urlRoot}}/content/glossary#checking-out), its components are automatically added to the corresponding ECS entity as part of the entity check out process.

A generated component's values can be read by injecting that component into a system, just like any other ECS component.

To send a component update, set the component to the value to be sent. A component update will be constructed and sent at the end of the tick.
To override this behaviour see [here]({{urlRoot}}/content/ecs/custom-replication-system).

```csharp
public class ChangeComponentFieldSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        public ComponentDataArray<Example.Component> ExampleComponents;
    }

    [Inject] private Data data;

    protected void OnUpdate()
    {
        for(var i = 0; i < data.Length; ++i)
        {
            var exampleComponent = data.ExampleComponents[i];
            exampleComponent.Value = 10;
            data.ExampleComponents[i] = exampleComponent;
        }
    }
}
```

## Updates

When a component update is received this will be added as a [reactive component]({{urlRoot}}/content/ecs/reactive-components).

## Generation details

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
| `EntityId`                     | `EntityId`              |

Note that, for the moment, schemalang `bool` corresponds to a `BlittableBool` which is required to make the components blittable.

### Collections

Schemalang has 3 collection types:

| Schemalang collection | SpatialOS GDK collection                          |
| --------------------- | :-----------------------------------------------: |
| `map<T, U>`           | `System.Collections.Generic.Dictionary<T, U>`     |
| `list<T>`             | `System.Collections.Generic.List<T>`              |
| `option<T>`           | `System.Nullable<T>`                              |

Note that the GDK does not use `Improbable.Collections` in Unity ECS component generation.

### Custom types

A `struct` is generated for every custom data type in schema. The generated struct has the [`[System.Serializable]` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

**Schemalang**:

```
type SomeData {
  int32 value = 1;
}
```

**Generated C#**:

```	csharp
[System.Serializable]
public struct SomeData
{
  public int Value;
}
```

### Enums
A C# enum is generated for every schemalang enum. The generated enum has the [`[System.Serializable]` attribute](https://docs.unity3d.com/ScriptReference/Serializable.html).

**Schemalang**:

```
enum Color {
    YELLOW = 0;
    GREEN = 1;
    BLUE = 2;
    RED = 3;
}
```

**Generated C#**:

```csharp
public enum Color : uint
{
    YELLOW = 0,
    GREEN = 1,
    BLUE = 2,
    RED = 3,
}
```

Note the `uint` values are coincidentally the same as the schemalang field IDs; there is no guarantee that they will be the same.
