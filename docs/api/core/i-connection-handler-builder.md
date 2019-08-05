
# IConnectionHandlerBuilder Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandlerBuilder.cs/#L10">Source</a>
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



<p>Intermediate object for building a <a href="{{urlRoot}}/api/core/i-connection-handler">IConnectionHandler</a> object. </p>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandlerBuilder.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType { get; }</code></p>
The type of worker that the resulting connection handler will represent. 


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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/e31c47b5050ee67cafe8962204aa86a259095db0/workers/unity/Packages/io.improbable.gdk.core/Worker/ConnectionHandlers/IConnectionHandlerBuilder.cs/#L17">Source</a></td>
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





