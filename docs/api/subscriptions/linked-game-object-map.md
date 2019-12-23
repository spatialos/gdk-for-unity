
# LinkedGameObjectMap Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L10">Source</a>
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



<p>Represents the mapping between SpatialOS entity IDs and linked GameObjects. </p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="linkedgameobjectmap-entitygameobjectlinker"></a><b>LinkedGameObjectMap</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> LinkedGameObjectMap(<a href="{{urlRoot}}/api/subscriptions/entity-game-object-linker">EntityGameObjectLinker</a> linker)</code></p>
Initializes a new instance of the <a href="{{urlRoot}}/api/subscriptions/linked-game-object-map">LinkedGameObjectMap</a> class backed with the data from the specified <a href="{{urlRoot}}/api/subscriptions/entity-game-object-linker">EntityGameObjectLinker</a>. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/entity-game-object-linker">EntityGameObjectLinker</a> linker</code> : The linker which contains the backing data for this map.</li>
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
        <td style="border-right:none"><a id="getlinkedgameobjects-entityid"></a><b>GetLinkedGameObjects</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L29">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>IReadOnlyList&lt;GameObject&gt; GetLinkedGameObjects(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>
Gets the GameObjects that are linked to a given SpatialOS entity ID. 
</p><b>Returns:</b></br>A readonly list of the linked GameObjects or null if there are none linked.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : The entity ID to get GameObjects for.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="trygetlinkedgameobjects-entityid-out-ireadonlylist-gameobject"></a><b>TryGetLinkedGameObjects</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/LinkedGameObjectMap.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool TryGetLinkedGameObjects(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, out IReadOnlyList&lt;GameObject&gt; linkedGameObjects)</code></p>
Tries to get the GameObjects that are linked to a given SpatialOS entity ID. 
</p><b>Returns:</b></br>True, if there are any GameObjects linked to the EntityId; otherwise false

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : The entity ID to get GameObjects for.</li>
<li><code>out IReadOnlyList&lt;GameObject&gt; linkedGameObjects</code> : When this method returns, contains the GameObjects linked to the specified EntityId, if any are linked; otherwise, null. This parameter is passed uninitialized. </li>
</ul>





</td>
    </tr>
</table>





