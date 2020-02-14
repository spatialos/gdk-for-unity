---
title: NetworkStats.NetStats Class
slug: api-core-networkstats-netstats
order: 119
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L12">Source</a></span></p>

</p>


<p>Storage object for network data for a fixed number of frames. </p>




</p>
<p><b>Notes</b></p>

- The underlying data is stored in a ring buffer. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Fields


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="datapoint"></a><b>DataPoint</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L91">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> DataPoint</code></p>Retrieves summary statistics for a given message type for the last numFrames frames. </td>    </tr></table>






</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Constructors


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="netstats-int"></a><b>NetStats</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L26">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code> NetStats(int sequenceLength)</code></p>Creates an instance of network stats storage with a specific size. </p><b>Parameters</b><ul><li><code>int sequenceLength</code> : The maximum number of frames to store data for at any given time. </li></ul></td>    </tr></table>



</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setframestats-netframestats-direction"></a><b>SetFrameStats</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L39">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetFrameStats([NetFrameStats](doc:api-core-networkstats-netframestats) frameData, [Direction](doc:api-core-networkstats-direction) direction)</code></p>Sets the network statistics for a given direction for the current frame. </p><b>Parameters</b><ul><li><code>[NetFrameStats](doc:api-core-networkstats-netframestats) frameData</code> : The network statistics.</li><li><code>[Direction](doc:api-core-networkstats-direction) direction</code> : The direction of those statistics.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="setframetime-float"></a><b>SetFrameTime</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L63">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void SetFrameTime(float dt)</code></p>Sets the frame time for the current frame. </p><b>Parameters</b><ul><li><code>float dt</code> : The frame time.</li></ul></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="finishframe"></a><b>FinishFrame</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L71">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void FinishFrame()</code></p>Finalize data capture for this frame. </td>    </tr></table>



