---
title: ConcurrentOpListConverter Class
slug: api-core-concurrentoplistconverter
order: 43
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/ConcurrentOpListConverter.cs/#L18">Source</a></span></p>

</p>


<p>Simplest possible current op-list-to-diff converter. Acquires a lock when deserializing an op list and releases it when done </p>




</p>
<p><b>Notes</b></p>

- This is not able to interrupt the deserialization thread to exchange diffs, which could increase latency when under heavy load. If this presents a problem it could be improved with either a more complicated lock or a lock free approach. Similarly if part of a critical section is received regularly the diffs may often not be possible to exchange, increasing latency. To solve this we could buffer critical sections into an intermediate object and then move them to the main diff. However this case is unlikely and the overhead associated with fixing it may be undesirable. 










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="parseoplistintodiff-oplist-commandmetadataaggregate"></a><b>ParseOpListIntoDiff</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/ConcurrentOpListConverter.cs/#L41">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void ParseOpListIntoDiff(OpList opList, [CommandMetaDataAggregate](doc:api-core-commandmetadataaggregate) commandMetaData)</code></p>Iterate over the op list and populate a [ViewDiff](doc:api-core-viewdiff) from the data contained. Must not be called again before it returns. </p><b>Parameters</b><ul><li><code>OpList opList</code> : </li><li><code>[CommandMetaDataAggregate](doc:api-core-commandmetadataaggregate) commandMetaData</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetviewdiff-out-viewdiff"></a><b>TryGetViewDiff</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/ConcurrentOpListConverter.cs/#L136">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetViewDiff(out [ViewDiff](doc:api-core-viewdiff) viewDiff)</code></p>Try to get a diff containing ops deserialized since the last call. If successful the diff may only contain part of the last op list. </p><b>Returns:</b></br>True if the diffs could be exchanged and false otherwise.</p><b>Parameters</b><ul><li><code>out [ViewDiff](doc:api-core-viewdiff) viewDiff</code> : </li></ul></td>    </tr></table>



