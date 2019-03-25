
# ViewDiff Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L8">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#fields">Fields</a>
<li><a href="#properties">Properties</a>
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>DisconnectMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string DisconnectMessage</code></p>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Disconnected</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool Disconnected { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>InCriticalSection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool InCriticalSection { get; }</code></p>



</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ViewDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ViewDiff()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Clear</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L94">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Clear()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L115">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddEntity(long entityId)</code></p>



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
        <td style="border-right:none"><b>RemoveEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L123">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveEntity(long entityId)</code></p>



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
        <td style="border-right:none"><b>AddComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L131">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddComponent&lt;T&gt;(T component, long entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T component</code> : </li>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RemoveComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L143">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveComponent(long entityId, uint componentId)</code></p>



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
        <td style="border-right:none"><b>SetAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L155">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetAuthority(long entityId, uint componentId, Authority authority)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>Authority authority</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddComponentUpdate&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L180">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddComponentUpdate&lt;T&gt;(T update, long entityId, uint componentId, uint updateId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T update</code> : </li>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>uint updateId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddEvent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L194">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddEvent&lt;T&gt;(T ev, long entityId, uint componentId, uint updateId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T ev</code> : </li>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>uint updateId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddCommandRequest&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L207">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddCommandRequest&lt;T&gt;(T request, uint componentId, uint commandId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T request</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddCommandResponse&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L222">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddCommandResponse&lt;T&gt;(T response, uint componentId, uint commandId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T response</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>uint commandId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddCreateEntityResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L238">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddCreateEntityResponse(<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/received-response">WorldCommands.CreateEntity.ReceivedResponse</a> response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/received-response">WorldCommands.CreateEntity.ReceivedResponse</a> response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddDeleteEntityResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L243">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddDeleteEntityResponse(<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/received-response">WorldCommands.DeleteEntity.ReceivedResponse</a> response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/received-response">WorldCommands.DeleteEntity.ReceivedResponse</a> response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddReserveEntityIdsResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L248">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddReserveEntityIdsResponse(<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/received-response">WorldCommands.ReserveEntityIds.ReceivedResponse</a> response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/received-response">WorldCommands.ReserveEntityIds.ReceivedResponse</a> response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddEntityQueryResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L253">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddEntityQueryResponse(<a href="{{urlRoot}}/api/core/commands/world-commands/entity-query/received-response">WorldCommands.EntityQuery.ReceivedResponse</a> response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/entity-query/received-response">WorldCommands.EntityQuery.ReceivedResponse</a> response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddLogMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L258">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddLogMessage(string message, LogLevel level)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string message</code> : </li>
<li><code>LogLevel level</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddMetrics</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L263">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddMetrics(Metrics metrics)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Metrics metrics</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Disconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L275">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Disconnect(string message)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string message</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SetCriticalSection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Worker/ViewDiff.cs/#L281">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetCriticalSection(bool inCriticalSection)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>bool inCriticalSection</code> : </li>
</ul>





</td>
    </tr>
</table>





