---
title: IDiffEventStorage<T> Interface
slug: api-core-idiffeventstorage
order: 86
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L57">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IComponentDiffStorage](doc:api-core-icomponentdiffstorage)</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addevent-componenteventreceived-t"></a><b>AddEvent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L59">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddEvent([ComponentEventReceived](doc:api-core-componenteventreceived)&lt;T&gt; ev)</code></p></p><b>Parameters</b><ul><li><code>[ComponentEventReceived](doc:api-core-componenteventreceived)&lt;T&gt; ev</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getevents"></a><b>GetEvents</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentEventReceived](doc:api-core-componenteventreceived)&lt;T&gt;&gt; GetEvents()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getevents-entityid"></a><b>GetEvents</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L61">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentEventReceived](doc:api-core-componenteventreceived)&lt;T&gt;&gt; GetEvents([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>



