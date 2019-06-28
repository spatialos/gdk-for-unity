
# CustomSpatialOSSendSystem&lt;T&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CustomSpatialOSSendSystem.cs/#L15">Source</a>
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
<li><a href="#overrides">Overrides</a>
</ul></nav>

</p>



<p>Base class for custom replication. </p>


</p>

<b>Type parameters</b>

<code>T</code> : The SpatialOS component to define custom replication logic for.


</p>

<b>Inheritance</b>

<code>ComponentSystem</code>


</p>

<b>Notes</b>

- Adding a system that inherits from this class will disable automatic replication for component type T. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerSystem</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CustomSpatialOSSendSystem.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/worker-system">WorkerSystem</a> WorkerSystem</code></p>


</td>
    </tr>
</table>









</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnCreate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CustomSpatialOSSendSystem.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnCreate()</code></p>






</td>
    </tr>
</table>




