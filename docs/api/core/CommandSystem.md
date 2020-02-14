---
title: CommandSystem Class
slug: api-core-commandsystem
order: 35
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L7">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendcommand-t-t-entity"></a><b>SendCommand&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L13">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>long SendCommand&lt;T&gt;(T request, Entity sendingEntity)</code></p></p><b>Parameters</b><ul><li><code>T request</code> : </li><li><code>Entity sendingEntity</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendcommand-t-t"></a><b>SendCommand&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L19">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>long SendCommand&lt;T&gt;(T request)</code></p></p><b>Parameters</b><ul><li><code>T request</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="sendresponse-t-t"></a><b>SendResponse&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SendResponse&lt;T&gt;(T response)</code></p></p><b>Parameters</b><ul><li><code>T response</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getrequests-t"></a><b>GetRequests&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;T&gt; GetRequests&lt;T&gt;()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getrequests-t-entityid"></a><b>GetRequests&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;T&gt; GetRequests&lt;T&gt;([EntityId](doc:api-core-entityid) targetEntityId)</code></p></p><b>Parameters</b><ul><li><code>[EntityId](doc:api-core-entityid) targetEntityId</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getresponses-t"></a><b>GetResponses&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L42">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;T&gt; GetResponses&lt;T&gt;()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getresponse-t-long"></a><b>GetResponse&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L48">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[MessagesSpan](doc:api-core-messagesspan)&lt;T&gt; GetResponse&lt;T&gt;(long requestId)</code></p></p><b>Parameters</b><ul><li><code>long requestId</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L54">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Systems/CommandSystem.cs/#L62">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


