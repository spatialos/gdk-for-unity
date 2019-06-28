
# TransformUtils Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/transform-synchronization-index">TransformSynchronization</a><br/>
GDK package: TransformSynchronization<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L8">Source</a>
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



<p>A collection of utility functions for use with the Transform Synchronization Feature Module. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateTransformSnapshot</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>TransformInternal.Snapshot CreateTransformSnapshot(Vector3 location = default, Quaternion rotation = default, Vector3 velocity = default)</code></p>
Utility method for creating a TransformInternal Snapshot. 


</p>

<b>Parameters</b>

<ul>
<li><code>Vector3 location</code> : The location of an entity, given as a Unity Vector3. </li>
<li><code>Quaternion rotation</code> : The rotation of an entity, given as a Unity Quaternion. </li>
<li><code>Vector3 velocity</code> : The velocity of an entity, given as a Unity Vector3. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This method populates a TransformInternal with compressed representations of the given arguments. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateTransformSnapshot</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L54">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>TransformInternal.Snapshot CreateTransformSnapshot(Coordinates location = default, Quaternion rotation = default, Vector3 velocity = default)</code></p>
Utility method for creating a TransformInternal Snapshot. 


</p>

<b>Parameters</b>

<ul>
<li><code>Coordinates location</code> : The location of an entity, given as Improbable Coordinates. </li>
<li><code>Quaternion rotation</code> : The rotation of an entity, given as a Unity Quaternion. </li>
<li><code>Vector3 velocity</code> : The velocity of an entity, given as a Unity Vector3. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This method populates a TransformInternal with compressed representations of the given arguments. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToCoordinates</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L286">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Coordinates ToCoordinates(this Vector3 vector3)</code></p>
Converts a Unity Vector3 to a Coordinates value. 


</p>

<b>Parameters</b>

<ul>
<li><code>this Vector3 vector3</code> : </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>Converts each component from a float to a double. </li>
</ul>




</td>
    </tr>
</table>







