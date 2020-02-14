---
title: MobileConnectionFlowInitializer Class
slug: api-mobile-mobileconnectionflowinitializer
order: 8
---

<p><b>Namespace:</b> Improbable.Gdk.Mobile<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L8">Source</a></span></p>



</p>
<p><b>Inheritance</b></p>

<code>[Improbable.Gdk.Core.IConnectionFlowInitializer<ReceptionistFlow>](doc:api-core-iconnectionflowinitializer)</code>
<code>[Improbable.Gdk.Core.IConnectionFlowInitializer<LocatorFlow>](doc:api-core-iconnectionflowinitializer)</code>



</p>
<p><b>Child types</b></p>


[block:parameters]
{
  "data": {
    "h-0": "Name",
    "h-1": "Description",
    "0-0": "[CommandLineSettingsProvider](doc:api-mobile-mobileconnectionflowinitializer-commandlinesettingsprovider)",
    "0-1": "",
    "1-0": "[IMobileSettingsProvider](doc:api-mobile-mobileconnectionflowinitializer-imobilesettingsprovider)",
    "1-1": "",
    "2-0": "[PlayerPrefsSettingsProvider](doc:api-mobile-mobileconnectionflowinitializer-playerprefssettingsprovider)",
    "2-1": ""
  },
  "cols": "2",
  "rows": "3"
}
[/block]









</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="mobileconnectionflowinitializer-params-imobilesettingsprovider"></a><b>MobileConnectionFlowInitializer</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L12">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> MobileConnectionFlowInitializer(params [IMobileSettingsProvider](doc:api-mobile-mobileconnectionflowinitializer-imobilesettingsprovider)[] settingsProviders)</code></p></p><b>Parameters</b><ul><li><code>params [IMobileSettingsProvider](doc:api-mobile-mobileconnectionflowinitializer-imobilesettingsprovider)[] settingsProviders</code> : </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getconnectionservice"></a><b>GetConnectionService</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L17">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>[ConnectionService](doc:api-core-connectionservice) GetConnectionService()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="initialize-receptionistflow"></a><b>Initialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L25">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Initialize([ReceptionistFlow](doc:api-core-receptionistflow) receptionist)</code></p></p><b>Parameters</b><ul><li><code>[ReceptionistFlow](doc:api-core-receptionistflow) receptionist</code> : </li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="initialize-locatorflow"></a><b>Initialize</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.mobile/Utility/MobileConnectionFlowInitializer.cs/#L42">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void Initialize([LocatorFlow](doc:api-core-locatorflow) locator)</code></p></p><b>Parameters</b><ul><li><code>[LocatorFlow](doc:api-core-locatorflow) locator</code> : </li></ul></td>    </tr></table>



