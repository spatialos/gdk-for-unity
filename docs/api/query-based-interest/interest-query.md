
# InterestQuery Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/query-based-interest-index">QueryBasedInterest</a><br/>
GDK package: QueryBasedInterest<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L10">Source</a>
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



<p>Utility class to help construct ComponentInterest.Query objects. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Query</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> Query(<a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> constraint)</code></p>
Creates an <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a>. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> constraint</code> : A <a href="{{urlRoot}}/api/query-based-interest/constraint">Constraint</a> object defining the constraints of the query. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>Returns the full snapshot result by default. </li>
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
        <td style="border-right:none"><b>FilterResults</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L78">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> FilterResults(uint resultComponentId, params uint [] resultComponentIds)</code></p>
Defines what components to return in the query results. 
</p><b>Returns:</b></br>An updated <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>uint resultComponentId</code> : First ID of a component to return from the query results. </li>
<li><code>params uint [] resultComponentIds</code> : Further IDs of components to return from the query results. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one component ID must be provided. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>FilterResults</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L99">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> FilterResults(IEnumerable&lt;uint&gt; resultComponentIds)</code></p>
Defines what components to return in the query results. 
</p><b>Returns:</b></br>An updated <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> object. 

</p>

<b>Parameters</b>

<ul>
<li><code>IEnumerable&lt;uint&gt; resultComponentIds</code> : Set of IDs of components to return from the query results. </li>
</ul>



</p>

<b>Notes:</b>

<ul>
<li>At least one component ID must be provided. Query results are not filtered if resultComponentIds is empty. </li>
</ul>




</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AsComponentInterestQuery</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/f54d7cdc/workers/unity/Packages/com.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L115">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ComponentInterest.Query AsComponentInterestQuery()</code></p>
Returns the underlying ComponentInterest.Query object from the <a href="{{urlRoot}}/api/query-based-interest/interest-query">InterestQuery</a> class. 





</td>
    </tr>
</table>





