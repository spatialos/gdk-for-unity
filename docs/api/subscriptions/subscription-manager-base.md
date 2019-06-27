
# SubscriptionManagerBase Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionManagerBase.cs/#L6">Source</a>
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
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Properties


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SubscriptionType</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionManagerBase.cs/#L8">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> abstract Type SubscriptionType { get; }</code></p>



</td>
    </tr>
</table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><b>SubscribeTypeErased</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionManagerBase.cs/#L10">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract <a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> SubscribeTypeErased(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>Cancel</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionManagerBase.cs/#L11">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract void Cancel(<a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>ResetValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/84243525d98aff511e7aa1f7703c37347017e386/workers/unity/Packages/com.improbable.gdk.core/Subscriptions/SubscriptionManagerBase.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>abstract void ResetValue(<a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription</code> : </li>
</ul>





</td>
    </tr>
</table>





