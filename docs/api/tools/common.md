
# Common Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/tools-index">Tools</a><br/>
GDK package: Tools<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L15">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#const-fields">Const Fields</a>
<li><a href="#static-fields">Static Fields</a>
<li><a href="#static-properties">Static Properties</a>
<li><a href="#static-methods">Static Methods</a>
</ul></nav>

</p>



<p>Catch-all class for common helpers and utilities. </p>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Const Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ProductName</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>const string ProductName = &quot;SpatialOS for Unity&quot;</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>PackagesDir</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>const string PackagesDir = &quot;Packages&quot;</code></p>


</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SpatialProjectRootDir</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L27">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string SpatialProjectRootDir</code></p>
The absolute path to the root folder of the SpatialOS project. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SpatialBinary</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string SpatialBinary</code></p>
The absolute path to the  binary, or the empty string if it doesn't exist. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>DotNetBinary</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L34">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string DotNetBinary</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ManifestPath</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L39">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string ManifestPath</code></p>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>CoreSdkVersion</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string CoreSdkVersion { get; }</code></p>
The version of the CoreSdk the GDK is pinned to. Modify the core-sdk.version file in this source file's directory to change the version. 


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetPackagePath</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L67">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetPackagePath(string packageName)</code></p>
Finds the "file:" reference path from the package manifest. 


</p>

<b>Parameters</b>

<ul>
<li><code>string packageName</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CheckDependencies</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.tools/Common.cs/#L173">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool CheckDependencies()</code></p>
Checks whether  and  exist on the PATH. 





</td>
    </tr>
</table>







