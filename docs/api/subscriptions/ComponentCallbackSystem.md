---
title: ComponentCallbackSystem Class
slug: api-subscriptions-componentcallbacksystem
order: 7
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L13">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registercomponentupdatecallback-t-entityid-action-t"></a><b>RegisterComponentUpdateCallback&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L26">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ulong RegisterComponentUpdateCallback&lt;T&gt;([EntityId](doc:api-core-entityid) entityId, Action&lt;T&gt; callback)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>Action&lt;T&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registercomponenteventcallback-t-entityid-action-t"></a><b>RegisterComponentEventCallback&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L40">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ulong RegisterComponentEventCallback&lt;T&gt;([EntityId](doc:api-core-entityid) entityId, Action&lt;T&gt; callback)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>Action&lt;T&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registerauthoritycallback-entityid-uint-action-authority"></a><b>RegisterAuthorityCallback</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L54">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ulong RegisterAuthorityCallback([EntityId](doc:api-core-entityid) entityId, uint componentId, Action&lt;Authority&gt; callback)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li><li><code>Action&lt;Authority&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="unregistercallback-ulong"></a><b>UnregisterCallback</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L67">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool UnregisterCallback(ulong callbackKey)</code></p></p><b>Parameters</b><ul><li><code>ulong callbackKey</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L88">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L94">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


