---
title: PlayerLifecycleConfig Class
slug: api-playerlifecycle-playerlifecycleconfig
order: 6
---

<p><b>Namespace:</b> Improbable.Gdk.PlayerLifecycle<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L26">Source</a></span></p>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="playerheartbeatintervalseconds"></a><b>PlayerHeartbeatIntervalSeconds</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L35">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>float PlayerHeartbeatIntervalSeconds</code></p>The time in seconds between player heartbeat requests. </p><b>Notes:</b><ul><li>This is used by the [SendPlayerHeartbeatRequestSystem](doc:api-playerlifecycle-sendplayerheartbeatrequestsystem) on the server-worker to determine how often to send a `PlayerHeartbeat` request to client-workers. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="maxnumfailedplayerheartbeats"></a><b>MaxNumFailedPlayerHeartbeats</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L44">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>int MaxNumFailedPlayerHeartbeats</code></p>The maximum number of failed heartbeats before a player is disconnected. </p><b>Notes:</b><ul><li>The [HandlePlayerHeartbeatResponseSystem](doc:api-playerlifecycle-handleplayerheartbeatresponsesystem) deletes a player entity if the corresponding client-worker fails to respond successfully to this number of consecutive `PlayerHeartbeat` requests. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="maxplayercreationretries"></a><b>MaxPlayerCreationRetries</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L53">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>int MaxPlayerCreationRetries</code></p>The maximum number of retries for player creation requests. </p><b>Notes:</b><ul><li>The number of times a player creation request is retried after the first attempt. Setting this to 0 disables retrying player creation after calling `RequestPlayerCreation`. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="maxplayercreatorqueryretries"></a><b>MaxPlayerCreatorQueryRetries</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L63">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>int MaxPlayerCreatorQueryRetries</code></p>The maximum number of retries for finding player creator entities, before any player creation occurs. </p><b>Notes:</b><ul><li>All player creation requests must be sent to a player creator entity, which are initially queried for when the [SendCreatePlayerRequestSystem](doc:api-playerlifecycle-sendcreateplayerrequestsystem) starts. This field indicates the maximum number of retries for the </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="autorequestplayercreation"></a><b>AutoRequestPlayerCreation</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L73">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool AutoRequestPlayerCreation</code></p>This indicates whether a player should be created automatically upon a worker connecting to SpatialOS. </p><b>Notes:</b><ul><li>If `true`, a Player entity is automatically created upon a client-worker connecting to SpatialOS. However, to be able to send arbitrary serialized data in the player creation request, or to provide a callback to be invoked upon receiving a player creation response, this field must be set to `false`. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createplayerentitytemplate"></a><b>CreatePlayerEntityTemplate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Config/PlayerLifecycleConfig.cs/#L82">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate</code></p>The delegate responsible for returning a player EntityTemplate when creating a player. </p><b>Notes:</b><ul><li>This must be set before initiating any player creation because it is called by the [HandleCreatePlayerRequestSystem](doc:api-playerlifecycle-handlecreateplayerrequestsystem). The system uses this delegate to request a new player entity based on the returned EntityTemplate. </li></ul></td>    </tr></table>









