
# IComponentReplicationHandler Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/codegen-adapters-index">CodegenAdapters</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L6">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#properties">Properties</a>
<li><a href="#methods">Methods</a>
</ul></nav>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L8">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint ComponentId { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ComponentUpdateQuery</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L9">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> EntityArchetypeQuery ComponentUpdateQuery { get; }</code></p>



</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendUpdates</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/CodegenAdapters/IComponentReplicationHandler.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendUpdates(NativeArray&lt;ArchetypeChunk&gt; chunkArray, ComponentSystemBase system, EntityManager entityManager, <a href="{{urlRoot}}/api/core/component-update-system">ComponentUpdateSystem</a> componentUpdateSystem)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>NativeArray&lt;ArchetypeChunk&gt; chunkArray</code> : </li>
<li><code>ComponentSystemBase system</code> : </li>
<li><code>EntityManager entityManager</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/component-update-system">ComponentUpdateSystem</a> componentUpdateSystem</code> : </li>
</ul>





</td>
    </tr>
</table>





