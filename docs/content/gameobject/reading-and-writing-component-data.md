[//]: # (Doc of docs reference 6.2)
[//]: # (TODO - Tech writer pass)
[//]: # (TODO - Remove “> Currently updating any field of the component will trigger the callback for all <component property name>Updated`.” line if if this PR gets merged in: https://github.com/spatialos/UnityGDK/pull/438 )

<%(TOC)%>
# Reading and writing SpatialOS component data
_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities#spatialos-entities)._

Before reading this document, make sure you are familiar with:

* [Linking SpatialOS entities with GameObjects]({{urlRoot}}/content/gameobject/linking-spatialos-entities)
* [Reader and Writer]({{urlRoot}}/content/gameobject/readers-writers)
* [SpatialOS components]({{urlRoot}}/content/glossary#spatialos-component)
* [Read and write access]({{urlRoot}}/content/glossary#authority)
* [Schema]({{urlRoot}}/content/glossary#schema)


We use the following schema for all examples described in this documentation.

```
package improbable.examples;

component Health {
  id = 10000;
  int32 current_health = 1;
  int32 damage_taken = 2;
}
```

The following examples assume that you have a [GameObject that is linked to a SpatialOS entity]({{urlRoot}}/content/gameobject/linking-spatialos-entities) containing the `Health` component.

## How to read component properties

This example MonoBehaviour would be enabled on any worker which has read access to the `Health` component.

```csharp
using Improbable.Examples;

public class ReadHealthBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Reader healthReader;

    private int ReadHealthValue()
    {
        // Read the current health value of your entity’s Health component.
        return healthReader.Data.CurrentHealth;
    }
}
```

## How to update SpatialOS component properties

This example MonoBehaviour would be enabled only on any worker that has write access over the `Health` component.

```csharp
using Improbable.Examples;

public class WriteHealthBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Writer healthWriter;

    private void Update()
    {
        // Create a new Health.Update object
        var healthUpdate = new Health.Update
        {
            CurrentHealth = newHealthValue
        };

        // Send component update to the SpatialOS Runtime
        healthWriter.Send(healthUpdate);
    }
}
```

### How to react to component property changes

The following code snippet registers a callback for whenever any property in the `Health` component gets updated.
This example MonoBehaviour would be enabled on any worker which has read access to the `Health` component.

```csharp
using Improbable.Examples;

public class ReactToHealthChangeBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Reader healthReader;

    private void OnEnable()
    {
        // Register callback for whenever any property of the component gets updated
        healthReader.ComponentUpdated += OnHealthComponentUpdated;
    }

    private void OnDisable()
    {
        // No need to deregister. All callbacks are automatically deregistered.
    }

    private void OnHealthComponentUpdated(Health.Update update)
    {
        // Check whether a specific property was updated.
        if (!update.CurrentHealth.HasValue)
        {
            return;
        }

        // do something with the new CurrentHealth value
    }
}
```

The following code example sets up a specific field update callback.
This example MonoBehaviour would be enabled on any worker which has read access to the `Health` component.

> Currently updating any field of the component will trigger the callback for all `{name of component property}Updated`.

```csharp
using Improbable.Examples;

public class ReactToCurrentHealthChangeBehaviour : MonoBehaviour
{
    [Require] private Health.Requirable.Reader healthReader;

    private void OnEnable()
    {
        // Register callback for whenever a specific property, e.g. current_health,
        // of the component gets updated
        healthReader.CurrentHealthUpdated += OnCurrentHealthUpdated;
    }

    private void OnCurrentHealthUpdated(int newCurrentHealth)
    {
        // do something
    }
}
```
