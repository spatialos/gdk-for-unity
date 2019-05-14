
# Request Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands">WorldCommands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity">CreateEntity</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L18">Source</a>
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



<p>An object that is a CreateEntity command request. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/commands/i-command-request">Improbable.Gdk.Core.Commands.ICommandRequest</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Entity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Improbable.Worker.CInterop.Entity Entity</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>EntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/entity-id">EntityId</a>? EntityId</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>TimeoutMillis</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L22">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L23">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/CreateEntity.cs/#L42">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Request(<a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a>? entityId = null, uint? timeoutMillis = null, object context = null)</code></p>
Constructor to create a CreateEntity command request payload. 
</p><b>Returns:</b></br>The CreateEntity command request payload.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template</code> : The <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> object that defines the SpatialOS components on the to-be-created entity. </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a>? entityId</code> : (Optional) The <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> that the to-be-created entity should take. This should only be provided if received as the result of a ReserveEntityIds command. </li>
<li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li>
<li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li>
</ul>





</td>
    </tr>
</table>






