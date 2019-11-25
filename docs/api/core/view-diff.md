
# ViewDiff Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L10">Source</a>
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
        <td style="border-right:none"><a id="disconnectmessage"></a><b>DisconnectMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L12">Source</a></td>
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
        <td style="border-right:none"><a id="disconnected"></a><b>Disconnected</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool Disconnected { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="incriticalsection"></a><b>InCriticalSection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L16">Source</a></td>
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
        <td style="border-right:none"><a id="viewdiff"></a><b>ViewDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L48">Source</a></td>
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
        <td style="border-right:none"><a id="clear"></a><b>Clear</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L104">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Clear()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addentity-long"></a><b>AddEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L128">Source</a></td>
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
        <td style="border-right:none"><a id="removeentity-long"></a><b>RemoveEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L136">Source</a></td>
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
        <td style="border-right:none"><a id="addcomponent-t-t-long-uint"></a><b>AddComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L144">Source</a></td>
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
        <td style="border-right:none"><a id="removecomponent-long-uint"></a><b>RemoveComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L156">Source</a></td>
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
        <td style="border-right:none"><a id="setauthority-long-uint-authority"></a><b>SetAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L168">Source</a></td>
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
        <td style="border-right:none"><a id="addcomponentupdate-t-t-long-uint-uint"></a><b>AddComponentUpdate&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L193">Source</a></td>
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
        <td style="border-right:none"><a id="addevent-t-t-long-uint-uint"></a><b>AddEvent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L207">Source</a></td>
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
        <td style="border-right:none"><a id="addcommandrequest-t-t-uint-uint"></a><b>AddCommandRequest&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L220">Source</a></td>
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
        <td style="border-right:none"><a id="addcommandresponse-t-t-uint-uint"></a><b>AddCommandResponse&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L235">Source</a></td>
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
        <td style="border-right:none"><a id="addcreateentityresponse-worldcommands-createentity-receivedresponse"></a><b>AddCreateEntityResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L251">Source</a></td>
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
        <td style="border-right:none"><a id="adddeleteentityresponse-worldcommands-deleteentity-receivedresponse"></a><b>AddDeleteEntityResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L256">Source</a></td>
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
        <td style="border-right:none"><a id="addreserveentityidsresponse-worldcommands-reserveentityids-receivedresponse"></a><b>AddReserveEntityIdsResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L261">Source</a></td>
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
        <td style="border-right:none"><a id="addentityqueryresponse-worldcommands-entityquery-receivedresponse"></a><b>AddEntityQueryResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L266">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddEntityQueryResponse(WorldCommands.EntityQuery.ReceivedResponse response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>WorldCommands.EntityQuery.ReceivedResponse response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addlogmessage-string-loglevel"></a><b>AddLogMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L271">Source</a></td>
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
        <td style="border-right:none"><a id="addmetrics-metrics"></a><b>AddMetrics</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L276">Source</a></td>
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
        <td style="border-right:none"><a id="setworkerflag-string-string"></a><b>SetWorkerFlag</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L288">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetWorkerFlag(string flag, string value)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string flag</code> : </li>
<li><code>string value</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="disconnect-string"></a><b>Disconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L293">Source</a></td>
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
        <td style="border-right:none"><a id="setcriticalsection-bool"></a><b>SetCriticalSection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/ViewDiff.cs/#L299">Source</a></td>
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





