
# ComponentCallbackSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L13">Source</a>
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
        <td style="border-right:none"><a id="registercomponentupdatecallback-t-entityid-action-t"></a><b>RegisterComponentUpdateCallback&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterComponentUpdateCallback&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, Action&lt;T&gt; callback)</code></p>



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
        <td style="border-right:none"><a id="registercomponenteventcallback-t-entityid-action-t"></a><b>RegisterComponentEventCallback&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L40">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterComponentEventCallback&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, Action&lt;T&gt; callback)</code></p>



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
        <td style="border-right:none"><a id="registerauthoritycallback-entityid-uint-action-authority"></a><b>RegisterAuthorityCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L54">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>ulong RegisterAuthorityCallback(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, uint componentId, Action&lt;Authority&gt; callback)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>uint componentId</code> : </li>
<li><code>Action&lt;Authority&gt; callback</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="unregistercallback-ulong"></a><b>UnregisterCallback</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L67">Source</a></td>
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




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="oncreate"></a><b>OnCreate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L88">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/ComponentCallbackSystem.cs/#L94">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




