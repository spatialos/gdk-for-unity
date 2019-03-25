
# AuthorityLossImminent&lt;T&gt; Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/reactive-components-index">ReactiveComponents</a><br/>
GDK package: ReactiveComponents<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/Authority/AuthorityComponents.cs/#L30">Source</a>
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



<p>ECS component denotes that this worker will lose authority over T imminently. If AcknowledgeAuthorityLoss is set then authority handover will complete before the handover timeout. </p>


</p>

<b>Type parameters</b>

<code>T</code> : The SpatialOS component.


</p>

<b>Inheritance</b>

<code>IComponentData</code>


</p>

<b>Notes</b>

- Note that this worker may still be authoritative over this component. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>AcknowledgeAuthorityLoss</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/Authority/AuthorityComponents.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> AcknowledgeAuthorityLoss</code></p>


</td>
    </tr>
</table>










