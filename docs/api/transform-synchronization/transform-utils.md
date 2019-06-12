
# TransformUtils Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/transform-synchronization-index">TransformSynchronization</a><br/>
GDK package: TransformSynchronization<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L5">Source</a>
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
        <td style="border-right:none"><b>HasChanged</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L8">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasChanged(Coordinates a, Coordinates b)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Coordinates a</code> : </li>
<li><code>Coordinates b</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasChanged</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasChanged(Location a, Location b)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Location a</code> : </li>
<li><code>Location b</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasChanged</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasChanged(Quaternion a, Quaternion b)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Quaternion a</code> : </li>
<li><code>Quaternion b</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToUnityQuaternion</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>UnityEngine.Quaternion ToUnityQuaternion(this Quaternion quaternion)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this Quaternion quaternion</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToImprobableQuaternion</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Quaternion ToImprobableQuaternion(this UnityEngine.Quaternion quaternion)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this UnityEngine.Quaternion quaternion</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToUnityVector3</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>UnityEngine.Vector3 ToUnityVector3(this Location location)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this Location location</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToImprobableLocation</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L41">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Location ToImprobableLocation(this Vector3 vector)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this Vector3 vector</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToUnityVector3</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L46">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>UnityEngine.Vector3 ToUnityVector3(this Velocity velocity)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this Velocity velocity</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToImprobableVelocity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L51">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Velocity ToImprobableVelocity(this Vector3 velocity)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this Vector3 velocity</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToCoordinates</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.3/workers/unity/Packages/com.improbable.gdk.transformsynchronization/TransformUtils.cs/#L56">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Coordinates ToCoordinates(this Location location)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>this Location location</code> : </li>
</ul>





</td>
    </tr>
</table>







