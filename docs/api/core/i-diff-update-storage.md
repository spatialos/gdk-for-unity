
# IDiffUpdateStorage&lt;T&gt; Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L49">Source</a>
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

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-component-diff-storage">Improbable.Gdk.Core.IComponentDiffStorage</a></code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addupdate-componentupdatereceived-t"></a><b>AddUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L51">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddUpdate(<a href="{{urlRoot}}/api/core/component-update-received">ComponentUpdateReceived</a>&lt;T&gt; update)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/component-update-received">ComponentUpdateReceived</a>&lt;T&gt; update</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getupdates"></a><b>GetUpdates</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L52">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-update-received">ComponentUpdateReceived</a>&lt;T&gt;&gt; GetUpdates()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getupdates-entityid"></a><b>GetUpdates</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L53">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-update-received">ComponentUpdateReceived</a>&lt;T&gt;&gt; GetUpdates(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>





