---
title: Subscriptions
slug: api-subscriptions
order: 8
---

## Subscriptions
<h4 class="header-scroll"><div class="anchor waypoint" id="classes-subscriptions"></div>Classes<a class="fa fa-anchor" href="#classes-subscriptions"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[AutoRegisterSubscriptionManagerAttribute](doc:api-subscriptions-autoregistersubscriptionmanagerattribute)",
    "0-1": "",
    "1-0": "[CommandCallbackSystem](doc:api-subscriptions-commandcallbacksystem)",
    "1-1": "",
    "2-0": "[CommandReceiverSubscriptionManagerBase<T>](doc:api-subscriptions-commandreceiversubscriptionmanagerbase)",
    "2-1": "",
    "3-0": "[CommandRequestCallbackManager<T>](doc:api-subscriptions-commandrequestcallbackmanager)",
    "3-1": "",
    "4-0": "[CommandResponseCallbackManager<T>](doc:api-subscriptions-commandresponsecallbackmanager)",
    "4-1": "",
    "5-0": "[CommandSenderSubscriptionManagerBase<T>](doc:api-subscriptions-commandsendersubscriptionmanagerbase)",
    "5-1": "",
    "6-0": "[ComponentCallbackSystem](doc:api-subscriptions-componentcallbacksystem)",
    "6-1": "",
    "7-0": "[ComponentConstraintsCallbackSystem](doc:api-subscriptions-componentconstraintscallbacksystem)",
    "7-1": "",
    "8-0": "[EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker)",
    "8-1": "",
    "9-0": "[EntityIdSubscriptionManager](doc:api-subscriptions-entityidsubscriptionmanager)",
    "9-1": "",
    "10-0": "[EntitySubscriptionManager](doc:api-subscriptions-entitysubscriptionmanager)",
    "10-1": "",
    "11-0": "[IndexedCallbacks<T>](doc:api-subscriptions-indexedcallbacks)",
    "11-1": "",
    "12-0": "[LinkedEntityComponent](doc:api-subscriptions-linkedentitycomponent)",
    "12-1": "",
    "13-0": "[LinkedGameObjectMap](doc:api-subscriptions-linkedgameobjectmap)",
    "13-1": "Represents the mapping between SpatialOS entity IDs and linked GameObjects. ",
    "14-0": "[LogDispatcherSubscriptionManager](doc:api-subscriptions-logdispatchersubscriptionmanager)",
    "14-1": "",
    "15-0": "[RequireAttribute](doc:api-subscriptions-requireattribute)",
    "15-1": "",
    "16-0": "[RequiredSubscriptionsInjector](doc:api-subscriptions-requiredsubscriptionsinjector)",
    "16-1": "",
    "17-0": "[RequireLifecycleGroup](doc:api-subscriptions-requirelifecyclegroup)",
    "17-1": "",
    "18-0": "[RequireLifecycleSystem](doc:api-subscriptions-requirelifecyclesystem)",
    "18-1": "",
    "19-0": "[SingleUseIndexCallbacks<T>](doc:api-subscriptions-singleuseindexcallbacks)",
    "19-1": "",
    "20-0": "[Subscription<T>](doc:api-subscriptions-subscription)",
    "20-1": "",
    "21-0": "[SubscriptionAggregate](doc:api-subscriptions-subscriptionaggregate)",
    "21-1": "",
    "22-0": "[SubscriptionManager<T>](doc:api-subscriptions-subscriptionmanager)",
    "22-1": "",
    "23-0": "[SubscriptionManagerBase](doc:api-subscriptions-subscriptionmanagerbase)",
    "23-1": "",
    "24-0": "[SubscriptionSystem](doc:api-subscriptions-subscriptionsystem)",
    "24-1": "",
    "25-0": "[WorkerFlagCallbackManager](doc:api-subscriptions-workerflagcallbackmanager)",
    "25-1": "",
    "26-0": "[WorkerFlagCallbackSystem](doc:api-subscriptions-workerflagcallbacksystem)",
    "26-1": "",
    "27-0": "[WorkerIdSubscriptionManager](doc:api-subscriptions-workeridsubscriptionmanager)",
    "27-1": "",
    "28-0": "[WorkerTypeAttribute](doc:api-subscriptions-workertypeattribute)",
    "28-1": "Marks MonoBehaviours which want to be enabled only for particular worker types. ",
    "29-0": "[WorldSubscriptionManager](doc:api-subscriptions-worldsubscriptionmanager)",
    "29-1": ""
  },
  "cols": "2",
  "rows": "30"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="structs-subscriptions"></div>Structs<a class="fa fa-anchor" href="#structs-subscriptions"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[WorkerId](doc:api-subscriptions-workerid)",
    "0-1": ""
  },
  "cols": "2",
  "rows": "1"
}
[/block]


<h4 class="header-scroll"><div class="anchor waypoint" id="interfaces-subscriptions"></div>Interfaces<a class="fa fa-anchor" href="#interfaces-subscriptions"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[ICommandReceiver](doc:api-subscriptions-icommandreceiver)",
    "0-1": "",
    "1-0": "[ICommandSender](doc:api-subscriptions-icommandsender)",
    "1-1": "",
    "2-0": "[ISubscription](doc:api-subscriptions-isubscription)",
    "2-1": "",
    "3-0": "[ISubscriptionAvailabilityHandler](doc:api-subscriptions-isubscriptionavailabilityhandler)",
    "3-1": ""
  },
  "cols": "2",
  "rows": "4"
}
[/block]



## Subscriptions.Editor
<h4 class="header-scroll"><div class="anchor waypoint" id="classes-subscriptions-editor"></div>Classes<a class="fa fa-anchor" href="#classes-subscriptions-editor"></a></h4>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[PrefabPreprocessor](doc:api-subscriptions-editor-prefabpreprocessor)",
    "0-1": "A prefab processor which disables any Monobehaviours on top-level GameObjects in the Resources folder which have fields with the [RequireAttribute](doc:api-subscriptions-requireattribute) on them. "
  },
  "cols": "2",
  "rows": "1"
}
[/block]





