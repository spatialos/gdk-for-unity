---
title: IComponentDiffStorage Interface
slug: api-core-icomponentdiffstorage
order: 74
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L7">Source</a></span></p>













</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="geteventtypes"></a><b>GetEventTypes</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L9">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Type[] GetEventTypes()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getupdatetype"></a><b>GetUpdateType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L10">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Type GetUpdateType()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentid"></a><b>GetComponentId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L11">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint GetComponentId()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="clear"></a><b>Clear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L13">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Clear()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removeentitycomponent-long"></a><b>RemoveEntityComponent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveEntityComponent(long entityId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentsadded"></a><b>GetComponentsAdded</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;[EntityId](doc:api-core-entityid)&gt; GetComponentsAdded()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentsremoved"></a><b>GetComponentsRemoved</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;[EntityId](doc:api-core-entityid)&gt; GetComponentsRemoved()</code></p></td>    </tr></table>



