
# WorkerSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L13">Source</a>
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
        <td style="border-right:none"><a id="workerentity"></a><b>WorkerEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L18">Source</a></td>
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
        <td style="border-right:none"><a id="logdispatcher"></a><b>LogDispatcher</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> LogDispatcher</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workertype"></a><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string WorkerType</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workerid"></a><b>WorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly string WorkerId</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="origin"></a><b>Origin</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L23">Source</a></td>
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
        <td style="border-right:none"><a id="workersystem-workerinworld"></a><b>WorkerSystem</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L34">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> WorkerSystem(<a href="{{urlRoot}}/api/core/worker-in-world">WorkerInWorld</a> worker)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/worker-in-world">WorkerInWorld</a> worker</code> : </li>
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
        <td style="border-right:none"><a id="trygetentity-entityid-out-entity"></a><b>TryGetEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L56">Source</a></td>
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
        <td style="border-right:none"><a id="hasentity-entityid"></a><b>HasEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L66">Source</a></td>
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
        <td style="border-right:none"><a id="sendlogmessage-string-string-loglevel-entityid"></a><b>SendLogMessage</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L71">Source</a></td>
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
        <td style="border-right:none"><a id="sendmetrics-metrics"></a><b>SendMetrics</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L76">Source</a></td>
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
        <td style="border-right:none"><a id="oncreate"></a><b>OnCreate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L91">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Systems/WorkerSystem.cs/#L100">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




