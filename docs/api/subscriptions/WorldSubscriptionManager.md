---
title: WorldSubscriptionManager Class
slug: api-subscriptions-worldsubscriptionmanager
order: 36
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldSubscriptionManager.cs/#L7">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Subscriptions.SubscriptionManager<World>](doc:api-subscriptions-subscriptionmanager)</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worldsubscriptionmanager-world"></a><b>WorldSubscriptionManager</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldSubscriptionManager.cs/#L9">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> WorldSubscriptionManager(World world)</code></p></p><b>Parameters</b><ul><li><code>World world</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscribe-entityid"></a><b>Subscribe</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldSubscriptionManager.cs/#L13">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override [Subscription](doc:api-subscriptions-subscription)&lt;World&gt; Subscribe([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="cancel-isubscription"></a><b>Cancel</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldSubscriptionManager.cs/#L21">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void Cancel([ISubscription](doc:api-subscriptions-isubscription) subscription)</code></p></p><b>Parameters</b><ul><li><code>[ISubscription](doc:api-subscriptions-isubscription) subscription</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="resetvalue-isubscription"></a><b>ResetValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldSubscriptionManager.cs/#L26">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void ResetValue([ISubscription](doc:api-subscriptions-isubscription) subscription)</code></p></p><b>Parameters</b><ul><li><code>[ISubscription](doc:api-subscriptions-isubscription) subscription</code> : </li></ul></td>    </tr></table>


