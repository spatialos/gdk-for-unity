---
title: SerializedMessagesToSend Class
slug: api-core-serializedmessagestosend
order: 131
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L10">Source</a></span></p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="serializedmessagestosend"></a><b>SerializedMessagesToSend</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> SerializedMessagesToSend()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="serializefrom-messagestosend-commandmetadata"></a><b>SerializeFrom</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L97">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SerializeFrom([MessagesToSend](doc:api-core-messagestosend) messages, [CommandMetaData](doc:api-core-commandmetadata) commandMetaData)</code></p></p><b>Parameters</b><ul><li><code>[MessagesToSend](doc:api-core-messagestosend) messages</code> : </li><li><code>[CommandMetaData](doc:api-core-commandmetadata) commandMetaData</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="clear"></a><b>Clear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L121">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Clear()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendandclear-connection-netframestats"></a><b>SendAndClear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L137">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[CommandMetaData](doc:api-core-commandmetadata) SendAndClear(Connection connection, [NetFrameStats](doc:api-core-networkstats-netframestats) frameStats)</code></p></p><b>Parameters</b><ul><li><code>Connection connection</code> : </li><li><code>[NetFrameStats](doc:api-core-networkstats-netframestats) frameStats</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponentupdate-componentupdate-long"></a><b>AddComponentUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L216">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponentUpdate(ComponentUpdate update, long entityId)</code></p></p><b>Parameters</b><ul><li><code>ComponentUpdate update</code> : </li><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-commandrequest-uint-long-uint-long"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L222">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(CommandRequest request, uint commandId, long entityId, uint? timeout, long requestId)</code></p></p><b>Parameters</b><ul><li><code>CommandRequest request</code> : </li><li><code>uint commandId</code> : </li><li><code>long entityId</code> : </li><li><code>uint? timeout</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addresponse-commandresponse-uint"></a><b>AddResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L228">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddResponse(CommandResponse response, uint requestId)</code></p></p><b>Parameters</b><ul><li><code>CommandResponse response</code> : </li><li><code>uint requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addfailure-uint-uint-string-uint"></a><b>AddFailure</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L234">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddFailure(uint componentId, uint commandIndex, string reason, uint requestId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandIndex</code> : </li><li><code>string reason</code> : </li><li><code>uint requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcreateentityrequest-entity-long-uint-long"></a><b>AddCreateEntityRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L240">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCreateEntityRequest(Entity entity, long? entityId, uint? timeout, long requestId)</code></p></p><b>Parameters</b><ul><li><code>Entity entity</code> : </li><li><code>long? entityId</code> : </li><li><code>uint? timeout</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="adddeleteentityrequest-long-uint-long"></a><b>AddDeleteEntityRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L246">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddDeleteEntityRequest(long entityId, uint? timeout, long requestId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li><li><code>uint? timeout</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addreserveentityidsrequest-uint-uint-long"></a><b>AddReserveEntityIdsRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L252">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddReserveEntityIdsRequest(uint numberOfEntityIds, uint? timeout, long requestId)</code></p></p><b>Parameters</b><ul><li><code>uint numberOfEntityIds</code> : </li><li><code>uint? timeout</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addentityqueryrequest-entityquery-uint-long"></a><b>AddEntityQueryRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/SerializedMessagesToSend.cs/#L258">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddEntityQueryRequest(EntityQuery query, uint? timeout, long requestId)</code></p></p><b>Parameters</b><ul><li><code>EntityQuery query</code> : </li><li><code>uint? timeout</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>



