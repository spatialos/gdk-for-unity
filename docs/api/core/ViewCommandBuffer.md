---
title: ViewCommandBuffer Class
slug: api-core-viewcommandbuffer
order: 147
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/ViewCommandBuffer.cs/#L15">Source</a></span></p>

</p>


<p>This class is for when a user wants to Add/Remove a Component (not IComponentData) during a system update without invalidating their injected arrays. The user must call Flush on this buffer at the end of the Update function to apply the buffered changes. </p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="viewcommandbuffer-entitymanager-ilogdispatcher"></a><b>ViewCommandBuffer</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/ViewCommandBuffer.cs/#L32">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> ViewCommandBuffer(EntityManager entityManager, [ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher)</code></p></p><b>Parameters</b><ul><li><code>EntityManager entityManager</code> : </li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponent-t-entity-t"></a><b>AddComponent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/ViewCommandBuffer.cs/#L46">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponent&lt;T&gt;(Entity entity, T component)</code></p>Adds a GameObject Component to an ECS entity. </p><b>Parameters</b><ul><li><code>Entity entity</code> : The ECS entity</li><li><code>T component</code> : The component</li></ul></p><b>Type parameters:</b><ul><li><code>T</code> : The type of the component.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponent-entity-componenttype-object"></a><b>AddComponent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/ViewCommandBuffer.cs/#L57">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponent(Entity entity, ComponentType componentType, object componentObj)</code></p>Adds a GameObject Component to an ECS entity. </p><b>Parameters</b><ul><li><code>Entity entity</code> : The ECS entity</li><li><code>ComponentType componentType</code> : The type of the component</li><li><code>object componentObj</code> : The component</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="removecomponent-entity-componenttype"></a><b>RemoveComponent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/ViewCommandBuffer.cs/#L73">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RemoveComponent(Entity entity, ComponentType componentType)</code></p>Removes a GameObject Component from an ECS entity. </p><b>Parameters</b><ul><li><code>Entity entity</code> : The ECS entity.</li><li><code>ComponentType componentType</code> : The type of the component to remove.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="flushbuffer"></a><b>FlushBuffer</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/ViewCommandBuffer.cs/#L87">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void FlushBuffer()</code></p>Plays back and applies all buffered actions in order. </td>    </tr></table>



