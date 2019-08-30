
# MockConnectionHandler Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L25">Source</a>
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
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-handler">Improbable.Gdk.Core.IConnectionHandler</a></code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="createentity-long-entitytemplate"></a><b>CreateEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L39">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void CreateEntity(long entityId, <a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-template">EntityTemplate</a> template</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="changeauthority-long-uint-authority"></a><b>ChangeAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L45">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ChangeAuthority(long entityId, uint componentId, Authority newAuthority)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>Authority newAuthority</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="updatecomponent-t-long-uint-t"></a><b>UpdateComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L50">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void UpdateComponent&lt;T&gt;(long entityId, uint componentId, T update)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>T update</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addevent-t-long-uint-t"></a><b>AddEvent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddEvent&lt;T&gt;(long entityId, uint componentId, T ev)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>T ev</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="removeentity-long"></a><b>RemoveEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveEntity(long entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="removecomponent-long-uint"></a><b>RemoveComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L65">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveComponent(long entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="updatecomponentandaddevents-tupdate-tevent-long-uint-tupdate-params-tevent"></a><b>UpdateComponentAndAddEvents&lt;TUpdate, TEvent&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L70">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void UpdateComponentAndAddEvents&lt;TUpdate, TEvent&gt;(long entityId, uint componentId, TUpdate update, params TEvent [] events)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>TUpdate update</code> : </li>
<li><code>params TEvent [] events</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getworkerid"></a><b>GetWorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L88">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetWorkerId()</code></p>
Gets the worker ID for this worker. 
</p><b>Returns:</b></br>The worker ID.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getworkerattributes"></a><b>GetWorkerAttributes</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L93">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>List&lt;string&gt; GetWorkerAttributes()</code></p>
Gets the worker attributes for this worker. 
</p><b>Returns:</b></br>The list of worker attributes.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getmessagesreceived-ref-viewdiff"></a><b>GetMessagesReceived</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L98">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void GetMessagesReceived(ref <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff)</code></p>
Populates the <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> object using the messages received since the last time this was called. 


</p>

<b>Parameters</b>

<ul>
<li><code>ref <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff</code> : The <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> to populate.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getmessagestosendcontainer"></a><b>GetMessagesToSendContainer</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L109">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> GetMessagesToSendContainer()</code></p>
Gets the current messages to send container. 
</p><b>Returns:</b></br>The messages to send container.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="pushmessagestosend-messagestosend"></a><b>PushMessagesToSend</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L114">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void PushMessagesToSend(<a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> messages)</code></p>
Adds a set of <a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> to be sent. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> messages</code> : The set of messages to send.</li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>The messages may not be sent immediately. This is up to the implementer. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="isconnected"></a><b>IsConnected</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L119">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsConnected()</code></p>
Gets a value indicating whether the underlying connection is connected. 
</p><b>Returns:</b></br>True if the underlying connection is connected, false otherwise.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="dispose"></a><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L158">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





