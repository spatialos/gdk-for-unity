---
title: Editor.SpatialdManager Class
slug: api-testutils-editor-spatialdmanager
order: 3
---

<p><b>Namespace:</b> Improbable.Gdk.TestUtils<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L15">Source</a></span></p>

</p>


<p>Manages the lifecycle of SpatialD and provides methods to interact with it. </p>



</p>
<p><b>Inheritance</b></p>

<code>IDisposable</code>



</p>
<p><b>Child types</b></p>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[LocalDeployment](doc:api-testutils-editor-spatialdmanager-localdeployment)",
    "0-1": "Represents a local deployment. "
  },
  "cols": "2",
  "rows": "1"
}
[/block]








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="start"></a><b>Start</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L68">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;[SpatialdManager](doc:api-testutils-editor-spatialdmanager)&gt; Start()</code></p>Starts SpatialD. </p><b>Notes:</b><ul><li>If SpatialD is already running, it will stop that instance and start a new one. </li></ul></p><b>Exceptions:</b><ul><li><code>Exception</code> : Thrown if this fails to start SpatialD.</li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="startlocaldeployment-string-string-string"></a><b>StartLocalDeployment</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L111">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;[LocalDeployment](doc:api-testutils-editor-spatialdmanager-localdeployment)&gt; StartLocalDeployment(string name, string deploymentJsonPath, string snapshotFileName = &quot;default.snapshot&quot;)</code></p>Starts a local deployment asynchronously. </p><b>Returns:</b></br>A task which represents the deployment that was started.</p><b>Parameters</b><ul><li><code>string name</code> : The name for the local deployment.</li><li><code>string deploymentJsonPath</code> : The path to the launch configuration JSON relative to the root of the SpatialOS project. </li><li><code>string snapshotFileName</code> : The name of the snapshot to use for this deployment. Must be in the snapshots directory of your SpatialOS project. </li></ul></p><b>Exceptions:</b><ul><li><code>ArgumentException</code> : Thrown if deploymentJsonPath does not exist.</li><li><code>Exception</code> : Thrown if the deployment fails to start.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="stoplocaldeployment-localdeployment"></a><b>StopLocalDeployment</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L166">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task StopLocalDeployment([LocalDeployment](doc:api-testutils-editor-spatialdmanager-localdeployment) deployment)</code></p>Stops a local deployment asynchronously. </p><b>Returns:</b></br>A task which represents the operation to stop the deployment.</p><b>Parameters</b><ul><li><code>[LocalDeployment](doc:api-testutils-editor-spatialdmanager-localdeployment) deployment</code> : The deployment to stop.</li></ul></p><b>Exceptions:</b><ul><li><code>Exception</code> : Thrown if the deployment fails to be stopped.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getrunningdeployments"></a><b>GetRunningDeployments</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L185">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;List&lt;[LocalDeployment](doc:api-testutils-editor-spatialdmanager-localdeployment)&gt;&gt; GetRunningDeployments()</code></p>Gets the details of currently running deployments asynchronously. </p><b>Returns:</b></br>A task which represents list of</td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L222">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Dispose()</code></p></td>    </tr></table>



