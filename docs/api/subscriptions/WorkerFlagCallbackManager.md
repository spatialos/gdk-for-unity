---
title: WorkerFlagCallbackManager Class
slug: api-subscriptions-workerflagcallbackmanager
order: 31
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/WorkerFlagCallbackManager.cs/#L7">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>Improbable.Gdk.Subscriptions.ICallbackManager</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workerflagcallbackmanager-world"></a><b>WorkerFlagCallbackManager</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/WorkerFlagCallbackManager.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> WorkerFlagCallbackManager(World world)</code></p></p><b>Parameters</b><ul><li><code>World world</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="registercallback-action-string-string"></a><b>RegisterCallback</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/WorkerFlagCallbackManager.cs/#L19">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ulong RegisterCallback(Action&lt;(string, string)&gt; callback)</code></p></p><b>Parameters</b><ul><li><code>Action&lt;(string, string)&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="unregistercallback-ulong"></a><b>UnregisterCallback</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/WorkerFlagCallbackManager.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool UnregisterCallback(ulong callbackKey)</code></p></p><b>Parameters</b><ul><li><code>ulong callbackKey</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="invokecallbacks"></a><b>InvokeCallbacks</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/WorkerFlagCallbackManager.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void InvokeCallbacks()</code></p></td>    </tr></table>



