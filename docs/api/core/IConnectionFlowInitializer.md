---
title: IConnectionFlowInitializer<TConnectionFlow> Interface
slug: api-core-iconnectionflowinitializer
order: 78
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L9">Source</a></span></p>

</p>


<p>Represents an object which can initialize a connection flow of a certain type. </p>


</p>
<p><b>Type parameters</b></p>

<code>TConnectionFlow</code> : The type of connection flow that this object can initialize.












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="initialize-tconnectionflow"></a><b>Initialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Initialize(TConnectionFlow flow)</code></p>Initializes the flow. Seeds the flow implementation with the data required to successfully connect. </p><b>Parameters</b><ul><li><code>TConnectionFlow flow</code> : The flow object to initialize.</li></ul></td>    </tr></table>



