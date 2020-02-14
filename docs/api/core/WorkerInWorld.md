---
title: WorkerInWorld Class
slug: api-core-workerinworld
order: 155
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L13">Source</a></span></p>

</p>


<p>Represents a SpatialOS worker that is coupled with an ECS World. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.Worker](doc:api-core-worker)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="origin"></a><b>Origin</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L18">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Vector3 Origin</code></p>The origin of the worker in global Unity space. </td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="world"></a><b>World</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L23">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> World World { get; }</code></p>The ECS world associated with this worker. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="ondisconnect"></a><b>OnDisconnect</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L28">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Action&lt;string&gt; OnDisconnect {  }</code></p>An event that triggers when the worker is disconnected. </td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createworkerinworldasync-iconnectionhandlerbuilder-string-ilogdispatcher-vector3-cancellationtoken"></a><b>CreateWorkerInWorldAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;[WorkerInWorld](doc:api-core-workerinworld)&gt; CreateWorkerInWorldAsync([IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder) connectionHandlerBuilder, string workerType, [ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher, Vector3 origin, CancellationToken? token = null)</code></p>Creates a [WorkerInWorld](doc:api-core-workerinworld) object asynchronously. </p><b>Returns:</b></br>A task which represents the asynchronous creation of a worker.</p><b>Parameters</b><ul><li><code>[IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder) connectionHandlerBuilder</code> : A builder which describes how to create the [IConnectionHandler](doc:api-core-iconnectionhandler) for this worker. </li><li><code>string workerType</code> : The type of worker to connect as.</li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher</code> : The logger to use for this worker.</li><li><code>Vector3 origin</code> : </li><li><code>CancellationToken? token</code> : A cancellation token which will cancel this asynchronous operation</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workerinworld-iconnectionhandler-string-ilogdispatcher-vector3"></a><b>WorkerInWorld</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L37">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> WorkerInWorld([IConnectionHandler](doc:api-core-iconnectionhandler) connectionHandler, string workerType, [ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher, Vector3 origin)</code></p></p><b>Parameters</b><ul><li><code>[IConnectionHandler](doc:api-core-iconnectionhandler) connectionHandler</code> : </li><li><code>string workerType</code> : </li><li><code>[ILogDispatcher](doc:api-core-ilogdispatcher) logDispatcher</code> : </li><li><code>Vector3 origin</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L88">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void Dispose()</code></p></td>    </tr></table>


