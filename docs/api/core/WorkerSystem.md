---
title: WorkerSystem Class
slug: api-core-workersystem
order: 156
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L14">Source</a></span></p>

</p>


<p>A SpatialOS worker instance. </p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workerentity"></a><b>WorkerEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L19">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Entity WorkerEntity</code></p>An ECS entity that represents the [Worker](doc:api-core-worker). </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="logdispatcher"></a><b>LogDispatcher</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L21">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [ILogDispatcher](doc:api-core-ilogdispatcher) LogDispatcher</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string WorkerType</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workerid"></a><b>WorkerId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L23">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string WorkerId</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="origin"></a><b>Origin</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Vector3 Origin</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isconnected"></a><b>IsConnected</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L29">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsConnected</code></p>Denotes whether the underlying worker is connected or not. </td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workersystem-workerinworld"></a><b>WorkerSystem</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L40">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> WorkerSystem([WorkerInWorld](doc:api-core-workerinworld) worker)</code></p></p><b>Parameters</b><ul><li><code>[WorkerInWorld](doc:api-core-workerinworld) worker</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetentity-entityid-out-entity"></a><b>TryGetEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L62">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetEntity([EntityId](doc:api-core-entityid) entityId, out Entity entity)</code></p>Attempts to find an ECS entity associated with a SpatialOS entity ID. </p><b>Returns:</b></br>True, if an ECS entity associated with the SpatialOS entity ID was found, false otherwise. </p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : The SpatialOS entity ID.</li><li><code>out Entity entity</code> : When this method returns, contains the ECS entity associated with the SpatialOS entity ID if one was found, else the default value for Entity. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hasentity-entityid"></a><b>HasEntity</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L72">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool HasEntity([EntityId](doc:api-core-entityid) entityId)</code></p>Checks whether a SpatialOS entity is checked out on this worker. </p><b>Returns:</b></br>True, if the SpatialOS entity is checked out on this worker, false otherwise.</p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) entityId</code> : The SpatialOS entity ID to check for.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendlogmessage-string-string-loglevel-entityid"></a><b>SendLogMessage</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L77">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendLogMessage(string message, string loggerName, LogLevel logLevel, [EntityId](doc:api-core-entityid)? entityId)</code></p></p><b>Parameters</b><ul><li><code>string message</code> : </li><li><code>string loggerName</code> : </li><li><code>LogLevel logLevel</code> : </li><li><code>[EntityId](doc:api-core-entityid)? entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendmetrics-metrics"></a><b>SendMetrics</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L82">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendMetrics(Metrics metrics)</code></p></p><b>Parameters</b><ul><li><code>Metrics metrics</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L97">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L106">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


