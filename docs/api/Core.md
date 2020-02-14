---
title: Core
slug: api-core
order: 2
---

## Core
<h4 class="header-scroll"><div class="anchor waypoint" id="classes-core"></div>Classes<a class="fa fa-anchor" href="#classes-core"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[AuthenticationFailedException](doc:api-core-authenticationfailedexception)",
    "0-1": "Represents an error that occurs when the player fails to authenticate via the anonymous authentication flow. ",
    "1-0": "[CleanTemporaryComponentsSystem](doc:api-core-cleantemporarycomponentssystem)",
    "1-1": "Removes components with attribute RemoveAtEndOfTick from all entities ",
    "2-0": "[CommandLineArgs](doc:api-core-commandlineargs)",
    "2-1": "",
    "3-0": "[CommandLineConnectionFlowInitializer](doc:api-core-commandlineconnectionflowinitializer)",
    "3-1": "Represents an object which can initialize the [ReceptionistFlow](doc:api-core-receptionistflow), [LocatorFlow](doc:api-core-locatorflow), and [LocatorFlow](doc:api-core-locatorflow) connection flows from the command line. ",
    "4-0": "[CommandLineConnectionParameterInitializer](doc:api-core-commandlineconnectionparameterinitializer)",
    "4-1": "",
    "5-0": "[CommandMetaData](doc:api-core-commandmetadata)",
    "5-1": "",
    "6-0": "[CommandMetaDataAggregate](doc:api-core-commandmetadataaggregate)",
    "6-1": "",
    "7-0": "[CommandPayloadStorage<T>](doc:api-core-commandpayloadstorage)",
    "7-1": "",
    "8-0": "[CommandRequestIdGenerator](doc:api-core-commandrequestidgenerator)",
    "8-1": "",
    "9-0": "[CommandSystem](doc:api-core-commandsystem)",
    "9-1": "",
    "10-0": "[ComponentDatabase](doc:api-core-componentdatabase)",
    "10-1": "",
    "11-0": "[ComponentSendSystem](doc:api-core-componentsendsystem)",
    "11-1": "Executes the default replication logic for each SpatialOS component. ",
    "12-0": "[ComponentUpdateSystem](doc:api-core-componentupdatesystem)",
    "12-1": "",
    "13-0": "[ConcurrentOpListConverter](doc:api-core-concurrentoplistconverter)",
    "13-1": "Simplest possible current op-list-to-diff converter. Acquires a lock when deserializing an op list and releases it when done ",
    "14-0": "[ConnectionFailedException](doc:api-core-connectionfailedexception)",
    "14-1": "Represents an error that occurs when a connection attempt failed. ",
    "15-0": "[CustomSpatialOSSendGroup](doc:api-core-spatialossendgroup-customspatialossendgroup)",
    "15-1": "",
    "16-0": "[CustomSpatialOSSendSystem<T>](doc:api-core-customspatialossendsystem)",
    "16-1": "Base class for custom replication. ",
    "17-0": "[Dynamic](doc:api-core-dynamic)",
    "17-1": "",
    "18-0": "[EcsViewSystem](doc:api-core-ecsviewsystem)",
    "18-1": "",
    "19-0": "[EmptyOptionException](doc:api-core-emptyoptionexception)",
    "19-1": "Represents an error when an [Option](doc:api-core-option)'s contained value is attempted to be accessed when the option is empty. ",
    "20-0": "[EntitySystem](doc:api-core-entitysystem)",
    "20-1": "",
    "21-0": "[EntityTemplate](doc:api-core-entitytemplate)",
    "21-1": "Utility class to help build SpatialOS entities. An [EntityTemplate](doc:api-core-entitytemplate) can be mutated be used multiple times. ",
    "22-0": "[FixedUpdateSystemGroup](doc:api-core-fixedupdatesystemgroup)",
    "22-1": "",
    "23-0": "[ForwardingDispatcher](doc:api-core-forwardingdispatcher)",
    "23-1": "Forwards logEvents and exceptions to the SpatialOS Console and logs locally. ",
    "24-0": "[InternalSpatialOSCleanGroup](doc:api-core-internalspatialoscleangroup)",
    "24-1": "",
    "25-0": "[InternalSpatialOSReceiveGroup](doc:api-core-spatialosreceivegroup-internalspatialosreceivegroup)",
    "25-1": "",
    "26-0": "[InternalSpatialOSSendGroup](doc:api-core-spatialossendgroup-internalspatialossendgroup)",
    "26-1": "",
    "27-0": "[LinqExtensions](doc:api-core-linqextensions)",
    "27-1": "",
    "28-0": "[LocatorFlow](doc:api-core-locatorflow)",
    "28-1": "Represents the Alpha Locator connection flow. ",
    "29-0": "[LoggingDispatcher](doc:api-core-loggingdispatcher)",
    "29-1": "Logs to the Unity Console. ",
    "30-0": "[LoggingUtils](doc:api-core-loggingutils)",
    "30-1": "",
    "31-0": "[MessagesToSend](doc:api-core-messagestosend)",
    "31-1": "",
    "32-0": "[MockConnectionHandler](doc:api-core-mockconnectionhandler)",
    "32-1": "",
    "33-0": "[MockConnectionHandlerBuilder](doc:api-core-mockconnectionhandlerbuilder)",
    "33-1": "",
    "34-0": "[MultiThreadedSpatialOSConnectionHandler](doc:api-core-multithreadedspatialosconnectionhandler)",
    "34-1": "",
    "35-0": "[ReceptionistFlow](doc:api-core-receptionistflow)",
    "35-1": "Represents the Receptionist connection flow. ",
    "36-0": "[RemoveAtEndOfTickAttribute](doc:api-core-removeatendoftickattribute)",
    "36-1": "Any component with this attribute will be removed from all entities by the CleanTemporaryComponentSystem Can be added to components extending IComponentData or ISharedComponentData ",
    "37-0": "[RuntimeConfigDefaults](doc:api-core-runtimeconfigdefaults)",
    "37-1": "Default values for connection parameters. ",
    "38-0": "[RuntimeConfigNames](doc:api-core-runtimeconfignames)",
    "38-1": "Command line argument names for worker and connection configuration. ",
    "39-0": "[SchemaObjectExtensions](doc:api-core-schemaobjectextensions)",
    "39-1": "",
    "40-0": "[SerializedMessagesToSend](doc:api-core-serializedmessagestosend)",
    "40-1": "",
    "41-0": "[Snapshot](doc:api-core-snapshot)",
    "41-1": "Convenience wrapper around the WorkerSDK [Snapshot](doc:api-core-snapshot) API. ",
    "42-0": "[SpatialOSConnectionHandler](doc:api-core-spatialosconnectionhandler)",
    "42-1": "",
    "43-0": "[SpatialOSConnectionHandlerBuilder](doc:api-core-spatialosconnectionhandlerbuilder)",
    "43-1": "",
    "44-0": "[SpatialOSReceiveGroup](doc:api-core-spatialosreceivegroup)",
    "44-1": "",
    "45-0": "[SpatialOSReceiveSystem](doc:api-core-spatialosreceivesystem)",
    "45-1": "Receives incoming messages from the SpatialOS runtime. ",
    "46-0": "[SpatialOSSendGroup](doc:api-core-spatialossendgroup)",
    "46-1": "",
    "47-0": "[SpatialOSSendSystem](doc:api-core-spatialossendsystem)",
    "47-1": "",
    "48-0": "[SpatialOSUpdateGroup](doc:api-core-spatialosupdategroup)",
    "48-1": "",
    "49-0": "[TaskUtility](doc:api-core-taskutility)",
    "49-1": "",
    "50-0": "[UnityObjectDestroyer](doc:api-core-unityobjectdestroyer)",
    "50-1": "Provides a helper method for calling Object.DestroyImmediate() instead of Object.Destroy() in EditMode unit tests. ",
    "51-0": "[View](doc:api-core-view)",
    "51-1": "",
    "52-0": "[ViewCommandBuffer](doc:api-core-viewcommandbuffer)",
    "52-1": "This class is for when a user wants to Add/Remove a Component (not IComponentData) during a system update without invalidating their injected arrays. The user must call Flush on this buffer at the end of the Update function to apply the buffered changes. ",
    "53-0": "[ViewDiff](doc:api-core-viewdiff)",
    "53-1": "",
    "54-0": "[Worker](doc:api-core-worker)",
    "54-1": "Represents a SpatialOS worker. ",
    "55-0": "[WorkerConnector](doc:api-core-workerconnector)",
    "55-1": "Connect workers via Monobehaviours. ",
    "56-0": "[WorkerDisconnectCallbackSystem](doc:api-core-workerdisconnectcallbacksystem)",
    "56-1": "Enables users to add a callback onto the disconnection event. ",
    "57-0": "[WorkerFlagReader](doc:api-core-workerflagreader)",
    "57-1": "",
    "58-0": "[WorkerFlagSubscriptionManager](doc:api-core-workerflagsubscriptionmanager)",
    "58-1": "",
    "59-0": "[WorkerInWorld](doc:api-core-workerinworld)",
    "59-1": "Represents a SpatialOS worker that is coupled with an ECS World. ",
    "60-0": "[WorkerSystem](doc:api-core-workersystem)",
    "60-1": "A SpatialOS worker instance. ",
    "61-0": "[WorldCommandMetaDataStorage](doc:api-core-worldcommandmetadatastorage)",
    "61-1": "",
    "62-0": "[WorldCommandSender](doc:api-core-worldcommandsender)",
    "62-1": "",
    "63-0": "[WorldCommandSenderSubscriptionManager](doc:api-core-worldcommandsendersubscriptionmanager)",
    "63-1": "",
    "64-0": "[WorldCommandSerializer](doc:api-core-worldcommandserializer)",
    "64-1": "",
    "65-0": "[WorldCommandsReceivedStorage](doc:api-core-worldcommandsreceivedstorage)",
    "65-1": "",
    "66-0": "[WorldCommandsToSendStorage](doc:api-core-worldcommandstosendstorage)",
    "66-1": "",
    "67-0": "[WorldsInitializationHelper](doc:api-core-worldsinitializationhelper)",
    "67-1": ""
  },
  "cols": "2",
  "rows": "68"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="structs-core"></div>Structs<a class="fa fa-anchor" href="#structs-core"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[AuthorityChangeReceived](doc:api-core-authoritychangereceived)",
    "0-1": "",
    "1-0": "[CommandContext<T>](doc:api-core-commandcontext)",
    "1-1": "",
    "2-0": "[ComponentEventReceived<T>](doc:api-core-componenteventreceived)",
    "2-1": "",
    "3-0": "[ComponentEventToSend<T>](doc:api-core-componenteventtosend)",
    "3-1": "",
    "4-0": "[ComponentUpdateReceived<T>](doc:api-core-componentupdatereceived)",
    "4-1": "",
    "5-0": "[ComponentUpdateToSend<T>](doc:api-core-componentupdatetosend)",
    "5-1": "",
    "6-0": "[EntityComponent](doc:api-core-entitycomponent)",
    "6-1": "",
    "7-0": "[EntityId](doc:api-core-entityid)",
    "7-1": "A unique identifier used to look up an entity in SpatialOS. ",
    "8-0": "[EntitySnapshot](doc:api-core-entitysnapshot)",
    "8-1": "A snapshot of a SpatialOS entity. ",
    "9-0": "[LogEvent](doc:api-core-logevent)",
    "9-1": "Represents a single log. Can contain data used for structured logging. ",
    "10-0": "[LogMessageReceived](doc:api-core-logmessagereceived)",
    "10-1": "",
    "11-0": "[LogMessageToSend](doc:api-core-logmessagetosend)",
    "11-1": "",
    "12-0": "[MessagesSpan<T>](doc:api-core-messagesspan)",
    "12-1": "",
    "13-0": "[NewlyAddedSpatialOSEntity](doc:api-core-newlyaddedspatialosentity)",
    "13-1": "Tag component for marking SpatialOS entities that were just checked-out and still require setup. This component is automatically added to an entity upon its creation and automatically removed at the end of the same frame. ",
    "14-0": "[OnConnected](doc:api-core-onconnected)",
    "14-1": "ECS Component added to the worker entity immediately after establishing a connection to a SpatialOS deployment. ",
    "15-0": "[OnDisconnected](doc:api-core-ondisconnected)",
    "15-1": "ECS Component added to the worker entity immediately after disconnecting from SpatialOS ",
    "16-0": "[Option<T>](doc:api-core-option)",
    "16-1": "An implementation of [Option](doc:api-core-option) which is compatible with Unity's ECS. ",
    "17-0": "[SpatialEntityId](doc:api-core-spatialentityid)",
    "17-1": "ECS component which contains the SpatialOS Entity ID. ",
    "18-0": "[VTable<TUpdate, TSnapshot>](doc:api-core-dynamic-vtable)",
    "18-1": "",
    "19-0": "[WorkerEntityTag](doc:api-core-workerentitytag)",
    "19-1": "ECS Component denoting a worker entity "
  },
  "cols": "2",
  "rows": "20"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="interfaces-core"></div>Interfaces<a class="fa fa-anchor" href="#interfaces-core"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[ICommandDiffDeserializer](doc:api-core-icommanddiffdeserializer)",
    "0-1": "",
    "1-0": "[ICommandDiffStorage](doc:api-core-icommanddiffstorage)",
    "1-1": "",
    "2-0": "[ICommandMetaDataStorage](doc:api-core-icommandmetadatastorage)",
    "2-1": "",
    "3-0": "[ICommandPayloadStorage<T>](doc:api-core-icommandpayloadstorage)",
    "3-1": "",
    "4-0": "[ICommandRequestSendStorage<T>](doc:api-core-icommandrequestsendstorage)",
    "4-1": "",
    "5-0": "[ICommandResponseSendStorage<T>](doc:api-core-icommandresponsesendstorage)",
    "5-1": "",
    "6-0": "[ICommandSendStorage](doc:api-core-icommandsendstorage)",
    "6-1": "",
    "7-0": "[ICommandSerializer](doc:api-core-icommandserializer)",
    "7-1": "",
    "8-0": "[IComponentCommandDiffStorage](doc:api-core-icomponentcommanddiffstorage)",
    "8-1": "",
    "9-0": "[IComponentCommandSendStorage](doc:api-core-icomponentcommandsendstorage)",
    "9-1": "",
    "10-0": "[IComponentDiffDeserializer](doc:api-core-icomponentdiffdeserializer)",
    "10-1": "",
    "11-0": "[IComponentDiffStorage](doc:api-core-icomponentdiffstorage)",
    "11-1": "",
    "12-0": "[IComponentMetaclass](doc:api-core-icomponentmetaclass)",
    "12-1": "",
    "13-0": "[IComponentSerializer](doc:api-core-icomponentserializer)",
    "13-1": "",
    "14-0": "[IConnectionFlow](doc:api-core-iconnectionflow)",
    "14-1": "Represents an implementation of a flow to connect to SpatialOS. ",
    "15-0": "[IConnectionFlowInitializer<TConnectionFlow>](doc:api-core-iconnectionflowinitializer)",
    "15-1": "Represents an object which can initialize a connection flow of a certain type. ",
    "16-0": "[IConnectionHandler](doc:api-core-iconnectionhandler)",
    "16-1": "Represents a handler for a SpatialOS connection. ",
    "17-0": "[IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder)",
    "17-1": "Intermediate object for building a [IConnectionHandler](doc:api-core-iconnectionhandler) object. ",
    "18-0": "[IConnectionParameterInitializer](doc:api-core-iconnectionparameterinitializer)",
    "18-1": "Represents an object which can initialize the connection parameters. ",
    "19-0": "[IDiffAuthorityStorage](doc:api-core-idiffauthoritystorage)",
    "19-1": "",
    "20-0": "[IDiffCommandRequestStorage<T>](doc:api-core-idiffcommandrequeststorage)",
    "20-1": "",
    "21-0": "[IDiffCommandResponseStorage<T>](doc:api-core-idiffcommandresponsestorage)",
    "21-1": "",
    "22-0": "[IDiffComponentAddedStorage<T>](doc:api-core-idiffcomponentaddedstorage)",
    "22-1": "",
    "23-0": "[IDiffEventStorage<T>](doc:api-core-idiffeventstorage)",
    "23-1": "",
    "24-0": "[IDiffUpdateStorage<T>](doc:api-core-idiffupdatestorage)",
    "24-1": "",
    "25-0": "[IDynamicInvokable](doc:api-core-idynamicinvokable)",
    "25-1": "",
    "26-0": "[IEcsViewManager](doc:api-core-iecsviewmanager)",
    "26-1": "",
    "27-0": "[IEvent](doc:api-core-ievent)",
    "27-1": "",
    "28-0": "[IHandler](doc:api-core-dynamic-ihandler)",
    "28-1": "",
    "29-0": "[ILogDispatcher](doc:api-core-ilogdispatcher)",
    "29-1": "The [ILogDispatcher](doc:api-core-ilogdispatcher) interface is used to implement different types of loggers. By default, the [ILogDispatcher](doc:api-core-ilogdispatcher) supports structured logging. ",
    "30-0": "[IReceivedEntityMessage](doc:api-core-ireceivedentitymessage)",
    "30-1": "",
    "31-0": "[IReceivedMessage](doc:api-core-ireceivedmessage)",
    "31-1": "",
    "32-0": "[ISnapshottable<T>](doc:api-core-isnapshottable)",
    "32-1": "Denotes that an object can be snapshotted. ",
    "33-0": "[ISpatialComponentData](doc:api-core-ispatialcomponentdata)",
    "33-1": "Denotes that an object is a SpatialOS component. ",
    "34-0": "[ISpatialComponentSnapshot](doc:api-core-ispatialcomponentsnapshot)",
    "34-1": "",
    "35-0": "[ISpatialComponentUpdate](doc:api-core-ispatialcomponentupdate)",
    "35-1": "Denotes that an object represents a SpatialOS component update. ",
    "36-0": "[IViewComponentStorage<T>](doc:api-core-iviewcomponentstorage)",
    "36-1": "",
    "37-0": "[IViewComponentUpdater<T>](doc:api-core-iviewcomponentupdater)",
    "37-1": "",
    "38-0": "[IViewStorage](doc:api-core-iviewstorage)",
    "38-1": ""
  },
  "cols": "2",
  "rows": "39"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="enums-core"></div>Enums<a class="fa fa-anchor" href="#enums-core"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[ConnectionErrorReason](doc:api-core-connectionerrorreason)",
    "0-1": "Describes why the connection failed. ",
    "1-0": "[ConnectionService](doc:api-core-connectionservice)",
    "1-1": "An enum listing the available connection services. "
  },
  "cols": "2",
  "rows": "2"
}
[/block]


