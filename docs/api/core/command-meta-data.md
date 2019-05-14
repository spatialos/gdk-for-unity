
# CommandMetaData Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L23">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>CommandMetaData</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> CommandMetaData()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>MarkIdForRemoval</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L57">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void MarkIdForRemoval(uint componentId, uint commandId, uint internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
<li><code>uint internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>FlushRemovedIds</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L62">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void FlushRemovedIds()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddRequest&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L74">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest&lt;T&gt;(uint componentId, uint commandId, in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;T&gt; context)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
<li><code>in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;T&gt; context</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddInternalRequestId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L80">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddInternalRequestId(uint componentId, uint commandId, long requestId, uint internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
<li><code>long requestId</code> : </li>
<li><code>uint internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetContext&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Worker/CommandMetaData.cs/#L87">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;T&gt; GetContext&lt;T&gt;(uint componentId, uint commandId, uint internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
<li><code>uint internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>





