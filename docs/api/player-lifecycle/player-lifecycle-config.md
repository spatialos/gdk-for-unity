
# PlayerLifecycleConfig Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/player-lifecycle-index">PlayerLifecycle</a><br/>
GDK package: PlayerLifecycle<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L26">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-fields">Static Fields</a>
</ul></nav>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>PlayerHeartbeatIntervalSeconds</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> float PlayerHeartbeatIntervalSeconds</code></p>
The time in seconds between player heartbeat requests. 

</p>

<b>Notes:</b>

<ul>
<li>This is used by the <a href="{{urlRoot}}/api/player-lifecycle/send-player-heartbeat-request-system">SendPlayerHeartbeatRequestSystem</a> on the server-worker to determine how often to send a  request to client-workers. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>MaxNumFailedPlayerHeartbeats</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L44">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> int MaxNumFailedPlayerHeartbeats</code></p>
The maximum number of failed heartbeats before a player is disconnected. 

</p>

<b>Notes:</b>

<ul>
<li>The <a href="{{urlRoot}}/api/player-lifecycle/handle-player-heartbeat-response-system">HandlePlayerHeartbeatResponseSystem</a> deletes a player entity if the corresponding client-worker fails to respond successfully to this number of consecutive  requests. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>MaxPlayerCreationRetries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L53">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> int MaxPlayerCreationRetries</code></p>
The maximum number of retries for player creation requests. 

</p>

<b>Notes:</b>

<ul>
<li>The number of times a player creation request is retried after the first attempt. Setting this to 0 disables retrying player creation after calling . </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>MaxPlayerCreatorQueryRetries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L63">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> int MaxPlayerCreatorQueryRetries</code></p>
The maximum number of retries for finding player creator entities, before any player creation occurs. 

</p>

<b>Notes:</b>

<ul>
<li>All player creation requests must be sent to a player creator entity, which are initially queried for when the <a href="{{urlRoot}}/api/player-lifecycle/send-create-player-request-system">SendCreatePlayerRequestSystem</a> starts. This field indicates the maximum number of retries for the </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AutoRequestPlayerCreation</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L73">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool AutoRequestPlayerCreation</code></p>
This indicates whether a player should be created automatically upon a worker connecting to SpatialOS. 

</p>

<b>Notes:</b>

<ul>
<li>If , a Player entity is automatically created upon a client-worker connecting to SpatialOS. However, to be able to send arbitrary serialized data in the player creation request, or to provide a callback to be invoked upon receiving a player creation response, this field must be set to . </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreatePlayerEntityTemplate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L82">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate</code></p>
The delegate responsible for returning a player EntityTemplate when creating a player. 

</p>

<b>Notes:</b>

<ul>
<li>This must be set before initiating any player creation because it is called by the <a href="{{urlRoot}}/api/player-lifecycle/handle-create-player-request-system">HandleCreatePlayerRequestSystem</a>. The system uses this delegate to request a new player entity based on the returned EntityTemplate. </li>
</ul>


</td>
    </tr>
</table>











