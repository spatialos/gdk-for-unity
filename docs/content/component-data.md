**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../README.md#recommended-use).

-----


## Component Data
SpatialOS components are generated from `.schema` files into components that the Unity ECS can understand. See the schemalang [docs](https://docs.improbable.io/reference/13.0/shared/schema/introduction#schema-introduction) for details on how to create schema components.

### Overview

A `struct`, which implements `Unity.Entities.IComponentData`, by implementing `Improbable.Gdk.Core.ISpatialComponentData`,
is generated for each [blittable](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types) SpatialOS component.
A `class`, extending `UnityEngine.Component`, is generated for each non-blittable SpatialOS component.

Each of these structs will be named `SpatialOS[schemalang component name]` and will contain only the schema data fields.
It will **Not** contain any fields or methods pertaining to [commands](commands.md) or [events](events.md) defined on that component. 

**NOTE** Blittable components actually implement `ISpatialComponentData` which implements `IComponentData`. 
This is an implementaion detail of the current replication system.

For example, the following component contains only blittable fields, so an `ISpatialComponentData` will be generated, similar to the example below.

Schemalang
```
component Blittable {
  id = 1;
  int32 value = 1;
}
```
Generated component
```	csharp
struct SpatialOSBlittable : ISpatialComponentData
{
  public int Value;
}
```

However the following component contains non-blittable fields, so a `Component` will be generated, similar to the example below.

Schemalang
```
component NonBlittableComponent {
  id = 2;
  int32 int_blittable = 1;
  map<int32, float> map_non_blittable = 2;
  string string_non_blittable = 3;
}
```
Generated component
```	csharp
class SpatialOSNonBlittable : Component
{
  public int IntBlittable;
  public Dictionary<int, float> MapNonBlittable;
  public string StringNonBlittable;
}
```

### Reading and writing

When a SpatialOS entity is [checked out](entity-checkout-process.md), the generated components will be automatically added to the corresponding ECS entity. 

A component's value can be read by injecting that component into a system, just like any other ECS component.
Note that if the component is non-blittable then it must be injected as a `ComponentArray` rather than a `ComponentDataArray`.

To send a component update, set the component to the value to be sent.
It will automatically be sent at the end of the tick.
To override this behaviour see [here](custom-replication-system.md).

```csharp
public class ChangeComponentFieldSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentDataArray<SpatialOSBlittable> BlittableComponents;
        public ComponentArray<SpatialOSNonBlittable> NonBlittableComponents;
    }

    [Inject] Data data;

    protected void OnUpdate()
    {
        for(var i = 0; i < data.Length; ++i)
        {
            // How to write to a blittable component - this will automatically trigger a component update
            var currentBlittable = data.BlittableComponents[i];
            FooData updatedBlittableValue = GetNewBlittable(currentBlittable);
            data.BlittableComponents[i] = updatedBlittableValue;                       

            // How to write to a non-blittable component - this will automatically trigger a component update
            data.NonBlittableComponents[i].value = GetNonBlittableValue(); 
        }
    }
}
```

### Updates
When a component update is received this will be added as a [reactive component](reactive-components.md).


### Generation details

#### Primitive types
Each primitive type in schemalang corresponds to some type in the Unity GDK.

| Schemalang type                | Unity GDK type      | Blittable |
| ------------------------------ | :-----------------: | --------: |
| `int32` / `sint32` / `fixed32` | `int`               | yes       |
| `uint32`                       | `uint`              | yes       |
| `int64` / `sint64`/ `fixed64`  | `long`              | yes       |
| `uint64`                       | `ulong`             | yes       |
| `float`                        | `float`             | yes       |
| `bool`                         | `bool1`             | yes       |
| `string`                       | `string`            | no        |
| `bytes`                        | `Not yet supported` | no        |
| `EntityId`                     | `long`              | yes       |

Note that, for the moment, schemalang `bool` corresponds to a Unity `bool1`.

#### Collections 
Schemalang has 3 collection types:

| Schemalang collection | Unity GDK collection                          | Blittable |
| --------------------- | :-------------------------------------------: | --------: |
| `map<T, U>`           | `System.Collections.Generic.Dictionary<T, U>` | no        |
| `list<T>`             | `System.Collections.Generic.List<T>`          | no        |
| `option<T>`           | `System.Nullable<T>`                          | no        |

Note that the Unity GDK does not use `Improbable.Collections` in Unity ECS component generation.

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
struct SomeData 
{
  public int Value;
}
```

If the type contains a non-blittable field then that type is itself non-blittable. 
Any component containing that type will then be non-blittable.

#### Enums 
For every schemalang enum, a C# enum will be generated.
Enums are blittable.

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
**Give us feedback:** We want your feedback on the Unity GDK and its documentation  - see [How to give us feedback](../../README.md#give-us-feedback).