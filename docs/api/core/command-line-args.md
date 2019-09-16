
# CommandLineArgs Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L6">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
</ul></nav>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="fromcommandline"></a><b>FromCommandLine</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/command-line-args">CommandLineArgs</a> FromCommandLine()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="from-dictionary-string-string"></a><b>From</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/command-line-args">CommandLineArgs</a> From(Dictionary&lt;string, string&gt; args)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Dictionary&lt;string, string&gt; args</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="from-ilist-string"></a><b>From</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L28">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/command-line-args">CommandLineArgs</a> From(IList&lt;string&gt; args)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>IList&lt;string&gt; args</code> : </li>
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
        <td style="border-right:none"><a id="contains-string"></a><b>Contains</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Contains(string key)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string key</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getcommandlinevalue-t-string-t"></a><b>GetCommandLineValue&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L41">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T GetCommandLineValue&lt;T&gt;(string key, T defaultValue)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string key</code> : </li>
<li><code>T defaultValue</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="trygetcommandlinevalue-t-string-ref-t"></a><b>TryGetCommandLineValue&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L48">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool TryGetCommandLineValue&lt;T&gt;(string key, ref T value)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string key</code> : </li>
<li><code>ref T value</code> : </li>
</ul>





</td>
    </tr>
</table>





