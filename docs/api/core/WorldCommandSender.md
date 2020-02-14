---
title: WorldCommandSender Class
slug: api-core-worldcommandsender
order: 158
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L95">Source</a></span></p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isvalid"></a><b>IsValid</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L97">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsValid</code></p></td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worldcommandsender-entity-world"></a><b>WorldCommandSender</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L103">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> WorldCommandSender(Entity entity, World world)</code></p></p><b>Parameters</b><ul><li><code>Entity entity</code> : </li><li><code>World world</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendcreateentitycommand-worldcommands-createentity-request-action-worldcommands-createentity-receivedresponse"></a><b>SendCreateEntityCommand</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L112">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendCreateEntityCommand([WorldCommands.CreateEntity.Request](doc:api-core-commands-worldcommands-createentity-request) request, Action&lt;[WorldCommands.CreateEntity.ReceivedResponse](doc:api-core-commands-worldcommands-createentity-receivedresponse)&gt; callback = null)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.CreateEntity.Request](doc:api-core-commands-worldcommands-createentity-request) request</code> : </li><li><code>Action&lt;[WorldCommands.CreateEntity.ReceivedResponse](doc:api-core-commands-worldcommands-createentity-receivedresponse)&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="senddeleteentitycommand-worldcommands-deleteentity-request-action-worldcommands-deleteentity-receivedresponse"></a><b>SendDeleteEntityCommand</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L131">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendDeleteEntityCommand([WorldCommands.DeleteEntity.Request](doc:api-core-commands-worldcommands-deleteentity-request) request, Action&lt;[WorldCommands.DeleteEntity.ReceivedResponse](doc:api-core-commands-worldcommands-deleteentity-receivedresponse)&gt; callback = null)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.DeleteEntity.Request](doc:api-core-commands-worldcommands-deleteentity-request) request</code> : </li><li><code>Action&lt;[WorldCommands.DeleteEntity.ReceivedResponse](doc:api-core-commands-worldcommands-deleteentity-receivedresponse)&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendreserveentityidscommand-worldcommands-reserveentityids-request-action-worldcommands-reserveentityids-receivedresponse"></a><b>SendReserveEntityIdsCommand</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L150">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendReserveEntityIdsCommand([WorldCommands.ReserveEntityIds.Request](doc:api-core-commands-worldcommands-reserveentityids-request) request, Action&lt;[WorldCommands.ReserveEntityIds.ReceivedResponse](doc:api-core-commands-worldcommands-reserveentityids-receivedresponse)&gt; callback = null)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.ReserveEntityIds.Request](doc:api-core-commands-worldcommands-reserveentityids-request) request</code> : </li><li><code>Action&lt;[WorldCommands.ReserveEntityIds.ReceivedResponse](doc:api-core-commands-worldcommands-reserveentityids-receivedresponse)&gt; callback</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendentityquerycommand-worldcommands-entityquery-request-action-worldcommands-entityquery-receivedresponse"></a><b>SendEntityQueryCommand</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L169">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendEntityQueryCommand(WorldCommands.EntityQuery.Request request, Action&lt;WorldCommands.EntityQuery.ReceivedResponse&gt; callback = null)</code></p></p><b>Parameters</b><ul><li><code>WorldCommands.EntityQuery.Request request</code> : </li><li><code>Action&lt;WorldCommands.EntityQuery.ReceivedResponse&gt; callback</code> : </li></ul></td>    </tr></table>



