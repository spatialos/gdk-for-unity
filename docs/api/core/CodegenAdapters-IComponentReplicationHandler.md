---
title: CodegenAdapters.IComponentReplicationHandler Interface
slug: api-core-codegenadapters-icomponentreplicationhandler
order: 4
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L6">Source</a></span></p>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="componentid"></a><b>ComponentId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L8">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> uint ComponentId { get; }</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="componentupdatequery"></a><b>ComponentUpdateQuery</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L9">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> EntityQueryDesc ComponentUpdateQuery { get; }</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendupdates-nativearray-archetypechunk-componentsystembase-entitymanager-componentupdatesystem"></a><b>SendUpdates</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L11">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendUpdates(NativeArray&lt;ArchetypeChunk&gt; chunkArray, ComponentSystemBase system, EntityManager entityManager, [ComponentUpdateSystem](doc:api-core-componentupdatesystem) componentUpdateSystem)</code></p></p><b>Parameters</b><ul><li><code>NativeArray&lt;ArchetypeChunk&gt; chunkArray</code> : </li><li><code>ComponentSystemBase system</code> : </li><li><code>EntityManager entityManager</code> : </li><li><code>[ComponentUpdateSystem](doc:api-core-componentupdatesystem) componentUpdateSystem</code> : </li></ul></td>    </tr></table>



