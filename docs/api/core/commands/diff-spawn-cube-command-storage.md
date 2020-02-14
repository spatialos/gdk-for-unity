
# DiffSpawnCubeCommandStorage&lt;TRequest, TResponse&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/commands-index">Commands</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L10">Source</a>
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
<li><a href="#methods">Methods</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-component-command-diff-storage">Improbable.Gdk.Core.IComponentCommandDiffStorage</a></code>
<code><a href="{{urlRoot}}/api/core/i-diff-command-request-storage">Improbable.Gdk.Core.IDiffCommandRequestStorage&lt;TRequest&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-diff-command-response-storage">Improbable.Gdk.Core.IDiffCommandResponseStorage&lt;TResponse&gt;</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="requesttype"></a><b>RequestType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L24">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Type RequestType</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="responsetype"></a><b>ResponseType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Type ResponseType</code></p>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="componentid"></a><b>ComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> abstract uint ComponentId { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="commandid"></a><b>CommandId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> abstract uint CommandId { get; }</code></p>



</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="clear"></a><b>Clear</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L27">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Clear()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="removerequests-long"></a><b>RemoveRequests</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RemoveRequests(long entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequest-trequest"></a><b>AddRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L40">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequest(TRequest request)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>TRequest request</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addresponse-tresponse"></a><b>AddResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L45">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddResponse(TResponse response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>TResponse response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getrequests"></a><b>GetRequests</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L50">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;TRequest&gt; GetRequests()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getrequests-entityid"></a><b>GetRequests</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;TRequest&gt; GetRequests(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> targetEntityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> targetEntityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getresponses"></a><b>GetResponses</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L67">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;TResponse&gt; GetResponses()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getresponse-long"></a><b>GetResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Commands/CommandStorageBase.cs/#L72">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;TResponse&gt; GetResponse(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>





