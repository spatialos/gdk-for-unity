---
title: IConnectionHandlerBuilder Interface
slug: api-core-iconnectionhandlerbuilder
order: 80
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandlerBuilder.cs/#L10">Source</a></span></p>

</p>


<p>Intermediate object for building a [IConnectionHandler](doc:api-core-iconnectionhandler) object. </p>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandlerBuilder.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> string WorkerType { get; }</code></p>The type of worker that the resulting connection handler will represent. </td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createasync-cancellationtoken"></a><b>CreateAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandlerBuilder.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Task&lt;[IConnectionHandler](doc:api-core-iconnectionhandler)&gt; CreateAsync(CancellationToken? token = null)</code></p>Creates a [IConnectionHandler](doc:api-core-iconnectionhandler) asynchronously. </p><b>Returns:</b></br>A task that represents the asynchronous creation of the connection handler object.</p><b>Parameters</b><ul><li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li></ul></td>    </tr></table>



