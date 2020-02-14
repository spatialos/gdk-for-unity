---
title: Collections.Result<T, E> Struct
slug: api-core-collections-result
order: 5
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L10">Source</a></span></p>

</p>


<p>A type to represent a result. Can either have a success value or an error, but not both. </p>


</p>
<p><b>Type parameters</b></p>

<code>T</code> : The type of the success value.
<code>E</code> : The type of the error.







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isokay"></a><b>IsOkay</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsOkay</code></p>True if the result contains a success, false otherwise. </td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="iserror"></a><b>IsError</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsError</code></p>True if the result contains an error, false otherwise. </td>    </tr></table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="ok-t"></a><b>Ok</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L32">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[Result](doc:api-core-collections-result)&lt;T, E&gt; Ok(T value)</code></p>Creates a result which contains a success value. </p><b>Returns:</b></br>The result object.</p><b>Parameters</b><ul><li><code>T value</code> : The value of the result.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="error-e"></a><b>Error</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L46">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[Result](doc:api-core-collections-result)&lt;T, E&gt; Error(E error)</code></p>Creates a result which contains an error. </p><b>Returns:</b></br>The result object.</p><b>Parameters</b><ul><li><code>E error</code> : The value of the error.</li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="unwrap"></a><b>Unwrap</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L60">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T Unwrap()</code></p>Attempts to get the success value from the result. </p><b>Returns:</b></br>The success value of the result.</p><b>Exceptions:</b><ul><li><code>InvalidOperationException</code> : Thrown if the result contains an error.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="unwraperror"></a><b>UnwrapError</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Collections/Result.cs/#L75">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>E UnwrapError()</code></p>Attempts to get the error from the result. </p><b>Returns:</b></br>The error from the result.</p><b>Exceptions:</b><ul><li><code>InvalidOperationException</code> : Thrown if result contains a success value.</li></ul></td>    </tr></table>



