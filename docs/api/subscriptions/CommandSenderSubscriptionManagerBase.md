---
title: CommandSenderSubscriptionManagerBase<T> Class
slug: api-subscriptions-commandsendersubscriptionmanagerbase
order: 6
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CommandSenderReceiverSubscriptionManagerBase.cs/#L23">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable::Gdk::Subscriptions::SubscriptionManager<T>](doc:api-subscriptions-subscriptionmanager)</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandsendersubscriptionmanagerbase-world"></a><b>CommandSenderSubscriptionManagerBase</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CommandSenderReceiverSubscriptionManagerBase.cs/#L29">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> CommandSenderSubscriptionManagerBase(World world)</code></p></p><b>Parameters</b><ul><li><code>World world</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createsender-entity-world"></a><b>CreateSender</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CommandSenderReceiverSubscriptionManagerBase.cs/#L68">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>abstract T CreateSender(Entity entity, World world)</code></p></p><b>Parameters</b><ul><li><code>Entity entity</code> : </li><li><code>World world</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscribe-entityid"></a><b>Subscribe</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CommandSenderReceiverSubscriptionManagerBase.cs/#L70">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override [Subscription](doc:api-subscriptions-subscription)&lt;T&gt; Subscribe([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="cancel-isubscription"></a><b>Cancel</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CommandSenderReceiverSubscriptionManagerBase.cs/#L103">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void Cancel([ISubscription](doc:api-subscriptions-isubscription) subscription)</code></p></p><b>Parameters</b><ul><li><code>[ISubscription](doc:api-subscriptions-isubscription) subscription</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="resetvalue-isubscription"></a><b>ResetValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CommandSenderReceiverSubscriptionManagerBase.cs/#L120">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void ResetValue([ISubscription](doc:api-subscriptions-isubscription) subscription)</code></p></p><b>Parameters</b><ul><li><code>[ISubscription](doc:api-subscriptions-isubscription) subscription</code> : </li></ul></td>    </tr></table>


