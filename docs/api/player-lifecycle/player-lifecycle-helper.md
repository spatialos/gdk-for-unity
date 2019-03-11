
# PlayerLifecycleHelper Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/player-lifecycle-index">PlayerLifecycle</a><br/>
GDK package: PlayerLifecycle<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L10">Source</a>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddPlayerLifecycleComponents(<a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template, string workerId, string clientAccess, string serverAccess)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template</code> : </li>
<li><code>string workerId</code> : </li>
<li><code>string clientAccess</code> : </li>
<li><code>string serverAccess</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SerializeArguments</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>byte [] SerializeArguments(object playerCreationArguments)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>object playerCreationArguments</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>DeserializeArguments&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T DeserializeArguments&lt;T&gt;(byte [] serializedArguments)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>byte [] serializedArguments</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsOwningWorker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L47">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsOwningWorker(<a href="{{urlRoot}}/api/core/spatial-entity-id">SpatialEntityId</a> entityId, World workerWorld)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/spatial-entity-id">SpatialEntityId</a> entityId</code> : </li>
<li><code>World workerWorld</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddClientSystems</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L73">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddClientSystems(World world, bool autoRequestPlayerCreation = true)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>World world</code> : </li>
<li><code>bool autoRequestPlayerCreation</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddServerSystems</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.playerlifecycle/PlayerLifecycleHelper.cs/#L80">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddServerSystems(World world)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>World world</code> : </li>
</ul>





</td>
    </tr>
</table>







