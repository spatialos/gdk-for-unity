---
title: CommandLineArgs Class
slug: api-core-commandlineargs
order: 7
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L6">Source</a></span></p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="fromcommandline"></a><b>FromCommandLine</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[CommandLineArgs](doc:api-core-commandlineargs) FromCommandLine()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="from-dictionary-string-string"></a><b>From</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[CommandLineArgs](doc:api-core-commandlineargs) From(Dictionary&lt;string, string&gt; args)</code></p></p><b>Parameters</b><ul><li><code>Dictionary&lt;string, string&gt; args</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="from-ilist-string"></a><b>From</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L28">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[CommandLineArgs](doc:api-core-commandlineargs) From(IList&lt;string&gt; args)</code></p></p><b>Parameters</b><ul><li><code>IList&lt;string&gt; args</code> : </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="contains-string"></a><b>Contains</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Contains(string key)</code></p></p><b>Parameters</b><ul><li><code>string key</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getcommandlinevalue-t-string-t"></a><b>GetCommandLineValue&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L41">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T GetCommandLineValue&lt;T&gt;(string key, T defaultValue)</code></p></p><b>Parameters</b><ul><li><code>string key</code> : </li><li><code>T defaultValue</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetcommandlinevalue-t-string-ref-t"></a><b>TryGetCommandLineValue&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Utility/CommandLineUtility.cs/#L48">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetCommandLineValue&lt;T&gt;(string key, ref T value)</code></p></p><b>Parameters</b><ul><li><code>string key</code> : </li><li><code>ref T value</code> : </li></ul></td>    </tr></table>



