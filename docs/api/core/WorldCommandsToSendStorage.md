---
title: WorldCommandsToSendStorage Class
slug: api-core-worldcommandstosendstorage
order: 162
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L10">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.ICommandSendStorage](doc:api-core-icommandsendstorage)</code>
<code>[Improbable.Gdk.Core.ICommandRequestSendStorage<WorldCommands.CreateEntity.Request>](doc:api-core-icommandrequestsendstorage)</code>
<code>[Improbable.Gdk.Core.ICommandRequestSendStorage<WorldCommands.DeleteEntity.Request>](doc:api-core-icommandrequestsendstorage)</code>
<code>[Improbable.Gdk.Core.ICommandRequestSendStorage<WorldCommands.ReserveEntityIds.Request>](doc:api-core-icommandrequestsendstorage)</code>
<code>[Improbable.Gdk.Core.ICommandRequestSendStorage<WorldCommands.EntityQuery.Request>](doc:api-core-icommandrequestsendstorage)</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="clear"></a><b>Clear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Clear()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-worldcommands-createentity-request-entity-long"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L32">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest([WorldCommands.CreateEntity.Request](doc:api-core-commands-worldcommands-createentity-request) request, Entity sendingEntity, long requestId)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.CreateEntity.Request](doc:api-core-commands-worldcommands-createentity-request) request</code> : </li><li><code>Entity sendingEntity</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-worldcommands-deleteentity-request-entity-long"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L39">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest([WorldCommands.DeleteEntity.Request](doc:api-core-commands-worldcommands-deleteentity-request) request, Entity sendingEntity, long requestId)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.DeleteEntity.Request](doc:api-core-commands-worldcommands-deleteentity-request) request</code> : </li><li><code>Entity sendingEntity</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-worldcommands-reserveentityids-request-entity-long"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L46">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest([WorldCommands.ReserveEntityIds.Request](doc:api-core-commands-worldcommands-reserveentityids-request) request, Entity sendingEntity, long requestId)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.ReserveEntityIds.Request](doc:api-core-commands-worldcommands-reserveentityids-request) request</code> : </li><li><code>Entity sendingEntity</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addrequest-worldcommands-entityquery-request-entity-long"></a><b>AddRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L53">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddRequest(WorldCommands.EntityQuery.Request request, Entity sendingEntity, long requestId)</code></p></p><b>Parameters</b><ul><li><code>WorldCommands.EntityQuery.Request request</code> : </li><li><code>Entity sendingEntity</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>



