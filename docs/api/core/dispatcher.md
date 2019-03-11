
# Dispatcher Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L153">Source</a>
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
        <td style="border-right:none"><b>Dispatcher</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L186">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Dispatcher()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnDisconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L190">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnDisconnect(Action&lt;DisconnectOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;DisconnectOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnFlagUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L196">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnFlagUpdate(Action&lt;FlagUpdateOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;FlagUpdateOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnLogMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L202">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnLogMessage(Action&lt;LogMessageOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;LogMessageOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnMetrics</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L208">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnMetrics(Action&lt;MetricsOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;MetricsOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnCriticalSection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L214">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnCriticalSection(Action&lt;CriticalSectionOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;CriticalSectionOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnAddEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L220">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnAddEntity(Action&lt;AddEntityOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;AddEntityOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnRemoveEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L226">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnRemoveEntity(Action&lt;RemoveEntityOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;RemoveEntityOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnReserveEntityIdsResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L232">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnReserveEntityIdsResponse(Action&lt;ReserveEntityIdsResponseOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;ReserveEntityIdsResponseOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnCreateEntityResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L238">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnCreateEntityResponse(Action&lt;CreateEntityResponseOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;CreateEntityResponseOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnDeleteEntityResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L244">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnDeleteEntityResponse(Action&lt;DeleteEntityResponseOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;DeleteEntityResponseOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnEntityQueryResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L251">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnEntityQueryResponse(Action&lt;EntityQueryResponseOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;EntityQueryResponseOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnAddComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L257">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnAddComponent(Action&lt;AddComponentOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;AddComponentOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnRemoveComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L263">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnRemoveComponent(Action&lt;RemoveComponentOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;RemoveComponentOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnAuthorityChange</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L269">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnAuthorityChange(Action&lt;AuthorityChangeOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;AuthorityChangeOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnComponentUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L275">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnComponentUpdate(Action&lt;ComponentUpdateOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;ComponentUpdateOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnCommandRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L281">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnCommandRequest(Action&lt;CommandRequestOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;CommandRequestOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnCommandResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L287">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong OnCommandResponse(Action&lt;CommandResponseOp&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;CommandResponseOp&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Remove</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L293">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Remove(ulong callbackKey)</code></p>



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
        <td style="border-right:none"><b>Process</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Dispatcher.cs/#L317">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Process(OpList opList)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>OpList opList</code> : </li>
</ul>





</td>
    </tr>
</table>





