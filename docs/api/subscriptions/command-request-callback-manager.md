
# CommandRequestCallbackManager&lt;T&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/CalllbackManagers/CommandRequestCallbackManager.cs/#L8">Source</a>
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
        <td style="border-right:none"><b>CommandRequestCallbackManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/CalllbackManagers/CommandRequestCallbackManager.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> CommandRequestCallbackManager(World world)</code></p>



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
        <td style="border-right:none"><b>InvokeCallbacks</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/CalllbackManagers/CommandRequestCallbackManager.cs/#L20">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void InvokeCallbacks()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>RegisterCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/CalllbackManagers/CommandRequestCallbackManager.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterCallback(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, Action&lt;T&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>Action&lt;T&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>UnregisterCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/CalllbackManagers/CommandRequestCallbackManager.cs/#L36">Source</a></td>
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





