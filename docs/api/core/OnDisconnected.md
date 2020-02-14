---
title: OnDisconnected Struct
slug: api-core-ondisconnected
order: 124
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L31">Source</a></span></p>

</p>


<p>ECS Component added to the worker entity immediately after disconnecting from SpatialOS </p>



</p>
<p><b>Inheritance</b></p>

<code>ISharedComponentData</code>
<code>IEquatable&lt;OnDisconnected&gt;</code>


</p>
<p><b>Notes</b></p>

- This is a temporary component and the [Improbable.Gdk.Core.CleanTemporaryComponentsSystem](doc:api-core-cleantemporarycomponentssystem) will remove it at the end of the frame. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="reasonfordisconnect"></a><b>ReasonForDisconnect</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string ReasonForDisconnect</code></p>The reported reason for disconnecting </td>    </tr></table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-ondisconnected"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L38">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Equals([OnDisconnected](doc:api-core-ondisconnected) other)</code></p></p><b>Parameters</b><ul><li><code>[OnDisconnected](doc:api-core-ondisconnected) other</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-object"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override bool Equals(object obj)</code></p></p><b>Parameters</b><ul><li><code>object obj</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="gethashcode"></a><b>GetHashCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L48">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override int GetHashCode()</code></p></td>    </tr></table>


