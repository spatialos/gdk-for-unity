
# SubscriptionAggregate Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L7">Source</a>
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
        <td style="border-right:none"><a id="subscriptionaggregate-ireadonlylist-type-isubscription"></a><b>SubscriptionAggregate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L17">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> SubscriptionAggregate(IReadOnlyList&lt;Type&gt; types, <a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a>[] subscriptions)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>IReadOnlyList&lt;Type&gt; types</code> : </li>
<li><code><a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a>[] subscriptions</code> : </li>
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
        <td style="border-right:none"><a id="setavailabilityhandler-isubscriptionavailabilityhandler"></a><b>SetAvailabilityHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L29">Source</a></td>
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
        <td style="border-right:none"><a id="getavailabilityhandler"></a><b>GetAvailabilityHandler</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L38">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/subscriptions/i-subscription-availability-handler">ISubscriptionAvailabilityHandler</a> GetAvailabilityHandler()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getvalue-t"></a><b>GetValue&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L43">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>T GetValue&lt;T&gt;()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="geterasedvalue-type"></a><b>GetErasedValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L56">Source</a></td>
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
        <td style="border-right:none"><a id="cancel"></a><b>Cancel</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/SubscriptionAggregate.cs/#L77">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Cancel()</code></p>






</td>
    </tr>
</table>





