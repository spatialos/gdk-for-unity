
# LocatorConfig Struct
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Config/RuntimeConfig.cs/#L41">Source</a>
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
</ul></nav>

</p>



<p>Stores the configuration needed to connect via the Lcoator. </p>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>LocatorHost</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Config/RuntimeConfig.cs/#L46">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string LocatorHost</code></p>
The host to connect to the Locator. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>LocatorParameters</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Config/RuntimeConfig.cs/#L51">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> LocatorParameters LocatorParameters</code></p>
The parameters needed to connect to the Locator. 

</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>DeploymentListCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Config/RuntimeConfig.cs/#L57">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Func&lt;DeploymentList, string&gt; DeploymentListCallback</code></p>
A function that takes as input a list of available deployments and returns the name of the deployment that we want to connect to via the Locator. 

</td>
    </tr>
</table>










