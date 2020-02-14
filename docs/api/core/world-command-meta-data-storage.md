
# WorldCommandMetaDataStorage Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L11">Source</a>
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
<li><a href="#methods">Methods</a>
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
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="commandid"></a><b>CommandId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L28">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint CommandId</code></p>


</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="removemetadata-long"></a><b>RemoveMetaData</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveMetaData(long internalRequestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long internalRequestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setinternalrequestid-long-long"></a><b>SetInternalRequestId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L45">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetInternalRequestId(long internalRequestId, long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long internalRequestId</code> : </li>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequest-in-commandcontext-worldcommands-createentity-request"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L50">Source</a></td>
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
        <td style="border-right:none"><a id="addrequest-in-commandcontext-worldcommands-deleteentity-request"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L55">Source</a></td>
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
        <td style="border-right:none"><a id="addrequest-in-commandcontext-worldcommands-reserveentityids-request"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L60">Source</a></td>
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
        <td style="border-right:none"><a id="addrequest-in-commandcontext-worldcommands-entityquery-request"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorldCommandMetaDataStorage.cs/#L65">Source</a></td>
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





