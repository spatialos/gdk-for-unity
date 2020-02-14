---
title: LoggingDispatcher Class
slug: api-core-loggingdispatcher
order: 105
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LoggingDispatcher.cs/#L12">Source</a></span></p>

</p>


<p>Logs to the Unity Console. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.ILogDispatcher](doc:api-core-ilogdispatcher)</code>


</p>
<p><b>Notes</b></p>

- Forwards logs to UnityEngine.Debug.unityLogger. 







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worker"></a><b>Worker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LoggingDispatcher.cs/#L14">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> [Worker](doc:api-core-worker) Worker { get; set; }</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LoggingDispatcher.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> string WorkerType { get; set; }</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="handlelog-logtype-logevent"></a><b>HandleLog</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LoggingDispatcher.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void HandleLog(LogType type, [LogEvent](doc:api-core-logevent) logEvent)</code></p>Log locally to the console. </p><b>Parameters</b><ul><li><code>LogType type</code> : The type of the log.</li><li><code>[LogEvent](doc:api-core-logevent) logEvent</code> : A [LogEvent](doc:api-core-logevent) instance.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LoggingDispatcher.cs/#L41">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Dispose()</code></p></td>    </tr></table>



