
# ICommandDiffDeserializer Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L13">Source</a>
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
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcomponentid"></a><b>GetComponentId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetComponentId()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcommandid"></a><b>GetCommandId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L16">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>uint GetCommandId()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addrequesttodiff-commandrequestop-viewdiff"></a><b>AddRequestToDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L18">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddRequestToDiff(CommandRequestOp op, <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>CommandRequestOp op</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addresponsetodiff-commandresponseop-viewdiff-commandmetadataaggregate"></a><b>AddResponseToDiff</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Worker/MessageSerialization.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddResponseToDiff(CommandResponseOp op, <a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff, <a href="{{urlRoot}}/api/core/command-meta-data-aggregate">CommandMetaDataAggregate</a> commandMetaData)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>CommandResponseOp op</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/view-diff">ViewDiff</a> diff</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/command-meta-data-aggregate">CommandMetaDataAggregate</a> commandMetaData</code> : </li>
</ul>





</td>
    </tr>
</table>





