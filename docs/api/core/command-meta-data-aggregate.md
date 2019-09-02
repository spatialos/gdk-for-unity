
# CommandMetaDataAggregate Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaDataAggregate.cs/#L6">Source</a>
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
        <td style="border-right:none"><a id="markidforremoval-uint-uint-long"></a><b>MarkIdForRemoval</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaDataAggregate.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void MarkIdForRemoval(uint componentId, uint commandId, long internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
<li><code>long internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="flushremovedids"></a><b>FlushRemovedIds</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaDataAggregate.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void FlushRemovedIds()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcontext-t-uint-uint-long"></a><b>GetContext&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/CommandMetaDataAggregate.cs/#L29">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;T&gt; GetContext&lt;T&gt;(uint componentId, uint commandId, long internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
<li><code>long internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>





