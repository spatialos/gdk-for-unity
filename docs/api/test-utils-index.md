
# Improbable.Gdk.TestUtils Namespace
<nav id="pageToc" class="page-toc"><ul><li><a href="#classes">Classes</a>
<li><a href="#structs">Structs</a>
</ul></nav>
<sup>
Namespace: Improbable.Gdk<br/>
GDK package: TestUtils<br />
</sup>


</p>

#### Classes

<table>
<tr>
<td style="padding: 14px; border: none; width: 23ch"><a href="{{urlRoot}}/api/test-utils/hybrid-gdk-system-test-base">HybridGdkSystemTestBase</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 23ch"><a href="{{urlRoot}}/api/test-utils/test-log-dispatcher">TestLogDispatcher</a></td>
<td style="padding: 14px; border: none;">A ILogDispatcher implementation designed to be used in testing. This replaces the LogAssert approach with a more specialised one. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 23ch"><a href="{{urlRoot}}/api/test-utils/test-mono-behaviour">TestMonoBehaviour</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 23ch"><a href="{{urlRoot}}/api/test-utils/worker-op-factory">WorkerOpFactory</a></td>
<td style="padding: 14px; border: none;">A static class that contains helper methods for constructing ops. All ops are empty outside of the required information given in the constructor. Underlying schema data can be populated using the return value of each function. </td>
</tr>
</table>



</p>

#### Structs

<table>
<tr>
<td style="padding: 14px; border: none; width: 23ch"><a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a></td>
<td style="padding: 14px; border: none;">A wrapped Worker Op that implements Dispose so that the allocated native memory is properly freed. Ensure to call Dispose or use <a href="{{urlRoot}}/api/test-utils/wrapped-op">WrappedOp</a> with a using scope. </td>
</tr>
</table>




