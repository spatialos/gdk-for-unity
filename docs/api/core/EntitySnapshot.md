---
title: EntitySnapshot Struct
slug: api-core-entitysnapshot
order: 58
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/EntitySnapshot.cs/#L9">Source</a></span></p>

</p>


<p>A snapshot of a SpatialOS entity. </p>













</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponentsnapshot-t"></a><b>GetComponentSnapshot&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/EntitySnapshot.cs/#L18">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T? GetComponentSnapshot&lt;T&gt;()</code></p>Gets the SpatialOS component snapshot if present. </p><b>Returns:</b></br>The component snapshot, if it exists, or null otherwise.</p><b>Type parameters:</b><ul><li><code>T</code> : The component type.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetcomponentsnapshot-t-out-t"></a><b>TryGetComponentSnapshot&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/EntitySnapshot.cs/#L37">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetComponentSnapshot&lt;T&gt;(out T snapshot)</code></p>Attempts to get the SpatialOS component if present. </p><b>Returns:</b></br>True, if the component exists; false otherwise.</p><b>Parameters</b><ul><li><code>out T snapshot</code> : When this method returns, this will be the component if it exists, default constructed otherwise. </li></ul></p><b>Type parameters:</b><ul><li><code>T</code> : The component type.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponentsnapshot-t-t"></a><b>AddComponentSnapshot&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/EntitySnapshot.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponentSnapshot&lt;T&gt;(T component)</code></p>Adds a component to this snapshot. </p><b>Parameters</b><ul><li><code>T component</code> : The component to add.</li></ul></p><b>Notes:</b><ul><li>Will override any pre-existing component in the snapshot. </li></ul></p><b>Type parameters:</b><ul><li><code>T</code> : The component type.</li></ul></td>    </tr></table>



