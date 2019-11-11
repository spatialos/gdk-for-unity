
# SingleUseIndexCallbacks&lt;T&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L7">Source</a>
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
        <td style="border-right:none"><a id="add-long-ulong-action-t"></a><b>Add</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Add(long index, ulong callbackKey, Action&lt;T&gt; value)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long index</code> : </li>
<li><code>ulong callbackKey</code> : </li>
<li><code>Action&lt;T&gt; value</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="removeallcallbacksforindex-long"></a><b>RemoveAllCallbacksForIndex</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveAllCallbacksForIndex(long index)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long index</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="remove-ulong"></a><b>Remove</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L27">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Remove(ulong callbackKey)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>ulong callbackKey</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="invokeall-long-t"></a><b>InvokeAll</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L41">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeAll(long index, T argument)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long index</code> : </li>
<li><code>T argument</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="invokeallreverse-long-t"></a><b>InvokeAllReverse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Callbacks/SingleUseIndexCallbacks.cs/#L49">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeAllReverse(long index, T argument)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long index</code> : </li>
<li><code>T argument</code> : </li>
</ul>





</td>
    </tr>
</table>





