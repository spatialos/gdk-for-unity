---
title: EntityId Struct
slug: api-core-entityid
order: 57
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L12">Source</a></span></p>

</p>


<p>A unique identifier used to look up an entity in SpatialOS. </p>



</p>
<p><b>Inheritance</b></p>

<code>IEquatable&lt;EntityId&gt;</code>


</p>
<p><b>Notes</b></p>

- Instances of this type should be treated as transient identifiers that will not be consistent between different runs of the same simulation. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="id"></a><b>Id</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly long Id</code></p>The value of the [EntityId](doc:api-core-entityid). </p><b>Notes:</b><ul><li>Though this value is numeric, you should not perform any mathematical operations on it. </li></ul></td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="entityid-long"></a><b>EntityId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> EntityId(long id)</code></p>Constructs a new instance of an [EntityId](doc:api-core-entityid). </p><b>Parameters</b><ul><li><code>long id</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isvalid"></a><b>IsValid</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L34">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsValid()</code></p>Whether this represents a valid SpatialOS entity ID. Specifically, </p><b>Returns:</b></br>True iff valid.</p><b>Notes:</b><ul><li>`Id > 0`.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-entityid"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L51">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Equals([EntityId](doc:api-core-entityid) obj)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) obj</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-object"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L40">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override bool Equals(object obj)</code></p></p><b>Parameters</b><ul><li><code>object obj</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="gethashcode"></a><b>GetHashCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L73">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override int GetHashCode()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tostring"></a><b>ToString</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L81">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override string ToString()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Operators


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-entityid-entityid"></a><b>operator==</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L59">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool operator==([EntityId](doc:api-core-entityid) entityId1, [EntityId](doc:api-core-entityid) entityId2)</code></p>Returns true if entityId1 is exactly equal to entityId2. </p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId1</code> : </li><li><code>[EntityId](doc:api-core-entityid) entityId2</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-entityid-entityid"></a><b>operator!=</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L67">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool operator!=([EntityId](doc:api-core-entityid) entityId1, [EntityId](doc:api-core-entityid) entityId2)</code></p>Returns true if entityId1 is not exactly equal to entityId2. </p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId1</code> : </li><li><code>[EntityId](doc:api-core-entityid) entityId2</code> : </li></ul></td>    </tr></table>

