
# LocatorFlow Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L72">Source</a>
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



<p>Represents the Locator connection flow. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-flow">Improbable.Gdk.Core.IConnectionFlow</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>LocatorHost</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L77">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string LocatorHost</code></p>
The IP address of the Locator to use when connecting. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>LocatorParameters</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L82">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> LocatorParameters LocatorParameters</code></p>
The parameters to use to connect to the Locator. 

</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>LocatorFlow</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L103">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> LocatorFlow(<a href="{{urlRoot}}/api/core/i-connection-flow-initializer">IConnectionFlowInitializer</a>&lt;<a href="{{urlRoot}}/api/core/locator-flow">LocatorFlow</a>&gt; initializer = null)</code></p>
Initializes a new instance of the <a href="{{urlRoot}}/api/core/locator-flow">LocatorFlow</a> class. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-flow-initializer">IConnectionFlowInitializer</a>&lt;<a href="{{urlRoot}}/api/core/locator-flow">LocatorFlow</a>&gt; initializer</code> : Optional. An initializer to seed the data required to connect via the Locator flow.</li>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L108">Source</a></td>
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


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SelectDeploymentName</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/6689e30/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L136">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string SelectDeploymentName(DeploymentList deploymentList)</code></p>
Selects a deployment to connect to. 
</p><b>Returns:</b></br>The name of the deployment to connect to.

</p>

<b>Parameters</b>

<ul>
<li><code>DeploymentList deploymentList</code> : The list of deployments to choose from.</li>
</ul>





</p>

<b>Exceptions:</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/connection-failed-exception">ConnectionFailedException</a></code> : The deployment list contains an error or is empty.</li>
</ul>


</td>
    </tr>
</table>





