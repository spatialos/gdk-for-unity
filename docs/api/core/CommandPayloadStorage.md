---
title: CommandPayloadStorage<T> Class
slug: api-core-commandpayloadstorage
order: 12
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ICommandMetaDataStorage.cs/#L20">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable::Gdk::Core::ICommandPayloadStorage<T>](doc:api-core-icommandpayloadstorage)</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removemetadata-long"></a><b>RemoveMetaData</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ICommandMetaDataStorage.cs/#L27">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveMetaData(long internalRequestId)</code></p></p><b>Parameters</b><ul><li><code>long internalRequestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setinternalrequestid-long-long"></a><b>SetInternalRequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ICommandMetaDataStorage.cs/#L34">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetInternalRequestId(long internalRequestId, long requestId)</code></p></p><b>Parameters</b><ul><li><code>long internalRequestId</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-in-commandcontext-t"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ICommandMetaDataStorage.cs/#L39">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(in [CommandContext](doc:api-core-commandcontext)&lt;T&gt; context)</code></p></p><b>Parameters</b><ul><li><code>in [CommandContext](doc:api-core-commandcontext)&lt;T&gt; context</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getpayload-long"></a><b>GetPayload</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ICommandMetaDataStorage.cs/#L44">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[CommandContext](doc:api-core-commandcontext)&lt;T&gt; GetPayload(long internalRequestId)</code></p></p><b>Parameters</b><ul><li><code>long internalRequestId</code> : </li></ul></td>    </tr></table>



