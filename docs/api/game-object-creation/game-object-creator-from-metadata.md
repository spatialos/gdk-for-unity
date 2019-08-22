
# GameObjectCreatorFromMetadata Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/game-object-creation-index">GameObjectCreation</a><br/>
GDK package: GameObjectCreation<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L11">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/game-object-creation/i-entity-game-object-creator">Improbable.Gdk.GameObjectCreation.IEntityGameObjectCreator</a></code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>GameObjectCreatorFromMetadata</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> GameObjectCreatorFromMetadata(string workerType, Vector3 workerOrigin, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : </li>
<li><code>Vector3 workerOrigin</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger</code> : </li>
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
        <td style="border-right:none"><b>OnEntityCreated</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L37">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnEntityCreated(<a href="{{urlRoot}}/api/game-object-creation/spatial-os-entity">SpatialOSEntity</a> entity, <a href="{{urlRoot}}/api/subscriptions/entity-game-object-linker">EntityGameObjectLinker</a> linker)</code></p>
Called when a new SpatialOS Entity is checked out by the worker. 
</p><b>Returns:</b></br>A GameObject to be linked to the entity, or null if no GameObject should be linked. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/game-object-creation/spatial-os-entity">SpatialOSEntity</a> entity</code> : </li>
<li><code><a href="{{urlRoot}}/api/subscriptions/entity-game-object-linker">EntityGameObjectLinker</a> linker</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnEntityRemoved</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.gameobjectcreation/GameObjectCreatorFromMetadata.cs/#L69">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnEntityRemoved(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>
Called when a SpatialOS Entity is removed from the worker's view. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>





