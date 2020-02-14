
# BuildContext Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/build-system-index">BuildSystem</a>.<a href="{{urlRoot}}/api/build-system/configuration-index">Configuration</a><br/>
GDK package: BuildSystem<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L9">Source</a>
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
<li><a href="#static-methods">Static Methods</a>
</ul></nav>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workertype"></a><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="buildtargetconfig"></a><b>BuildTargetConfig</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/build-system/configuration/build-target-config">BuildTargetConfig</a> BuildTargetConfig</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="buildenvironment"></a><b>BuildEnvironment</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/build-system/configuration/build-environment">BuildEnvironment</a> BuildEnvironment</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="scriptingimplementation"></a><b>ScriptingImplementation</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ScriptingImplementation ScriptingImplementation</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="iossdkversion"></a><b>IOSSdkVersion</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> iOSSdkVersion? IOSSdkVersion</code></p>


</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getbuildcontexts-ienumerable-string-buildenvironment-scriptingimplementation-icollection-buildtarget-iossdkversion"></a><b>GetBuildContexts</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L17">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>List&lt;<a href="{{urlRoot}}/api/build-system/configuration/build-context">BuildContext</a>&gt; GetBuildContexts(IEnumerable&lt;string&gt; wantedWorkerTypes, <a href="{{urlRoot}}/api/build-system/configuration/build-environment">BuildEnvironment</a> buildEnvironment, ScriptingImplementation? scriptImplementation = null, ICollection&lt;BuildTarget&gt; buildTargetFilter = null, iOSSdkVersion? iosSdkVersion = null)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>IEnumerable&lt;string&gt; wantedWorkerTypes</code> : </li>
<li><code><a href="{{urlRoot}}/api/build-system/configuration/build-environment">BuildEnvironment</a> buildEnvironment</code> : </li>
<li><code>ScriptingImplementation? scriptImplementation</code> : </li>
<li><code>ICollection&lt;BuildTarget&gt; buildTargetFilter</code> : </li>
<li><code>iOSSdkVersion? iosSdkVersion</code> : </li>
</ul>





</td>
    </tr>
</table>







