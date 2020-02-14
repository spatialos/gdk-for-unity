---
title: Snapshot Class
slug: api-core-snapshot
order: 132
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L10">Source</a></span></p>

</p>


<p>Convenience wrapper around the WorkerSDK [Snapshot](doc:api-core-snapshot) API. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="count"></a><b>Count</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>int Count</code></p></td>    </tr></table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addentity-entitytemplate"></a><b>AddEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[EntityId](doc:api-core-entityid) AddEntity([EntityTemplate](doc:api-core-entitytemplate) entityTemplate)</code></p>Adds an entity to the snapshot </p><b>Returns:</b></br>The entity ID assigned to the entity in the snapshot.</p><b>Parameters</b><ul><li><code>[EntityTemplate](doc:api-core-entitytemplate) entityTemplate</code> : The entity to be added to the snapshot.</li></ul></p><b>Notes:</b><ul><li>The entity ID is automatically assigned. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="writetofile-string"></a><b>WriteToFile</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L35">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void WriteToFile(string path)</code></p>Writes the snapshot out to a file. </p><b>Parameters</b><ul><li><code>string path</code> : The file path.</li></ul></td>    </tr></table>



