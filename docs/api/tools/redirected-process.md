
# RedirectedProcess Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/tools-index">Tools</a><br/>
GDK package: Tools<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L52">Source</a>
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



<p>Runs a windowless process. </p>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Command</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L68">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/tools/redirected-process">RedirectedProcess</a> Command(string command)</code></p>
Creates the redirected process for the command. 


</p>

<b>Parameters</b>

<ul>
<li><code>string command</code> : The filename to run.</li>
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
        <td style="border-right:none"><b>WithArgs</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L78">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/tools/redirected-process">RedirectedProcess</a> WithArgs(params string [] arguments)</code></p>
Adds arguments to process command call. 


</p>

<b>Parameters</b>

<ul>
<li><code>params string [] arguments</code> : Parameters that will be passed to the command.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>InDirectory</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L88">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/tools/redirected-process">RedirectedProcess</a> InDirectory(string directory)</code></p>
Sets which directory run the process in. 


</p>

<b>Parameters</b>

<ul>
<li><code>string directory</code> : Working directory of the process.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddOutputProcessing</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L98">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/tools/redirected-process">RedirectedProcess</a> AddOutputProcessing(Action&lt;string&gt; outputProcessor)</code></p>
Adds custom processing for regular output of process. 


</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;string&gt; outputProcessor</code> : Processing action for regular output.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>AddErrorProcessing</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L108">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/tools/redirected-process">RedirectedProcess</a> AddErrorProcessing(Action&lt;string&gt; errorProcessor)</code></p>
Adds custom processing for error output of process. 


</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;string&gt; errorProcessor</code> : Processing action for error output.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RedirectOutputOptions</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L118">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/tools/redirected-process">RedirectedProcess</a> RedirectOutputOptions(<a href="{{urlRoot}}/api/tools/output-redirect-behaviour">OutputRedirectBehaviour</a> redirectBehaviour)</code></p>
Adds custom processing for error output of process. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/tools/output-redirect-behaviour">OutputRedirectBehaviour</a> redirectBehaviour</code> : Options for redirecting process output to Debug.Log().</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Run</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L127">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>int Run()</code></p>
Runs the redirected process and waits for it to return. 





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RunAsync</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.tools/RedirectedProcess.cs/#L251">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>Task&lt;<a href="{{urlRoot}}/api/tools/redirected-process-result">RedirectedProcessResult</a>&gt; RunAsync()</code></p>
Runs the redirected process and returns a task which can be waited on. 
</p><b>Returns:</b></br>A task which would return the exit code and output.




</td>
    </tr>
</table>





