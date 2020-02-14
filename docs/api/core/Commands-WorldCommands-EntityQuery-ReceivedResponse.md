---
title: Commands.WorldCommands.EntityQuery.ReceivedResponse Struct
slug: api-core-commands-worldcommands-entityquery-receivedresponse
order: 30
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L44">Source</a></span></p>

</p>


<p>An object that is the response of an EntityQuery command from the SpatialOS runtime. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Commands.IReceivedCommandResponse](doc:api-core-commands-ireceivedcommandresponse)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendingentity"></a><b>SendingEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L46">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Entity SendingEntity</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="statuscode"></a><b>StatusCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly StatusCode StatusCode</code></p>The status code of the command response. If equal to StatusCode.Success then the command succeeded. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="message"></a><b>Message</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L57">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string Message</code></p>The failure message of the command. Will only be non-null if the command failed. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="result"></a><b>Result</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L63">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Dictionary&lt;[EntityId](doc:api-core-entityid), [EntitySnapshot](doc:api-core-entitysnapshot)&gt; Result</code></p>A dictionary that represents the results of a SnapshotResultType entity query. This is null for CountResultType entity queries. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="resultcount"></a><b>ResultCount</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L68">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly int ResultCount</code></p>The number of entities that matched the entity query constraints. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestpayload"></a><b>RequestPayload</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L73">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [Request](doc:api-core-commands-worldcommands-entityquery-request) RequestPayload</code></p>The request payload that was originally sent with this command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L78">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly object Context</code></p>The context object that was provided when sending the command. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestid"></a><b>RequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly long RequestId</code></p>The unique request ID of this command. Will match the request ID in the corresponding request. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestid"></a><b>RequestId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L108">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>long IReceivedCommandResponse. RequestId</code></p></td>    </tr></table>








