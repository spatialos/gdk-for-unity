
# MobileConnectionFlowInitializer Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/mobile-index">Mobile</a><br/>
GDK package: Mobile<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L8">Source</a>
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

<b>Inheritance</b>

<code><a href="{{urlRoot}}/api/core/i-connection-flow-initializer">Improbable.Gdk.Core.IConnectionFlowInitializer&lt;ReceptionistFlow&gt;</a></code>
<code><a href="{{urlRoot}}/api/core/i-connection-flow-initializer">Improbable.Gdk.Core.IConnectionFlowInitializer&lt;LocatorFlow&gt;</a></code>



</p>

<b>Child types</b>

<table>
<tr>
<td style="padding: 14px; border: none; width: 27ch"><a href="{{urlRoot}}/api/mobile/mobile-connection-flow-initializer/command-line-settings-provider">CommandLineSettingsProvider</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 27ch"><a href="{{urlRoot}}/api/mobile/mobile-connection-flow-initializer/i-mobile-settings-provider">IMobileSettingsProvider</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 27ch"><a href="{{urlRoot}}/api/mobile/mobile-connection-flow-initializer/player-prefs-settings-provider">PlayerPrefsSettingsProvider</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
</table>









</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="mobileconnectionflowinitializer-params-imobilesettingsprovider"></a><b>MobileConnectionFlowInitializer</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L12">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> MobileConnectionFlowInitializer(params <a href="{{urlRoot}}/api/mobile/mobile-connection-flow-initializer/i-mobile-settings-provider">IMobileSettingsProvider</a> [] settingsProviders)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code>params <a href="{{urlRoot}}/api/mobile/mobile-connection-flow-initializer/i-mobile-settings-provider">IMobileSettingsProvider</a> [] settingsProviders</code> : </li>
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
        <td style="border-right:none"><a id="getconnectionservice"></a><b>GetConnectionService</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L17">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code><a href="{{urlRoot}}/api/core/connection-service">ConnectionService</a> GetConnectionService()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="initialize-receptionistflow"></a><b>Initialize</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L25">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Initialize(<a href="{{urlRoot}}/api/core/receptionist-flow">ReceptionistFlow</a> receptionist)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/receptionist-flow">ReceptionistFlow</a> receptionist</code> : </li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="initialize-locatorflow"></a><b>Initialize</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/15bb5eac/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L42">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void Initialize(<a href="{{urlRoot}}/api/core/locator-flow">LocatorFlow</a> locator)</code></p>



</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/locator-flow">LocatorFlow</a> locator</code> : </li>
</ul>





</td>
    </tr>
</table>





