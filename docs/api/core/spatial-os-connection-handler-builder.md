
# SpatialOSConnectionHandlerBuilder Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L8">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#properties">Properties</a>
<li><a href="#methods">Methods</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-handler-builder">Improbable.Gdk.Core.IConnectionHandlerBuilder</a></code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workertype"></a><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType { get; }</code></p>



</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setconnectionflow-iconnectionflow"></a><b>SetConnectionFlow</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/spatial-os-connection-handler-builder">SpatialOSConnectionHandlerBuilder</a> SetConnectionFlow(<a href="{{urlRoot}}/api/core/i-connection-flow">IConnectionFlow</a> flow)</code></p>
Sets the connection flow implementation to use when creating the underlying SpatialOS connection. 
</p><b>Returns:</b></br>Itself

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-flow">IConnectionFlow</a> flow</code> : The connection flow implementation.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setthreadingmode-threadingmode"></a><b>SetThreadingMode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L49">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/spatial-os-connection-handler-builder">SpatialOSConnectionHandlerBuilder</a> SetThreadingMode(ThreadingMode mode)</code></p>
Sets the threading mode for the resultant <a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a>. 
</p><b>Returns:</b></br>Itself

</p>

<b>Parameters</b>

<ul>
<li><code>ThreadingMode mode</code> : The desired threading mode.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setconnectionparameters-connectionparameters"></a><b>SetConnectionParameters</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/spatial-os-connection-handler-builder">SpatialOSConnectionHandlerBuilder</a> SetConnectionParameters(ConnectionParameters parameters)</code></p>
Sets the connection parameters to use for the underlying SpatialOS connection. 
</p><b>Returns:</b></br>Itself

</p>

<b>Parameters</b>

<ul>
<li><code>ConnectionParameters parameters</code> : The connection parameters to use.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="createasync-cancellationtoken"></a><b>CreateAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/SpatialOSConnectionHandlerBuilder.cs/#L67">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;<a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a>&gt; CreateAsync(CancellationToken? token = null)</code></p>
Creates a <a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> asynchronously. 
</p><b>Returns:</b></br>A task that represents the asynchronous creation of the connection handler object.

</p>

<b>Parameters</b>

<ul>
<li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li>
</ul>





</td>
    </tr>
</table>





