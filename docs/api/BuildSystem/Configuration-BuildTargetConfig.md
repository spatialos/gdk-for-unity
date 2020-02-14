---
title: Configuration.BuildTargetConfig Struct
slug: api-buildsystem-configuration-buildtargetconfig
order: 4
---

<p><b>Namespace:</b> Improbable.Gdk.BuildSystem<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildTargetConfig.cs/#L10">Source</a></span></p>

</p>


<p>Build options for a particular build target. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="options"></a><b>Options</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildTargetConfig.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>BuildOptions Options</code></p>The options to apply when the target is built. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="target"></a><b>Target</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildTargetConfig.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>BuildTarget Target</code></p>The target to build. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="enabled"></a><b>Enabled</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildTargetConfig.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Enabled</code></p>Should this target be built? </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="required"></a><b>Required</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildTargetConfig.cs/#L31">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Required</code></p>Is this build target required? If a required target cannot be built, it will be treated as a failure. </td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="buildtargetconfig-buildtarget-buildoptions-bool-bool"></a><b>BuildTargetConfig</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.buildsystem/Configuration/BuildTargetConfig.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> BuildTargetConfig(BuildTarget target, BuildOptions options, bool enabled, bool required)</code></p>Creates a new instance of a build target and its options. </p><b>Parameters</b><ul><li><code>BuildTarget target</code> : </li><li><code>BuildOptions options</code> : </li><li><code>bool enabled</code> : </li><li><code>bool required</code> : </li></ul></td>    </tr></table>




