---
title: WorkerConnector Class
slug: api-core-workerconnector
order: 150
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L19">Source</a></span></p>

</p>


<p>Connect workers via Monobehaviours. </p>



</p>
<p><b>Inheritance</b></p>

<code>MonoBehaviour</code>
<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="maxconnectionattempts"></a><b>MaxConnectionAttempts</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>int MaxConnectionAttempts</code></p>The number of connection attempts before giving up. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worker"></a><b>Worker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L32">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[WorkerInWorld](doc:api-core-workerinworld) Worker</code></p>Represents a SpatialOS worker. </p><b>Notes:</b><ul><li>Only safe to access after the connection has succeeded. </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onworkercreationfinished"></a><b>OnWorkerCreationFinished</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L39">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Action&lt;[Worker](doc:api-core-worker)&gt; OnWorkerCreationFinished {  }</code></p>An event that triggers when the worker has been fully created. </td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createnewworkerid-string"></a><b>CreateNewWorkerId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L205">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string CreateNewWorkerId(string workerType)</code></p></p><b>Parameters</b><ul><li><code>string workerType</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onapplicationquit"></a><b>OnApplicationQuit</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L55">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnApplicationQuit()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="ondestroy"></a><b>OnDestroy</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnDestroy()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="connect-iconnectionhandlerbuilder-ilogdispatcher"></a><b>Connect</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L71">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task Connect([IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder) builder, [ILogDispatcher](doc:api-core-ilogdispatcher) logger)</code></p>Asynchronously connects a worker to the SpatialOS runtime. </p><b>Parameters</b><ul><li><code>[IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder) builder</code> : Describes how to create a [IConnectionHandler](doc:api-core-iconnectionhandler) for this worker.</li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logger</code> : The logger for the worker to use.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="handleworkerconnectionestablished"></a><b>HandleWorkerConnectionEstablished</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L153">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void HandleWorkerConnectionEstablished()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="handleworkerconnectionfailure-string"></a><b>HandleWorkerConnectionFailure</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L157">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void HandleWorkerConnectionFailure(string errorMessage)</code></p></p><b>Parameters</b><ul><li><code>string errorMessage</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createconnectionparameters-string-iconnectionparameterinitializer"></a><b>CreateConnectionParameters</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L210">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ConnectionParameters CreateConnectionParameters(string workerType, [IConnectionParameterInitializer](doc:api-core-iconnectionparameterinitializer) initializer = null)</code></p></p><b>Parameters</b><ul><li><code>string workerType</code> : </li><li><code>[IConnectionParameterInitializer](doc:api-core-iconnectionparameterinitializer) initializer</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="deferreddisposeworker"></a><b>DeferredDisposeWorker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L240">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>IEnumerator DeferredDisposeWorker()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L248">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Dispose()</code></p></td>    </tr></table>



