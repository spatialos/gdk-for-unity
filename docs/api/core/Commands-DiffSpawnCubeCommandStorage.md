---
title: Commands.DiffSpawnCubeCommandStorage<TRequest, TResponse> Class
slug: api-core-commands-diffspawncubecommandstorage
order: 15
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L10">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IComponentCommandDiffStorage](doc:api-core-icomponentcommanddiffstorage)</code>
<code>[Improbable.Gdk.Core.IDiffCommandRequestStorage<TRequest>](doc:api-core-idiffcommandrequeststorage)</code>
<code>[Improbable.Gdk.Core.IDiffCommandResponseStorage<TResponse>](doc:api-core-idiffcommandresponsestorage)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requesttype"></a><b>RequestType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Type RequestType</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="responsetype"></a><b>ResponseType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Type ResponseType</code></p></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="componentid"></a><b>ComponentId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L21">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> abstract uint ComponentId { get; }</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandid"></a><b>CommandId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> abstract uint CommandId { get; }</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="clear"></a><b>Clear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L27">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Clear()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removerequests-long"></a><b>RemoveRequests</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L35">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveRequests(long entityId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-trequest"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L40">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(TRequest request)</code></p></p><b>Parameters</b><ul><li><code>TRequest request</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addresponse-tresponse"></a><b>AddResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L45">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddResponse(TResponse response)</code></p></p><b>Parameters</b><ul><li><code>TResponse response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getrequests"></a><b>GetRequests</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;TRequest&gt; GetRequests()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getrequests-entityid"></a><b>GetRequests</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L55">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;TRequest&gt; GetRequests([EntityId](doc:api-core-entityid) targetEntityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) targetEntityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getresponses"></a><b>GetResponses</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L67">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;TResponse&gt; GetResponses()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getresponse-long"></a><b>GetResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L72">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;TResponse&gt; GetResponse(long requestId)</code></p></p><b>Parameters</b><ul><li><code>long requestId</code> : </li></ul></td>    </tr></table>



