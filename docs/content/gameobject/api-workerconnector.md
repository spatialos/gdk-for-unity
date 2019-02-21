[//]: # (Doc of docs reference 15.1a)
<%(TOC)%>
# API - WorkerConnector
_This document relates to the [MonoBehaviour workflow]({{urlRoot}}/content/intro-workflows-spatialos-entities)._

Before reading this document, see the documentation on [(MonoBehaviour) Creating workers with the WorkerConnector]({{urlRoot}}/content/gameobject/creating-workers-with-workerconnector).

During the creation of your workers, they attempt to connect to the SpatialOS Runtime. Only a successful connection leads to the creation of a worker. Many of the fields relate to this.

**Fields**

| Field                	| Type       	| Description                	|
|--------------------------|----------------|--------------------------------|
| MaxConnectionAttempts	| int        	| The number of connection attempts to make before failing to create this worker. The default is `3`. |
| UseExternalIp        	| bool       	| Defines whether the workers should use the external IP to connect to the SpatialOS Runtime. This is needed for client-workers when connecting to a cloud deployment. The default is `false`. |
| Worker               	| Worker     	| A reference to the worker that is created upon successful connection to the SpatialOS Runtime. |
| OnWorkerCreationFinished | Action<Worker> | A callback that is invoked when the worker was created successfully. |


**Methods**

```csharp
public async Task Connect(string workerType, ILogDispatcher logger);
```
Parameters:

  * `string workerType`: The type of the worker
  * `ILogDispatcher logger`: The logdispatcher to use for [logging]({{urlRoot}}/content/ecs/logging) on this worker.

Returns: a task which finishes when the worker either connects or fails to connect.

```csharp
protected abstract bool ShouldUseLocator();
```

Returns: true, if the worker should connect using the Locator flow, false otherwise.

```csharp
protected virtual string SelectDeploymentName(DeploymentList deployments);
```
Only use this method when connecting through the [Locator connection flow]({{urlRoot}}/content/glossary#locator-connection-flow).

Parameters:

  * `DeploymentList deployment`: a list of all deployments for the project name specified in the `spatialos.json` file.

Returns: the name of the selected cloud deployment.

```csharp
protected abstract ReceptionistConfig GetReceptionistConfig(string workerType);
```
This method is only used when connecting through the [Receptionist connection flow]({{urlRoot}}/content/glossary#receptionist-connection-flow).

Parameters:

* `string workerType`: The [type of the worker]({{urlRoot}}/content/glossary#worker-types)

Returns: a connection configuration to connect using the [Receptionist connection flow]({{urlRoot}}/content/glossary#receptionist-connection-flow).

```csharp
protected abstract LocatorConfig GetLocatorConfig(string workerType);
```
This method is only used when connecting through the [Locator connection flow]({{urlRoot}}/content/glossary#locator-connection-flow).

Parameters:

  * `string workerType`: The [type of the worker]({{urlRoot}}/content/glossary#worker-types)

Returns: a connection configuration to connect using the [Locator connection flow]({{urlRoot}}/content/glossary#locator-connection-flow) stored
as a [`LocatorConfig`](https://github.com/spatialos/gdk-for-unity/blob/master/workers/unity/Packages/com.improbable.gdk.core/Config/LocatorConfig.cs) object.

```csharp
protected virtual void HandleWorkerConnectionEstablished();
```
This method provides a way to add additional logic after a connection has been established.


```csharp
protected virtual void HandleWorkerConnectionFailure(string errorMessage);
```
This method provides a way to add additional logic after the `WorkerConnector` failed to create a connection.

Parameters:

  * `string errorMessage`: Contains the reason for failing the connection.

```csharp
public virtual void Dispose();
```
Disposes of the `WorkerConnector` object.
