---
title: CommandMetaData Class
slug: api-core-commandmetadata
order: 10
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L24">Source</a></span></p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandmetadata"></a><b>CommandMetaData</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> CommandMetaData()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="markidforremoval-uint-uint-long"></a><b>MarkIdForRemoval</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L66">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void MarkIdForRemoval(uint componentId, uint commandId, long internalRequestId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandId</code> : </li><li><code>long internalRequestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="flushremovedids"></a><b>FlushRemovedIds</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L71">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void FlushRemovedIds()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-t-uint-uint-in-commandcontext-t"></a><b>AddRequest&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest&lt;T&gt;(uint componentId, uint commandId, in [CommandContext](doc:api-core-commandcontext)&lt;T&gt; context)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandId</code> : </li><li><code>in [CommandContext](doc:api-core-commandcontext)&lt;T&gt; context</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addinternalrequestid-uint-uint-long-long"></a><b>AddInternalRequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L89">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddInternalRequestId(uint componentId, uint commandId, long requestId, long internalRequestId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandId</code> : </li><li><code>long requestId</code> : </li><li><code>long internalRequestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcontext-t-uint-uint-long"></a><b>GetContext&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaData.cs/#L96">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[CommandContext](doc:api-core-commandcontext)&lt;T&gt; GetContext&lt;T&gt;(uint componentId, uint commandId, long internalRequestId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandId</code> : </li><li><code>long internalRequestId</code> : </li></ul></td>    </tr></table>



