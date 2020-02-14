---
title: NetworkStats.MessageTypeUnion Struct
slug: api-core-networkstats-messagetypeunion
order: 117
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L37">Source</a></span></p>

</p>


<p>Describes a type of a message. </p>



</p>
<p><b>Inheritance</b></p>

<code>IEquatable&lt;MessageTypeUnion&gt;</code>


</p>
<p><b>Notes</b></p>

- Implemented as a C-style union. Can be thought of as a sum type where the discriminants are: Update, CommandRequest, CommandResponse, WorldCommandRequest, WorldCommandResponse 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandinfo"></a><b>CommandInfo</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>uint CommandInfo</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="update-uint"></a><b>Update</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L47">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) Update(uint componentId)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandrequest-uint-uint"></a><b>CommandRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L56">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) CommandRequest(uint componentId, uint commandIndex)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandIndex</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="commandresponse-uint-uint"></a><b>CommandResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L65">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) CommandResponse(uint componentId, uint commandIndex)</code></p></p><b>Parameters</b><ul><li><code>uint componentId</code> : </li><li><code>uint commandIndex</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worldcommandrequest-worldcommand"></a><b>WorldCommandRequest</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L74">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) WorldCommandRequest([WorldCommand](doc:api-core-networkstats-worldcommand) worldCommand)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommand](doc:api-core-networkstats-worldcommand) worldCommand</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worldcommandresponse-worldcommand"></a><b>WorldCommandResponse</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) WorldCommandResponse([WorldCommand](doc:api-core-networkstats-worldcommand) worldCommand)</code></p></p><b>Parameters</b><ul><li><code>[WorldCommand](doc:api-core-networkstats-worldcommand) worldCommand</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-messagetypeunion"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L92">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Equals([MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) other)</code></p></p><b>Parameters</b><ul><li><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) other</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-object"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L116">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override bool Equals(object obj)</code></p></p><b>Parameters</b><ul><li><code>object obj</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="gethashcode"></a><b>GetHashCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L121">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override int GetHashCode()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Operators


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-messagetypeunion-messagetypeunion"></a><b>operator==</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L152">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool operator==([MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) left, [MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) right)</code></p></p><b>Parameters</b><ul><li><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) left</code> : </li><li><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) right</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-messagetypeunion-messagetypeunion"></a><b>operator!=</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L157">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool operator!=([MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) left, [MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) right)</code></p></p><b>Parameters</b><ul><li><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) left</code> : </li><li><code>[MessageTypeUnion](doc:api-core-networkstats-messagetypeunion) right</code> : </li></ul></td>    </tr></table>

