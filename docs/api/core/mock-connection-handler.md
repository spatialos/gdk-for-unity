
# MockConnectionHandler Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L5">Source</a>
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
        <td style="border-right:none"><b>CreateEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L19">Source</a></td>
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
        <td style="border-right:none"><b>ChangeAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L25">Source</a></td>
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
        <td style="border-right:none"><b>UpdateComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L30">Source</a></td>
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
        <td style="border-right:none"><b>AddEvent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L35">Source</a></td>
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
        <td style="border-right:none"><b>UpdateComponentAndAddEvents&lt;TUpdate, TEvent&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L40">Source</a></td>
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
        <td style="border-right:none"><b>GetMessagesReceived</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L58">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> GetMessagesReceived()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>PushMessagesToSend</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L69">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void PushMessagesToSend(<a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> messages)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> messages</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsConnected</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L74">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsConnected()</code></p>






</td>
    </tr>
</table>





