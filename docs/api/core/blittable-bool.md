
# BlittableBool Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L11">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
<li><a href="#operators">Operators</a>
</ul></nav>

</p>



<p>A blittable bool implementation to use in Unity's ECS. </p>



</p>

<b>Inheritance</b>

<code>IEquatable&lt;BlittableBool&gt;</code>


</p>

<b>Notes</b>

- Can be used in place of bool where a blittable type is needed. 









</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>BlittableBool</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> BlittableBool(bool value)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>bool value</code> : </li>
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
        <td style="border-right:none"><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Equals(<a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> other)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> other</code> : </li>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override bool Equals(object obj)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>object obj</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetHashCode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L45">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L60">Source</a></td>
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
        <td style="border-right:none"><b>operator bool</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>implicit operator bool(<a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> blittableBool)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> blittableBool</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator BlittableBool</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>implicit operator BlittableBool(bool value)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>bool value</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator==</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L50">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator==(<a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> left, <a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> right)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> left</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> right</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>operator!=</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/develop/workers/unity/Packages/com.improbable.gdk.core/Utility/BlittableBool.cs/#L55">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator!=(<a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> left, <a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> right)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> left</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/blittable-bool">BlittableBool</a> right</code> : </li>
</ul>





</td>
    </tr>
</table>



