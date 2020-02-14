---
title: Editor.SpatialdManager.LocalDeployment Struct
slug: api-testutils-editor-spatialdmanager-localdeployment
order: 4
---

<p><b>Namespace:</b> Improbable.Gdk.TestUtils<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L240">Source</a></span></p>

</p>


<p>Represents a local deployment. </p>



</p>
<p><b>Inheritance</b></p>

<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="id"></a><b>Id</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L245">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string Id</code></p>The ID of this deployment. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="name"></a><b>Name</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L250">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string Name</code></p>The name of this deployment. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="projectname"></a><b>ProjectName</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L255">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string ProjectName</code></p>The project that this deployment belongs to. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tags"></a><b>Tags</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L260">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly List&lt;string&gt; Tags</code></p>The tags that are present on this deployment. </td>    </tr></table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="adddevlogintag"></a><b>AddDevLoginTag</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L278">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task AddDevLoginTag()</code></p>Adds the "dev_login" tag to this deployment asynchronously. </p><b>Returns:</b></br>A task which represents the underlying operation to add the tag.</p><b>Exceptions:</b><ul><li><code>InvalidOperationException</code> : Thrown if the operation to set the tag fails.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L296">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Dispose()</code></p></td>    </tr></table>



