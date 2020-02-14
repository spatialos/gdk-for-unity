---
title: SendCreatePlayerRequestSystem Class
slug: api-playerlifecycle-sendcreateplayerrequestsystem
order: 8
---

<p><b>Namespace:</b> Improbable.Gdk.PlayerLifecycle<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L18">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="requestplayercreation-byte-action-playercreator-createplayer-receivedresponse"></a><b>RequestPlayerCreation</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L116">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void RequestPlayerCreation(byte[] serializedArguments = null, Action&lt;PlayerCreator.CreatePlayer.ReceivedResponse&gt; callback = null)</code></p>Queues a request for player creation, if none are currently in progress. If player creation entities have already been found, the request is sent in the next tick. </p><b>Parameters</b><ul><li><code>byte[] serializedArguments</code> : A serialized byte array of arbitrary player creation arguments.</li><li><code>Action&lt;PlayerCreator.CreatePlayer.ReceivedResponse&gt; callback</code> : An action to be invoked when the worker receives a response to the player creation request. </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="oncreate"></a><b>OnCreate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L42">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnCreate()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onupdate"></a><b>OnUpdate</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L220">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override void OnUpdate()</code></p></td>    </tr></table>


