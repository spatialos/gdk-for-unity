
# WorkerInWorld Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L16">Source</a>
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
<li><a href="#constructors">Constructors</a>
<li><a href="#overrides">Overrides</a>
</ul></nav>

</p>



<p>Represents a SpatialOS worker that is coupled with an ECS World. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/worker">Improbable.Gdk.Core.Worker</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="origin"></a><b>Origin</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly Vector3 Origin</code></p>
The origin of the worker in global Unity space. 

</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="world"></a><b>World</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L26">Source</a></td>
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
        <td style="border-right:none"><a id="ondisconnect"></a><b>OnDisconnect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L31">Source</a></td>
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
        <td style="border-right:none"><a id="createworkerinworldasync-iconnectionhandlerbuilder-string-ilogdispatcher-vector3-cancellationtoken"></a><b>CreateWorkerInWorldAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L63">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/core/worker-in-world">WorkerInWorld</a>&gt; CreateWorkerInWorldAsync(<a href="{{urlRoot}}/api/core/i-connection-handler-builder">IConnectionHandlerBuilder</a> connectionHandlerBuilder, string workerType, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logDispatcher, Vector3 origin, CancellationToken? token = null)</code></p>
Creates a <a href="{{urlRoot}}/api/core/worker-in-world">WorkerInWorld</a> object asynchronously. 
</p><b>Returns:</b></br>A task which represents the asynchronous creation of a worker.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-handler-builder">IConnectionHandlerBuilder</a> connectionHandlerBuilder</code> : A builder which describes how to create the <a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> for this worker. </li>
<li><code>string workerType</code> : The type of worker to connect as.</li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logDispatcher</code> : The logger to use for this worker.</li>
<li><code>Vector3 origin</code> : </li>
<li><code>CancellationToken? token</code> : A cancellation token which will cancel this asynchronous operation</li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workerinworld-iconnectionhandler-string-ilogdispatcher-vector3"></a><b>WorkerInWorld</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L40">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> WorkerInWorld(<a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> connectionHandler, string workerType, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logDispatcher, Vector3 origin)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> connectionHandler</code> : </li>
<li><code>string workerType</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logDispatcher</code> : </li>
<li><code>Vector3 origin</code> : </li>
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
        <td style="border-right:none"><a id="dispose"></a><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerInWorld.cs/#L96">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void Dispose()</code></p>






</td>
    </tr>
</table>




