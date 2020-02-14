---
title: PlayerLifecycleHelper Class
slug: api-playerlifecycle-playerlifecyclehelper
order: 7
---

<p><b>Namespace:</b> Improbable.Gdk.PlayerLifecycle<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L7">Source</a></span></p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addplayerlifecyclecomponents-entitytemplate-string-string"></a><b>AddPlayerLifecycleComponents</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddPlayerLifecycleComponents([EntityTemplate](doc:api-core-entitytemplate) template, string clientWorkerId, string serverAccess)</code></p>Adds the SpatialOS components used by the player lifecycle module to an entity template. </p><b>Parameters</b><ul><li><code>[EntityTemplate](doc:api-core-entitytemplate) template</code> : The entity template to add player lifecycle components to.</li><li><code>string clientWorkerId</code> : The ID of the client-worker.</li><li><code>string serverAccess</code> : The server-worker write access attribute.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isowningworker-entityid-world"></a><b>IsOwningWorker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsOwningWorker([EntityId](doc:api-core-entityid) entityId, World workerWorld)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>World workerWorld</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addclientsystems-world-bool"></a><b>AddClientSystems</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L55">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddClientSystems(World world, bool autoRequestPlayerCreation = true)</code></p>Adds all the systems a client-worker requires for the player lifecycle module. </p><b>Parameters</b><ul><li><code>World world</code> : A world that belongs to a client-worker.</li><li><code>bool autoRequestPlayerCreation</code> : An option to toggle automatic player creation.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addserversystems-world"></a><b>AddServerSystems</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L66">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddServerSystems(World world)</code></p>Adds all the systems a server-worker requires for the player lifecycle module. </p><b>Parameters</b><ul><li><code>World world</code> : A world that belongs to a server-worker.</li></ul></td>    </tr></table>





