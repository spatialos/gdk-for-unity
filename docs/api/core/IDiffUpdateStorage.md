---
title: IDiffUpdateStorage<T> Interface
slug: api-core-idiffupdatestorage
order: 87
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L49">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IComponentDiffStorage](doc:api-core-icomponentdiffstorage)</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addupdate-componentupdatereceived-t"></a><b>AddUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L51">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddUpdate([ComponentUpdateReceived](doc:api-core-componentupdatereceived)&lt;T&gt; update)</code></p></p><b>Parameters</b><ul><li><code>[ComponentUpdateReceived](doc:api-core-componentupdatereceived)&lt;T&gt; update</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getupdates"></a><b>GetUpdates</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentUpdateReceived](doc:api-core-componentupdatereceived)&lt;T&gt;&gt; GetUpdates()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getupdates-entityid"></a><b>GetUpdates</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L53">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;[ComponentUpdateReceived](doc:api-core-componentupdatereceived)&lt;T&gt;&gt; GetUpdates([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>



