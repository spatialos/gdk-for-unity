
# WorkerConnector Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L17">Source</a>
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
        <td style="border-right:none"><b>MaxConnectionAttempts</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L24">Source</a></td>
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
        <td style="border-right:none"><b>Worker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/worker">Worker</a> Worker</code></p>
Represents a SpatialOS worker. 

</p>

<b>Notes:</b>

<ul>
<li>Only safe to access after the connection has succeeded. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>DevelopmentAuthToken</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L34">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string DevelopmentAuthToken</code></p>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnWorkerCreationFinished</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L41">Source</a></td>
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
        <td style="border-right:none"><b>CreateNewWorkerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L343">Source</a></td>
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
        <td style="border-right:none"><b>OnApplicationQuit</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L57">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnApplicationQuit()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnDestroy</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L62">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnDestroy()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetConnectionService</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L165">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract <a href="{{urlRoot}}/api/core/connection-service">ConnectionService</a> GetConnectionService()</code></p>
Determines which <a href="{{urlRoot}}/api/core/connection-service">ConnectionService</a> to use to connect to the SpatialOS Runtime. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/core/connection-service">ConnectionService</a> object describing which connection servce to use.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetConnectionParameters</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L173">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract ConnectionParameters GetConnectionParameters(string workerType, <a href="{{urlRoot}}/api/core/connection-service">ConnectionService</a> service)</code></p>
Retrieves the ConnectionParameters needed to be able to connect to any connection service. 
</p><b>Returns:</b></br>A ConnectionParameters object.

</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : The type of worker you want to connect.</li>
<li><code><a href="{{urlRoot}}/api/core/connection-service">ConnectionService</a> service</code> : The connection service used to connect.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetLocatorConfig</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L179">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract <a href="{{urlRoot}}/api/core/locator-config">LocatorConfig</a> GetLocatorConfig()</code></p>
Retrieves the configuration needed to connect via the Locator service. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/core/locator-config">LocatorConfig</a> object.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetAlphaLocatorConfig</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L188">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract <a href="{{urlRoot}}/api/core/alpha-locator-config">AlphaLocatorConfig</a> GetAlphaLocatorConfig(string workerType)</code></p>
Retrieves the configuration needed to connect via the Alpha Locator service. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/core/alpha-locator-config">AlphaLocatorConfig</a> object.

</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>This connection service is still in Alpha and does not provide an integration with Steam. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetReceptionistConfig</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L195">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract <a href="{{urlRoot}}/api/core/receptionist-config">ReceptionistConfig</a> GetReceptionistConfig(string workerType)</code></p>
Retrieves the configuration needed to connect via the Receptionist service. 
</p><b>Returns:</b></br>A <a href="{{urlRoot}}/api/core/receptionist-config">ReceptionistConfig</a> object.

</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : The type of worker you want to connect.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetPlayerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L201">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetPlayerId()</code></p>
Retrieves the player id for the player trying to connect via the anonymous authentication flow. 
</p><b>Returns:</b></br>A string containing the player id.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetDisplayName</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L210">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetDisplayName()</code></p>
Retrieves the display name for the player trying to connect via the anonymous authentication flow. 
</p><b>Returns:</b></br>A string containing the display name.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SelectDeploymentName</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L220">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string SelectDeploymentName(DeploymentList deployments)</code></p>
Selects which deployment to connect to. 
</p><b>Returns:</b></br>The name of the deployment to connect to.

</p>

<b>Parameters</b>

<ul>
<li><code>DeploymentList deployments</code> : The list of deployments.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SelectLoginToken</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L230">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string SelectLoginToken(List&lt;LoginTokenDetails&gt; loginTokens)</code></p>
Selects which login token to use to connect via the anonymous authentication flow. 
</p><b>Returns:</b></br>The selected login token.

</p>

<b>Parameters</b>

<ul>
<li><code>List&lt;LoginTokenDetails&gt; loginTokens</code> : A list of available login tokens.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetDevelopmentPlayerIdentityToken</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L248">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetDevelopmentPlayerIdentityToken(string authToken, string playerId, string displayName)</code></p>
Retrieves the player identity token needed to generate a login token when using the anonymous authentication flow. 
</p><b>Returns:</b></br>The player identity token.

</p>

<b>Parameters</b>

<ul>
<li><code>string authToken</code> : The authentication token that you generated.</li>
<li><code>string playerId</code> : The id of the player that wants to connect.</li>
<li><code>string displayName</code> : The display name of the player that wants to connect.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetDevelopmentLoginTokens</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L282">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>List&lt;LoginTokenDetails&gt; GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)</code></p>
Retrieves the login tokens for all active deployments that the player can connect to via the anonymous authentication flow. 
</p><b>Returns:</b></br>A list of all available login tokens and their deployments.

</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : The type of the worker that wants to connect.</li>
<li><code>string playerIdentityToken</code> : The player identity token of the player that wants to connect.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HandleWorkerConnectionEstablished</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L310">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void HandleWorkerConnectionEstablished()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HandleWorkerConnectionFailure</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L314">Source</a></td>
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
        <td style="border-right:none"><b>Connect</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L77">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>async Task Connect(string workerType, <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger)</code></p>
Asynchronously connects a worker to the SpatialOS runtime. 


</p>

<b>Parameters</b>

<ul>
<li><code>string workerType</code> : The type of the worker to connect as</li>
<li><code><a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> logger</code> : The logger for the worker to use.</li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>Uses the global position of this GameObject as the worker origin. Uses ShouldUseLocator to determine whether to connect via the Locator. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.core/Worker/WorkerConnector.cs/#L363">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





