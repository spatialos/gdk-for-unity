
# LocalDeployment Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/test-utils-index">TestUtils</a>.<a href="{{urlRoot}}/api/test-utils/editor-index">Editor</a>.<a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager">SpatialdManager</a><br/>
GDK package: TestUtils<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L240">Source</a>
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
</ul></nav>

</p>



<p>Represents a local deployment. </p>



</p>

<b>Inheritance</b>

<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="id"></a><b>Id</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L245">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string Id</code></p>
The ID of this deployment. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="name"></a><b>Name</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L250">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string Name</code></p>
The name of this deployment. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="projectname"></a><b>ProjectName</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L255">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string ProjectName</code></p>
The project that this deployment belongs to. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="tags"></a><b>Tags</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L260">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly List&lt;string&gt; Tags</code></p>
The tags that are present on this deployment. 

</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="adddevlogintag"></a><b>AddDevLoginTag</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L278">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task AddDevLoginTag()</code></p>
Adds the "dev_login" tag to this deployment asynchronously. 
</p><b>Returns:</b></br>A task which represents the underlying operation to add the tag.




</p>

<b>Exceptions:</b>

<ul>
<li><code>InvalidOperationException</code> : Thrown if the operation to set the tag fails.</li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="dispose"></a><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L296">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





