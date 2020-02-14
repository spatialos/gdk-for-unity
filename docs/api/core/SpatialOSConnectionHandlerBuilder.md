---
title: SpatialOSConnectionHandlerBuilder Class
slug: api-core-spatialosconnectionhandlerbuilder
order: 135
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L8">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IConnectionHandlerBuilder](doc:api-core-iconnectionhandlerbuilder)</code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L26">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> string WorkerType { get; }</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setconnectionflow-iconnectionflow"></a><b>SetConnectionFlow</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L38">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[SpatialOSConnectionHandlerBuilder](doc:api-core-spatialosconnectionhandlerbuilder) SetConnectionFlow([IConnectionFlow](doc:api-core-iconnectionflow) flow)</code></p>Sets the connection flow implementation to use when creating the underlying SpatialOS connection. </p><b>Returns:</b></br>Itself</p><b>Parameters</b><ul><li><code>[IConnectionFlow](doc:api-core-iconnectionflow) flow</code> : The connection flow implementation.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setthreadingmode-threadingmode"></a><b>SetThreadingMode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L49">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[SpatialOSConnectionHandlerBuilder](doc:api-core-spatialosconnectionhandlerbuilder) SetThreadingMode(ThreadingMode mode)</code></p>Sets the threading mode for the resultant [IConnectionHandler](doc:api-core-iconnectionhandler). </p><b>Returns:</b></br>Itself</p><b>Parameters</b><ul><li><code>ThreadingMode mode</code> : The desired threading mode.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setconnectionparameters-connectionparameters"></a><b>SetConnectionParameters</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[SpatialOSConnectionHandlerBuilder](doc:api-core-spatialosconnectionhandlerbuilder) SetConnectionParameters(ConnectionParameters parameters)</code></p>Sets the connection parameters to use for the underlying SpatialOS connection. </p><b>Returns:</b></br>Itself</p><b>Parameters</b><ul><li><code>ConnectionParameters parameters</code> : The connection parameters to use.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createasync-cancellationtoken"></a><b>CreateAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L67">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;[IConnectionHandler](doc:api-core-iconnectionhandler)&gt; CreateAsync(CancellationToken? token = null)</code></p>Creates a [IConnectionHandler](doc:api-core-iconnectionhandler) asynchronously. </p><b>Returns:</b></br>A task that represents the asynchronous creation of the connection handler object.</p><b>Parameters</b><ul><li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li></ul></td>    </tr></table>



