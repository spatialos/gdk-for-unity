---
title: Commands.WorldCommands.DeleteEntity.ReceivedResponse Struct
slug: api-core-commands-worldcommands-deleteentity-receivedresponse
order: 27
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L41">Source</a></span></p>

</p>


<p>An object that is the response of a DeleteEntity command from the SpatialOS runtime. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Commands.IReceivedCommandResponse](doc:api-core-commands-ireceivedcommandresponse)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendingentity"></a><b>SendingEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Entity SendingEntity</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="statuscode"></a><b>StatusCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L49">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly StatusCode StatusCode</code></p>The status code of the command response. If equal to StatusCode.Success then the command succeeded. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="message"></a><b>Message</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L54">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string Message</code></p>The failure message of the command. Will only be non-null if the command failed. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entityid"></a><b>EntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L59">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [EntityId](doc:api-core-entityid) EntityId</code></p>The Entity ID that was the target of the DeleteEntity command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestpayload"></a><b>RequestPayload</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L64">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [Request](doc:api-core-commands-worldcommands-deleteentity-request) RequestPayload</code></p>The request payload that was originally sent with this command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L69">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly object Context</code></p>The context object that was provided when sending the command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestid"></a><b>RequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L74">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly long RequestId</code></p>The unique request ID of this command. Will match the request ID in the corresponding request. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestid"></a><b>RequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L87">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>long IReceivedCommandResponse. RequestId</code></p></td>    </tr></table>








