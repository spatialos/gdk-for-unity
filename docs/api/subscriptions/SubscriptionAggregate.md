---
title: SubscriptionAggregate Class
slug: api-subscriptions-subscriptionaggregate
order: 27
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L7">Source</a></span></p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="subscriptionaggregate-ireadonlylist-type-isubscription"></a><b>SubscriptionAggregate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> SubscriptionAggregate(IReadOnlyList&lt;Type&gt; types, [ISubscription](doc:api-subscriptions-isubscription)[] subscriptions)</code></p></p><b>Parameters</b><ul><li><code>IReadOnlyList&lt;Type&gt; types</code> : </li><li><code>[ISubscription](doc:api-subscriptions-isubscription)[] subscriptions</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setavailabilityhandler-isubscriptionavailabilityhandler"></a><b>SetAvailabilityHandler</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L29">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetAvailabilityHandler([ISubscriptionAvailabilityHandler](doc:api-subscriptions-isubscriptionavailabilityhandler) handler)</code></p></p><b>Parameters</b><ul><li><code>[ISubscriptionAvailabilityHandler](doc:api-subscriptions-isubscriptionavailabilityhandler) handler</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getavailabilityhandler"></a><b>GetAvailabilityHandler</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L38">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[ISubscriptionAvailabilityHandler](doc:api-subscriptions-isubscriptionavailabilityhandler) GetAvailabilityHandler()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getvalue-t"></a><b>GetValue&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T GetValue&lt;T&gt;()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="geterasedvalue-type"></a><b>GetErasedValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L56">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>object GetErasedValue(Type type)</code></p></p><b>Parameters</b><ul><li><code>Type type</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="cancel"></a><b>Cancel</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L77">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Cancel()</code></p></td>    </tr></table>



