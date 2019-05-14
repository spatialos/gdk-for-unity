
# SingleUseIndexCallbacks&lt;T&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Callbacks.cs/#L209">Source</a>
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
        <td style="border-right:none"><b>Add</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Callbacks.cs/#L213">Source</a></td>
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
        <td style="border-right:none"><b>RemoveAllCallbacksForIndex</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Callbacks.cs/#L224">Source</a></td>
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
        <td style="border-right:none"><b>Remove</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Callbacks.cs/#L229">Source</a></td>
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
        <td style="border-right:none"><b>InvokeAll</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Callbacks.cs/#L242">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeAll(long index, T op)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long index</code> : </li>
<li><code>T op</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>InvokeAllReverse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Callbacks.cs/#L250">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeAllReverse(long index, T op)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long index</code> : </li>
<li><code>T op</code> : </li>
</ul>





</td>
    </tr>
</table>





