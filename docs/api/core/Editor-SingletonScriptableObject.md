---
title: Editor.SingletonScriptableObject<TSelf> Class
slug: api-core-editor-singletonscriptableobject
order: 53
---

<p><b>Namespace:</b> Improbable.Gdk.Core<span style="float: right"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L19">Source</a></span></p>

</p>


<p>Base object for a singleton scriptable object. </p>


</p>
<p><b>Type parameters</b></p>

<code>TSelf</code> : The type to make a singleton scriptable object.


</p>
<p><b>Inheritance</b></p>

<code>ScriptableObject</code>


</p>
<p><b>Notes</b></p>

- This differs from Unity's ScriptableSingleton<T> in that: null is returned if no instance of TSelf is found. In Unity's implementation, an instance of TSelf will be created for you. 








</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Static Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="getinstance"></a><b>GetInstance</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L79">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>TSelf GetInstance()</code></p>Finds the instance of TSelf </p><b>Returns:</b></br>The instance of TSelf if one exists, null otherwise.</p><b>Notes:</b><ul><li>An error will be logged if more than one instance is found. If more than one instance is found, only the first is returned. </li></ul></td>    </tr></table>




</p>
<hr style="width:100%; border-top-color:#d8d8d8" />
## Methods


</p>


<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="onenable"></a><b>OnEnable</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L24">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnEnable()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="ondisable"></a><b>OnDisable</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L54">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>void OnDisable()</code></p></td>    </tr></table>
<table class="io-api-doc">    <tr>        <td class="io-api-doc-name"><a id="isanasset"></a><b>IsAnAsset</b></td>        <td class="io-api-doc-source"><a href="https://www.github.com/spatialos/gdk-for-unity/blob/0.3.3/workers/unity/Packages/io.improbable.gdk.core/Editor/SingletonScriptableObject.cs/#L46">Source</a></td>    </tr>    <tr>        <td class="io-api-doc-content" colspan="2"><code>bool IsAnAsset()</code></p></td>    </tr></table>



