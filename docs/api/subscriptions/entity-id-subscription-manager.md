
# EntityIdSubscriptionManager Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/subscriptions-index">Subscriptions</a><br/>
GDK package: Subscriptions<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/EntityIdSubscriptionManager.cs/#L7">Source</a>
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
<li><a href="#overrides">Overrides</a>
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/subscriptions/subscription-manager">Improbable.Gdk.Subscriptions.SubscriptionManager&lt;EntityId&gt;</a></code>










</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="entityidsubscriptionmanager-world"></a><b>EntityIdSubscriptionManager</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/EntityIdSubscriptionManager.cs/#L9">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> EntityIdSubscriptionManager(World world)</code></p>



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
#### Overrides


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="subscribe-entityid"></a><b>Subscribe</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/EntityIdSubscriptionManager.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override <a href="{{urlRoot}}/api/subscriptions/subscription">Subscription</a>&lt;<a href="{{urlRoot}}/api/core/entity-id">EntityId</a>&gt; Subscribe(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



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
        <td style="border-right:none"><a id="cancel-isubscription"></a><b>Cancel</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/EntityIdSubscriptionManager.cs/#L21">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void Cancel(<a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription)</code></p>



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
        <td style="border-right:none"><a id="resetvalue-isubscription"></a><b>ResetValue</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.core/Subscriptions/StandardSubscriptionManagers/EntityIdSubscriptionManager.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void ResetValue(<a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/subscriptions/i-subscription">ISubscription</a> subscription</code> : </li>
</ul>





</td>
    </tr>
</table>




