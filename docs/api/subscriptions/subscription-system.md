
# SubscriptionSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L10">Source</a>
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
        <td style="border-right:none"><a id="registersubscriptionmanager-type-subscriptionmanagerbase"></a><b>RegisterSubscriptionManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L15">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void RegisterSubscriptionManager(Type type, <a href="{{urlRoot}}/api/subscriptions/subscription-manager-base">SubscriptionManagerBase</a> manager)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Type type</code> : </li>
<li><code><a href="{{urlRoot}}/api/subscriptions/subscription-manager-base">SubscriptionManagerBase</a> manager</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="subscribe-t-entityid"></a><b>Subscribe&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/subscriptions/subscription">Subscription</a>&lt;T&gt; Subscribe&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entity)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entity</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="subscribe-entityid-type"></a><b>Subscribe</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L35">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> Subscribe(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entity, Type type)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entity</code> : </li>
<li><code>Type type</code> : </li>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L45">Source</a></td>
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
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/c62f1703b591ee684fba123ba0dc6c231eca5126/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/Systems/SubscriptionSystem.cs/#L53">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




