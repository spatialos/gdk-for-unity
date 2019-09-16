
# NetStats Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/network-stats-index">NetworkStats</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L12">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#fields">Fields</a>
<li><a href="#constructors">Constructors</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Storage object for network data for a fixed number of frames. </p>




</p>

<b>Notes</b>

- The underlying data is stored in a ring buffer. 





</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Fields


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="datapoint"></a><b>DataPoint</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L91">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>  DataPoint</code></p>
Retrieves summary statistics for a given message type for the last numFrames frames. 

</td>
    </tr>
</table>







</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Constructors


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="netstats-int"></a><b>NetStats</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L26">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code> NetStats(int sequenceLength)</code></p>
Creates an instance of network stats storage with a specific size. 


</p>

<b>Parameters</b>

<ul>
<li><code>int sequenceLength</code> : The maximum number of frames to store data for at any given time. </li>
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
        <td style="border-right:none"><a id="setframestats-netframestats-direction"></a><b>SetFrameStats</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L39">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetFrameStats(<a href="{{urlRoot}}/api/core/network-stats/net-frame-stats">NetFrameStats</a> frameData, <a href="{{urlRoot}}/api/core/network-stats/direction">Direction</a> direction)</code></p>
Sets the network statistics for a given direction for the current frame. 


</p>

<b>Parameters</b>

<ul>
<li><code><a href="{{urlRoot}}/api/core/network-stats/net-frame-stats">NetFrameStats</a> frameData</code> : The network statistics.</li>
<li><code><a href="{{urlRoot}}/api/core/network-stats/direction">Direction</a> direction</code> : The direction of those statistics.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="setframetime-float"></a><b>SetFrameTime</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L63">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void SetFrameTime(float dt)</code></p>
Sets the frame time for the current frame. 


</p>

<b>Parameters</b>

<ul>
<li><code>float dt</code> : The frame time.</li>
</ul>





</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="finishframe"></a><b>FinishFrame</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/88a422dc255ef1d47ee9385f226ca439f31c000b/workers/unity/Packages/io.improbable.gdk.core/NetworkStats/NetStats.cs/#L71">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void FinishFrame()</code></p>
Finalize data capture for this frame. 





</td>
    </tr>
</table>





