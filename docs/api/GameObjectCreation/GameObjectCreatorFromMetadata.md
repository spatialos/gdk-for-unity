---
title: GameObjectCreatorFromMetadata Class
slug: api-gameobjectcreation-gameobjectcreatorfrommetadata
order: 2
---

<p><b>Namespace:</b> Improbable.Gdk.GameObjectCreation<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L11">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.GameObjectCreation.IEntityGameObjectCreator](doc:api-gameobjectcreation-ientitygameobjectcreator)</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="gameobjectcreatorfrommetadata-string-vector3-ilogdispatcher"></a><b>GameObjectCreatorFromMetadata</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> GameObjectCreatorFromMetadata(string workerType, Vector3 workerOrigin, [ILogDispatcher](doc:api-core-ilogdispatcher) logger)</code></p></p><b>Parameters</b><ul><li><code>string workerType</code> : </li><li><code>Vector3 workerOrigin</code> : </li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logger</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onentitycreated-spatialosentity-entitygameobjectlinker"></a><b>OnEntityCreated</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L37">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnEntityCreated([SpatialOSEntity](doc:api-gameobjectcreation-spatialosentity) entity, [EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker) linker)</code></p>Called when a new SpatialOS Entity is checked out by the worker. </p><b>Returns:</b></br>A GameObject to be linked to the entity, or null if no GameObject should be linked. </p><b>Parameters</b><ul><li><code>[SpatialOSEntity](doc:api-gameobjectcreation-spatialosentity) entity</code> : </li><li><code>[EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker) linker</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onentityremoved-entityid"></a><b>OnEntityRemoved</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L69">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnEntityRemoved([EntityId](doc:api-core-entityid) entityId)</code></p>Called when a SpatialOS Entity is removed from the worker's view. </p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>



