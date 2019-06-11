
# IDynamicInvokable Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Dynamic/IDynamicInvokable.cs/#L3">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#properties">Properties</a>
<li><a href="#methods">Methods</a>
</ul></nav>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Dynamic/IDynamicInvokable.cs/#L5">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint ComponentId { get; }</code></p>



</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>InvokeHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Dynamic/IDynamicInvokable.cs/#L6">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeHandler(<a href="{{urlRoot}}/api/core/dynamic/i-handler">Dynamic.IHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/dynamic/i-handler">Dynamic.IHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>InvokeSnapshotHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Dynamic/IDynamicInvokable.cs/#L7">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeSnapshotHandler(<a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">DynamicSnapshot.ISnapshotHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/dynamic-snapshot/i-snapshot-handler">DynamicSnapshot.ISnapshotHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>InvokeConvertHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Dynamic/IDynamicInvokable.cs/#L8">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeConvertHandler(<a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">DynamicConverter.IConverterHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/dynamic-converter/i-converter-handler">DynamicConverter.IConverterHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>





