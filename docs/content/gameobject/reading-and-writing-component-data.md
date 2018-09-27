**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

## GameObject: Reading and writing SpatialOS component property data

SpatialOS schema defines what objects in your game interact with SpatialOS. Find out about [what schema does](https://docs.improbable.io/reference/latest/shared/glossary#schema) and how to implement schema via the [schemalang reference](https://docs.improbable.io/reference/latest/shared/schema/reference) in the SpatialOS documentation.

We provide code-generated Readers and Writers for interacting with SpatialOS component [properties](https://docs.improbable.io/reference/latest/shared/glossary#property). 

For every component defined in SpatialOS schema, we generate a pair of Readers and Writers within:

* `<namespace of schema component>.<component name>.Requirable.Reader`
* `<namespace of schema component>.<component name>.Requirable.Writer` 

You can access Readers and Writers by declaring a field in your MonoBehaviour and decorating it with the `[Require]` attribute. `[Require]` fields are automatically injected and removed based on certain requirements:

* `Readers` are injected as long as a component is present on a worker giving you read-access over the component.
* `Writers` are injected as long as a worker has write authority over a component.

Every Writer is also a Reader of the same component, so any functionality provided by a Reader is available on a Writer too.

**Example schema component `Health`**

We use the following `Health` schema component for all the code examples in this document:

```
package improbable.examples;

type IntMessage {
  int32 value = 1;
}

type Empty {}

component Health {
  id = 12345;
  int32 current_health = 1;
  event IntMessage health_changed;
  command Empty set_health(IntMessage);
}
```

## How to read component properties

1. Declare a field of type `Health` Reader or Writer and decorate it with a `[Require]` attribute. 

2. Access the current component property values using `Reader.Data`. 
</br>(This returns a generated `ISpatialComponentData` struct which contains all the component property values of a component.)

**Example**
```csharp
using Improbable.Examples

public class ReadHealthBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Reader healthReader;

    private int ReadHealthValue()
    {
        // Read the current health value of your entity’s Health component.
        var healthvalue = healthReader.Data.CurrentHealth;
        return healthvalue;
    }
}
```

## How to update component properties

1. Declare a field of type `Health` Writer and decorate it with a `[Require]` attribute.
</br>**Note**: The GDK only injects a Writer when your worker gains write authority over the `Health` component. The MonoBehaviour requiring the Writer remains disabled otherwise.

2. Send a component update to specify the new component values that your component should be updated to using `Writer.Send(TComponentUpdate update)`.
</br>(`ISpatialComponentUpdate` types are generated under `<namespace of schema component>.<component name>.Update`.) 

**Known issue warning:** 
- When you use `Writer.Send` to update a component's properties, it causes the SpatialOS GDK to update all properties of the component, even if you have set it to only update some of the properties. Note that the SpatialOS GDK updates unchanged properties to the value they already have. This is unintended behavior which we will fix in an upcoming update.

**Example**
```csharp
using Improbable.Examples

public class WriteHealthBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Writer healthWriter;

    private void SetHealthValue(int newHealthValue)
    {
        // Create an update type
        var healthUpdate = new Health.Update
        {
            CurrentHealth = newHealthValue
        };

        // Update component values
        healthWriter.Send(healthUpdate);
    }
}
```

## How to react to component property changes

1. Declare a field of type `Health` Writer and decorate it with a `[Require]` attribute. 

2. Register a callback for `Reader.ComponentUpdated +=` or for `Reader.<component property name>Updated +=` during `OnEnable()`.
    *  `Reader.ComponentUpdated` is invoked when any component property is updated.
    *  `Reader.<component property name>Updated` is invoked when a specific component property is updated.

**Note:** 
`Reader.ComponentUpdated` callbacks are invoked before specific property update callbacks. You can deregister callbacks using `Reader.ComponentUpdated -=` and `Reader.<component property name>Updated -=`. The SpatialOS GDK also automatically deregisters all callbacks of a Reader or Writer upon validating them when requirements are no longer met. Do not deregister callbacks during `OnDisable()` as that’s an invalid operation.

**Known issue warning:**
- The `ISpatialComponentUpdate update` argument of `Reader.ComponentUpdated` may indicate that a component property has changed even when it has not. This is unintended behavior which we will fix in an upcoming update.
- `Reader.<component property name>Updated` may be invoked even if `<component property name>` did not change. This is unintended behavior which we will fix in an upcoming update.

**Example 1**

The following code example sets up a `Reader.ComponentUpdated` callback.

```csharp
using Improbable.Examples

public class ReactToHealthChangeBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Reader healthReader;

    private void OnEnable()
    {
        healthReader.ComponentUpdated += OnHealthComponentUpdated;
    }

    private void OnHealthComponentUpdated(Health.Update update)
    {
        // Check whether a specific property was updated.
        if (!update.CurrentHealth.HasValue)
        {
            return;
        }

        DoSomethingWithNewHealthValue(update.CurrentHealth.Value);
    }
}
```

**Example 2**

The following code example sets up a `Reader.<component property name>Updated` callback.

```csharp
using Improbable.Examples

public class ReactToHealthChangeBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Reader healthReader;

    private void OnEnable()
    {
        healthReader.CurrentHealthUpdated += OnCurrentHealthUpdated;
    }

    private void OnCurrentHealthUpdated(int newCurrentHealth)
    {
        DoSomethingWithNewHealthValue(newCurrentHealth);
    }
}
```

----

**Give us feedback:** We want your feedback on the SpatialOS GDK for Unity and its documentation  - see [How to give us feedback](../../../README.md#give-us-feedback).