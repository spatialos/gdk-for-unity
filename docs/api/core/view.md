---
title: View Class
slug: api-core-view
order: 146
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L9">Source</a></span></p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="view"></a><b>View</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L18">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> View()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="updatecomponent-t-entityid-in-t"></a><b>UpdateComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L33">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void UpdateComponent&lt;T&gt;([EntityId](doc:api-core-entityid) entityId, in T update)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>in T update</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getentityids"></a><b>GetEntityIds</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L39">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>HashSet&lt;[EntityId](doc:api-core-entityid)&gt; GetEntityIds()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hasentity-entityid"></a><b>HasEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L44">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasEntity([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcomponent-t-entityid"></a><b>GetComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T GetComponent&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hascomponent-t-entityid"></a><b>HasComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L61">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasComponent&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hascomponent-entityid-uint"></a><b>HasComponent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L72">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasComponent([EntityId](doc:api-core-entityid) entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getauthority-t-entityid"></a><b>GetAuthority&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Authority GetAuthority&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getauthority-entityid-uint"></a><b>GetAuthority</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L93">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Authority GetAuthority([EntityId](doc:api-core-entityid) entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isauthoritative-t-entityid"></a><b>IsAuthoritative&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L103">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsAuthoritative&lt;T&gt;([EntityId](doc:api-core-entityid) entityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isauthoritative-entityid-uint"></a><b>IsAuthoritative</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L113">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsAuthoritative([EntityId](doc:api-core-entityid) entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getworkerflag-string"></a><b>GetWorkerFlag</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/View/View.cs/#L123">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetWorkerFlag(string name)</code></p></p><b>Parameters</b><ul><li><code>string name</code> : </li></ul></td>    </tr></table>



