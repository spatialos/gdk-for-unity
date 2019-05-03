<%(TOC)%>

# Readers and Writers: Component data and updates

_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/reference/workflows/overview#monobehaviour-centric-workflow)._

Before reading this document, make sure you are familiar with:

* [Reader and Writer]({{urlRoot}}/reference/workflows/monobehaviour/interaction/reader-writers/index)
* [SpatialOS components]({{urlRoot}}/reference/glossary#spatialos-component)
* [Read and write access]({{urlRoot}}/reference/glossary#authority)
* [Schema]({{urlRoot}}/reference/glossary#schema)

We use the following schema for all examples described in this documentation.

```schemalang
package improbable.examples;

component Health {
  id = 10000;
  int32 current_health = 1;
  int32 damage_taken = 2;
}
```

The following examples assume that you have a GameObject that is [linked to a SpatialOS entity]({{urlRoot}}/modules/game-object-creation/overview) containing the `Health` component.

## How to read component properties

This example MonoBehaviour would be enabled on any worker which has read access to the `Health` component.

```csharp
using Improbable.Examples;
using Improbable.Gdk.Subscriptions;

public class ReadHealthBehaviour : MonoBehaviour
{
    [Require] private HealthReader healthReader;

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
using Improbable.Gdk.Subscriptions;


public class WriteHealthBehaviour : MonoBehaviour
{
    [Require] private HealthWriter healthWriter;

    private void Update()
    {
        // Create a new Health.Update object
        var healthUpdate = new Health.Update
        {
            CurrentHealth = newHealthValue
        };

        // Send component update to the SpatialOS Runtime
        healthWriter.SendUpdate(healthUpdate);
    }
}
```

### How to react to component property changes

The following code snippet registers a callback for whenever any property in the `Health` component gets updated.
This example MonoBehaviour would be enabled on any worker which has read access to the `Health` component.

```csharp
using Improbable.Examples;
using Improbable.Gdk.Subscriptions;

public class ReactToHealthChangeBehaviour : MonoBehaviour
{
    [Require] private HealthReader healthReader;

    private void OnEnable()
    {
        // Register callback for whenever any property of the component gets updated
        healthReader.OnUpdate += OnHealthComponentUpdated;
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

```csharp
using Improbable.Examples;
using Improbable.Gdk.Subscriptions;

public class ReactToCurrentHealthChangeBehaviour : MonoBehaviour
{
    [Require] private HealthReader healthReader;

    private void OnEnable()
    {
        // Register callback for whenever a specific property, e.g. current_health,
        // of the component gets updated
        healthReader.OnCurrentHealthUpdate += OnCurrentHealthUpdated;
    }

    private void OnCurrentHealthUpdated(int newCurrentHealth)
    {
        // do something
    }
}
```
