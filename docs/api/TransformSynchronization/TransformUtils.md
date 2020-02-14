---
title: TransformUtils Class
slug: api-transformsynchronization-transformutils
order: 37
---

<p><b>Namespace:</b> Improbable.Gdk.TransformSynchronization<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.transformsynchronization/TransformUtils.cs/#L10">Source</a></span></p>

</p>


<p>A collection of utility functions for use with the Transform Synchronization Feature Module. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createtransformsnapshot-vector3-quaternion-vector3"></a><b>CreateTransformSnapshot</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.transformsynchronization/TransformUtils.cs/#L27">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>TransformInternal.Snapshot CreateTransformSnapshot(Vector3 location = default, Quaternion rotation = default, Vector3 velocity = default)</code></p>Utility method for creating a TransformInternal Snapshot. </p><b>Parameters</b><ul><li><code>Vector3 location</code> : The location of an entity, given as a Unity Vector3. </li><li><code>Quaternion rotation</code> : The rotation of an entity, given as a Unity Quaternion. </li><li><code>Vector3 velocity</code> : The velocity of an entity, given as a Unity Vector3. </li></ul></p><b>Notes:</b><ul><li>This method populates a TransformInternal with compressed representations of the given arguments. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tocoordinates-this-vector3"></a><b>ToCoordinates</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.transformsynchronization/TransformUtils.cs/#L44">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Coordinates ToCoordinates(this Vector3 unityVector)</code></p>Extension method for converting a Unity Vector to a Coordinates value. </p><b>Parameters</b><ul><li><code>this Vector3 unityVector</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tofixedpointvector3-this-vector3"></a><b>ToFixedPointVector3</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.transformsynchronization/TransformUtils.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>FixedPointVector3 ToFixedPointVector3(this Vector3 unityVector)</code></p>Extension method for converting a Unity Vector to a FixedPointVector3. </p><b>Parameters</b><ul><li><code>this Vector3 unityVector</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tocompressedquaternion-this-quaternion"></a><b>ToCompressedQuaternion</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.transformsynchronization/TransformUtils.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>CompressedQuaternion ToCompressedQuaternion(this Quaternion quaternion)</code></p>Extension method for converting a Quaternion to a CompressedQuaternion. </p><b>Parameters</b><ul><li><code>this Quaternion quaternion</code> : </li></ul></td>    </tr></table>





