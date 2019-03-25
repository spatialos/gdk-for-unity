
# CommandCallbackSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L13">Source</a>
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
        <td style="border-right:none"><b>RegisterCommandRequestCallback&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L23">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterCommandRequestCallback&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, Action&lt;T&gt; callback)</code></p>



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
        <td style="border-right:none"><b>RegisterCommandResponseCallback&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L37">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterCommandResponseCallback&lt;T&gt;(long requestId, Action&lt;T&gt; callback)</code></p>



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
        <td style="border-right:none"><b>UnregisterCommandRequestCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L51">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool UnregisterCommandRequestCallback(ulong callbackKey)</code></p>



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
        <td style="border-right:none"><b>OnCreateManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L67">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnCreateManager()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Systems/CommandCallbackSystem.cs/#L73">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




