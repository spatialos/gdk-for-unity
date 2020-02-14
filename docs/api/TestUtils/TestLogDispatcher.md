---
title: TestLogDispatcher Class
slug: api-testutils-testlogdispatcher
order: 7
---

<p><b>Namespace:</b> Improbable.Gdk.TestUtils<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L17">Source</a></span></p>

</p>


<p>A ILogDispatcher implementation designed to be used in testing. This replaces the LogAssert approach with a more specialised one. </p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.ILogDispatcher](doc:api-core-ilogdispatcher)</code>


</p>
<p><b>Notes</b></p>

- The expected usage is to use EnterExpectingScope() with a using block. This methods returns a Disposable object which you can mark logs as expected. When the object is disposed - it will assert against any logs.


</p>
<p><b>Child types</b></p>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[ExpectingScope](doc:api-testutils-testlogdispatcher-expectingscope)",
    "0-1": ""
  },
  "cols": "2",
  "rows": "1"
}
[/block]







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="worker"></a><b>Worker</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L21">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> [Core.Worker](doc:api-core-worker) Worker { get; set; }</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="workertype"></a><b>WorkerType</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L22">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> string WorkerType { get; set; }</code></p></td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="handlelog-logtype-logevent"></a><b>HandleLog</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void HandleLog(LogType type, [LogEvent](doc:api-core-logevent) logEvent)</code></p></p><b>Parameters</b><ul><li><code>LogType type</code> : </li><li><code>[LogEvent](doc:api-core-logevent) logEvent</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="enterexpectingscope"></a><b>EnterExpectingScope</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L43">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[ExpectingScope](doc:api-testutils-testlogdispatcher-expectingscope) EnterExpectingScope()</code></p>Creates and returns an disposable [ExpectingScope](doc:api-testutils-testlogdispatcher-expectingscope) object. This is intended to be used with a using block. </p><b>Returns:</b></br>An [ExpectingScope](doc:api-testutils-testlogdispatcher-expectingscope) instance.</p><b>Exceptions:</b><ul><li><code>InvalidOperationException</code> : Throws if you already have an un-disposed [ExpectingScope](doc:api-testutils-testlogdispatcher-expectingscope) from this logger. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="exitexpectingscope"></a><b>ExitExpectingScope</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L55">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void ExitExpectingScope()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="dispose"></a><b>Dispose</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.testutils/TestLogDispatcher.cs/#L66">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Dispose()</code></p></td>    </tr></table>



