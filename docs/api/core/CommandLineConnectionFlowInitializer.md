---
title: CommandLineConnectionFlowInitializer Class
slug: api-core-commandlineconnectionflowinitializer
order: 8
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L24">Source</a></span></p>

</p>


<p>Represents an object which can initialize the [ReceptionistFlow](doc:api-core-receptionistflow), [LocatorFlow](doc:api-core-locatorflow), and [LocatorFlow](doc:api-core-locatorflow) connection flows from the command line. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IConnectionFlowInitializer<ReceptionistFlow>](doc:api-core-iconnectionflowinitializer)</code>
<code>[Improbable.Gdk.Core.IConnectionFlowInitializer<LocatorFlow>](doc:api-core-iconnectionflowinitializer)</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandlineconnectionflowinitializer"></a><b>CommandLineConnectionFlowInitializer</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L28">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> CommandLineConnectionFlowInitializer()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getconnectionservice"></a><b>GetConnectionService</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L42">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[ConnectionService](doc:api-core-connectionservice) GetConnectionService()</code></p>Gets the connection service to use based on command line arguments. </p><b>Returns:</b></br>The connection service to use.</td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="initialize-receptionistflow"></a><b>Initialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L52">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Initialize([ReceptionistFlow](doc:api-core-receptionistflow) receptionist)</code></p></p><b>Parameters</b><ul><li><code>[ReceptionistFlow](doc:api-core-receptionistflow) receptionist</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="initialize-locatorflow"></a><b>Initialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L61">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Initialize([LocatorFlow](doc:api-core-locatorflow) locator)</code></p></p><b>Parameters</b><ul><li><code>[LocatorFlow](doc:api-core-locatorflow) locator</code> : </li></ul></td>    </tr></table>



