
# IConnectionFlowInitializer&lt;TConnectionFlow&gt; Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L10">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Represents an object which can initialize a connection flow of a certain type. </p>


</p>

<b>Type parameters</b>

<code>TConnectionFlow</code> : The type of connection flow that this object can initialize.












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Initialize</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Worker/ConnectionHandlers/ConnectionFlowInitializers.cs/#L16">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Initialize(TConnectionFlow flow)</code></p>
Initializes the flow. Seeds the flow implementation with the data required to successfully connect. 


</p>

<b>Parameters</b>

<ul>
<li><code>TConnectionFlow flow</code> : The flow object to initialize.</li>
</ul>





</td>
    </tr>
</table>





