
# ComponentUpdateSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L9">Source</a>
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
        <td style="border-right:none"><a id="sendupdate-t-in-t-entityid"></a><b>SendUpdate&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendUpdate&lt;T&gt;(in T update, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in T update</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="sendevent-t-t-entityid"></a><b>SendEvent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendEvent&lt;T&gt;(T eventToSend, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T eventToSend</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="geteventsreceived-t"></a><b>GetEventsReceived&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L24">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-event-received">ComponentEventReceived</a>&lt;T&gt;&gt; GetEventsReceived&lt;T&gt;()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="geteventsreceived-t-entityid"></a><b>GetEventsReceived&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-event-received">ComponentEventReceived</a>&lt;T&gt;&gt; GetEventsReceived&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponentupdatesreceived-t"></a><b>GetComponentUpdatesReceived&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-update-received">ComponentUpdateReceived</a>&lt;T&gt;&gt; GetComponentUpdatesReceived&lt;T&gt;()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getentitycomponentupdatesreceived-t-entityid"></a><b>GetEntityComponentUpdatesReceived&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-update-received">ComponentUpdateReceived</a>&lt;T&gt;&gt; GetEntityComponentUpdatesReceived&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getauthoritychangesreceived-uint"></a><b>GetAuthorityChangesReceived</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L50">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/authority-change-received">AuthorityChangeReceived</a>&gt; GetAuthorityChangesReceived(uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getauthoritychangesreceived-entityid-uint"></a><b>GetAuthorityChangesReceived</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L56">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/authority-change-received">AuthorityChangeReceived</a>&gt; GetAuthorityChangesReceived(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponentsadded-uint"></a><b>GetComponentsAdded</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L63">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>List&lt;<a href="{{urlRoot}}/api/core/entity-id">EntityId</a>&gt; GetComponentsAdded(uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponentsremoved-uint"></a><b>GetComponentsRemoved</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L69">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>List&lt;<a href="{{urlRoot}}/api/core/entity-id">EntityId</a>&gt; GetComponentsRemoved(uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getauthority-entityid-uint"></a><b>GetAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L75">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Authority GetAuthority(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponent-t-entityid"></a><b>GetComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L80">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T GetComponent&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="acknowledgeauthorityloss-entityid-uint"></a><b>AcknowledgeAuthorityLoss</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L85">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AcknowledgeAuthorityLoss(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="hascomponent-uint-entityid"></a><b>HasComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L90">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasComponent(uint componentId, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L95">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Systems/ComponentUpdateSystem.cs/#L104">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




