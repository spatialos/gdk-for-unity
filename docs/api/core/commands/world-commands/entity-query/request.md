
# Request Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands">WorldCommands</a>.<a href="{{urlRoot}}/api/core/commands/world-commands/entity-query">EntityQuery</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L19">Source</a>
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



<p>An object that is a EntityQuery command request. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/commands/i-command-request">Improbable.Gdk.Core.Commands.ICommandRequest</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>EntityQuery</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Improbable.Worker.CInterop.Query.EntityQuery EntityQuery</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>TimeoutMillis</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L22">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L23">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Commands/WorldCommands/EntityQuery.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Request(Improbable.Worker.CInterop.Query.EntityQuery entityQuery, uint? timeoutMillis = null, object context = null)</code></p>
Method to create an EntityQuery command request payload. 


</p>

<b>Parameters</b>

<ul>
<li><code>Improbable.Worker.CInterop.Query.EntityQuery entityQuery</code> : The EntityQuery object defining the constraints and query type.</li>
<li><code>uint? timeoutMillis</code> : (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds. </li>
<li><code>object context</code> : (Optional) A context object that will be returned with the command response. </li>
</ul>





</td>
    </tr>
</table>






