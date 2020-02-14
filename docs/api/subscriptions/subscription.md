---
title: Subscription<T> Class
slug: api-subscriptions-subscription
order: 26
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L24">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Subscriptions.ISubscription](doc:api-subscriptions-isubscription)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hasvalue"></a><b>HasValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L26">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasValue</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="value"></a><b>Value</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L28">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T Value</code></p></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entityid"></a><b>EntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L27">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> [EntityId](doc:api-core-entityid) EntityId { get; }</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onavailable"></a><b>OnAvailable</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L38">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Action&lt;T&gt; OnAvailable {  }</code></p></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscription-subscriptionmanagerbase-entityid"></a><b>Subscription</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L59">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Subscription([SubscriptionManagerBase](doc:api-subscriptions-subscriptionmanagerbase) manager, [EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[SubscriptionManagerBase](doc:api-subscriptions-subscriptionmanagerbase) manager</code> : </li><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="cancel"></a><b>Cancel</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L65">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Cancel()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setavailable-t"></a><b>SetAvailable</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L72">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetAvailable(T subscribedObject)</code></p></p><b>Parameters</b><ul><li><code>T subscribedObject</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setunavailable"></a><b>SetUnavailable</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Subscription.cs/#L87">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetUnavailable()</code></p></td>    </tr></table>



