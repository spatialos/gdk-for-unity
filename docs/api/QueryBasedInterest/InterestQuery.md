---
title: InterestQuery Class
slug: api-querybasedinterest-interestquery
order: 2
---

<p><b>Namespace:</b> Improbable.Gdk.QueryBasedInterest<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L11">Source</a></span></p>

</p>


<p>Utility class to help construct ComponentInterest.Query objects. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="query-constraint"></a><b>Query</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L31">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[InterestQuery](doc:api-querybasedinterest-interestquery) Query([Constraint](doc:api-querybasedinterest-constraint) constraint)</code></p>Creates an [InterestQuery](doc:api-querybasedinterest-interestquery). </p><b>Returns:</b></br>An [InterestQuery](doc:api-querybasedinterest-interestquery) object. </p><b>Parameters</b><ul><li><code>[Constraint](doc:api-querybasedinterest-constraint) constraint</code> : A [Constraint](doc:api-querybasedinterest-constraint) object defining the constraints of the query. </li></ul></p><b>Notes:</b><ul><li>Returns the full snapshot result by default. </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="withmaxfrequencyhz-float"></a><b>WithMaxFrequencyHz</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L57">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[InterestQuery](doc:api-querybasedinterest-interestquery) WithMaxFrequencyHz(float frequencyHz)</code></p>Sets the maximum frequency (Hz) of the query. </p><b>Returns:</b></br>An updated [InterestQuery](doc:api-querybasedinterest-interestquery) object. </p><b>Parameters</b><ul><li><code>float frequencyHz</code> : The maximum frequency (Hz) to return query results. </li></ul></p><b>Notes:</b><ul><li>A frequency of 0 means there will be no rate limiting. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="filterresults-uint-params-uint"></a><b>FilterResults</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L83">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[InterestQuery](doc:api-querybasedinterest-interestquery) FilterResults(uint resultComponentId, params uint[] resultComponentIds)</code></p>Defines what components to return in the query results. </p><b>Returns:</b></br>An updated [InterestQuery](doc:api-querybasedinterest-interestquery) object. </p><b>Parameters</b><ul><li><code>uint resultComponentId</code> : First ID of a component to return from the query results. </li><li><code>params uint[] resultComponentIds</code> : Further IDs of components to return from the query results. </li></ul></p><b>Notes:</b><ul><li>At least one component ID must be provided. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="filterresults-ienumerable-uint"></a><b>FilterResults</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L104">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[InterestQuery](doc:api-querybasedinterest-interestquery) FilterResults(IEnumerable&lt;uint&gt; resultComponentIds)</code></p>Defines what components to return in the query results. </p><b>Returns:</b></br>An updated [InterestQuery](doc:api-querybasedinterest-interestquery) object. </p><b>Parameters</b><ul><li><code>IEnumerable&lt;uint&gt; resultComponentIds</code> : Set of IDs of components to return from the query results. </li></ul></p><b>Notes:</b><ul><li>At least one component ID must be provided. Query results are not filtered if resultComponentIds is empty. </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="ascomponentinterestquery"></a><b>AsComponentInterestQuery</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.querybasedinteresthelper/InterestQuery.cs/#L120">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>ComponentInterest.Query AsComponentInterestQuery()</code></p>Returns the underlying ComponentInterest.Query object from the [InterestQuery](doc:api-querybasedinterest-interestquery) class. </td>    </tr></table>



