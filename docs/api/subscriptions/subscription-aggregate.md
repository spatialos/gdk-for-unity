
# SubscriptionAggregate Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L7">Source</a>
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
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SubscriptionAggregate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L16">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> SubscriptionAggregate(<a href="{{urlRoot}}/api/subscriptions/subscription-system">SubscriptionSystem</a> subscriptionSystem, <a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId, params Type [] typesToSubscribeTo)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/subscription-system">SubscriptionSystem</a> subscriptionSystem</code> : </li>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
<li><code>params Type [] typesToSubscribeTo</code> : </li>
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
        <td style="border-right:none"><b>SetAvailabilityHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L31">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetAvailabilityHandler(<a href="{{urlRoot}}/api/subscriptions/i-subscription-availability-handler">ISubscriptionAvailabilityHandler</a> handler)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/i-subscription-availability-handler">ISubscriptionAvailabilityHandler</a> handler</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetAvailabilityHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L40">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/subscriptions/i-subscription-availability-handler">ISubscriptionAvailabilityHandler</a> GetAvailabilityHandler()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetValue&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L45">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T GetValue&lt;T&gt;()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetErasedValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L62">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>object GetErasedValue(Type type)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>Type type</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Cancel</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.2/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L77">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Cancel()</code></p>






</td>
    </tr>
</table>





