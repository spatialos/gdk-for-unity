
# IComponentDiffDeserializer Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/MessageSerialization.cs/#L5">Source</a>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/MessageSerialization.cs/#L7">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetComponentId()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddUpdateToDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/MessageSerialization.cs/#L9">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddUpdateToDiff(ComponentUpdateOp op, <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff, uint updateId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>ComponentUpdateOp op</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff</code> : </li>
<li><code>uint updateId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddComponentToDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/MessageSerialization.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddComponentToDiff(AddComponentOp op, <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>AddComponentOp op</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff</code> : </li>
</ul>





</td>
    </tr>
</table>





