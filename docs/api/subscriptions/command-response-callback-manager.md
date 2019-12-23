
# CommandResponseCallbackManager&lt;T&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/CommandResponseCallbackManager.cs/#L8">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code>Improbable.Gdk.Subscriptions.ICallbackManager</code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="commandresponsecallbackmanager-world"></a><b>CommandResponseCallbackManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/CommandResponseCallbackManager.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> CommandResponseCallbackManager(World world)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>World world</code> : </li>
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
        <td style="border-right:none"><a id="invokecallbacks"></a><b>InvokeCallbacks</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/CommandResponseCallbackManager.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeCallbacks()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="registercallback-long-action-t"></a><b>RegisterCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/CommandResponseCallbackManager.cs/#L31">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterCallback(long requestId, Action&lt;T&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
<li><code>Action&lt;T&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="unregistercallback-ulong"></a><b>UnregisterCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/CallbackManagers/CommandResponseCallbackManager.cs/#L37">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool UnregisterCallback(ulong callbackKey)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>ulong callbackKey</code> : </li>
</ul>





</td>
    </tr>
</table>





