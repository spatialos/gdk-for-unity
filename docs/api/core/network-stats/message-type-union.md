
# MessageTypeUnion Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/network-stats-index">NetworkStats</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L37">Source</a>
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
<li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
<li><a href="#operators">Operators</a>
</ul></nav>

</p>



<p>Describes a type of a message. </p>



</p>

<b>Inheritance</b>

<code>IEquatable&lt;MessageTypeUnion&gt;</code>


</p>

<b>Notes</b>

- Implemented as a C-style union. Can be thought of as a sum type where the discriminants are: Update, CommandRequest, CommandResponse, WorldCommandRequest, WorldCommandResponse 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="commandinfo"></a><b>CommandInfo</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> uint CommandInfo</code></p>


</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="update-uint"></a><b>Update</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L47">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> Update(uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="commandrequest-uint-uint"></a><b>CommandRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L56">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> CommandRequest(uint componentId, uint commandIndex)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandIndex</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="commandresponse-uint-uint"></a><b>CommandResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L65">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> CommandResponse(uint componentId, uint commandIndex)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : </li>
<li><code>uint commandIndex</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="worldcommandrequest-worldcommand"></a><b>WorldCommandRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L74">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> WorldCommandRequest(<a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> worldCommand)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> worldCommand</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="worldcommandresponse-worldcommand"></a><b>WorldCommandResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L83">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> WorldCommandResponse(<a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> worldCommand)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> worldCommand</code> : </li>
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
        <td style="border-right:none"><a id="equals-messagetypeunion"></a><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L92">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Equals(<a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> other)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> other</code> : </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="equals-object"></a><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L116">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override bool Equals(object obj)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>object obj</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="gethashcode"></a><b>GetHashCode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L121">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override int GetHashCode()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Operators


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="operator-messagetypeunion-messagetypeunion"></a><b>operator==</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L152">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator==(<a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> left, <a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> right)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> left</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> right</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="operator-messagetypeunion-messagetypeunion"></a><b>operator!=</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/MessageType.cs/#L157">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator!=(<a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> left, <a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> right)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> left</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a> right</code> : </li>
</ul>





</td>
    </tr>
</table>



