---
title: NetworkStats.NetFrameStats Class
slug: api-core-networkstats-netframestats
order: 118
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L10">Source</a></span></p>

</p>


<p>Represents a single frame's data for either incoming or outgoing network messages. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="messages"></a><b>Messages</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L12">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Dictionary&lt;[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion), [DataPoint](doc:api-core-networkstats-datapoint)&gt; Messages</code></p></td>    </tr></table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addupdate-in-componentupdate"></a><b>AddUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddUpdate(in ComponentUpdate update)</code></p></p><b>Parameters</b><ul><li><code>in ComponentUpdate update</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandrequest-in-commandrequest"></a><b>AddCommandRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandRequest(in CommandRequest request)</code></p></p><b>Parameters</b><ul><li><code>in CommandRequest request</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandresponse-in-commandresponse-string"></a><b>AddCommandResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L44">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandResponse(in CommandResponse response, string message)</code></p></p><b>Parameters</b><ul><li><code>in CommandResponse response</code> : </li><li><code>string message</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addcommandresponse-string-uint-uint"></a><b>AddCommandResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L69">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddCommandResponse(string message, uint componentId, uint commandIndex)</code></p></p><b>Parameters</b><ul><li><code>string message</code> : </li><li><code>uint componentId</code> : </li><li><code>uint commandIndex</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addworldcommandrequest-worldcommand"></a><b>AddWorldCommandRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddWorldCommandRequest([WorldCommand](doc:api-core-networkstats-worldcommand) command)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommand](doc:api-core-networkstats-worldcommand) command</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="addworldcommandresponse-worldcommand"></a><b>AddWorldCommandResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L93">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void AddWorldCommandResponse([WorldCommand](doc:api-core-networkstats-worldcommand) command)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommand](doc:api-core-networkstats-worldcommand) command</code> : </li></ul></td>    </tr></table>



