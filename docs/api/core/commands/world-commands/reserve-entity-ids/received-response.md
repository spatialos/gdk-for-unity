
# ReceivedResponse Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands">WorldCommands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids">ReserveEntityIds</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L47">Source</a>
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



<p>An object that is the response of a ReserveEntityIds command from the SpatialOS runtime. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/commands/i-received-command-response">Improbable.Gdk.Core.Commands.IReceivedCommandResponse</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendingEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L49">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly Entity SendingEntity</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>StatusCode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly StatusCode StatusCode</code></p>
The status code of the command response. If equal to StatusCode.Success then the command succeeded. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Message</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string Message</code></p>
The failure message of the command. Will only be non-null if the command failed. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>FirstEntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L66">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly? <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> FirstEntityId</code></p>
The first entity ID in the range that was reserved. Will only be non-null if the command succeeded. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>NumberOfEntityIds</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L71">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly int NumberOfEntityIds</code></p>
The number of entity IDs that were reserved. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RequestPayload</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L76">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly <a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">Request</a> RequestPayload</code></p>
The request payload that was originally sent with this command. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Context</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L81">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly object Context</code></p>
The context object that was provided when sending the command. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RequestId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L86">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly long RequestId</code></p>
The unique request ID of this command. Will match the request ID in the corresponding request. 

</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetRequestId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L103">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>long IReceivedCommandResponse. GetRequestId()</code></p>
Gets the request ID from the request. For use in generic methods. 
</p><b>Returns:</b></br>The request ID associated with the request 




</td>
    </tr>
</table>





