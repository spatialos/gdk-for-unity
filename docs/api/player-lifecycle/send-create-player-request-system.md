
# SendCreatePlayerRequestSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/player-lifecycle-index">PlayerLifecycle</a><br/>
GDK package: PlayerLifecycle<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L18">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="requestplayercreation-byte-action-playercreator-createplayer-receivedresponse"></a><b>RequestPlayerCreation</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L116">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RequestPlayerCreation(byte[] serializedArguments = null, Action&lt;PlayerCreator.CreatePlayer.ReceivedResponse&gt; callback = null)</code></p>
Queues a request for player creation, if none are currently in progress. If player creation entities have already been found, the request is sent in the next tick. 


</p>

<b>Parameters</b>

<ul>
<li><code>byte[] serializedArguments</code> : A serialized byte array of arbitrary player creation arguments.</li>
<li><code>Action&lt;PlayerCreator.CreatePlayer.ReceivedResponse&gt; callback</code> : An action to be invoked when the worker receives a response to the player creation request. </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="oncreate"></a><b>OnCreate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L42">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnCreate()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="onupdate"></a><b>OnUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.playerlifecycle/Systems/PlayerCreation/SendCreatePlayerRequestSystem.cs/#L220">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




