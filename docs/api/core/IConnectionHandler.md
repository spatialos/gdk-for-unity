---
title: IConnectionHandler Interface
slug: api-core-iconnectionhandler
order: 79
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L11">Source</a></span></p>

</p>


<p>Represents a handler for a SpatialOS connection. </p>



</p>
<p><b>Inheritance</b></p>

<code>IDisposable</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getworkerid"></a><b>GetWorkerId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetWorkerId()</code></p>Gets the worker ID for this worker. </p><b>Returns:</b></br>The worker ID.</td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getworkerattributes"></a><b>GetWorkerAttributes</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L23">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;string&gt; GetWorkerAttributes()</code></p>Gets the worker attributes for this worker. </p><b>Returns:</b></br>The list of worker attributes.</td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getmessagesreceived-ref-viewdiff"></a><b>GetMessagesReceived</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void GetMessagesReceived(ref [ViewDiff](doc:api-core-viewdiff) viewDiff)</code></p>Populates the [ViewDiff](doc:api-core-viewdiff) object using the messages received since the last time this was called. </p><b>Parameters</b><ul><li><code>ref [ViewDiff](doc:api-core-viewdiff) viewDiff</code> : The [ViewDiff](doc:api-core-viewdiff) to populate.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getmessagestosendcontainer"></a><b>GetMessagesToSendContainer</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesToSend](doc:api-core-messagestosend) GetMessagesToSendContainer()</code></p>Gets the current messages to send container. </p><b>Returns:</b></br>The messages to send container.</td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="pushmessagestosend-messagestosend-netframestats"></a><b>PushMessagesToSend</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L45">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void PushMessagesToSend([MessagesToSend](doc:api-core-messagestosend) messages, [NetFrameStats](doc:api-core-networkstats-netframestats) frameStats)</code></p>Adds a set of [MessagesToSend](doc:api-core-messagestosend) to be sent. </p><b>Parameters</b><ul><li><code>[MessagesToSend](doc:api-core-messagestosend) messages</code> : The set of messages to send.</li><li><code>[NetFrameStats](doc:api-core-networkstats-netframestats) frameStats</code> : </li></ul></p><b>Notes:</b><ul><li>The messages may not be sent immediately. This is up to the implementer. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isconnected"></a><b>IsConnected</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L51">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsConnected()</code></p>Gets a value indicating whether the underlying connection is connected. </p><b>Returns:</b></br>True if the underlying connection is connected, false otherwise.</td>    </tr></table>



