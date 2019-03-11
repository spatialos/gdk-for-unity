
# AuthorityChanges&lt;T&gt; Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/reactive-components-index">ReactiveComponents</a><br/>
GDK package: ReactiveComponents<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/Authority/AuthorityChanges.cs/#L17">Source</a>
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
<li><a href="#properties">Properties</a>
</ul></nav>

</p>



<p>ECS Component stores an ordered list of authority changes. </p>


</p>

<b>Type parameters</b>

<code>T</code> : The SpatialOS component which had authority changes.


</p>

<b>Inheritance</b>

<code>IComponentData</code>


</p>

<b>Notes</b>

- This component is created during the SpatialOSReceiveSystem and populated with all the authority changes received in a single update. This component will be removed during the <a href="{{urlRoot}}/api/reactive-components/clean-reactive-components-system">CleanReactiveComponentsSystem</a>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Handle</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/Authority/AuthorityChanges.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint Handle</code></p>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Changes</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/ReactiveComponents/Authority/AuthorityChanges.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> List&lt;Authority&gt; Changes { get; set; }</code></p>



</td>
    </tr>
</table>








