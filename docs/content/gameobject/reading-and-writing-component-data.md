**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../../../README.md#recommended-use).

-----

## GameObject: Reading and writing SpatialOS component field data

SpatialOS schema defines what objects in your game interact with SpatialOS. Find out about [what schema does](https://docs.improbable.io/reference/latest/shared/glossary#schema) and how to implement schema via the [schemalang reference](https://docs.improbable.io/reference/latest/shared/schema/reference) in the SpatialOS documentation.

We provide code-generated Reader and Writer interfaces for interacting with SpatialOS component fields. 

For every component defined in SpatialOS schema, we automatically generate a pair of Reader and Writer interfaces within:

* `Generated.<namespace of schema component>.<component name>.Requirables.Reader`
* `Generated.<namespace of schema component>.<component name>.Requirables.Writer` 

(Where `<example content>` is the name of relevant component, without the angle brackets.)

You can access Reader and Writer interfaces by declaring a field with a type corresponding to a SpatialOS component in your MonoBehaviour and decorating the field with the `[Require]` attribute. The fields are automatically populated as soon as certain requirements are met or assume `null` otherwise.

* `Readers` are injected as soon a component is added to your SpatialOS entity on a worker giving you read-only access to the component.
* `Writers` are injected as soon as a worker gains write authority over a component.

Every Writer is also a Reader of the same component, so anything you can do with a Reader, you can do with a Writer too.

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

## How to read component field values

1. Declare a field of type `Health` Reader or Writer and decorate it with a `[Require]` attribute. 

2. Access the current component field values using `Reader.Data`. 
</br>(This returns a generated `ISpatialComponentData` struct which contains all the component field values of `Health`.)

**Example**
```csharp
using Generated.Improbable.Examples

public class ReadHealthBehaviour : MonoBehaviour
{
    [Require] private Health.Requirables.Reader healthReader;

    private int ReadHealthValue()
    {
        // Read the current health value of your entity’s Health component.
        var healthvalue = healthReader.Data.CurrentHealth;
        return healthvalue;
    }
}
```

## How to update component field values

1. Declare a field of type `Health` Writer and decorate it with a `[Require]` attribute.
</br>**Note**: The GDK only injects a Writer when your worker gains write authority over the `Health` component. The MonoBehaviour requiring the Writer remains disabled otherwise.

2. Send a component update to specify the new component values that your component should be updated to using `Writer.Send(TComponentUpdate update)`.
</br>(`ISpatialComponentUpdate` types are generated under `Generated.<namespace of schema component>.<component name>.Update`.) 

**Example**
```csharp
using Generated.Improbable.Examples

public class WriteHealthBehaviour : MonoBehaviour
{
    [Require] private Health.Requirables.Writer healthWriter;

    private void SetHealthValue(int newHealthValue)
    {
        // Create update type
        var healthUpdate = new Health.Update
        {
            CurrentHealth = newHealthValue
        };

        // Update component value
        healthWriter.Send(healthUpdate);
    }
}
```

## How to react to component field changes

1. Declare a field of type `Health` Writer and decorate it with a `[Require]` attribute. 

2. Register a callback for `Reader.ComponentUpdated(ISpatialComponentUpdate update) +=` or for `Reader.<component field name>Updated() +=` during `OnEnable(<type of component field> newFieldValue)`.
    *  `Reader.ComponentUpdated` is invoked when any component field gets updated.
    *  `Reader.<component field name>Updated` is invoked when that component field gets updated.

**Note:** 
`Reader.ComponentUpdated` callbacks are invoked before specific field update callbacks. Callbacks can be deregistered using `Reader.ComponentUpdated(ISpatialComponentUpdate update) -=` and `Reader.<component field name>Updated() -=`. Callbacks are also automatically deregistered when a Reader wor Writer is removed. Do not deregister callbacks during `OnDisable()` as that’s an invalid operation.

**Example 1**

The following code example sets up a `Reader.ComponentUpdated` callback.

```csharp
using Generated.Improbable.Examples

public class ReactToHealthChangeBehaviour : MonoBehaviour
{
    [Require] private Health.Requirables.Reader healthReader;

    private void OnEnable()
    {
        healthReader.ComponentUpdated += OnGeneralHealthComponentUpdated;
    }

    private void OnGeneralHealthComponentUpdated(Health.Update update)
    {
        // Check whether a specific field was updated.
        if (!update.CurrentHealth.HasValue)
        {
            return;
        }

        DoSomethingWithNewHealthValue(update.CurrentHealth.Value);
    }
}
```

**Example 2**

The following code example sets up a `Reader.<component field name>Updated` callback.

```csharp
using Generated.Improbable.Examples

public class ReactToHealthChangeBehaviour : MonoBehaviour
{
    [Require] private Health.Requirables.Reader healthReader;

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