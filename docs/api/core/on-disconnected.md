
# OnDisconnected Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L32">Source</a>
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
</ul></nav>

</p>



<p>ECS Component added to the worker entity immediately after disconnecting from SpatialOS </p>



</p>

<b>Inheritance</b>

<code>ISharedComponentData</code>


</p>

<b>Notes</b>

- This is a reactive component and the Improbable.Gdk.Core.CleanReactiveComponentsSystem will remove it at the end of the frame. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ReasonForDisconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Components/WorkerEntityComponents.cs/#L37">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string ReasonForDisconnect</code></p>
The reported reason for disconnecting 

</td>
    </tr>
</table>










