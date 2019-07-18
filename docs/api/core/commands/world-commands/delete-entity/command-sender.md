
# CommandSender Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands">WorldCommands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity">DeleteEntity</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L101">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#properties">Properties</a>
</ul></nav>

</p>



<p>ECS component is for sending DeleteEntity command requests to the SpatialOS runtime. </p>



</p>

<b>Inheritance</b>

<code>IComponentData</code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>RequestsToSend</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L109">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> List&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">Request</a>&gt; RequestsToSend { get; set; }</code></p>
The list of pending DeleteEntity command requests. To send a command request, add an element to this list. 


</td>
    </tr>
</table>








