---
title: SpatialOSEntity Struct
slug: api-gameobjectcreation-spatialosentity
order: 5
---

<p><b>Namespace:</b> Improbable.Gdk.GameObjectCreation<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/SpatialOSEntity.cs/#L10">Source</a></span></p>

</p>


<p>Used to easily retrieve information about a SpatialOS Entity instance from a Unity ECS Entity instance. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="spatialosentityid"></a><b>SpatialOSEntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/SpatialOSEntity.cs/#L12">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [EntityId](doc:api-core-entityid) SpatialOSEntityId</code></p></td>    </tr></table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hascomponent-t"></a><b>HasComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/SpatialOSEntity.cs/#L28">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasComponent&lt;T&gt;()</code></p>Checks if this entity has a component of type T </p><b>Returns:</b></br>True, if the entity has the component; false otherwise.</p><b>Type parameters:</b><ul><li><code>T</code> : The SpatialOS component.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponent-t"></a><b>GetComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/SpatialOSEntity.cs/#L39">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T GetComponent&lt;T&gt;()</code></p>Gets a component of type T attached to this entity. </p><b>Returns:</b></br>The component T attached to this entity.</p><b>Type parameters:</b><ul><li><code>T</code> : The SpatialOS component.</li></ul></p><b>Exceptions:</b><ul><li><code>System.ArgumentException</code> : Thrown if the component T is not attached to this entity.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetcomponent-t-out-t"></a><b>TryGetComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/SpatialOSEntity.cs/#L53">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetComponent&lt;T&gt;(out T component)</code></p>Attempts to get a component of type T attached to this entity. </p><b>Returns:</b></br>True, if the entity has the component; false otherwise.</p><b>Parameters</b><ul><li><code>out T component</code> : When this method returns, this will be the component attached to this entity if it exists; default constructed otherwise. </li></ul></p><b>Type parameters:</b><ul><li><code>T</code> : The SpatialOS component.</li></ul></td>    </tr></table>



