---
title: Commands.WorldCommands.DeleteEntity.Request Struct
slug: api-core-commands-worldcommands-deleteentity-request
order: 28
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L13">Source</a></span></p>

</p>


<p>An object that is a DeleteEntity command request. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Commands.ICommandRequest](doc:api-core-commands-icommandrequest)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entityid"></a><b>EntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[EntityId](doc:api-core-entityid) EntityId</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="timeoutmillis"></a><b>TimeoutMillis</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint? TimeoutMillis</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>object Context</code></p></td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="request-entityid-uint-object"></a><b>Request</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Request([EntityId](doc:api-core-entityid) entityId, uint? timeoutMillis = null, object context = null)</code></p>Method to create a DeleteEntity command request payload. </p><b>Returns:</b></br>The DeleteEntity command request payload.</p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : The entity ID that is to be deleted.</li><li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li><li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li></ul></td>    </tr></table>




