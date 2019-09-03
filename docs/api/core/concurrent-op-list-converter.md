
# ConcurrentOpListConverter Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/ConcurrentOpListConverter.cs/#L18">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Simplest possible current op-list-to-diff converter. Acquires a lock when deserializing an op list and releases it when done </p>




</p>

<b>Notes</b>

- This is not able to interrupt the deserialization thread to exchange diffs, which could increase latency when under heavy load. If this presents a problem it could be improved with either a more complicated lock or a lock free approach. Similarly if part of a critical section is received regularly the diffs may often not be possible to exchange, increasing latency. To solve this we could buffer critical sections into an intermediate object and then move them to the main diff. However this case is unlikely and the overhead associated with fixing it may be undesirable. 










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="parseoplistintodiff-oplist-commandmetadataaggregate"></a><b>ParseOpListIntoDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/ConcurrentOpListConverter.cs/#L41">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ParseOpListIntoDiff(OpList opList, <a href="{{urlRoot}}/api/core/command-meta-data-aggregate">CommandMetaDataAggregate</a> commandMetaData)</code></p>
Iterate over the op list and populate a <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> from the data contained. Must not be called again before it returns. 


</p>

<b>Parameters</b>

<ul>
<li><code>OpList opList</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/command-meta-data-aggregate">CommandMetaDataAggregate</a> commandMetaData</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="trygetviewdiff-out-viewdiff"></a><b>TryGetViewDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/ConcurrentOpListConverter.cs/#L136">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool TryGetViewDiff(out <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff)</code></p>
Try to get a diff containing ops deserialized since the last call. If successful the diff may only contain part of the last op list. 
</p><b>Returns:</b></br>True if the diffs could be exchanged and false otherwise.

</p>

<b>Parameters</b>

<ul>
<li><code>out <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff</code> : </li>
</ul>





</td>
    </tr>
</table>





