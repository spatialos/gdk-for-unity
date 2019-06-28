
# View Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L8">Source</a>
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
</ul></nav>












</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>View</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L17">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> View()</code></p>






</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>UpdateComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L32">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void UpdateComponent&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, in T update)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>in T update</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetEntityIds</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>HashSet&lt;<a href="{{urlRoot}}/api/core/entity-id">EntityId</a>&gt; GetEntityIds()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasEntity</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasEntity(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L49">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T GetComponent&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasComponent&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasComponent&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasComponent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L71">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool HasComponent(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetAuthority&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L82">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Authority GetAuthority&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetAuthority</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L92">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Authority GetAuthority(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsAuthoritative&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L102">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsAuthoritative&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>IsAuthoritative</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L112">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsAuthoritative(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetWorkerFlag</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/View/View.cs/#L122">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>string GetWorkerFlag(string name)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>string name</code> : </li>
</ul>





</td>
    </tr>
</table>





