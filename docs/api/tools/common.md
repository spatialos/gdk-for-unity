---
title: Common Class
slug: api-tools-common
order: 1
---

<p><b>Namespace:</b> Improbable.Gdk.Tools<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L14">Source</a></span></p>

</p>


<p>Catch-all class for common helpers and utilities. </p>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Const Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="productname"></a><b>ProductName</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L38">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>const string ProductName = &quot;SpatialOS for Unity&quot;</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="spatialprojectrootdir"></a><b>SpatialProjectRootDir</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string SpatialProjectRootDir</code></p>The absolute path to the root folder of the SpatialOS project. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="buildscratchdirectory"></a><b>BuildScratchDirectory</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string BuildScratchDirectory</code></p>The path to the Unity project build directory that worker build artifacts are placed into. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="spatialbinary"></a><b>SpatialBinary</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L31">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string SpatialBinary</code></p>The absolute path to the `spatial` binary, or an empty string if it doesn't exist. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dotnetbinary"></a><b>DotNetBinary</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string DotNetBinary</code></p>The absolute path to the `dotnet` binary, or an empty string if it doesn't exist. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="runtimeipeditorprefkey"></a><b>RuntimeIpEditorPrefKey</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string RuntimeIpEditorPrefKey</code></p></td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getpackagepath-string"></a><b>GetPackagePath</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L54">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetPackagePath(string packageName)</code></p>Finds the path for a given package referenced directly in the manifest.json, or indirectly referenced as a package dependency. </p><b>Parameters</b><ul><li><code>string packageName</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="checkdependencies"></a><b>CheckDependencies</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/Common.cs/#L149">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool CheckDependencies()</code></p>Checks whether `dotnet` and `spatial` exist on the PATH. </td>    </tr></table>





