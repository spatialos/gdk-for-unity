
# ReceptionistFlow Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L30">Source</a>
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
</ul></nav>

</p>



<p>Represents the Receptionist connection flow. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-flow">Improbable.Gdk.Core.IConnectionFlow</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ReceptionistHost</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string ReceptionistHost</code></p>
The IP address of the Receptionist to use when connecting. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ReceptionistPort</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L40">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ushort ReceptionistPort</code></p>
The port of the Receptionist to use when connecting. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L46">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerId</code></p>
The worker ID to use for the worker connection that will be created when CreateAsync is called. 

</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ReceptionistFlow</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L53">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ReceptionistFlow(string workerId, <a href="{{urlRoot}}/api/core/i-connection-flow-initializer">IConnectionFlowInitializer</a>&lt;<a href="{{urlRoot}}/api/core/receptionist-flow">ReceptionistFlow</a>&gt; initializer = null)</code></p>
Initializes a new instance of the <a href="{{urlRoot}}/api/core/receptionist-flow">ReceptionistFlow</a> class. 


</p>

<b>Parameters</b>

<ul>
<li><code>string workerId</code> : The worker ID to use for the worker connection.</li>
<li><code><a href="{{urlRoot}}/api/core/i-connection-flow-initializer">IConnectionFlowInitializer</a>&lt;<a href="{{urlRoot}}/api/core/receptionist-flow">ReceptionistFlow</a>&gt; initializer</code> : Optional. An initializer to seed the data required to connect via the Receptionist flow.</li>
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
        <td style="border-right:none"><b>CreateAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L59">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task&lt;Connection&gt; CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)</code></p>
Creates a Connection asynchronously. 
</p><b>Returns:</b></br>A task that represents the asynchronous creation of the Connection object.

</p>

<b>Parameters</b>

<ul>
<li><code>ConnectionParameters parameters</code> : The connection parameters to use for the connection.</li>
<li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li>
</ul>





</td>
    </tr>
</table>