## Core.CodegenAdapters


<h4 class="header-scroll"><div class="anchor waypoint" id="interfaces-core-codegenadapters"></div>Interfaces<a class="fa fa-anchor" href="#interfaces-core-codegenadapters"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[IComponentReplicationHandler](doc:api-core-codegenadapters-icomponentreplicationhandler)",
    "0-1": ""
  },
  "cols": "2",
  "rows": "1"
}
[/block]



## Core.Collections

<h4 class="header-scroll"><div class="anchor waypoint" id="structs-core-collections"></div>Structs<a class="fa fa-anchor" href="#structs-core-collections"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[Result<T, E>](doc:api-core-collections-result)",
    "0-1": "A type to represent a result. Can either have a success value or an error, but not both. "
  },
  "cols": "2",
  "rows": "1"
}
[/block]




## Core.Commands
<h4 class="header-scroll"><div class="anchor waypoint" id="classes-core-commands"></div>Classes<a class="fa fa-anchor" href="#classes-core-commands"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[CommandSendStorage<TRequest, TResponse>](doc:api-core-commands-commandsendstorage)",
    "0-1": "",
    "1-0": "[CreateEntity](doc:api-core-commands-worldcommands-createentity)",
    "1-1": "",
    "2-0": "[DeleteEntity](doc:api-core-commands-worldcommands-deleteentity)",
    "2-1": "",
    "3-0": "[DiffSpawnCubeCommandStorage<TRequest, TResponse>](doc:api-core-commands-diffspawncubecommandstorage)",
    "3-1": "",
    "4-0": "[EntityQuery](doc:api-core-commands-worldcommands-entityquery)",
    "4-1": "",
    "5-0": "[ReserveEntityIds](doc:api-core-commands-worldcommands-reserveentityids)",
    "5-1": "",
    "6-0": "[WorldCommands](doc:api-core-commands-worldcommands)",
    "6-1": ""
  },
  "cols": "2",
  "rows": "7"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="structs-core-commands"></div>Structs<a class="fa fa-anchor" href="#structs-core-commands"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[ReceivedResponse](doc:api-core-commands-worldcommands-createentity-receivedresponse)",
    "0-1": "An object that is the response of a CreateEntity command from the SpatialOS runtime. ",
    "1-0": "[ReceivedResponse](doc:api-core-commands-worldcommands-deleteentity-receivedresponse)",
    "1-1": "An object that is the response of a DeleteEntity command from the SpatialOS runtime. ",
    "2-0": "[ReceivedResponse](doc:api-core-commands-worldcommands-entityquery-receivedresponse)",
    "2-1": "An object that is the response of an EntityQuery command from the SpatialOS runtime. ",
    "3-0": "[ReceivedResponse](doc:api-core-commands-worldcommands-reserveentityids-receivedresponse)",
    "3-1": "An object that is the response of a ReserveEntityIds command from the SpatialOS runtime. ",
    "4-0": "[Request](doc:api-core-commands-worldcommands-createentity-request)",
    "4-1": "An object that is a CreateEntity command request. ",
    "5-0": "[Request](doc:api-core-commands-worldcommands-deleteentity-request)",
    "5-1": "An object that is a DeleteEntity command request. ",
    "6-0": "[Request](doc:api-core-commands-worldcommands-entityquery-request)",
    "6-1": "An object that is a EntityQuery command request. ",
    "7-0": "[Request](doc:api-core-commands-worldcommands-reserveentityids-request)",
    "7-1": "An object that is a ReserveEntityIds command request. "
  },
  "cols": "2",
  "rows": "8"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="interfaces-core-commands"></div>Interfaces<a class="fa fa-anchor" href="#interfaces-core-commands"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[ICommandMetaclass](doc:api-core-commands-icommandmetaclass)",
    "0-1": "",
    "1-0": "[ICommandRequest](doc:api-core-commands-icommandrequest)",
    "1-1": "",
    "2-0": "[ICommandResponse](doc:api-core-commands-icommandresponse)",
    "2-1": "",
    "3-0": "[IRawReceivedCommandResponse](doc:api-core-commands-irawreceivedcommandresponse)",
    "3-1": "",
    "4-0": "[IReceivedCommandRequest](doc:api-core-commands-ireceivedcommandrequest)",
    "4-1": "",
    "5-0": "[IReceivedCommandResponse](doc:api-core-commands-ireceivedcommandresponse)",
    "5-1": ""
  },
  "cols": "2",
  "rows": "6"
}
[/block]



