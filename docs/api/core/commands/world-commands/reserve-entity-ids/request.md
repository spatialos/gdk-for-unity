
# Request Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands">WorldCommands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids">ReserveEntityIds</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L18">Source</a>
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
<li><a href="#constructors">Constructors</a>
</ul></nav>

</p>



<p>An object that is a ReserveEntityIds command request. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/commands/i-command-request">Improbable.Gdk.Core.Commands.ICommandRequest</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>NumberOfEntityIds</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint NumberOfEntityIds</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>TimeoutMillis</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint? TimeoutMillis</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Context</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> object Context</code></p>


</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Request</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/ReserveEntityIds.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Request(uint numberOfEntityIds, uint? timeoutMillis = null, object context = null)</code></p>
Used to create a ReserveEntityIds command request payload. 
</p><b>Returns:</b></br>The ReserveEntityIds command request payload.

</p>

<b>Parameters</b>

<ul>
<li><code>uint numberOfEntityIds</code> : The number of entity IDs to reserve.</li>
<li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li>
<li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li>
</ul>





</td>
    </tr>
</table>






