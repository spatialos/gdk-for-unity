---
title: Commands.WorldCommands.CreateEntity.ReceivedResponse Struct
slug: api-core-commands-worldcommands-createentity-receivedresponse
order: 24
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L50">Source</a></span></p>

</p>


<p>An object that is the response of a CreateEntity command from the SpatialOS runtime. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Commands.IReceivedCommandResponse](doc:api-core-commands-ireceivedcommandresponse)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendingentity"></a><b>SendingEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Unity.Entities.Entity SendingEntity</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="statuscode"></a><b>StatusCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L58">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly StatusCode StatusCode</code></p>The status code of the command response. If equal to StatusCode.Success then the command succeeded. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="message"></a><b>Message</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L63">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string Message</code></p>The failure message of the command. Will only be non-null if the command failed. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entityid"></a><b>EntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L68">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly? [EntityId](doc:api-core-entityid) EntityId</code></p>The Entity ID of the created entity. Will only be non-null if the command succeeded. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestpayload"></a><b>RequestPayload</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L73">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [Request](doc:api-core-commands-worldcommands-createentity-request) RequestPayload</code></p>The request payload that was originally sent with this command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L78">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly object Context</code></p>The context object that was provided when sending the command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestid"></a><b>RequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly long RequestId</code></p>The unique request ID of this command. Will match the request ID in the corresponding request. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestid"></a><b>RequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L99">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>long IReceivedCommandResponse. RequestId</code></p></td>    </tr></table>








