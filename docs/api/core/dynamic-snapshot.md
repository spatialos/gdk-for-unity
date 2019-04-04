
# DynamicSnapshot Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicSnapshot.cs/#L7">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
</ul></nav>





</p>

<b>Child types</b>

<table>
<tr>
<td style="padding: 14px; border: none; width: 16ch"><a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">ISnapshotHandler</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ForEachSnapshotComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicSnapshot.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ForEachSnapshotComponent(<a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">ISnapshotHandler</a> snapshotHandler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">ISnapshotHandler</a> snapshotHandler</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ForSnapshotComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicSnapshot.cs/#L29">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ForSnapshotComponent(uint componentId, <a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">ISnapshotHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">ISnapshotHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetSnapshotComponentId&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicSnapshot.cs/#L39">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetSnapshotComponentId&lt;T&gt;()</code></p>






</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SnapshotDeserializer&lt;out T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicSnapshot.cs/#L9">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>delegate T SnapshotDeserializer&lt;out T&gt;(ComponentData update)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>ComponentData update</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SnapshotSerializer&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicSnapshot.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>delegate void SnapshotSerializer&lt;T&gt;(T snapshot, ComponentData data)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T snapshot</code> : </li>
<li><code>ComponentData data</code> : </li>
</ul>





</td>
    </tr>
</table>





