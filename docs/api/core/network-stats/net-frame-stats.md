
# NetFrameStats Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/network-stats-index">NetworkStats</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L10">Source</a>
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
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Represents a single frame's data for either incoming or outgoing network messages. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="messages"></a><b>Messages</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly Dictionary&lt;<a href="{{urlRoot}}/api/core/network-stats/message-type-union">MessageTypeUnion</a>, <a href="{{urlRoot}}/api/core/network-stats/data-point">DataPoint</a>&gt; Messages</code></p>


</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addupdate-in-componentupdate"></a><b>AddUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L17">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddUpdate(in ComponentUpdate update)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in ComponentUpdate update</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addcommandrequest-in-commandrequest"></a><b>AddCommandRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddCommandRequest(in CommandRequest request)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in CommandRequest request</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addcommandresponse-in-commandresponse-string"></a><b>AddCommandResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L44">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddCommandResponse(in CommandResponse response, string message)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>in CommandResponse response</code> : </li>
<li><code>string message</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addcommandresponse-string-uint-uint"></a><b>AddCommandResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L69">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddCommandResponse(string message, uint componentId, uint commandIndex)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string message</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>uint commandIndex</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addworldcommandrequest-worldcommand"></a><b>AddWorldCommandRequest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L83">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddWorldCommandRequest(<a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> command)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> command</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addworldcommandresponse-worldcommand"></a><b>AddWorldCommandResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetFrameStats.cs/#L93">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddWorldCommandResponse(<a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> command)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/world-command">WorldCommand</a> command</code> : </li>
</ul>





</td>
    </tr>
</table>





