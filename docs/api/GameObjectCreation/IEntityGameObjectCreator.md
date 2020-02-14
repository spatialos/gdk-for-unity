---
title: IEntityGameObjectCreator Interface
slug: api-gameobjectcreation-ientitygameobjectcreator
order: 4
---

<p><b>Namespace:</b> Improbable.Gdk.GameObjectCreation<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/IEntityGameObjectCreator.cs/#L12">Source</a></span></p>

</p>


<p>Interface for listening for SpatialOS Entity creation to be used for binding GameObjects. Implementing classes can be passed to GameObjectCreationSystemHelper in order to be called. </p>













</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onentitycreated-spatialosentity-entitygameobjectlinker"></a><b>OnEntityCreated</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/IEntityGameObjectCreator.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnEntityCreated([SpatialOSEntity](doc:api-gameobjectcreation-spatialosentity) entity, [EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker) linker)</code></p>Called when a new SpatialOS Entity is checked out by the worker. </p><b>Returns:</b></br>A GameObject to be linked to the entity, or null if no GameObject should be linked. </p><b>Parameters</b><ul><li><code>[SpatialOSEntity](doc:api-gameobjectcreation-spatialosentity) entity</code> : </li><li><code>[EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker) linker</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onentityremoved-entityid"></a><b>OnEntityRemoved</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/IEntityGameObjectCreator.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnEntityRemoved([EntityId](doc:api-core-entityid) entityId)</code></p>Called when a SpatialOS Entity is removed from the worker's view. </p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>



