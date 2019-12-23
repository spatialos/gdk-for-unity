
# OnDisconnected Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L31">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#fields">Fields</a>
<li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
</ul></nav>

</p>



<p>ECS Component added to the worker entity immediately after disconnecting from SpatialOS </p>



</p>

<b>Inheritance</b>

<code>ISharedComponentData</code>
<code>IEquatable&lt;OnDisconnected&gt;</code>


</p>

<b>Notes</b>

- This is a temporary component and the <a href="{{urlRoot}}/api/core/clean-temporary-components-system">Improbable.Gdk.Core.CleanTemporaryComponentsSystem</a> will remove it at the end of the frame. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="reasonfordisconnect"></a><b>ReasonForDisconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string ReasonForDisconnect</code></p>
The reported reason for disconnecting 

</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="equals-ondisconnected"></a><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Equals(<a href="{{urlRoot}}/api/core/on-disconnected">OnDisconnected</a> other)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/on-disconnected">OnDisconnected</a> other</code> : </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="equals-object"></a><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override bool Equals(object obj)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>object obj</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="gethashcode"></a><b>GetHashCode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L48">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override int GetHashCode()</code></p>






</td>
    </tr>
</table>




