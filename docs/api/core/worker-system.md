
# WorkerSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L13">Source</a>
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
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
</ul></nav>

</p>



<p>A SpatialOS worker instance. </p>



</p>

<b>Inheritance</b>

<code>ComponentSystem</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L18">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Entity WorkerEntity</code></p>
An ECS entity that represents the <a href="{{urlRoot}}/api/core/worker">Worker</a>. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Connection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly Connection Connection</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>LogDispatcher</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> LogDispatcher</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string WorkerType</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Origin</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly Vector3 Origin</code></p>


</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerSystem</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L34">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> WorkerSystem(<a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> connectionHandler, Connection connection, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logDispatcher, string workerType, Vector3 origin)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> connectionHandler</code> : </li>
<li><code>Connection connection</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logDispatcher</code> : </li>
<li><code>string workerType</code> : </li>
<li><code>Vector3 origin</code> : </li>
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
        <td style="border-right:none"><b>TryGetEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L56">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool TryGetEntity(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, out Entity entity)</code></p>
Attempts to find an ECS entity associated with a SpatialOS entity ID. 
</p><b>Returns:</b></br>True, if an ECS entity associated with the SpatialOS entity ID was found, false otherwise. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : The SpatialOS entity ID.</li>
<li><code>out Entity entity</code> : When this method returns, contains the ECS entity associated with the SpatialOS entity ID if one was found, else the default value for Entity. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L66">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasEntity(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>
Checks whether a SpatialOS entity is checked out on this worker. 
</p><b>Returns:</b></br>True, if the SpatialOS entity is checked out on this worker, false otherwise.

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : The SpatialOS entity ID to check for.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendLogMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L71">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendLogMessage(string message, string loggerName, LogLevel logLevel, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a>? entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string message</code> : </li>
<li><code>string loggerName</code> : </li>
<li><code>LogLevel logLevel</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a>? entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendMetrics</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L76">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendMetrics(Metrics metrics)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Metrics metrics</code> : </li>
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
        <td style="border-right:none"><b>OnCreateManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L92">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnCreateManager()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnDestroyManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L101">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnDestroyManager()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Systems/WorkerSystem.cs/#L107">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




