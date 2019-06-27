
# AlphaLocatorFlow Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L177">Source</a>
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



<p>Represents the Alpha Locator connection flow. </p>



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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L182">Source</a></td>
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
        <td style="border-right:none"><b>AnonymousAuthPort</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L187">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ushort AnonymousAuthPort</code></p>
The port to use when connecting via development authentication. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>DevAuthToken</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L192">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string DevAuthToken</code></p>
The development authentication token to use when connecting via with development authentication. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>UseDevAuthFlow</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L201">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool UseDevAuthFlow</code></p>
Denotes whether we should connect with development authentication. 

</p>

<b>Notes:</b>

<ul>
<li>If this is false, it is assumed that the PlayerIdentityCredentials element in the LocatorParameters has been filled. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>LocatorParameters</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L206">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Improbable.Worker.CInterop.Alpha.LocatorParameters LocatorParameters</code></p>
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
        <td style="border-right:none"><b>AlphaLocatorFlow</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L220">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> AlphaLocatorFlow(<a href="{{urlRoot}}/api/core/i-connection-flow-initializer">IConnectionFlowInitializer</a>&lt;<a href="{{urlRoot}}/api/core/alpha-locator-flow">AlphaLocatorFlow</a>&gt; initializer = null)</code></p>
Initializes a new instance of the <a href="{{urlRoot}}/api/core/alpha-locator-flow">AlphaLocatorFlow</a> class. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/i-connection-flow-initializer">IConnectionFlowInitializer</a>&lt;<a href="{{urlRoot}}/api/core/alpha-locator-flow">AlphaLocatorFlow</a>&gt; initializer</code> : Optional. An initializer to seed the data required to connect via the Alpha Locator flow.</li>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L225">Source</a></td>
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
        <td style="border-right:none"><b>GetDevelopmentPlayerIdentityToken</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L252">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetDevelopmentPlayerIdentityToken()</code></p>
Retrieves a development player identity token using development authentication. 
</p><b>Returns:</b></br>The player identity token string.




</p>

<b>Exceptions:</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/authentication-failed-exception">AuthenticationFailedException</a></code> : Failed to get a development player identity token.</li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetDevelopmentLoginTokens</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L286">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>List&lt;LoginTokenDetails&gt; GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)</code></p>
Retrieves the login tokens for all active deployments that the player can connect to via the development authentication flow. 
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
        <td style="border-right:none"><b>SelectLoginToken</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L319">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string SelectLoginToken(List&lt;LoginTokenDetails&gt; loginTokens)</code></p>
Selects which login token to use to connect via the development authentication flow. 
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
        <td style="border-right:none"><b>GetPlayerId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L333">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetPlayerId()</code></p>
Gets the player ID for the player trying to connect via the development authentication flow. 
</p><b>Returns:</b></br>A string containing the player id.




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetDisplayName</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L342">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetDisplayName()</code></p>
Retrieves the display name for the player trying to connect via the development authentication flow. 
</p><b>Returns:</b></br>A string containing the display name.




</td>
    </tr>
</table>





