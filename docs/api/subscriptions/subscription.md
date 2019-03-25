
# Subscription&lt;T&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L24">Source</a>
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

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/subscriptions/i-subscription">Improbable.Gdk.Subscriptions.ISubscription</a></code>








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>HasValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L27">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> bool HasValue { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>EntityId</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L28">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> EntityId { get; }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnAvailable</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Action&lt;T&gt; OnAvailable {  }</code></p>



</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Value</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L59">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> T Value { get; }</code></p>



</td>
    </tr>
</table>





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>Subscription</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L72">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> Subscription(<a href="{{urlRoot}}/api/subscriptions/subscription-manager-base">SubscriptionManagerBase</a> manager, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/subscription-manager-base">SubscriptionManagerBase</a> manager</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
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
        <td style="border-right:none"><b>Cancel</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L78">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Cancel()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SetAvailable</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L85">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetAvailable(T subscribedObject)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T subscribedObject</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SetUnavailable</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.0/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/Subscription.cs/#L101">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetUnavailable()</code></p>






</td>
    </tr>
</table>





