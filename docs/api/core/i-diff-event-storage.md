
# IDiffEventStorage&lt;T&gt; Interface
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L57">Source</a>
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
</ul></nav>



</p>

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-component-diff-storage">Improbable.Gdk.Core.IComponentDiffStorage</a></code>











</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="addevent-componenteventreceived-t"></a><b>AddEvent</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L59">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void AddEvent(<a href="{{urlRoot}}/api/core/component-event-received">ComponentEventReceived</a>&lt;T&gt; ev)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/component-event-received">ComponentEventReceived</a>&lt;T&gt; ev</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getevents"></a><b>GetEvents</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L60">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-event-received">ComponentEventReceived</a>&lt;T&gt;&gt; GetEvents()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getevents-entityid"></a><b>GetEvents</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/51790202/workers/unity/Packages/io.improbable.gdk.core/Worker/DiffStorage.cs/#L61">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/messages-span">MessagesSpan</a>&lt;<a href="{{urlRoot}}/api/core/component-event-received">ComponentEventReceived</a>&lt;T&gt;&gt; GetEvents(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> entityId</code> : </li>
</ul>





</td>
    </tr>
</table>





