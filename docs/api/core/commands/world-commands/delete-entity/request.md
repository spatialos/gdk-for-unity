
# Request Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands">WorldCommands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity">DeleteEntity</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L20">Source</a>
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



<p>An object that is a DeleteEntity command request. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/commands/i-command-request">Improbable.Gdk.Core.Commands.ICommandRequest</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="entityid"></a><b>EntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> EntityId</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="timeoutmillis"></a><b>TimeoutMillis</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint? TimeoutMillis</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="context"></a><b>Context</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L24">Source</a></td>
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
        <td style="border-right:none"><a id="request-entityid-uint-object"></a><b>Request</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Commands/WorldCommands/DeleteEntity.cs/#L37">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Request(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint? timeoutMillis = null, object context = null)</code></p>
Method to create a DeleteEntity command request payload. 
</p><b>Returns:</b></br>The DeleteEntity command request payload.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : The entity ID that is to be deleted.</li>
<li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li>
<li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li>
</ul>





</td>
    </tr>
</table>






