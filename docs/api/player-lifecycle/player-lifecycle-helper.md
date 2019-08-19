
# PlayerLifecycleHelper Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/player-lifecycle-index">PlayerLifecycle</a><br/>
GDK package: PlayerLifecycle<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L7">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-methods">Static Methods</a>
</ul></nav>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddPlayerLifecycleComponents</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddPlayerLifecycleComponents(<a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template, string clientWorkerId, string serverAccess)</code></p>
Adds the SpatialOS components used by the player lifecycle module to an entity template. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template</code> : The entity template to add player lifecycle components to.</li>
<li><code>string clientWorkerId</code> : The ID of the client-worker.</li>
<li><code>string serverAccess</code> : The server-worker write access attribute.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsOwningWorker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsOwningWorker(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, World workerWorld)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>World workerWorld</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddClientSystems</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddClientSystems(World world, bool autoRequestPlayerCreation = true)</code></p>
Adds all the systems a client-worker requires for the player lifecycle module. 


</p>

<b>Parameters</b>

<ul>
<li><code>World world</code> : A world that belongs to a client-worker.</li>
<li><code>bool autoRequestPlayerCreation</code> : An option to toggle automatic player creation.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddServerSystems</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L66">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddServerSystems(World world)</code></p>
Adds all the systems a server-worker requires for the player lifecycle module. 


</p>

<b>Parameters</b>

<ul>
<li><code>World world</code> : A world that belongs to a server-worker.</li>
</ul>





</td>
    </tr>
</table>







