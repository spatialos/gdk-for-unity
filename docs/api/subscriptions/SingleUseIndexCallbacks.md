---
title: SingleUseIndexCallbacks<T> Class
slug: api-subscriptions-singleuseindexcallbacks
order: 25
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L7">Source</a></span></p>













</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="add-long-ulong-action-t"></a><b>Add</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L11">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Add(long index, ulong callbackKey, Action&lt;T&gt; value)</code></p></p><b>Parameters</b><ul><li><code>long index</code> : </li><li><code>ulong callbackKey</code> : </li><li><code>Action&lt;T&gt; value</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removeallcallbacksforindex-long"></a><b>RemoveAllCallbacksForIndex</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveAllCallbacksForIndex(long index)</code></p></p><b>Parameters</b><ul><li><code>long index</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="remove-ulong"></a><b>Remove</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L27">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Remove(ulong callbackKey)</code></p></p><b>Parameters</b><ul><li><code>ulong callbackKey</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="invokeall-long-t"></a><b>InvokeAll</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L41">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void InvokeAll(long index, T argument)</code></p></p><b>Parameters</b><ul><li><code>long index</code> : </li><li><code>T argument</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="invokeallreverse-long-t"></a><b>InvokeAllReverse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L49">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void InvokeAllReverse(long index, T argument)</code></p></p><b>Parameters</b><ul><li><code>long index</code> : </li><li><code>T argument</code> : </li></ul></td>    </tr></table>



