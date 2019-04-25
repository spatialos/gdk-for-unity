
# TestLogDispatcher Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/test-utils-index">TestUtils</a><br/>
GDK package: TestUtils<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L18">Source</a>
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



<p>A ILogDispatcher implementation designed to be used in testing. This replaces the LogAssert approach with a more specialised one. </p>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-log-dispatcher">Improbable.Gdk.Core.ILogDispatcher</a></code>


</p>

<b>Notes</b>

- The expected usage is to use EnterExpectingScope() with a using block. This methods returns a Disposable object which you can mark logs as expected. When the object is disposed - it will assert against any logs. 


</p>

<b>Child types</b>

<table>
<tr>
<td style="padding: 14px; border: none; width: 14ch"><a href="{{urlRoot}}/api/test-utils/test-log-dispatcher/expecting-scope">ExpectingScope</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Connection</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L22">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Connection Connection { get; set; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>WorkerType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> string WorkerType { get; set; }</code></p>



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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L25">Source</a></td>
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


<table width="100%">
    <tr>
        <td style="border-right:none"><b>EnterExpectingScope</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L44">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/test-utils/test-log-dispatcher/expecting-scope">ExpectingScope</a> EnterExpectingScope()</code></p>
Creates and returns an disposable <a href="{{urlRoot}}/api/test-utils/test-log-dispatcher/expecting-scope">ExpectingScope</a> object. This is intended to be used with a using block. 
</p><b>Returns:</b></br>An <a href="{{urlRoot}}/api/test-utils/test-log-dispatcher/expecting-scope">ExpectingScope</a> instance.




</p>

<b>Exceptions:</b>

<ul>
<li><code>InvalidOperationException</code> : Throws if you already have an un-disposed <a href="{{urlRoot}}/api/test-utils/test-log-dispatcher/expecting-scope">ExpectingScope</a> from this logger. </li>
</ul>


</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ExitExpectingScope</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L56">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void ExitExpectingScope()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Dispose</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.1/workers/unity/Packages/com.improbable.gdk.testutils/TestLogDispatcher.cs/#L67">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Dispose()</code></p>






</td>
    </tr>
</table>





