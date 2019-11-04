
# MockConnectionHandlerBuilder Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L9">Source</a>
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
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-handler-builder">Improbable.Gdk.Core.IConnectionHandlerBuilder</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="connectionhandler"></a><b>ConnectionHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/mock-connection-handler">MockConnectionHandler</a> ConnectionHandler</code></p>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workertype"></a><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType { get; }</code></p>



</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="mockconnectionhandlerbuilder"></a><b>MockConnectionHandlerBuilder</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> MockConnectionHandlerBuilder()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="createasync-cancellationtoken"></a><b>CreateAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/MockConnectionHandler.cs/#L18">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Task&lt;<a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a>&gt; CreateAsync(CancellationToken? token = null)</code></p>
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





