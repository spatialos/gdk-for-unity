
# ILogDispatcher Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L11">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#properties">Properties</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>The <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> interface is used to implement different types of loggers. By default, the <a href="{{urlRoot}}/api/core/i-log-dispatcher">ILogDispatcher</a> supports structured logging. </p>



</p>

<b>Inheritance</b>

<code>IDisposable</code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Worker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L16">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/worker">Worker</a> Worker { get; set; }</code></p>
The associated GDK <a href="{{urlRoot}}/api/core/worker">Worker</a>. 


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType { get; set; }</code></p>
The worker type associated with this logger. 


</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>HandleLog</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/06858069/workers/unity/Packages/io.improbable.gdk.core/Logging/ILogDispatcher.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void HandleLog(LogType type, <a href="{{urlRoot}}/api/core/log-event">LogEvent</a> logEvent)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>LogType type</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/log-event">LogEvent</a> logEvent</code> : </li>
</ul>





</td>
    </tr>
</table>





