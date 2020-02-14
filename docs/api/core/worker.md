---
title: Worker Class
slug: api-core-worker
order: 149
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L13">Source</a></span></p>

</p>


<p>Represents a SpatialOS worker. </p>



</p>
<p><b>Inheritance</b></p>

<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L18">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string WorkerType</code></p>The type of the worker. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workerid"></a><b>WorkerId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L26">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string WorkerId</code></p>The worker ID. </p><b>Notes:</b><ul><li>Unique for a given SpatialOS deployment. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="attributes"></a><b>Attributes</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L31">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly List&lt;string&gt; Attributes</code></p>The worker attribute list </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="logdispatcher"></a><b>LogDispatcher</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[ILogDispatcher](doc:api-core-ilogdispatcher) LogDispatcher</code></p>The logger for this worker. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isconnected"></a><b>IsConnected</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L41">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsConnected</code></p>Denotes whether this worker is connected or not. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="connectionhandler"></a><b>ConnectionHandler</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[IConnectionHandler](doc:api-core-iconnectionhandler) ConnectionHandler</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createworkerasync-iconnectionhandlerbuilder-string-ilogdispatcher-cancellationtoken"></a><b>CreateWorkerAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L76">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;[Worker](doc:api-core-worker)&gt; CreateWorkerAsync([IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder) connectionHandlerBuilder, string workerType, [ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher, CancellationToken? token = null)</code></p>Creates a [Worker](doc:api-core-worker) object asynchronously. </p><b>Returns:</b></br>A task which represents the asynchronous creation of a worker.</p><b>Parameters</b><ul><li><code>[IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder) connectionHandlerBuilder</code> : A builder which describes how to create the [IConnectionHandler](doc:api-core-iconnectionhandler) for this worker. </li><li><code>string workerType</code> : The type of worker to connect as.</li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher</code> : The logger to use for this worker.</li><li><code>CancellationToken? token</code> : A cancellation token which will cancel this asynchronous operation</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worker-iconnectionhandler-string-ilogdispatcher"></a><b>Worker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L51">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Worker([IConnectionHandler](doc:api-core-iconnectionhandler) connectionHandler, string workerType, [ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher)</code></p></p><b>Parameters</b><ul><li><code>[IConnectionHandler](doc:api-core-iconnectionhandler) connectionHandler</code> : </li><li><code>string workerType</code> : </li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tick"></a><b>Tick</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L87">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Tick()</code></p>Ticks the worker. Fetches all messages received since the last Tick call and applies the diff. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="ensuremessagesflushed-netframestats"></a><b>EnsureMessagesFlushed</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L93">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void EnsureMessagesFlushed([NetFrameStats](doc:api-core-networkstats-netframestats) frameStats)</code></p></p><b>Parameters</b><ul><li><code>[NetFrameStats](doc:api-core-networkstats-netframestats) frameStats</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendlogmessage-loglevel-string-string-entityid"></a><b>SendLogMessage</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L110">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendLogMessage(LogLevel logLevel, string message, string loggerName, [EntityId](doc:api-core-entityid)? entityId)</code></p>Sends a log message to SpatialOS from this worker. </p><b>Parameters</b><ul><li><code>LogLevel logLevel</code> : The log verbosity level.</li><li><code>string message</code> : The log message.</li><li><code>string loggerName</code> : A name for the sender of the log.</li><li><code>[EntityId](doc:api-core-entityid)? entityId</code> : The [EntityId](doc:api-core-entityid) to associate with the log message. Set to null for no [EntityId](doc:api-core-entityid). </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getworkerflag-string"></a><b>GetWorkerFlag</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L120">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetWorkerFlag(string key)</code></p>Gets the value for a given worker flag. </p><b>Returns:</b></br>The value of the flag, if it exists, null otherwise.</p><b>Parameters</b><ul><li><code>string key</code> : The key of the worker flag.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/Worker.cs/#L125">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Dispose()</code></p></td>    </tr></table>



