
# IConnectionHandler Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L10">Source</a>
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



<p>Represents a handler for a SpatialOS connection. </p>



</p>

<b>Inheritance</b>

<code>IDisposable</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getworkerid"></a><b>GetWorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L16">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L22">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L29">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L35">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L44">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandler.cs/#L50">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsConnected()</code></p>
Gets a value indicating whether the underlying connection is connected. 
</p><b>Returns:</b></br>True if the underlying connection is connected, false otherwise.




</td>
    </tr>
</table>





