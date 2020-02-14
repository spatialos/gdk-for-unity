---
title: Commands.WorldCommands.CreateEntity.Request Struct
slug: api-core-commands-worldcommands-createentity-request
order: 25
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L13">Source</a></span></p>

</p>


<p>An object that is a CreateEntity command request. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Commands.ICommandRequest](doc:api-core-commands-icommandrequest)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entity"></a><b>Entity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Improbable.Worker.CInterop.Entity Entity</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entityid"></a><b>EntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[EntityId](doc:api-core-entityid)? EntityId</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="timeoutmillis"></a><b>TimeoutMillis</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint? TimeoutMillis</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L18">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>object Context</code></p></td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="request-entitytemplate-entityid-uint-object"></a><b>Request</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L37">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Request([EntityTemplate](doc:api-core-entitytemplate) template, [EntityId](doc:api-core-entityid)? entityId = null, uint? timeoutMillis = null, object context = null)</code></p>Constructor to create a CreateEntity command request payload. </p><b>Returns:</b></br>The CreateEntity command request payload.</p><b>Parameters</b><ul><li><code>[EntityTemplate](doc:api-core-entitytemplate) template</code> : The [EntityTemplate](doc:api-core-entitytemplate) object that defines the SpatialOS components on the to-be-created entity. </li><li><code>[EntityId](doc:api-core-entityid)? entityId</code> : (Optional) The [EntityId](doc:api-core-entityid) that the to-be-created entity should take. This should only be provided if received as the result of a ReserveEntityIds command. </li><li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li><li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li></ul></td>    </tr></table>




