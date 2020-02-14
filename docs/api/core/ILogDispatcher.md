---
title: ILogDispatcher Interface
slug: api-core-ilogdispatcher
order: 91
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L11">Source</a></span></p>

</p>


<p>The [ILogDispatcher](doc:api-core-ilogdispatcher) interface is used to implement different types of loggers. By default, the [ILogDispatcher](doc:api-core-ilogdispatcher) supports structured logging. </p>



</p>
<p><b>Inheritance</b></p>

<code>IDisposable</code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worker"></a><b>Worker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L16">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> [Worker](doc:api-core-worker) Worker { get; set; }</code></p>The associated GDK [Worker](doc:api-core-worker). </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L21">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> string WorkerType { get; set; }</code></p>The worker type associated with this logger. </td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="handlelog-logtype-logevent"></a><b>HandleLog</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L23">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void HandleLog(LogType type, [LogEvent](doc:api-core-logevent) logEvent)</code></p></p><b>Parameters</b><ul><li><code>LogType type</code> : </li><li><code>[LogEvent](doc:api-core-logevent) logEvent</code> : </li></ul></td>    </tr></table>



