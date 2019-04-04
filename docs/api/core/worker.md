
# Worker Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L15">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#fields">Fields</a>
<li><a href="#properties">Properties</a>
<li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Represents a SpatialOS worker. </p>



</p>

<b>Inheritance</b>

<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Origin</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly Vector3 Origin</code></p>
The origin of the worker in global Unity space. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string WorkerType</code></p>
The type of the worker. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L33">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string WorkerId</code></p>
The worker ID. 

</p>

<b>Notes:</b>

<ul>
<li>Unique for a given SpatialOS deployment. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>LogDispatcher</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> LogDispatcher</code></p>
The logger for this worker. 

</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Connection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Connection Connection { get; }</code></p>
The connection to the SpatialOS runtime. 


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>World</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L48">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> World World { get; }</code></p>
The ECS world associated with this worker. 


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnDisconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Action&lt;string&gt; OnDisconnect {  }</code></p>
An event that triggers when the worker is disconnected. 


</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateWorkerAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L135">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/core/worker">Worker</a>&gt; CreateWorkerAsync(<a href="{{urlRoot}}/api/core/receptionist-config">ReceptionistConfig</a> config, ConnectionParameters connectionParameters, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger, Vector3 origin)</code></p>
Connects to the SpatialOS Runtime via the Receptionist service and creates a <a href="{{urlRoot}}/api/core/worker">Worker</a> object asynchronously. 
</p><b>Returns:</b></br>A Task<TResult> to run this method asynchronously and retrieve the created <a href="{{urlRoot}}/api/core/worker">Worker</a> object upon connecting successfully. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/receptionist-config">ReceptionistConfig</a> config</code> : The <a href="{{urlRoot}}/api/core/receptionist-config">ReceptionistConfig</a> object stores the configuration needed to connect via the Receptionist Service. </li>
<li><code>ConnectionParameters connectionParameters</code> : The ConnectionParameters storing </li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger</code> : The logger used by this worker.</li>
<li><code>Vector3 origin</code> : The origin of this worker in the local Unity space.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateWorkerAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L163">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/core/worker">Worker</a>&gt; CreateWorkerAsync(<a href="{{urlRoot}}/api/core/locator-config">LocatorConfig</a> parameters, ConnectionParameters connectionParameters, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger, Vector3 origin)</code></p>
Connects to the SpatialOS Runtime via the Locator service and creates a <a href="{{urlRoot}}/api/core/worker">Worker</a> object asynchronously. 
</p><b>Returns:</b></br>A Task<TResult> to run this method asynchronously and retrieve the created <a href="{{urlRoot}}/api/core/worker">Worker</a> object upon connecting successfully. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/locator-config">LocatorConfig</a> parameters</code> : </li>
<li><code>ConnectionParameters connectionParameters</code> : The ConnectionParameters storing </li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger</code> : The logger used by this worker.</li>
<li><code>Vector3 origin</code> : The origin of this worker in the local Unity space.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>CreateWorkerAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L201">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/core/worker">Worker</a>&gt; CreateWorkerAsync(<a href="{{urlRoot}}/api/core/alpha-locator-config">AlphaLocatorConfig</a> parameters, ConnectionParameters connectionParameters, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger, Vector3 origin)</code></p>
Connects to the SpatialOS Runtime via the Alpha Locator service and creates a <a href="{{urlRoot}}/api/core/worker">Worker</a> object asynchronously. 
</p><b>Returns:</b></br>A Task<TResult> to run this method asynchronously and retrieve the created <a href="{{urlRoot}}/api/core/worker">Worker</a> object upon connecting successfully. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/alpha-locator-config">AlphaLocatorConfig</a> parameters</code> : </li>
<li><code>ConnectionParameters connectionParameters</code> : The ConnectionParameters storing </li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger</code> : The logger used by this worker.</li>
<li><code>Vector3 origin</code> : The origin of this worker in the local Unity space.</li>
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
        <td style="border-right:none"><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.core/Worker/Worker.cs/#L266">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





