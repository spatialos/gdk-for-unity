---
title: LocatorFlow Class
slug: api-core-locatorflow
order: 103
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L69">Source</a></span></p>

</p>


<p>Represents the Alpha Locator connection flow. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IConnectionFlow](doc:api-core-iconnectionflow)</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="locatorhost"></a><b>LocatorHost</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L74">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string LocatorHost</code></p>The host of the Locator to use for the development authentication flow and the Locator. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="locatorport"></a><b>LocatorPort</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L79">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ushort LocatorPort</code></p>The port of the Locator to use for the development authentication flow and the Locator. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="devauthtoken"></a><b>DevAuthToken</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L84">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string DevAuthToken</code></p>The development authentication token to use when connecting via with development authentication. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="usedevauthflow"></a><b>UseDevAuthFlow</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L93">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool UseDevAuthFlow</code></p>Denotes whether we should connect with development authentication. </p><b>Notes:</b><ul><li>If this is false, it is assumed that the PlayerIdentityCredentials element in the LocatorParameters has been filled. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="logintoken"></a><b>LoginToken</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L98">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string LoginToken</code></p>The login token to use to connect via the Locator. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="playeridentitytoken"></a><b>PlayerIdentityToken</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L103">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string PlayerIdentityToken</code></p>The player identity token to use to connect via the Locator. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="useinsecureconnection"></a><b>UseInsecureConnection</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L108">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool UseInsecureConnection</code></p>Denotes whether to connect to the Locator via an insecure connection or not. </td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="locatorflow-iconnectionflowinitializer-locatorflow"></a><b>LocatorFlow</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L114">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> LocatorFlow([IConnectionFlowInitializer](doc:api-core-iconnectionflowinitializer)&lt;[LocatorFlow](doc:api-core-locatorflow)&gt; initializer = null)</code></p>Initializes a new instance of the [LocatorFlow](doc:api-core-locatorflow) class. </p><b>Parameters</b><ul><li><code>[IConnectionFlowInitializer](doc:api-core-iconnectionflowinitializer)&lt;[LocatorFlow](doc:api-core-locatorflow)&gt; initializer</code> : Optional. An initializer to seed the data required to connect via the Alpha Locator flow.</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="createasync-connectionparameters-cancellationtoken"></a><b>CreateAsync</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L119">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>async Task&lt;Connection&gt; CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)</code></p>Creates a Connection asynchronously. </p><b>Returns:</b></br>A task that represents the asynchronous creation of the Connection object.</p><b>Parameters</b><ul><li><code>ConnectionParameters parameters</code> : The connection parameters to use for the connection.</li><li><code>CancellationToken? token</code> : A cancellation token which should cancel the underlying connection attempt.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getdevelopmentplayeridentitytoken"></a><b>GetDevelopmentPlayerIdentityToken</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L155">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetDevelopmentPlayerIdentityToken()</code></p>Retrieves a development player identity token using development authentication. </p><b>Returns:</b></br>The player identity token string.</p><b>Exceptions:</b><ul><li><code>[AuthenticationFailedException](doc:api-core-authenticationfailedexception)</code> : Failed to get a development player identity token.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getdevelopmentlogintokens-string-string"></a><b>GetDevelopmentLoginTokens</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L190">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>List&lt;LoginTokenDetails&gt; GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)</code></p>Retrieves the login tokens for all active deployments that the player can connect to via the development authentication flow. </p><b>Returns:</b></br>A list of all available login tokens and their deployments.</p><b>Parameters</b><ul><li><code>string workerType</code> : The type of the worker that wants to connect.</li><li><code>string playerIdentityToken</code> : The player identity token of the player that wants to connect.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="selectlogintoken-list-logintokendetails"></a><b>SelectLoginToken</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L223">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string SelectLoginToken(List&lt;LoginTokenDetails&gt; loginTokens)</code></p>Selects which login token to use to connect via the development authentication flow. </p><b>Returns:</b></br>The selected login token.</p><b>Parameters</b><ul><li><code>List&lt;LoginTokenDetails&gt; loginTokens</code> : A list of available login tokens.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getplayerid"></a><b>GetPlayerId</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L237">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetPlayerId()</code></p>Gets the player ID for the player trying to connect via the development authentication flow. </p><b>Returns:</b></br>A string containing the player id.</td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getdisplayname"></a><b>GetDisplayName</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlows.cs/#L246">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string GetDisplayName()</code></p>Retrieves the display name for the player trying to connect via the development authentication flow. </p><b>Returns:</b></br>A string containing the display name.</td>    </tr></table>



