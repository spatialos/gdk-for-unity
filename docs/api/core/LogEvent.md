---
title: LogEvent Struct
slug: api-core-logevent
order: 104
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L10">Source</a></span></p>

</p>


<p>Represents a single log. Can contain data used for structured logging. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="message"></a><b>Message</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly string Message</code></p>The main content of the log. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="data"></a><b>Data</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly Dictionary&lt;string, object&gt; Data</code></p>The data used for structured logging. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="context"></a><b>Context</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>UnityEngine.Object Context</code></p>Optional context object used with Unity logging. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="exception"></a><b>Exception</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>Exception Exception</code></p>An exception if the [LogEvent](doc:api-core-logevent) is associated with an exception. </td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="logevent-string"></a><b>LogEvent</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L36">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> LogEvent(string message)</code></p>Constructor for the log event </p><b>Parameters</b><ul><li><code>string message</code> : The log content.</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="withfield-string-object"></a><b>WithField</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L50">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[LogEvent](doc:api-core-logevent) WithField(string key, object value)</code></p>Sets additional information to be displayed with the log message. </p><b>Returns:</b></br>Itself</p><b>Parameters</b><ul><li><code>string key</code> : The key.</li><li><code>object value</code> : The value.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="withcontext-unityengine-object"></a><b>WithContext</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L62">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[LogEvent](doc:api-core-logevent) WithContext(UnityEngine.Object context)</code></p>Adds a context object to be passed as the second parameter into UnityEngine.Debug.Log(object, UnityEngine.Object) </p><b>Returns:</b></br>Itself</p><b>Parameters</b><ul><li><code>UnityEngine.Object context</code> : The context object</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="withexception-exception"></a><b>WithException</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L73">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[LogEvent](doc:api-core-logevent) WithException(Exception exception)</code></p>Associates an exception to the [LogEvent](doc:api-core-logevent). </p><b>Returns:</b></br>Itself</p><b>Parameters</b><ul><li><code>Exception exception</code> : The exception</li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tostring"></a><b>ToString</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Logging/LogEvent.cs/#L79">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override string ToString()</code></p></td>    </tr></table>


