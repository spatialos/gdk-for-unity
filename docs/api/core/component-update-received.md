
# ComponentUpdateReceived&lt;T&gt; Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/UpdatesAndEvents/ComponentUpdateToSend.cs/#L29">Source</a>
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

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-received-entity-message">Improbable.Gdk.Core.IReceivedEntityMessage</a></code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Update</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/UpdatesAndEvents/ComponentUpdateToSend.cs/#L31">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly T Update</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>UpdateId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/UpdatesAndEvents/ComponentUpdateToSend.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly ulong UpdateId</code></p>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>EntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/UpdatesAndEvents/ComponentUpdateToSend.cs/#L33">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> EntityId</code></p>


</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>ComponentUpdateReceived</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/UpdatesAndEvents/ComponentUpdateToSend.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ComponentUpdateReceived(T update, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, ulong updateId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T update</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>ulong updateId</code> : </li>
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
        <td style="border-right:none"><b>GetEntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/UpdatesAndEvents/ComponentUpdateToSend.cs/#L42">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> IReceivedEntityMessage. GetEntityId()</code></p>






</td>
    </tr>
</table>





