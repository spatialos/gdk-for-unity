
# Constraint Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/query-based-interest-index">QueryBasedInterest</a><br/>
GDK package: QueryBasedInterest<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L11">Source</a>
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



<p>Utility class to help define QueryConstraint objects for Interest queries. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Sphere</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L41">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Sphere(double radius, Coordinates center)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Sphere QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double radius</code> : Radius of the Sphere QueryConstraint. </li>
<li><code>Coordinates center</code> : Center of the Sphere QueryConstraint. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Sphere</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L70">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Sphere(double radius, double centerX, double centerY, double centerZ)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Sphere QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double radius</code> : Radius of the Sphere QueryConstraint. </li>
<li><code>double centerX</code> : X coordinate of the center of the Sphere QueryConstraint. </li>
<li><code>double centerY</code> : Y coordinate of the center of the Sphere QueryConstraint. </li>
<li><code>double centerZ</code> : Z coordinate of the center of the Sphere QueryConstraint. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Cylinder</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L91">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Cylinder(double radius, Coordinates center)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Cylinder QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double radius</code> : Radius of the Cylinder QueryConstraint. </li>
<li><code>Coordinates center</code> : Center of the Cylinder QueryConstraint. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Cylinder</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L120">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Cylinder(double radius, double centerX, double centerY, double centerZ)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Cylinder QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double radius</code> : Radius of the Cylinder QueryConstraint. </li>
<li><code>double centerX</code> : X coordinate of the center of the Cylinder QueryConstraint. </li>
<li><code>double centerY</code> : Y coordinate of the center of the Cylinder QueryConstraint. </li>
<li><code>double centerZ</code> : Z coordinate of the center of the Cylinder QueryConstraint. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Box</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L147">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Box(double xWidth, double yHeight, double zDepth, Coordinates center)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Box queryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double xWidth</code> : Width of Box QueryConstraint in the X-axis. </li>
<li><code>double yHeight</code> : Height of Box QueryConstraint in the Y-axis. </li>
<li><code>double zDepth</code> : Depth of Box QueryConstraint in the Z-axis. </li>
<li><code>Coordinates center</code> : Center of the Box QueryConstraint. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Box</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L186">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Box(double xWidth, double yHeight, double zDepth, double centerX, double centerY, double centerZ)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Box QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double xWidth</code> : Width of Box QueryConstraint in the X-axis. </li>
<li><code>double yHeight</code> : Height of Box QueryConstraint in the Y-axis. </li>
<li><code>double zDepth</code> : Depth of Box QueryConstraint in the Z-axis. </li>
<li><code>double centerX</code> : X coordinate of the center of the Box QueryConstraint. </li>
<li><code>double centerY</code> : Y coordinate of the center of the Box QueryConstraint. </li>
<li><code>double centerZ</code> : Z coordinate of the center of the Box QueryConstraint. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RelativeSphere</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L209">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> RelativeSphere(double radius)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a RelativeSphere QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double radius</code> : Radius of the RelativeSphere QueryConstraint. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> defines a sphere relative to the position of the entity. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RelativeCylinder</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L231">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> RelativeCylinder(double radius)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a RelativeCylinder QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double radius</code> : Radius of the cylinder QueryConstraint. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> defines a cylinder relative to the position of the entity. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RelativeBox</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L259">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> RelativeBox(double xWidth, double yHeight, double zDepth)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a RelativeBox QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>double xWidth</code> : Width of box QueryConstraint in the X-axis. </li>
<li><code>double yHeight</code> : Height of box QueryConstraint in the Y-axis. </li>
<li><code>double zDepth</code> : Depth of box QueryConstraint in the Z-axis. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> defines a box relative to the position of the entity. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>EntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L278">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> EntityId(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with an EntityId QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : EntityId of an entity to interested in. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Component&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L294">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Component&lt;T&gt;()</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with an Component QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : Type of the component to constrain. </li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Component</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L310">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Component(uint componentId)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with a Component QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : Component ID of the component to constrain. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>All</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L332">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> All(<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> constraint, params <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> [] constraints)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with an And QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> constraint</code> : First <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> in the list of conjunctions. </li>
<li><code>params <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> [] constraints</code> : Further Constraints for the list of conjunctions. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> must be provided to create a valid "All" QueryConstraint. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>All</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L353">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> All(IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a>&gt; constraints)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with an And QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a>&gt; constraints</code> : Constraints for the list of conjunctions. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> must be provided to create a valid "All" QueryConstraint. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Any</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L382">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Any(<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> constraint, params <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> [] constraints)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with an Or QueryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> constraint</code> : First <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> in the list of disjunctions. </li>
<li><code>params <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> [] constraints</code> : Further Constraints for the list of disjunctions. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> must be provided to create a valid "Any" QueryConstraint. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Any</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L403">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> Any(IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a>&gt; constraints)</code></p>
Creates a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object with an Or queryConstraint. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a>&gt; constraints</code> : Set of Constraints for the list of disjunctions. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> must be provided to create a valid "Any" QueryConstraint. </li>
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
        <td style="border-right:none"><b>AsQueryConstraint</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/Constraint.cs/#L423">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ComponentInterest.QueryConstraint AsQueryConstraint()</code></p>
Returns a QueryConstraint object from a <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a>. 
</p><b>Returns:</b></br>A QueryConstraint object. 




</td>
    </tr>
</table>





