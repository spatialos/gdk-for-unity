
# EntityTemplate Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L11">Source</a>
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
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Utility class to help build SpatialOS entities. An <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> can be mutated be used multiple times. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getworkeraccessattribute-string"></a><b>GetWorkerAccessAttribute</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetWorkerAccessAttribute(string workerId)</code></p>
Constructs a worker access attribute, given a worker ID. 
</p><b>Returns:</b></br>A string representing the worker access attribute.

</p>

<b>Parameters</b>

<ul>
<li><code>string workerId</code> : An ID of a worker.</li>
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
        <td style="border-right:none"><a id="addcomponent-tsnapshot-tsnapshot-string"></a><b>AddComponent&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L45">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddComponent&lt;TSnapshot&gt;(TSnapshot snapshot, string writeAccess)</code></p>
Adds a SpatialOS component to the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>. 


</p>

<b>Parameters</b>

<ul>
<li><code>TSnapshot snapshot</code> : The component snapshot to add.</li>
<li><code>string writeAccess</code> : The worker attribute that should be granted write access over the TSnapshot component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>EntityACL is handled automatically by the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>, so a EntityACL snapshot will be ignored. </li>
</ul>



</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component snapshot.</li>
</ul>



</p>

<b>Exceptions:</b>

<ul>
<li><code>InvalidOperationException</code> : Thrown if the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> already contains a component snapshot of type TSnapshot. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponent-tsnapshot"></a><b>GetComponent&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L70">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>TSnapshot? GetComponent&lt;TSnapshot&gt;()</code></p>
Attempts to get a component snapshot stored in the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>. 
</p><b>Returns:</b></br>The component snapshot, if the component snapshot exists, null otherwise.



</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component snapshot.</li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="hascomponent-uint"></a><b>HasComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L85">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasComponent(uint componentId)</code></p>
Checks if a component snapshot is stored in the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>. 
</p><b>Returns:</b></br>True, if the component snapshot exists, false otherwise.

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : The component id to check.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="hascomponent-tsnapshot"></a><b>HasComponent&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L95">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasComponent&lt;TSnapshot&gt;()</code></p>
Checks if a component snapshot is stored in the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>. 
</p><b>Returns:</b></br>True, if the component snapshot exists, false otherwise.



</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component snapshot.</li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setcomponent-tsnapshot-tsnapshot"></a><b>SetComponent&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L108">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetComponent&lt;TSnapshot&gt;(TSnapshot snapshot)</code></p>
Sets a component snapshot in the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>. 


</p>

<b>Parameters</b>

<ul>
<li><code>TSnapshot snapshot</code> : The component snapshot that will be inserted into the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>.</li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This will override a snapshot of type TSnapshot in the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> if one already exists. </li>
</ul>



</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component snapshot.</li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="removecomponent-tsnapshot"></a><b>RemoveComponent&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L117">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveComponent&lt;TSnapshot&gt;()</code></p>
Removes a component snapshot from the <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a>, if it exists. 




</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component snapshot.</li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponentwriteaccess-uint"></a><b>GetComponentWriteAccess</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L129">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetComponentWriteAccess(uint componentId)</code></p>
Retrieves the write access worker attribute for a given component. 
</p><b>Returns:</b></br>The write access worker attribute, if it exists, null otherwise.

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : The component id for that component.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponentwriteaccess-tsnapshot"></a><b>GetComponentWriteAccess&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L139">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetComponentWriteAccess&lt;TSnapshot&gt;()</code></p>
Retrieves the write access worker attribute for a given component. 
</p><b>Returns:</b></br>The write access worker attribute, if it exists, null otherwise.



</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component.</li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setcomponentwriteaccess-uint-string"></a><b>SetComponentWriteAccess</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L149">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetComponentWriteAccess(uint componentId, string writeAccess)</code></p>
Sets the write access worker attribute for a given component. 


</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : The component id for that component.</li>
<li><code>string writeAccess</code> : The write access worker attribute.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setcomponentwriteaccess-tsnapshot-string"></a><b>SetComponentWriteAccess&lt;TSnapshot&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L159">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetComponentWriteAccess&lt;TSnapshot&gt;(string writeAccess)</code></p>
Sets the write access worker attribute for a given component. 


</p>

<b>Parameters</b>

<ul>
<li><code>string writeAccess</code> : The write access worker attribute.</li>
</ul>




</p>

<b>Type parameters:</b>

<ul>
<li><code>TSnapshot</code> : The type of the component.</li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setreadaccess-params-string"></a><b>SetReadAccess</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L169">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetReadAccess(params string[] attributes)</code></p>
Sets the worker attributes which should have read access over this entity. 


</p>

<b>Parameters</b>

<ul>
<li><code>params string[] attributes</code> : The worker attributes which should have read access.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getentity"></a><b>GetEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L185">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Entity GetEntity()</code></p>
Creates an Entity instance from this template. 
</p><b>Returns:</b></br>The Entity object.


</p>

<b>Notes:</b>

<ul>
<li>This function allocates native memory. The Entity returned from this function should be handed to a GDK API, which will take ownership, or otherwise must be disposed of manually. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getentitysnapshot"></a><b>GetEntitySnapshot</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Utility/EntityTemplate.cs/#L199">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/entity-snapshot">EntitySnapshot</a> GetEntitySnapshot()</code></p>
Creates an <a href="{{urlRoot}}/api/core/entity-snapshot">EntitySnapshot</a> from this template. 
</p><b>Returns:</b></br>The <a href="{{urlRoot}}/api/core/entity-snapshot">EntitySnapshot</a> object.




</td>
    </tr>
</table>





