---
title: WorldCommandMetaDataStorage Class
slug: api-core-worldcommandmetadatastorage
order: 157
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L11">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.ICommandMetaDataStorage](doc:api-core-icommandmetadatastorage)</code>
<code>[Improbable.Gdk.Core.ICommandPayloadStorage<WorldCommands.CreateEntity.Request>](doc:api-core-icommandpayloadstorage)</code>
<code>[Improbable.Gdk.Core.ICommandPayloadStorage<WorldCommands.DeleteEntity.Request>](doc:api-core-icommandpayloadstorage)</code>
<code>[Improbable.Gdk.Core.ICommandPayloadStorage<WorldCommands.ReserveEntityIds.Request>](doc:api-core-icommandpayloadstorage)</code>
<code>[Improbable.Gdk.Core.ICommandPayloadStorage<WorldCommands.EntityQuery.Request>](doc:api-core-icommandpayloadstorage)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandid"></a><b>CommandId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L28">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint CommandId</code></p></td>    </tr></table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removemetadata-long"></a><b>RemoveMetaData</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveMetaData(long internalRequestId)</code></p></p><b>Parameters</b><ul><li><code>long internalRequestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setinternalrequestid-long-long"></a><b>SetInternalRequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L45">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetInternalRequestId(long internalRequestId, long requestId)</code></p></p><b>Parameters</b><ul><li><code>long internalRequestId</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-in-commandcontext-worldcommands-createentity-request"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(in [CommandContext](doc:api-core-commandcontext)&lt;[WorldCommands.CreateEntity.Request](doc:api-core-commands-worldcommands-createentity-request)&gt; context)</code></p></p><b>Parameters</b><ul><li><code>in [CommandContext](doc:api-core-commandcontext)&lt;[WorldCommands.CreateEntity.Request](doc:api-core-commands-worldcommands-createentity-request)&gt; context</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-in-commandcontext-worldcommands-deleteentity-request"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L55">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(in [CommandContext](doc:api-core-commandcontext)&lt;[WorldCommands.DeleteEntity.Request](doc:api-core-commands-worldcommands-deleteentity-request)&gt; context)</code></p></p><b>Parameters</b><ul><li><code>in [CommandContext](doc:api-core-commandcontext)&lt;[WorldCommands.DeleteEntity.Request](doc:api-core-commands-worldcommands-deleteentity-request)&gt; context</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-in-commandcontext-worldcommands-reserveentityids-request"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(in [CommandContext](doc:api-core-commandcontext)&lt;[WorldCommands.ReserveEntityIds.Request](doc:api-core-commands-worldcommands-reserveentityids-request)&gt; context)</code></p></p><b>Parameters</b><ul><li><code>in [CommandContext](doc:api-core-commandcontext)&lt;[WorldCommands.ReserveEntityIds.Request](doc:api-core-commands-worldcommands-reserveentityids-request)&gt; context</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-in-commandcontext-worldcommands-entityquery-request"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L65">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(in [CommandContext](doc:api-core-commandcontext)&lt;WorldCommands.EntityQuery.Request&gt; context)</code></p></p><b>Parameters</b><ul><li><code>in [CommandContext](doc:api-core-commandcontext)&lt;WorldCommands.EntityQuery.Request&gt; context</code> : </li></ul></td>    </tr></table>



