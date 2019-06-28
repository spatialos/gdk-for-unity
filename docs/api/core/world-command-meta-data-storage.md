
# WorldCommandMetaDataStorage Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L11">Source</a>
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

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-command-meta-data-storage">Improbable.Gdk.Core.ICommandMetaDataStorage</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-payload-storage">Improbable.Gdk.Core.ICommandPayloadStorage&lt;WorldCommands.CreateEntity.Request&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-payload-storage">Improbable.Gdk.Core.ICommandPayloadStorage&lt;WorldCommands.DeleteEntity.Request&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-payload-storage">Improbable.Gdk.Core.ICommandPayloadStorage&lt;WorldCommands.ReserveEntityIds.Request&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-payload-storage">Improbable.Gdk.Core.ICommandPayloadStorage&lt;WorldCommands.EntityQuery.Request&gt;</a></code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L28">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L33">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L38">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L53">Source</a></td>
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


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L58">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/request">WorldCommands.CreateEntity.Request</a>&gt; context)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/request">WorldCommands.CreateEntity.Request</a>&gt; context</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L63">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">WorldCommands.DeleteEntity.Request</a>&gt; context)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">WorldCommands.DeleteEntity.Request</a>&gt; context</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L68">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">WorldCommands.ReserveEntityIds.Request</a>&gt; context)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">WorldCommands.ReserveEntityIds.Request</a>&gt; context</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L73">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;WorldCommands.EntityQuery.Request&gt; context)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in <a href="{{urlRoot}}/api/core/command-context">CommandContext</a>&lt;WorldCommands.EntityQuery.Request&gt; context</code> : </li>
</ul>





</td>
    </tr>
</table>





