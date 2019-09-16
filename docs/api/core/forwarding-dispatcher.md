
# ForwardingDispatcher Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Logging/ForwardingDispatcher.cs/#L11">Source</a>
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
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Forwards logEvents and exceptions to the SpatialOS Console and logs locally. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-log-dispatcher">Improbable.Gdk.Core.ILogDispatcher</a></code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="worker"></a><b>Worker</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Logging/ForwardingDispatcher.cs/#L17">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/worker">Worker</a> Worker { get; set; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="workertype"></a><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Logging/ForwardingDispatcher.cs/#L18">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType { get; set; }</code></p>



</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="forwardingdispatcher-logtype"></a><b>ForwardingDispatcher</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Logging/ForwardingDispatcher.cs/#L33">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> ForwardingDispatcher(LogType minimumLogLevel = LogType.Warning)</code></p>
Constructor for the Forwarding Dispatcher 


</p>

<b>Parameters</b>

<ul>
<li><code>LogType minimumLogLevel</code> : The minimum log level to forward logs to the SpatialOS runtime.</li>
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
        <td style="border-right:none"><a id="handlelog-logtype-logevent"></a><b>HandleLog</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Logging/ForwardingDispatcher.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void HandleLog(LogType type, <a href="{{urlRoot}}/api/core/log-event">LogEvent</a> logEvent)</code></p>
Log locally and conditionally forward to the SpatialOS runtime. 


</p>

<b>Parameters</b>

<ul>
<li><code>LogType type</code> : The type of the log.</li>
<li><code><a href="{{urlRoot}}/api/core/log-event">LogEvent</a> logEvent</code> : A <a href="{{urlRoot}}/api/core/log-event">LogEvent</a> instance.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="dispose"></a><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/Logging/ForwardingDispatcher.cs/#L114">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>
Unregisters callbacks and ensures that the SpatialOS connection is no longer referenced 





</td>
    </tr>
</table>





