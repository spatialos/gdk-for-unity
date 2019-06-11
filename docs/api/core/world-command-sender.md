
# WorldCommandSender Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L102">Source</a>
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
<li><a href="#methods">Methods</a>
</ul></nav>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsValid</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L104">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool IsValid</code></p>


</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorldCommandSender</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L110">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> WorldCommandSender(Entity entity, World world)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Entity entity</code> : </li>
<li><code>World world</code> : </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendCreateEntityCommand</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L119">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendCreateEntityCommand(<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/request">WorldCommands.CreateEntity.Request</a> request, Action&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/received-response">WorldCommands.CreateEntity.ReceivedResponse</a>&gt; callback = null)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/request">WorldCommands.CreateEntity.Request</a> request</code> : </li>
<li><code>Action&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/create-entity/received-response">WorldCommands.CreateEntity.ReceivedResponse</a>&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendDeleteEntityCommand</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L138">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendDeleteEntityCommand(<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">WorldCommands.DeleteEntity.Request</a> request, Action&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/received-response">WorldCommands.DeleteEntity.ReceivedResponse</a>&gt; callback = null)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/request">WorldCommands.DeleteEntity.Request</a> request</code> : </li>
<li><code>Action&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/delete-entity/received-response">WorldCommands.DeleteEntity.ReceivedResponse</a>&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendReserveEntityIdsCommand</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L157">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendReserveEntityIdsCommand(<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">WorldCommands.ReserveEntityIds.Request</a> request, Action&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/received-response">WorldCommands.ReserveEntityIds.ReceivedResponse</a>&gt; callback = null)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/request">WorldCommands.ReserveEntityIds.Request</a> request</code> : </li>
<li><code>Action&lt;<a href="{{urlRoot}}/api/core/commands/world-commands/reserve-entity-ids/received-response">WorldCommands.ReserveEntityIds.ReceivedResponse</a>&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendEntityQueryCommand</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/WorldCommands.cs/#L176">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendEntityQueryCommand(WorldCommands.EntityQuery.Request request, Action&lt;WorldCommands.EntityQuery.ReceivedResponse&gt; callback = null)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>WorldCommands.EntityQuery.Request request</code> : </li>
<li><code>Action&lt;WorldCommands.EntityQuery.ReceivedResponse&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>





