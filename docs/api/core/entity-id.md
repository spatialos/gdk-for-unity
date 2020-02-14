
# EntityId Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L12">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#fields">Fields</a>
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
<li><a href="#operators">Operators</a>
</ul></nav>

</p>



<p>A unique identifier used to look up an entity in SpatialOS. </p>



</p>

<b>Inheritance</b>

<code>IEquatable&lt;EntityId&gt;</code>


</p>

<b>Notes</b>

- Instances of this type should be treated as transient identifiers that will not be consistent between different runs of the same simulation. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="id"></a><b>Id</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> readonly long Id</code></p>
The value of the <a href="{{urlRoot}}/api/core/entity-id">EntityId</a>. 

</p>

<b>Notes:</b>

<ul>
<li>Though this value is numeric, you should not perform any mathematical operations on it. </li>
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
        <td style="border-right:none"><a id="entityid-long"></a><b>EntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> EntityId(long id)</code></p>
Constructs a new instance of an <a href="{{urlRoot}}/api/core/entity-id">EntityId</a>. 


</p>

<b>Parameters</b>

<ul>
<li><code>long id</code> : </li>
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
        <td style="border-right:none"><a id="isvalid"></a><b>IsValid</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L34">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsValid()</code></p>
Whether this represents a valid SpatialOS entity ID. Specifically, 
</p><b>Returns:</b></br>True iff valid.


</p>

<b>Notes:</b>

<ul>
<li>.</li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="equals-entityid"></a><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L51">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool Equals(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> obj)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> obj</code> : </li>
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
        <td style="border-right:none"><a id="equals-object"></a><b>Equals</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L40">Source</a></td>
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
        <td style="border-right:none"><a id="gethashcode"></a><b>GetHashCode</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L73">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override int GetHashCode()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="tostring"></a><b>ToString</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L81">Source</a></td>
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
        <td style="border-right:none"><a id="operator-entityid-entityid"></a><b>operator==</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L59">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator==(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId1, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId2)</code></p>
Returns true if entityId1 is exactly equal to entityId2. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId1</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId2</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="operator-entityid-entityid"></a><b>operator!=</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/EntityId.cs/#L67">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool operator!=(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId1, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId2)</code></p>
Returns true if entityId1 is not exactly equal to entityId2. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId1</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId2</code> : </li>
</ul>





</td>
    </tr>
</table>



