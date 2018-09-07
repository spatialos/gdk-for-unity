## GameObject: Reading and writing SpatialOS component data

SpatialOS schema defines what objects in your game interact with SpatialOS. Find out about [what schema does](https://docs.improbable.io/reference/latest/shared/glossary#schema) and how to implement schema via the [schemalang reference](https://docs.improbable.io/reference/latest/shared/schema/reference) in the SpatialOS documentation.

We provide code-generated Reader and Writer objects for interacting with SpatialOS component data. 

For every component defined in SpatialOS schema, the code generation creates a pair of Reader and Writer interfaces within:

* `Generated.<namespace of schema component>.<component name>.Requirables.Reader`

* `Generated.<namespace of schema component>.<component name>.Requirables.Writer` 

(Where `<example content>` is the name of relevant component, without the angle brackets.)

You can set up Reader and Writer objects to inject into MonoBehaviours using the `[Require]` annotation.

As soon as you add a component to your SpatialOS entity on a worker, the GDK injects Readers into its  MonoBehaviours - this gives the worker read-only access to the component. As soon as your worker gains Writers write authority over a component, the GDK injects Writers, giving the worker read and write access over a component. This means that every Writer is also a Reader of the same component, so anything you can do with a Reader, you can do with a Writer too.

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

## How to read the component state

1. Add a  `[Require]` notation to a Reader or Writer for the `Health` component. 

2. Access the current component field values using `Reader.Data` as shown in the example below. 
</br>(This returns a generated `ISpatialComponentData` which contains all the component field values. `ISpatialComponentUpdate` types are generated under `Generated.<namespace of schema component>.<component name>.Update`. )

**Example**
The following code example reads (that is it returns) the `Health` of your entity’s component using `IReader.Data` (`healthReader.Data.CurrentHealth` in the example below).

```code

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

## How to write the component state

1. Add a  `[Require]` notation to a Reader or Writer for the `Health` component. 
</br></br> **Note**: The GDK only injects a Writer when  your worker gains write authority over the Health component or when a worker checks out a new component that it has authority over. The MonoBehaviour requiring the Writer remains disabled in any other circumstances.

2. Send a component update to specify the new component values that your component should be updated to using `Writer.Send(TComponentUpdate update)` as shown in the example below.
</br>(This returns a generated `ISpatialComponentData` containing the component field values. `ISpatialComponentUpdate` types are generated under `Generated.<namespace of schema component>.<component name>.Update`. ) 

**Example**

The following code example writes (that is updates) the new value for `Health` using `Writer.Send(TComponentUpdate update)`. 

```code

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

## How to react to component state changes

1. Add a  `[Require]` notation to a Reader or Writer for the `Health` component. 

2. Register a callback for `Reader.ComponentUpdated(ISpatialComponentUpdate update)` or for `Reader.<component field name>Updated()` during `OnEnable(<type of component field> newFieldValue)`.
</br>They are slightly different:
    *  `Reader.ComponentUpdated` is invoked when any component field gets updated.

    *  `Reader.<component field name>Updated` is invoked when that component field gets updated.


**Note:** 
`Reader.ComponentUpdated` callbacks are invoked before specific field update callbacks.  You can deregister registered callbacks if you want to; callbacks are automatically deregistered when field which has been injected as a result of a component with a `[Require]` notation is removed. 
Do not deregister callbacks during `OnDisable()` as that’s an invalid operation because your callbacks are already deregistered at this point.

**Example 1**

The following code example sets up `Reader.ComponentUpdated`.

```code

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

The following code example sets up `Reader.<component field name>Updated`.

```code

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

