---
title: IConnectionFlow Interface
slug: api-core-iconnectionflow
order: 77
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L13">Source</a></span></p>

</p>


<p>Represents an implementation of a flow to connect to SpatialOS. </p>













</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createasync-connectionparameters-cancellationtoken"></a><b>CreateAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L21">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Task&lt;Connection&gt; CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)</code></p>Creates a Connection asynchronously. </p><b>Returns:</b></br>A task that represents the asynchronous creation of the Connection object.</p><b>Parameters</b><ul><li><code>ConnectionParameters parameters</code> : The connection parameters to use for the connection.</li><li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li></ul></td>    </tr></table>



