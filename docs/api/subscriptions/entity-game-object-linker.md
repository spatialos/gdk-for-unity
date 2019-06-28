
# EntityGameObjectLinker Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L10">Source</a>
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
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>EntityGameObjectLinker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L29">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> EntityGameObjectLinker(World world)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>World world</code> : </li>
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
        <td style="border-right:none"><b>LinkGameObjectToSpatialOSEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void LinkGameObjectToSpatialOSEntity(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, GameObject gameObject, params Type [] componentTypesToAdd)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>GameObject gameObject</code> : </li>
<li><code>params Type [] componentTypesToAdd</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>UnlinkGameObjectFromEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L112">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void UnlinkGameObjectFromEntity(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, GameObject gameObject)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>GameObject gameObject</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>UnlinkAllGameObjects</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L162">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void UnlinkAllGameObjects()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>UnlinkAllGameObjectsFromEntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L171">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void UnlinkAllGameObjectsFromEntityId(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>FlushCommandBuffer</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/EntityGameObjectLinker.cs/#L186">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void FlushCommandBuffer()</code></p>






</td>
    </tr>
</table>





