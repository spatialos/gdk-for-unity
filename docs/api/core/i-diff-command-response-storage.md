
# IDiffCommandResponseStorage&lt;T&gt; Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L73">Source</a>
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

<code><a href="{{urlRoot}}/api/core/i-command-diff-storage">Improbable.Gdk.Core.ICommandDiffStorage</a></code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addresponse-t"></a><b>AddResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L75">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddResponse(T response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getresponses"></a><b>GetResponses</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L76">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/received-messages-span">ReceivedMessagesSpan</a>&lt;T&gt; GetResponses()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getresponse-long"></a><b>GetResponse</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L77">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/received-messages-span">ReceivedMessagesSpan</a>&lt;T&gt; GetResponse(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
</ul>





</td>
    </tr>
</table>





