
# EntityQuerySnapshot Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Utility/EntityQuerySnapshot.cs/#L15">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>A snapshot of a SpatialOS entity, containing the result of a entity query. </p>




</p>

<b>Notes</b>

- This copies entity components from Improbable.Worker.CInterop.Entity for long term storage. This may only be a partial snapshot of an entity. The components present depend on the component filter used when making the entity query. 










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetComponentSnapshot&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Utility/EntityQuerySnapshot.cs/#L53">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T? GetComponentSnapshot&lt;T&gt;()</code></p>
Get the SpatialOS component snapshot if present. 
</p><b>Returns:</b></br>The component snapshot, if it exists, or null otherwise.



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : The component type.</li>
</ul>



</td>
    </tr>
</table>





