
# WrappedOp&lt;T&gt; Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/test-utils-index">TestUtils</a><br/>
GDK package: TestUtils<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L156">Source</a>
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
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>A wrapped Worker Op that implements Dispose so that the allocated native memory is properly freed. Ensure to call Dispose or use <a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a> with a using scope. </p>


</p>

<b>Type parameters</b>

<code>T</code> : The worker op type. E.g. - AddComponentOp


</p>

<b>Inheritance</b>

<code>IDisposable</code>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Op</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L158">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> T Op</code></p>


</td>
    </tr>
</table>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/b136dc2b/workers/unity/Packages/com.improbable.gdk.testutils/WorkerOpFactory.cs/#L165">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





