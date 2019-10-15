
# SpatialdManager Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/test-utils-index">TestUtils</a>.<a href="{{urlRoot}}/api/test-utils/editor-index">Editor</a><br/>
GDK package: TestUtils<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L15">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Manages the lifecycle of SpatialD and provides methods to interact with it. </p>



</p>

<b>Inheritance</b>

<code>IDisposable</code>



</p>

<b>Child types</b>

<table>
<tr>
<td style="padding: 14px; border: none; width: 15ch"><a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager/local-deployment">LocalDeployment</a></td>
<td style="padding: 14px; border: none;">Represents a local deployment. </td>
</tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="start"></a><b>Start</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L68">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager">SpatialdManager</a>&gt; Start()</code></p>
Starts SpatialD. 



</p>

<b>Notes:</b>

<ul>
<li>If SpatialD is already running, it will stop that instance and start a new one. </li>
</ul>




</p>

<b>Exceptions:</b>

<ul>
<li><code>Exception</code> : Thrown if this fails to start SpatialD.</li>
</ul>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="startlocaldeployment-string-string-string"></a><b>StartLocalDeployment</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L111">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager/local-deployment">LocalDeployment</a>&gt; StartLocalDeployment(string name, string deploymentJsonPath, string snapshotFileName = &quot;default.snapshot&quot;)</code></p>
Starts a local deployment asynchronously. 
</p><b>Returns:</b></br>A task which represents the deployment that was started.

</p>

<b>Parameters</b>

<ul>
<li><code>string name</code> : The name for the local deployment.</li>
<li><code>string deploymentJsonPath</code> : The path to the launch configuration JSON relative to the root of the SpatialOS project. </li>
<li><code>string snapshotFileName</code> : The name of the snapshot to use for this deployment. Must be in the snapshots directory of your SpatialOS project. </li>
</ul>





</p>

<b>Exceptions:</b>

<ul>
<li><code>ArgumentException</code> : Thrown if deploymentJsonPath does not exist.</li>
<li><code>Exception</code> : Thrown if the deployment fails to start.</li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="stoplocaldeployment-localdeployment"></a><b>StopLocalDeployment</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L166">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task StopLocalDeployment(<a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager/local-deployment">LocalDeployment</a> deployment)</code></p>
Stops a local deployment asynchronously. 
</p><b>Returns:</b></br>A task which represents the operation to stop the deployment.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager/local-deployment">LocalDeployment</a> deployment</code> : The deployment to stop.</li>
</ul>





</p>

<b>Exceptions:</b>

<ul>
<li><code>Exception</code> : Thrown if the deployment fails to be stopped.</li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getrunningdeployments"></a><b>GetRunningDeployments</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L185">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;List&lt;<a href="{{urlRoot}}/api/test-utils/editor/spatiald-manager/local-deployment">LocalDeployment</a>&gt;&gt; GetRunningDeployments()</code></p>
Gets the details of currently running deployments asynchronously. 
</p><b>Returns:</b></br>A task which represents list of




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="dispose"></a><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.testutils/Editor/SpatialdManager.cs/#L222">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





