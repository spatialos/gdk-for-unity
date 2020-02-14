---
title: LogDispatcherSubscriptionManager Class
slug: api-subscriptions-logdispatchersubscriptionmanager
order: 20
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/LogDispatcherSubscriptionManager.cs/#L7">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Subscriptions.SubscriptionManager<ILogDispatcher>](doc:api-subscriptions-subscriptionmanager)</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="logdispatchersubscriptionmanager-world"></a><b>LogDispatcherSubscriptionManager</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/LogDispatcherSubscriptionManager.cs/#L11">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> LogDispatcherSubscriptionManager(World world)</code></p></p><b>Parameters</b><ul><li><code>World world</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscribe-entityid"></a><b>Subscribe</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/LogDispatcherSubscriptionManager.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override [Subscription](doc:api-subscriptions-subscription)&lt;[ILogDispatcher](doc:api-core-ilogdispatcher)&gt; Subscribe([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="cancel-isubscription"></a><b>Cancel</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/LogDispatcherSubscriptionManager.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void Cancel([ISubscription](doc:api-subscriptions-isubscription) subscription)</code></p></p><b>Parameters</b><ul><li><code>[ISubscription](doc:api-subscriptions-isubscription) subscription</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="resetvalue-isubscription"></a><b>ResetValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/LogDispatcherSubscriptionManager.cs/#L29">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void ResetValue([ISubscription](doc:api-subscriptions-isubscription) subscription)</code></p></p><b>Parameters</b><ul><li><code>[ISubscription](doc:api-subscriptions-isubscription) subscription</code> : </li></ul></td>    </tr></table>


