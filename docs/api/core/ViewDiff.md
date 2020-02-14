---
title: ViewDiff Class
slug: api-core-viewdiff
order: 148
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L10">Source</a></span></p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="disconnectmessage"></a><b>DisconnectMessage</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L12">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string DisconnectMessage</code></p></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="disconnected"></a><b>Disconnected</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> bool Disconnected { get; }</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="incriticalsection"></a><b>InCriticalSection</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> bool InCriticalSection { get; }</code></p></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="viewdiff"></a><b>ViewDiff</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L48">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> ViewDiff()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="clear"></a><b>Clear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L104">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Clear()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addentity-long"></a><b>AddEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L128">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddEntity(long entityId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removeentity-long"></a><b>RemoveEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L136">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveEntity(long entityId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponent-t-t-long-uint"></a><b>AddComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L144">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponent&lt;T&gt;(T component, long entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>T component</code> : </li><li><code>long entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removecomponent-long-uint"></a><b>RemoveComponent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L156">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveComponent(long entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setauthority-long-uint-authority"></a><b>SetAuthority</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L168">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetAuthority(long entityId, uint componentId, Authority authority)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li><li><code>uint componentId</code> : </li><li><code>Authority authority</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponentupdate-t-t-long-uint-uint"></a><b>AddComponentUpdate&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L193">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponentUpdate&lt;T&gt;(T update, long entityId, uint componentId, uint updateId)</code></p></p><b>Parameters</b><ul><li><code>T update</code> : </li><li><code>long entityId</code> : </li><li><code>uint componentId</code> : </li><li><code>uint updateId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addevent-t-t-long-uint-uint"></a><b>AddEvent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L207">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddEvent&lt;T&gt;(T ev, long entityId, uint componentId, uint updateId)</code></p></p><b>Parameters</b><ul><li><code>T ev</code> : </li><li><code>long entityId</code> : </li><li><code>uint componentId</code> : </li><li><code>uint updateId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandrequest-t-t-uint-uint"></a><b>AddCommandRequest&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L220">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandRequest&lt;T&gt;(T request, uint componentId, uint commandId)</code></p></p><b>Parameters</b><ul><li><code>T request</code> : </li><li><code>uint componentId</code> : </li><li><code>uint commandId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandresponse-t-t-uint-uint"></a><b>AddCommandResponse&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L235">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandResponse&lt;T&gt;(T response, uint componentId, uint commandId)</code></p></p><b>Parameters</b><ul><li><code>T response</code> : </li><li><code>uint componentId</code> : </li><li><code>uint commandId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcreateentityresponse-worldcommands-createentity-receivedresponse"></a><b>AddCreateEntityResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L251">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCreateEntityResponse([WorldCommands.CreateEntity.ReceivedResponse](doc:api-core-commands-worldcommands-createentity-receivedresponse) response)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.CreateEntity.ReceivedResponse](doc:api-core-commands-worldcommands-createentity-receivedresponse) response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="adddeleteentityresponse-worldcommands-deleteentity-receivedresponse"></a><b>AddDeleteEntityResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L256">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddDeleteEntityResponse([WorldCommands.DeleteEntity.ReceivedResponse](doc:api-core-commands-worldcommands-deleteentity-receivedresponse) response)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.DeleteEntity.ReceivedResponse](doc:api-core-commands-worldcommands-deleteentity-receivedresponse) response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addreserveentityidsresponse-worldcommands-reserveentityids-receivedresponse"></a><b>AddReserveEntityIdsResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L261">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddReserveEntityIdsResponse([WorldCommands.ReserveEntityIds.ReceivedResponse](doc:api-core-commands-worldcommands-reserveentityids-receivedresponse) response)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommands.ReserveEntityIds.ReceivedResponse](doc:api-core-commands-worldcommands-reserveentityids-receivedresponse) response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addentityqueryresponse-worldcommands-entityquery-receivedresponse"></a><b>AddEntityQueryResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L266">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddEntityQueryResponse(WorldCommands.EntityQuery.ReceivedResponse response)</code></p></p><b>Parameters</b><ul><li><code>WorldCommands.EntityQuery.ReceivedResponse response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addlogmessage-string-loglevel"></a><b>AddLogMessage</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L271">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddLogMessage(string message, LogLevel level)</code></p></p><b>Parameters</b><ul><li><code>string message</code> : </li><li><code>LogLevel level</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addmetrics-metrics"></a><b>AddMetrics</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L276">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddMetrics(Metrics metrics)</code></p></p><b>Parameters</b><ul><li><code>Metrics metrics</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setworkerflag-string-string"></a><b>SetWorkerFlag</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L288">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetWorkerFlag(string flag, string value)</code></p></p><b>Parameters</b><ul><li><code>string flag</code> : </li><li><code>string value</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="disconnect-string"></a><b>Disconnect</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L293">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Disconnect(string message)</code></p></p><b>Parameters</b><ul><li><code>string message</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setcriticalsection-bool"></a><b>SetCriticalSection</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L299">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetCriticalSection(bool inCriticalSection)</code></p></p><b>Parameters</b><ul><li><code>bool inCriticalSection</code> : </li></ul></td>    </tr></table>



