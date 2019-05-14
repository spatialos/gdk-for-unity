
# MultiThreadedSpatialOSConnectionHandler Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L5">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-handler">Improbable.Gdk.Core.IConnectionHandler</a></code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>MultiThreadedSpatialOSConnectionHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> MultiThreadedSpatialOSConnectionHandler(Connection connection)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Connection connection</code> : </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsConnected</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsConnected()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetMessagesReceived</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L28">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void GetMessagesReceived(ref <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>ref <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> viewDiff</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetMessagesToSendContainer</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-to-send">MessagesToSend</a> GetMessagesToSendContainer()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>PushMessagesToSend</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L48">Source</a></td>
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
        <td style="border-right:none"><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/MultithreadedSpatialOSConnectionHandler/MultiThreadedSpatialOSConnectionHandler.cs/#L64">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





