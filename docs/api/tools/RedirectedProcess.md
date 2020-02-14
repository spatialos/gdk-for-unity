---
title: RedirectedProcess Class
slug: api-tools-redirectedprocess
order: 10
---

<p><b>Namespace:</b> Improbable.Gdk.Tools<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L52">Source</a></span></p>

</p>


<p>Runs a windowless process. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="command-string"></a><b>Command</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L68">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) Command(string command)</code></p>Creates the redirected process for the command. </p><b>Parameters</b><ul><li><code>string command</code> : The filename to run.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="spatial-params-string"></a><b>Spatial</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L74">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) Spatial(params string[] args)</code></p></p><b>Parameters</b><ul><li><code>params string[] args</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="withargs-params-string"></a><b>WithArgs</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L93">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) WithArgs(params string[] arguments)</code></p>Adds arguments to process command call. </p><b>Parameters</b><ul><li><code>params string[] arguments</code> : Parameters that will be passed to the command.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="indirectory-string"></a><b>InDirectory</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L103">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) InDirectory(string directory)</code></p>Sets which directory run the process in. </p><b>Parameters</b><ul><li><code>string directory</code> : Working directory of the process.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addoutputprocessing-action-string"></a><b>AddOutputProcessing</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L117">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) AddOutputProcessing(Action&lt;string&gt; outputProcessor)</code></p>Adds custom processing for regular output of process. </p><b>Parameters</b><ul><li><code>Action&lt;string&gt; outputProcessor</code> : Processing action for regular output.</li></ul></p><b>Notes:</b><ul><li>The outputProcessor callback will be ran on a different thread to the one which registered it. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="adderrorprocessing-action-string"></a><b>AddErrorProcessing</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L131">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) AddErrorProcessing(Action&lt;string&gt; errorProcessor)</code></p>Adds custom processing for error output of process. </p><b>Parameters</b><ul><li><code>Action&lt;string&gt; errorProcessor</code> : Processing action for error output.</li></ul></p><b>Notes:</b><ul><li>The errorProcessor callback will be ran on a different thread to the one which registered it. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="redirectoutputoptions-outputredirectbehaviour"></a><b>RedirectOutputOptions</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L141">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcess](doc:api-tools-redirectedprocess) RedirectOutputOptions([OutputRedirectBehaviour](doc:api-tools-outputredirectbehaviour) redirectBehaviour)</code></p>Adds custom processing for error output of process. </p><b>Parameters</b><ul><li><code>[OutputRedirectBehaviour](doc:api-tools-outputredirectbehaviour) redirectBehaviour</code> : Options for redirecting process output to Debug.Log().</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="run"></a><b>Run</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L150">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[RedirectedProcessResult](doc:api-tools-redirectedprocessresult) Run()</code></p>Runs the redirected process and waits for it to return. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="runasync-cancellationtoken"></a><b>RunAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/RedirectedProcess.cs/#L210">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Task&lt;[RedirectedProcessResult](doc:api-tools-redirectedprocessresult)&gt; RunAsync(CancellationToken token = default)</code></p>Runs the redirected process and returns a task which can be waited on. </p><b>Returns:</b></br>A task which would return the exit code and output.</p><b>Parameters</b><ul><li><code>CancellationToken token</code> : A cancellation token which can be used for cancelling the underlying process. Default is CancellationToken.None.</li></ul></td>    </tr></table>



