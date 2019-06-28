
# CommandSystem Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L7">Source</a>
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
        <td style="border-right:none"><b>SendCommand&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L13">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>long SendCommand&lt;T&gt;(T request, Entity sendingEntity)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T request</code> : </li>
<li><code>Entity sendingEntity</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendCommand&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L19">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>long SendCommand&lt;T&gt;(T request)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T request</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>SendResponse&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SendResponse&lt;T&gt;(T response)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>T response</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetRequests&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L30">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/received-messages-span">ReceivedMessagesSpan</a>&lt;T&gt; GetRequests&lt;T&gt;()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetRequests&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L36">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/received-messages-span">ReceivedMessagesSpan</a>&lt;T&gt; GetRequests&lt;T&gt;(<a href="{{urlRoot}}/api/core/entity-id">EntityId</a> targetEntityId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/entity-id">EntityId</a> targetEntityId</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetResponses&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L42">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/received-messages-span">ReceivedMessagesSpan</a>&lt;T&gt; GetResponses&lt;T&gt;()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>GetResponse&lt;T&gt;</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L48">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/received-messages-span">ReceivedMessagesSpan</a>&lt;T&gt; GetResponse&lt;T&gt;(long requestId)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>long requestId</code> : </li>
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
        <td style="border-right:none"><b>OnCreate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L54">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnCreate()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><b>OnUpdate</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.2.4/workers/unity/Packages/com.improbable.gdk.core/Systems/CommandSystem.cs/#L62">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>override void OnUpdate()</code></p>






</td>
    </tr>
</table>




