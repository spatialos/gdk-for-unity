---
title: IComponentDiffDeserializer Interface
slug: api-core-icomponentdiffdeserializer
order: 73
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L5">Source</a></span></p>













</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentid"></a><b>GetComponentId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L7">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint GetComponentId()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addupdatetodiff-componentupdateop-viewdiff-uint"></a><b>AddUpdateToDiff</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L9">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddUpdateToDiff(ComponentUpdateOp op, [ViewDiff](doc:api-core-viewdiff) diff, uint updateId)</code></p></p><b>Parameters</b><ul><li><code>ComponentUpdateOp op</code> : </li><li><code>[ViewDiff](doc:api-core-viewdiff) diff</code> : </li><li><code>uint updateId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponenttodiff-addcomponentop-viewdiff"></a><b>AddComponentToDiff</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L10">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponentToDiff(AddComponentOp op, [ViewDiff](doc:api-core-viewdiff) diff)</code></p></p><b>Parameters</b><ul><li><code>AddComponentOp op</code> : </li><li><code>[ViewDiff](doc:api-core-viewdiff) diff</code> : </li></ul></td>    </tr></table>