## Core.Editor
<h4 class="header-scroll"><div class="anchor waypoint" id="classes-core-editor"></div>Classes<a class="fa fa-anchor" href="#classes-core-editor"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[CommonUIElements](doc:api-core-editor-commonuielements)",
    "0-1": "",
    "1-0": "[SingletonScriptableObject<TSelf>](doc:api-core-editor-singletonscriptableobject)",
    "1-1": "Base object for a singleton scriptable object. ",
    "2-0": "[UIStateManager](doc:api-core-editor-uistatemanager)",
    "2-1": "Unity's GUIUtility.GetStateObject changes based on the structure of the GUI, for example when expanding or collapsing foldouts. Even with hints, tracking the state objects goes awry. This is a simpler implementation, meant to be used with object hashes generated by the call site, which at least has insight into what parts of the object will be stable enough to track. "
  },
  "cols": "2",
  "rows": "3"
}
[/block]





## Core.NetworkStats
<h4 class="header-scroll"><div class="anchor waypoint" id="classes-core-networkstats"></div>Classes<a class="fa fa-anchor" href="#classes-core-networkstats"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[NetFrameStats](doc:api-core-networkstats-netframestats)",
    "0-1": "Represents a single frame's data for either incoming or outgoing network messages. ",
    "1-0": "[NetStats](doc:api-core-networkstats-netstats)",
    "1-1": "Storage object for network data for a fixed number of frames. ",
    "2-0": "[NetworkStatisticsSystem](doc:api-core-networkstats-networkstatisticssystem)",
    "2-1": ""
  },
  "cols": "2",
  "rows": "3"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="structs-core-networkstats"></div>Structs<a class="fa fa-anchor" href="#structs-core-networkstats"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[DataPoint](doc:api-core-networkstats-datapoint)",
    "0-1": "",
    "1-0": "[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion)",
    "1-1": "Describes a type of a message. "
  },
  "cols": "2",
  "rows": "2"
}
[/block]



<h4 class="header-scroll"><div class="anchor waypoint" id="enums-core-networkstats"></div>Enums<a class="fa fa-anchor" href="#enums-core-networkstats"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[Direction](doc:api-core-networkstats-direction)",
    "0-1": "",
    "1-0": "[MessageType](doc:api-core-networkstats-messagetype)",
    "1-1": "",
    "2-0": "[WorldCommand](doc:api-core-networkstats-worldcommand)",
    "2-1": ""
  },
  "cols": "2",
  "rows": "3"
}
[/block]


