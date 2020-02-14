---
title: LinkedGameObjectMap Class
slug: api-subscriptions-linkedgameobjectmap
order: 19
---

<p><b>Namespace:</b> Improbable.Gdk.Subscriptions<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L10">Source</a></span></p>

</p>


<p>Represents the mapping between SpatialOS entity IDs and linked GameObjects. </p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="linkedgameobjectmap-entitygameobjectlinker"></a><b>LinkedGameObjectMap</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L19">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> LinkedGameObjectMap([EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker) linker)</code></p>Initializes a new instance of the [LinkedGameObjectMap](doc:api-subscriptions-linkedgameobjectmap) class backed with the data from the specified [EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker). </p><b>Parameters</b><ul><li><code>[EntityGameObjectLinker](doc:api-subscriptions-entitygameobjectlinker) linker</code> : The linker which contains the backing data for this map.</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getlinkedgameobjects-entityid"></a><b>GetLinkedGameObjects</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L29">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>IReadOnlyList&lt;GameObject&gt; GetLinkedGameObjects([EntityId](doc:api-core-entityid) entityId)</code></p>Gets the GameObjects that are linked to a given SpatialOS entity ID. </p><b>Returns:</b></br>A readonly list of the linked GameObjects or null if there are none linked.</p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : The entity ID to get GameObjects for.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetlinkedgameobjects-entityid-out-ireadonlylist-gameobject"></a><b>TryGetLinkedGameObjects</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetLinkedGameObjects([EntityId](doc:api-core-entityid) entityId, out IReadOnlyList&lt;GameObject&gt; linkedGameObjects)</code></p>Tries to get the GameObjects that are linked to a given SpatialOS entity ID. </p><b>Returns:</b></br>True, if there are any GameObjects linked to the EntityId; otherwise false</p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : The entity ID to get GameObjects for.</li><li><code>out IReadOnlyList&lt;GameObject&gt; linkedGameObjects</code> : When this method returns, contains the GameObjects linked to the specified EntityId, if any are linked; otherwise, null. This parameter is passed uninitialized. </li></ul></td>    </tr></table>



