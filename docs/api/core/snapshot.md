
# Snapshot Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L10">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#fields">Fields</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Convenience wrapper around the WorkerSDK <a href="{{urlRoot}}/api/core/snapshot">Snapshot</a> API. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="count"></a><b>Count</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> int Count</code></p>


</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addentity-entitytemplate"></a><b>AddEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L24">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> AddEntity(<a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> entityTemplate)</code></p>
Adds an entity to the snapshot 
</p><b>Returns:</b></br>The entity ID assigned to the entity in the snapshot.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> entityTemplate</code> : The entity to be added to the snapshot.</li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>The entity ID is automatically assigned. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="writetofile-string"></a><b>WriteToFile</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/Snapshot.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void WriteToFile(string path)</code></p>
Writes the snapshot out to a file. 


</p>

<b>Parameters</b>

<ul>
<li><code>string path</code> : The file path.</li>
</ul>





</td>
    </tr>
</table>





