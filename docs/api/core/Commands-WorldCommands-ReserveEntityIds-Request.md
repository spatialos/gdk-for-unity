---
title: Commands.WorldCommands.ReserveEntityIds.Request Struct
slug: api-core-commands-worldcommands-reserveentityids-request
order: 34
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L13">Source</a></span></p>

</p>


<p>An object that is a ReserveEntityIds command request. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Commands.ICommandRequest](doc:api-core-commands-icommandrequest)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="numberofentityids"></a><b>NumberOfEntityIds</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint NumberOfEntityIds</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="timeoutmillis"></a><b>TimeoutMillis</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint? TimeoutMillis</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>object Context</code></p></td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="request-uint-uint-object"></a><b>Request</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Request(uint numberOfEntityIds, uint? timeoutMillis = null, object context = null)</code></p>Used to create a ReserveEntityIds command request payload. </p><b>Returns:</b></br>The ReserveEntityIds command request payload.</p><b>Parameters</b><ul><li><code>uint numberOfEntityIds</code> : The number of entity IDs to reserve.</li><li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li><li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li></ul></td>    </tr></table>




