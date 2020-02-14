---
title: MiniJSON.Json Class
slug: api-tools-minijson-json
order: 8
---

<p><b>Namespace:</b> Improbable.Gdk.Tools<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/MiniJson.cs/#L79">Source</a></span></p>

</p>


<p>This class encodes and decodes JSON strings. Spec. details, see  JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary. All numbers are parsed to doubles. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="deserialize-string"></a><b>Deserialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/MiniJson.cs/#L86">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Dictionary&lt;string, object&gt; Deserialize(string json)</code></p>Parses the string json into a value </p><b>Returns:</b></br>An List<object>, a Dictionary<string, object>, a double, an integer,a string, null, true, or false</p><b>Parameters</b><ul><li><code>string json</code> : A JSON string.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="serialize-object"></a><b>Serialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.tools/MiniJson.cs/#L445">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>string Serialize(object obj)</code></p>Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string </p><b>Returns:</b></br>A JSON encoded string, or null if object 'json' is not serializable</p><b>Parameters</b><ul><li><code>object obj</code> : </li></ul></td>    </tr></table>





