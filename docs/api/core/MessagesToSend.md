---
title: MessagesToSend Class
slug: api-core-messagestosend
order: 110
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L10">Source</a></span></p>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="messagestosend"></a><b>MessagesToSend</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L40">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> MessagesToSend()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="clear"></a><b>Clear</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L95">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Clear()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="acknowledgeauthorityloss-long-uint"></a><b>AcknowledgeAuthorityLoss</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L112">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AcknowledgeAuthorityLoss(long entityId, uint componentId)</code></p></p><b>Parameters</b><ul><li><code>long entityId</code> : </li><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcomponentupdate-t-in-t-long"></a><b>AddComponentUpdate&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L117">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddComponentUpdate&lt;T&gt;(in T update, long entityId)</code></p></p><b>Parameters</b><ul><li><code>in T update</code> : </li><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addevent-t-t-long"></a><b>AddEvent&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L127">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddEvent&lt;T&gt;(T ev, long entityId)</code></p></p><b>Parameters</b><ul><li><code>T ev</code> : </li><li><code>long entityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandrequest-t-t-unity-entities-entity-long"></a><b>AddCommandRequest&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L136">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandRequest&lt;T&gt;(T request, Unity.Entities.Entity sendingEntity, long requestId)</code></p></p><b>Parameters</b><ul><li><code>T request</code> : </li><li><code>Unity.Entities.Entity sendingEntity</code> : </li><li><code>long requestId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandresponse-t-t"></a><b>AddCommandResponse&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L142">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandResponse&lt;T&gt;(T response)</code></p></p><b>Parameters</b><ul><li><code>T response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addlogmessage-in-logmessagetosend"></a><b>AddLogMessage</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L149">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddLogMessage(in [LogMessageToSend](doc:api-core-logmessagetosend) log)</code></p></p><b>Parameters</b><ul><li><code>in [LogMessageToSend](doc:api-core-logmessagetosend) log</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addmetrics-metrics"></a><b>AddMetrics</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/MessagesToSend.cs/#L154">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddMetrics(Metrics metrics)</code></p></p><b>Parameters</b><ul><li><code>Metrics metrics</code> : </li></ul></td>    </tr></table>



