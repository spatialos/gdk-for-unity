---
title: ComponentUpdateSystem Class
slug: api-core-componentupdatesystem
order: 41
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L9">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendupdate-t-in-t-entityid"></a><b>SendUpdate&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L13">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendUpdate&lt;T&gt;(in T update, [EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>in T update</code> : </li><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendevent-t-t-entityid"></a><b>SendEvent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L19">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendEvent&lt;T&gt;(T eventToSend, [EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>T eventToSend</code> : </li><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="geteventsreceived-t"></a><b>GetEventsReceived&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentEventReceived](doc:api-core-componenteventreceived)&lt;T&gt;&gt; GetEventsReceived&lt;T&gt;()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="geteventsreceived-t-entityid"></a><b>GetEventsReceived&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentEventReceived](doc:api-core-componenteventreceived)&lt;T&gt;&gt; GetEventsReceived&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentupdatesreceived-t"></a><b>GetComponentUpdatesReceived&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentUpdateReceived](doc:api-core-componentupdatereceived)&lt;T&gt;&gt; GetComponentUpdatesReceived&lt;T&gt;()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getentitycomponentupdatesreceived-t-entityid"></a><b>GetEntityComponentUpdatesReceived&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentUpdateReceived](doc:api-core-componentupdatereceived)&lt;T&gt;&gt; GetEntityComponentUpdatesReceived&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getauthoritychangesreceived-uint"></a><b>GetAuthorityChangesReceived</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[AuthorityChangeReceived](doc:api-core-authoritychangereceived)&gt; GetAuthorityChangesReceived(uint componentId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getauthoritychangesreceived-entityid-uint"></a><b>GetAuthorityChangesReceived</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L56">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[AuthorityChangeReceived](doc:api-core-authoritychangereceived)&gt; GetAuthorityChangesReceived([EntityId](doc:api-core-entityid) entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentsadded-uint"></a><b>GetComponentsAdded</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L63">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;[EntityId](doc:api-core-entityid)&gt; GetComponentsAdded(uint componentId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentsremoved-uint"></a><b>GetComponentsRemoved</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L69">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;[EntityId](doc:api-core-entityid)&gt; GetComponentsRemoved(uint componentId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getauthority-entityid-uint"></a><b>GetAuthority</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L75">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Authority GetAuthority([EntityId](doc:api-core-entityid) entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponent-t-entityid"></a><b>GetComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L80">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T GetComponent&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="acknowledgeauthorityloss-entityid-uint"></a><b>AcknowledgeAuthorityLoss</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L85">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AcknowledgeAuthorityLoss([EntityId](doc:api-core-entityid) entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hascomponent-uint-entityid"></a><b>HasComponent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L90">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasComponent(uint componentId, [EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L95">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L104">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


