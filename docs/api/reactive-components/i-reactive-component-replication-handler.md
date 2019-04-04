
# IReactiveComponentReplicationHandler Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/reactive-components-index">ReactiveComponents</a><br/>
GDK package: ReactiveComponents<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/CodegenAdapters/IReactiveComponentReplicationHandler.cs/#L7">Source</a>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/CodegenAdapters/IReactiveComponentReplicationHandler.cs/#L9">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint ComponentId { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>EventQuery</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/CodegenAdapters/IReactiveComponentReplicationHandler.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> EntityArchetypeQuery EventQuery { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CommandQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/CodegenAdapters/IReactiveComponentReplicationHandler.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> EntityArchetypeQuery [] CommandQueries { get; }</code></p>



</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendEvents</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/CodegenAdapters/IReactiveComponentReplicationHandler.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendEvents(NativeArray&lt;ArchetypeChunk&gt; chunkArray, ComponentSystemBase system, <a href="{{urlRoot}}/api/core/component-update-system">ComponentUpdateSystem</a> componentUpdateSystem)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>NativeArray&lt;ArchetypeChunk&gt; chunkArray</code> : </li>
<li><code>ComponentSystemBase system</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/component-update-system">ComponentUpdateSystem</a> componentUpdateSystem</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendCommands</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/CodegenAdapters/IReactiveComponentReplicationHandler.cs/#L16">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendCommands(NativeArray&lt;ArchetypeChunk&gt; chunkArray, ComponentSystemBase system, <a href="{{urlRoot}}/api/core/command-system">CommandSystem</a> commandSystem)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>NativeArray&lt;ArchetypeChunk&gt; chunkArray</code> : </li>
<li><code>ComponentSystemBase system</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/command-system">CommandSystem</a> commandSystem</code> : </li>
</ul>





</td>
    </tr>
</table>





