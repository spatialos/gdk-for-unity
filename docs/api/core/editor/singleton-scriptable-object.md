
# SingletonScriptableObject&lt;TSelf&gt; Class
<sup>
Namespace: Improbable.Gdk.<a href="{{urlRoot}}/api/core-index">Core</a>.<a href="{{urlRoot}}/api/core/editor-index">Editor</a><br/>
GDK package: Core<br/>
<a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L19">Source</a>
<style>
a code {
                    padding: 0em 0.25em!important;
}
code {
                    background-color: #ffffff!important;
}
</style>
</sup>
<nav id="pageToc" class="page-toc"><ul><li><a href="#static-methods">Static Methods</a>
<li><a href="#methods">Methods</a>
</ul></nav>

</p>



<p>Base object for a singleton scriptable object. </p>


</p>

<b>Type parameters</b>

<code>TSelf</code> : The type to make a singleton scriptable object.


</p>

<b>Inheritance</b>

<code>ScriptableObject</code>


</p>

<b>Notes</b>

- This differs from Unity's ScriptableSingleton<T> in that: null is returned if no instance of TSelf is found. In Unity's implementation, an instance of TSelf will be created for you. 








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
#### Static Methods


</p>




<table width="100%">
    <tr>
        <td style="border-right:none"><a id="getinstance"></a><b>GetInstance</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L79">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>TSelf GetInstance()</code></p>
Finds the instance of TSelf 
</p><b>Returns:</b></br>The instance of TSelf if one exists, null otherwise.


</p>

<b>Notes:</b>

<ul>
<li>An error will be logged if more than one instance is found. If more than one instance is found, only the first is returned. </li>
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
        <td style="border-right:none"><a id="onenable"></a><b>OnEnable</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L24">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnEnable()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="ondisable"></a><b>OnDisable</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L54">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>void OnDisable()</code></p>






</td>
    </tr>
</table>


<table width="100%">
    <tr>
        <td style="border-right:none"><a id="isanasset"></a><b>IsAnAsset</b></td>
        <td style="border-left:none; text-align:right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/3a2a2965/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L46">Source</a></td>
    </tr>
    <tr>
        <td colspan="2">
<code>bool IsAnAsset()</code></p>






</td>
    </tr>
</table>





