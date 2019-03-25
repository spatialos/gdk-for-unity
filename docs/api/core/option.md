
# Option&lt;T&gt; Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L12">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-fields">Static Fields</a>
<li><a href="#properties">Properties</a>
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
<li><a href="#operators">Operators</a>
</ul></nav>

</p>



<p>An implementation of <a href="{{urlRoot}}/api/core/option">Option</a> which is compatible with Unity's ECS. </p>


</p>

<b>Type parameters</b>

<code>T</code> : The contained type in the <a href="{{urlRoot}}/api/core/option">Option</a>.


</p>

<b>Inheritance</b>

<code>IEquatable&lt;Option&lt;T&gt;&gt;</code>


</p>

<b>Notes</b>

- This is required because bool is not blittable by default. 




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Empty</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L14">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly <a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; Empty</code></p>


</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> HasValue { get; }</code></p>
True if the <a href="{{urlRoot}}/api/core/option">Option</a> contains a value, false if not. 


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Value</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L29">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> T Value { get; }</code></p>
Returns the value contained inside the <a href="{{urlRoot}}/api/core/option">Option</a>. 


</p>

<b>Exceptions:</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/called-value-on-empty-option-exception">CalledValueOnEmptyOptionException</a></code> : Thrown if the <a href="{{urlRoot}}/api/core/option">Option</a> is empty. </li>
</ul>


</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Option</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L51">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Option(T value)</code></p>
Constructor for an option. 


</p>

<b>Parameters</b>

<ul>
<li><code>T value</code> : The value to be contained in the option </li>
</ul>





</p>

<b>Exceptions:</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/created-option-with-null-payload-exception">CreatedOptionWithNullPayloadException</a></code> : Thrown if the value parameter is null. </li>
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
        <td style="border-right:none"><b>TryGetValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L72">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool TryGetValue(out T outValue)</code></p>
Attempts to get the value contained within the <a href="{{urlRoot}}/api/core/option">Option</a>. 
</p><b>Returns:</b></br>A bool indicating success. 

</p>

<b>Parameters</b>

<ul>
<li><code>out T outValue</code> : When this method returns, contains the value contained within the <a href="{{urlRoot}}/api/core/option">Option</a> if the <a href="{{urlRoot}}/api/core/option">Option</a> is non-empty, otherwise the default value for the type of the outValue parameter. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetValueOrDefault</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L93">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T GetValueOrDefault(T defaultValue)</code></p>
Gets the value within the <a href="{{urlRoot}}/api/core/option">Option</a> or the provided default value if the <a href="{{urlRoot}}/api/core/option">Option</a> is empty. 
</p><b>Returns:</b></br>The value contained within the <a href="{{urlRoot}}/api/core/option">Option</a> or the provided value. 

</p>

<b>Parameters</b>

<ul>
<li><code>T defaultValue</code> : The default value to return if the <a href="{{urlRoot}}/api/core/option">Option</a> is empty. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L103">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Equals(<a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; other)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; other</code> : </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L98">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override bool Equals(object other)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>object other</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetHashCode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L128">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override int GetHashCode()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ToString</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L133">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override string ToString()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Operators


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator==</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L118">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator==(<a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; a, <a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; b)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; a</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; b</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator!=</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L123">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator!=(<a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; a, <a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; b)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; a</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; b</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator Option&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L138">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>implicit operator Option&lt;T&gt;(T value)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T value</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator T</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Components/Option.cs/#L143">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>implicit operator T(<a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; option)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/option">Option</a>&lt;T&gt; option</code> : </li>
</ul>





</td>
    </tr>
</table>



