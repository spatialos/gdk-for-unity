---
title: SubscriptionSystem Class
slug: api-subscriptions-subscriptionsystem
order: 30
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L9">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registersubscriptionmanager-type-subscriptionmanagerbase"></a><b>RegisterSubscriptionManager</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RegisterSubscriptionManager(Type type, [SubscriptionManagerBase](doc:api-subscriptions-subscriptionmanagerbase) manager)</code></p></p><b>Parameters</b><ul><li><code>Type type</code> : </li><li><code>[SubscriptionManagerBase](doc:api-subscriptions-subscriptionmanagerbase) manager</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscribe-t-entityid"></a><b>Subscribe&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L32">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[Subscription](doc:api-subscriptions-subscription)&lt;T&gt; Subscribe&lt;T&gt;([EntityId](doc:api-core-entityid) entity)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entity</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscribe-entityid-type"></a><b>Subscribe</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L42">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[ISubscription](doc:api-subscriptions-isubscription) Subscribe([EntityId](doc:api-core-entityid) entity, Type type)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entity</code> : </li><li><code>Type type</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscribe-entityid-params-type"></a><b>Subscribe</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[SubscriptionAggregate](doc:api-subscriptions-subscriptionaggregate) Subscribe([EntityId](doc:api-core-entityid) entity, params Type[] types)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entity</code> : </li><li><code>params Type[] types</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L63">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


