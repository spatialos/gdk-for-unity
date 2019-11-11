
# InterestTemplate Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/query-based-interest-index">QueryBasedInterest</a><br/>
GDK package: QueryBasedInterest<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L11">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Utility class to help construct Interest component snapshots. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="create"></a><b>Create</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> Create()</code></p>
Creates a new <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 
</p><b>Returns:</b></br>A new <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="create-interesttemplate"></a><b>Create</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> Create(<a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> interestTemplate)</code></p>
Creates a new <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object given an existing <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a>. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> interestTemplate</code> : An existing <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a>. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>The underlying data is deep copied. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="create-dictionary-uint-componentinterest"></a><b>Create</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> Create(Dictionary&lt;uint, ComponentInterest&gt; interest)</code></p>
Creates a new <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object from the content of an existing Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>Dictionary&lt;uint, ComponentInterest&gt; interest</code> : The underlying dictionary of an Interest component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>The underlying data is deep copied. </li>
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
        <td style="border-right:none"><a id="addqueries-t-interestquery-params-interestquery"></a><b>AddQueries&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L103">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> AddQueries&lt;T&gt;(<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery, params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries)</code></p>
Add InterestQueries to the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery</code> : First <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> to add for a given authoritative component. </li>
<li><code>params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries</code> : Further InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to update the Interest component. </li>
</ul>



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : Type of the authoritative component to add the InterestQueries to. </li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addqueries-uint-interestquery-params-interestquery"></a><b>AddQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L128">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> AddQueries(uint componentId, <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery, params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries)</code></p>
Add InterestQueries to the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : Component ID of the authoritative component to add the InterestQueries to. </li>
<li><code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery</code> : First <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> to add for a given authoritative component. </li>
<li><code>params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries</code> : Further InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to update the Interest component. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addqueries-t-ienumerable-interestquery"></a><b>AddQueries&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L150">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> AddQueries&lt;T&gt;(IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries)</code></p>
Add InterestQueries to the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries</code> : Set of InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to update the Interest component. </li>
</ul>



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : Type of the authoritative component to add the InterestQueries to. </li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addqueries-uint-ienumerable-interestquery"></a><b>AddQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L172">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> AddQueries(uint componentId, IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries)</code></p>
Add InterestQueries to the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : Component ID of the authoritative component to add the InterestQueries to. </li>
<li><code>IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries</code> : Set of InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to update the Interest component. No queries are added if interestQueries is empty. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="replacequeries-t-interestquery-params-interestquery"></a><b>ReplaceQueries&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L216">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ReplaceQueries&lt;T&gt;(<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery, params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries)</code></p>
Replaces a component's InterestQueries in the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery</code> : First <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> to add for a given authoritative component. </li>
<li><code>params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries</code> : Further InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to replace a component's interest. </li>
</ul>



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : Type of the authoritative component to replace InterestQueries of. </li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="replacequeries-uint-interestquery-params-interestquery"></a><b>ReplaceQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L241">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ReplaceQueries(uint componentId, <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery, params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries)</code></p>
Replaces a component's InterestQueries in the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : Component ID of the authoritative component to replace InterestQueries of. </li>
<li><code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> interestQuery</code> : First <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> to add for a given authoritative component. </li>
<li><code>params <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>[] interestQueries</code> : Further InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to replace a component's interest. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="replacequeries-t-ienumerable-interestquery"></a><b>ReplaceQueries&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L263">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ReplaceQueries&lt;T&gt;(IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries)</code></p>
Replaces a component's InterestQueries in the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries</code> : Set of InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to replace a component's interest. </li>
</ul>



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : Type of the authoritative component to replace InterestQueries of. </li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="replacequeries-uint-ienumerable-interestquery"></a><b>ReplaceQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L285">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ReplaceQueries(uint componentId, IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries)</code></p>
Replaces a component's InterestQueries in the Interest component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : Component ID of the authoritative component to replace InterestQueries of. </li>
<li><code>IEnumerable&lt;<a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>&gt; interestQueries</code> : Set of InterestQueries to add for a given authoritative component. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> must be provided to replace a component's interest. No queries are replaced if interestQueries is empty. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="clearqueries-t"></a><b>ClearQueries&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L322">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ClearQueries&lt;T&gt;()</code></p>
Clears all InterestQueries for a given authoritative component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 



</p>

<b>Type parameters:</b>

<ul>
<li><code>T</code> : Type of the authoritative component to clear InterestQueries from. </li>
</ul>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="clearqueries-uint"></a><b>ClearQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L337">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ClearQueries(uint componentId)</code></p>
Clears all InterestQueries for a given authoritative component. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint componentId</code> : Component ID of the authoritative component to clear InterestQueries from. </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="clearallqueries"></a><b>ClearAllQueries</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L349">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> ClearAllQueries()</code></p>
Clears all InterestQueries. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-template">InterestTemplate</a> object. 




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="tosnapshot"></a><b>ToSnapshot</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L361">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Interest.Snapshot ToSnapshot()</code></p>
Builds the Interest snapshot. 
</p><b>Returns:</b></br>A Interest.Snapshot object. 




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="ascomponentinterest"></a><b>AsComponentInterest</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/180a1fc2/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestTemplate.cs/#L372">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Dictionary&lt;uint, ComponentInterest&gt; AsComponentInterest()</code></p>
Returns the underlying data of an Interest component. 
</p><b>Returns:</b></br>A Dictionary<uint, ComponentInterest>. 




</td>
    </tr>
</table>





