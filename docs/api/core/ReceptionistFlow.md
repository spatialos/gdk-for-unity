---
title: ReceptionistFlow Class
slug: api-core-receptionistflow
order: 126
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L27">Source</a></span></p>

</p>


<p>Represents the Receptionist connection flow. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IConnectionFlow](doc:api-core-iconnectionflow)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="receptionisthost"></a><b>ReceptionistHost</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L32">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string ReceptionistHost</code></p>The IP address of the Receptionist to use when connecting. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="receptionistport"></a><b>ReceptionistPort</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L37">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ushort ReceptionistPort</code></p>The port of the Receptionist to use when connecting. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workerid"></a><b>WorkerId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string WorkerId</code></p>The worker ID to use for the worker connection that will be created when CreateAsync is called. </td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="receptionistflow-string-iconnectionflowinitializer-receptionistflow"></a><b>ReceptionistFlow</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> ReceptionistFlow(string workerId, [IConnectionFlowInitializer](doc:api-core-iconnectionflowinitializer)&lt;[ReceptionistFlow](doc:api-core-receptionistflow)&gt; initializer = null)</code></p>Initializes a new instance of the [ReceptionistFlow](doc:api-core-receptionistflow) class. </p><b>Parameters</b><ul><li><code>string workerId</code> : The worker ID to use for the worker connection.</li><li><code>[IConnectionFlowInitializer](doc:api-core-iconnectionflowinitializer)&lt;[ReceptionistFlow](doc:api-core-receptionistflow)&gt; initializer</code> : Optional. An initializer to seed the data required to connect via the Receptionist flow.</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createasync-connectionparameters-cancellationtoken"></a><b>CreateAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L56">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;Connection&gt; CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)</code></p>Creates a Connection asynchronously. </p><b>Returns:</b></br>A task that represents the asynchronous creation of the Connection object.</p><b>Parameters</b><ul><li><code>ConnectionParameters parameters</code> : The connection parameters to use for the connection.</li><li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li></ul></td>    </tr></table>



