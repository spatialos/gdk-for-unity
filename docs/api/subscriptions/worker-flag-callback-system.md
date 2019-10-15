
# WorkerFlagCallbackSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/WorkerFlagCallbackSystem.cs/#L7">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#methods">Methods</a>
<li><a href="#overrides">Overrides</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code>ComponentSystem</code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="registerworkerflagchangecallback-action-string-string"></a><b>RegisterWorkerFlagChangeCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/WorkerFlagCallbackSystem.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterWorkerFlagChangeCallback(Action&lt;(string, string)&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Action&lt;(string, string)&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="unregisterworkerflagchangecallback-ulong"></a><b>UnregisterWorkerFlagChangeCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/WorkerFlagCallbackSystem.cs/#L24">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool UnregisterWorkerFlagChangeCallback(ulong callbackKey)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>ulong callbackKey</code> : </li>
</ul>





</td>
    </tr>
</table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="oncreate"></a><b>OnCreate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/WorkerFlagCallbackSystem.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnCreate()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="onupdate"></a><b>OnUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/decea028/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/WorkerFlagCallbackSystem.cs/#L34">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




