
# IViewStorage Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L6">Source</a>
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
        <td style="border-right:none"><b>GetSnapshotType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L8">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Type GetSnapshotType()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetUpdateType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L9">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Type GetUpdateType()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetComponentId()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasComponent(long entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Authority GetAuthority(long entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ApplyDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/View/ViewStorage.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ApplyDiff(<a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff</code> : </li>
</ul>





</td>
    </tr>
</table>





