
# WorkerConnector Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L18">Source</a>
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



<p>Connect workers via Monobehaviours. </p>



</p>

<b>Inheritance</b>

<code>MonoBehaviour</code>
<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="maxconnectionattempts"></a><b>MaxConnectionAttempts</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> int MaxConnectionAttempts</code></p>
The number of connection attempts before giving up. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="worker"></a><b>Worker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L31">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/worker-in-world">WorkerInWorld</a> Worker</code></p>
Represents a SpatialOS worker. 

</p>

<b>Notes:</b>

<ul>
<li>Only safe to access after the connection has succeeded. </li>
</ul>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="onworkercreationfinished"></a><b>OnWorkerCreationFinished</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Action&lt;<a href="{{urlRoot}}/api/core/worker">Worker</a>&gt; OnWorkerCreationFinished {  }</code></p>
An event that triggers when the worker has been fully created. 


</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="createnewworkerid-string"></a><b>CreateNewWorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L204">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string CreateNewWorkerId(string workerType)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : </li>
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
        <td style="border-right:none"><a id="onapplicationquit"></a><b>OnApplicationQuit</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L54">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnApplicationQuit()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="ondestroy"></a><b>OnDestroy</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L59">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnDestroy()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="connect-iconnectionhandlerbuilder-ilogdispatcher"></a><b>Connect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L70">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task Connect(<a href="{{urlRoot}}/api/core/i-connection-handler-builder">IConnectionHandlerBuilder</a> builder, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger)</code></p>
Asynchronously connects a worker to the SpatialOS runtime. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-handler-builder">IConnectionHandlerBuilder</a> builder</code> : Describes how to create a <a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> for this worker.</li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger</code> : The logger for the worker to use.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="handleworkerconnectionestablished"></a><b>HandleWorkerConnectionEstablished</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L152">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void HandleWorkerConnectionEstablished()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="handleworkerconnectionfailure-string"></a><b>HandleWorkerConnectionFailure</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L156">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void HandleWorkerConnectionFailure(string errorMessage)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string errorMessage</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="createconnectionparameters-string-iconnectionparameterinitializer"></a><b>CreateConnectionParameters</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L209">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ConnectionParameters CreateConnectionParameters(string workerType, <a href="{{urlRoot}}/api/core/i-connection-parameter-initializer">IConnectionParameterInitializer</a> initializer = null)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/i-connection-parameter-initializer">IConnectionParameterInitializer</a> initializer</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="deferreddisposeworker"></a><b>DeferredDisposeWorker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L230">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>IEnumerator DeferredDisposeWorker()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="dispose"></a><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/WorkerConnector.cs/#L238">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





