**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Component Data
The [code generator](./code-generator.md) uses `.schema` files to generate components that the Unity ECS can understand. See the schemalang [docs](https://docs.improbable.io/reference/latest/shared/schema/introduction#schema-introduction) for details on how to create schema components.

> Note that code generation runs when you open the Unity Editor or when you select Improbable > Generate Code from the Editor menu.

### Overview

A `struct`, which implements `Unity.Entities.IComponentData` and `Improbable.Gdk.Core.ISpatialComponentData`,
is generated for each SpatialOS component.

The generation process names each of these structs according to the relevant schemalang component name, SpatialOS[schemalang component name]. The structs only contain the schema data fields. They do *not* contain any fields or methods relating to [commands](commands.md) or [events](events.md) defined on that component.

For example:

Schemalang
```
component Example {
  id = 1;
  int32 value = 1;
}
```
Generated component
```	csharp
public struct SpatialOSExample : IComponentData, ISpatialComponentData
{
  public int Value;
}
```

### Reading and writing

When a SpatialOS entity is [checked out](entity-checkout-process.md), its components are automatically added to the corresponding ECS entity as part of the entity check out process.

A generated component's values can be read by injecting that component into a system, just like any other ECS component.

To send a component update, set the component to the value to be sent. A component update will be constructed and sent at the end of the tick.
To override this behaviour see [here](custom-replication-system.md).

```csharp
public class ChangeComponentFieldSystem : ComponentSystem
{
    private struct Data
    {
        public readonly int Length;
        public ComponentDataArray<SpatialOSExample> ExampleComponents;
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

### Updates
When a component update is received this will be added as a [reactive component](reactive-components.md).


### Generation details

#### Primitive types
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
| `EntityId`                     | `long`                  |

Note that, for the moment, schemalang `bool` corresponds to a `BlittableBool` which is required to make the components blittable.

#### Collections
Schemalang has 3 collection types:

| Schemalang collection | SpatialOS GDK collection                          |
| --------------------- | :-----------------------------------------------: |
| `map<T, U>`           | `System.Collections.Generic.Dictionary<T, U>`     |
| `list<T>`             | `System.Collections.Generic.List<T>`              |
| `option<T>`           | `System.Nullable<T>`                              |

Note that the GDK does not use `Improbable.Collections` in Unity ECS component generation.

#### Custom types
For every custom data type in schema a `struct` will be generated.

Schemalang
```
type SomeData {
  int32 value = 1;
}
```
Generated C#
```	csharp
public struct SomeData
{
  public int Value;
}
```

#### Enums
For every schemalang enum, a C# enum will be generated.

Schemalang
```
enum Color {
    YELLOW = 0;
    GREEN = 1;
    BLUE = 2;
    RED = 3;
}

```
Generated C#
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

----
**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).
