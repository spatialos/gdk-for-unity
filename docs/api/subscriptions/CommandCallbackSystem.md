---
title: CommandCallbackSystem Class
slug: api-subscriptions-commandcallbacksystem
order: 2
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L14">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registercommandrequestcallback-t-entityid-action-t"></a><b>RegisterCommandRequestCallback&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ulong RegisterCommandRequestCallback&lt;T&gt;([EntityId](doc:api-core-entityid) entityId, Action&lt;T&gt; callback)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>Action&lt;T&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registercommandresponsecallback-t-long-action-t"></a><b>RegisterCommandResponseCallback&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L38">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RegisterCommandResponseCallback&lt;T&gt;(long requestId, Action&lt;T&gt; callback)</code></p></p><b>Parameters</b><ul><li><code>long requestId</code> : </li><li><code>Action&lt;T&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="unregistercommandrequestcallback-ulong"></a><b>UnregisterCommandRequestCallback</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool UnregisterCommandRequestCallback(ulong callbackKey)</code></p></p><b>Parameters</b><ul><li><code>ulong callbackKey</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L66">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L72">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


