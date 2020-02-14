---
title: Option<T> Struct
slug: api-core-option
order: 125
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L13">Source</a></span></p>

</p>


<p>An implementation of [Option](doc:api-core-option) which is compatible with Unity's ECS. </p>


</p>
<p><b>Type parameters</b></p>

<code>T</code> : The contained type in the [Option](doc:api-core-option).


</p>
<p><b>Inheritance</b></p>

<code>IEquatable&lt;Option&lt;T&gt;&gt;</code>


</p>
<p><b>Notes</b></p>

- This is required because bool is not blittable by default. 




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="empty"></a><b>Empty</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L15">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly [Option](doc:api-core-option)&lt;T&gt; Empty</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="hasvalue"></a><b>HasValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L20">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>readonly bool HasValue</code></p>True if the [Option](doc:api-core-option) contains a value, false if not. </td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Properties


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="value"></a><b>Value</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L30">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> T Value { get; }</code></p>Returns the value contained inside the [Option](doc:api-core-option). </p><b>Exceptions:</b><ul><li><code>[EmptyOptionException](doc:api-core-emptyoptionexception)</code> : Thrown if the [Option](doc:api-core-option) is empty. </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="option-t"></a><b>Option</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L49">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> Option(T value)</code></p>Constructor for an option. </p><b>Parameters</b><ul><li><code>T value</code> : The value to be contained in the option </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="trygetvalue-out-t"></a><b>TryGetValue</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L65">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool TryGetValue(out T outValue)</code></p>Attempts to get the value contained within the [Option](doc:api-core-option). </p><b>Returns:</b></br>A bool indicating success. </p><b>Parameters</b><ul><li><code>out T outValue</code> : When this method returns, contains the value contained within the [Option](doc:api-core-option) if the [Option](doc:api-core-option) is non-empty, otherwise the default value for the type of the outValue parameter. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getvalueordefault-t"></a><b>GetValueOrDefault</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L80">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>T GetValueOrDefault(T defaultValue)</code></p>Gets the value within the [Option](doc:api-core-option) or the provided default value if the [Option](doc:api-core-option) is empty. </p><b>Returns:</b></br>The value contained within the [Option](doc:api-core-option) or the provided value. </p><b>Parameters</b><ul><li><code>T defaultValue</code> : The default value to return if the [Option](doc:api-core-option) is empty. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-option-t"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L90">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool Equals([Option](doc:api-core-option)&lt;T&gt; other)</code></p></p><b>Parameters</b><ul><li><code>[Option](doc:api-core-option)&lt;T&gt; other</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Overrides


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="equals-object"></a><b>Equals</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L85">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override bool Equals(object other)</code></p></p><b>Parameters</b><ul><li><code>object other</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="gethashcode"></a><b>GetHashCode</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L115">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override int GetHashCode()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="tostring"></a><b>ToString</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L120">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>override string ToString()</code></p></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Operators


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-option-t-option-t"></a><b>operator==</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L105">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool operator==([Option](doc:api-core-option)&lt;T&gt; a, [Option](doc:api-core-option)&lt;T&gt; b)</code></p></p><b>Parameters</b><ul><li><code>[Option](doc:api-core-option)&lt;T&gt; a</code> : </li><li><code>[Option](doc:api-core-option)&lt;T&gt; b</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-option-t-option-t"></a><b>operator!=</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L110">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool operator!=([Option](doc:api-core-option)&lt;T&gt; a, [Option](doc:api-core-option)&lt;T&gt; b)</code></p></p><b>Parameters</b><ul><li><code>[Option](doc:api-core-option)&lt;T&gt; a</code> : </li><li><code>[Option](doc:api-core-option)&lt;T&gt; b</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-option-t-t"></a><b>operator Option&lt;T&gt;</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L125">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>implicit operator Option&lt;T&gt;(T value)</code></p></p><b>Parameters</b><ul><li><code>T value</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="operator-t-option-t"></a><b>operator T</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Components/Option.cs/#L130">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>implicit operator T([Option](doc:api-core-option)&lt;T&gt; option)</code></p></p><b>Parameters</b><ul><li><code>[Option](doc:api-core-option)&lt;T&gt; option</code> : </li></ul></td>    </tr></table>

