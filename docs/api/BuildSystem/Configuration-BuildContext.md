---
title: Configuration.BuildContext Struct
slug: api-buildsystem-configuration-buildcontext
order: 2
---

<p><b>Namespace:</b> Improbable.Gdk.BuildSystem<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L9">Source</a></span></p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L11">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string WorkerType</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="buildtargetconfig"></a><b>BuildTargetConfig</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L12">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[BuildTargetConfig](doc:api-buildsystem-configuration-buildtargetconfig) BuildTargetConfig</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="buildenvironment"></a><b>BuildEnvironment</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L13">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[BuildEnvironment](doc:api-buildsystem-configuration-buildenvironment) BuildEnvironment</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="scriptingimplementation"></a><b>ScriptingImplementation</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ScriptingImplementation ScriptingImplementation</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="iossdkversion"></a><b>IOSSdkVersion</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>iOSSdkVersion? IOSSdkVersion</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getbuildcontexts-ienumerable-string-buildenvironment-scriptingimplementation-icollection-buildtarget-iossdkversion"></a><b>GetBuildContexts</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildContext.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;[BuildContext](doc:api-buildsystem-configuration-buildcontext)&gt; GetBuildContexts(IEnumerable&lt;string&gt; wantedWorkerTypes, [BuildEnvironment](doc:api-buildsystem-configuration-buildenvironment) buildEnvironment, ScriptingImplementation? scriptImplementation = null, ICollection&lt;BuildTarget&gt; buildTargetFilter = null, iOSSdkVersion? iosSdkVersion = null)</code></p></p><b>Parameters</b><ul><li><code>IEnumerable&lt;string&gt; wantedWorkerTypes</code> : </li><li><code>[BuildEnvironment](doc:api-buildsystem-configuration-buildenvironment) buildEnvironment</code> : </li><li><code>ScriptingImplementation? scriptImplementation</code> : </li><li><code>ICollection&lt;BuildTarget&gt; buildTargetFilter</code> : </li><li><code>iOSSdkVersion? iosSdkVersion</code> : </li></ul></td>    </tr></table>





