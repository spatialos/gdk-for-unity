
# ICommandMetaDataStorage Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ICommandMetaDataStorage.cs/#L3">Source</a>
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
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ICommandMetaDataStorage.cs/#L5">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetComponentId()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetCommandId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ICommandMetaDataStorage.cs/#L6">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetCommandId()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RemoveMetaData</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ICommandMetaDataStorage.cs/#L8">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveMetaData(uint internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SetInternalRequestId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ICommandMetaDataStorage.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetInternalRequestId(uint internalRequestId, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint internalRequestId</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>





