
# DynamicConverter Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicConverter.cs/#L5">Source</a>
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
<td style="padding: 14px; border: none; width: 17ch"><a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">IConverterHandler</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ForEachComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicConverter.cs/#L18">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ForEachComponent(<a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">IConverterHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">IConverterHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ForComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicConverter.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ForComponent(uint componentId, <a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">IConverterHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">IConverterHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SnapshotToUpdate&lt;TSnapshot, out TUpdate&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Dynamic/DynamicConverter.cs/#L7">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>delegate TUpdate SnapshotToUpdate&lt;TSnapshot, out TUpdate&gt;(in TSnapshot snapshot)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in TSnapshot snapshot</code> : </li>
</ul>





</td>
    </tr>
</table>





