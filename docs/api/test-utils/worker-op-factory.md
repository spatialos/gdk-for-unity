
# WorkerOpFactory Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/test-utils-index">TestUtils</a><br/>
GDK package: TestUtils<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L12">Source</a>
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
</ul></nav>

</p>



<p>A static class that contains helper methods for constructing ops. All ops are empty outside of the required information given in the constructor. Underlying schema data can be populated using the return value of each function. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateAddEntityOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;AddEntityOp&gt; CreateAddEntityOp(long entityId)</code></p>



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
        <td style="border-right:none"><b>CreateRemoveEntityOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;RemoveEntityOp&gt; CreateRemoveEntityOp(long entityId)</code></p>



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
        <td style="border-right:none"><b>CreateAddComponentOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;AddComponentOp&gt; CreateAddComponentOp(long entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateComponentUpdateOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;ComponentUpdateOp&gt; CreateComponentUpdateOp(long entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateRemoveComponentOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;RemoveComponentOp&gt; CreateRemoveComponentOp(long entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateAuthorityChangeOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L66">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;AuthorityChangeOp&gt; CreateAuthorityChangeOp(long entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateCommandRequestOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L77">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;CommandRequestOp&gt; CreateCommandRequestOp(uint componentId, uint commandIndex, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandIndex</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateCommandResponseOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L89">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;CommandResponseOp&gt; CreateCommandResponseOp(uint componentId, uint commandIndex, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandIndex</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateDisconnectOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L102">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;DisconnectOp&gt; CreateDisconnectOp(string reason)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string reason</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateCreateEntityResponseOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L108">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;CreateEntityResponseOp&gt; CreateCreateEntityResponseOp(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateDeleteEntityResponseOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L118">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;DeleteEntityResponseOp&gt; CreateDeleteEntityResponseOp(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateReserveEntityIdsResponseOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L128">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;ReserveEntityIdsResponseOp&gt; CreateReserveEntityIdsResponseOp(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateEntityQueryResponseOp</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L138">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a>&lt;EntityQueryResponseOp&gt; CreateEntityQueryResponseOp(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>







