
# WorldCommandsToSendStorage Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L10">Source</a>
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

<code><a href="{{urlRoot}}/api/core/i-command-send-storage">Improbable.Gdk.Core.ICommandSendStorage</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-request-send-storage">Improbable.Gdk.Core.ICommandRequestSendStorage&lt;WorldCommands.CreateEntity.Request&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-request-send-storage">Improbable.Gdk.Core.ICommandRequestSendStorage&lt;WorldCommands.DeleteEntity.Request&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-request-send-storage">Improbable.Gdk.Core.ICommandRequestSendStorage&lt;WorldCommands.ReserveEntityIds.Request&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-command-request-send-storage">Improbable.Gdk.Core.ICommandRequestSendStorage&lt;WorldCommands.EntityQuery.Request&gt;</a></code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="clear"></a><b>Clear</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L24">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Clear()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequest-worldcommands-createentity-request-entity-long"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/request">WorldCommands.CreateEntity.Request</a> request, Entity sendingEntity, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/request">WorldCommands.CreateEntity.Request</a> request</code> : </li>
<li><code>Entity sendingEntity</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequest-worldcommands-deleteentity-request-entity-long"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L39">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">WorldCommands.DeleteEntity.Request</a> request, Entity sendingEntity, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">WorldCommands.DeleteEntity.Request</a> request</code> : </li>
<li><code>Entity sendingEntity</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequest-worldcommands-reserveentityids-request-entity-long"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L46">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">WorldCommands.ReserveEntityIds.Request</a> request, Entity sendingEntity, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">WorldCommands.ReserveEntityIds.Request</a> request</code> : </li>
<li><code>Entity sendingEntity</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequest-worldcommands-entityquery-request-entity-long"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandsToSendStorage.cs/#L53">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(WorldCommands.EntityQuery.Request request, Entity sendingEntity, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>WorldCommands.EntityQuery.Request request</code> : </li>
<li><code>Entity sendingEntity</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>





