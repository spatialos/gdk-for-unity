
# Improbable.Gdk.ReactiveComponents Namespace
<nav id="pageToc" class="page-toc"><ul><li><a href="#classes">Classes</a>
<li><a href="#structs">Structs</a>
<li><a href="#interfaces">Interfaces</a>
</ul></nav>
<sup>
Namespace: Improbable.Gdk<br/>
GDK package: ReactiveComponents<br />
</sup>


</p>

#### Classes

<table>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/abstract-acknowledge-authority-loss-handler">AbstractAcknowledgeAuthorityLossHandler</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/acknowledge-authority-loss-system">AcknowledgeAuthorityLossSystem</a></td>
<td style="padding: 14px; border: none;">Checks for entities that have acknowledged authority loss during soft-handover and forwards this to SpatialOS. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/authority-changes-provider">AuthorityChangesProvider</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/clean-reactive-components-system">CleanReactiveComponentsSystem</a></td>
<td style="padding: 14px; border: none;">Removes GDK reactive components from all entities </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/command-sender-component-system">CommandSenderComponentSystem</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/component-cleanup-handler">ComponentCleanupHandler</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/reactive-command-component-system">ReactiveCommandComponentSystem</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/reactive-component-send-system">ReactiveComponentSendSystem</a></td>
<td style="padding: 14px; border: none;">Executes the default replication logic for each SpatialOS component. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/reactive-components-helper">ReactiveComponentsHelper</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/reactive-component-system">ReactiveComponentSystem</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/world-commands-clean-system">WorldCommandsCleanSystem</a></td>
<td style="padding: 14px; border: none;">Removes reactive World Command components </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/world-commands-send-system">WorldCommandsSendSystem</a></td>
<td style="padding: 14px; border: none;">Sends World Command requests. </td>
</tr>
</table>



</p>

#### Structs

<table>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/authoritative">Authoritative</a></td>
<td style="padding: 14px; border: none;">ECS component denotes that this worker is authoritative over T. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/authority-changes">AuthorityChanges</a></td>
<td style="padding: 14px; border: none;">ECS Component stores an ordered list of authority changes. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/authority-loss-imminent">AuthorityLossImminent</a></td>
<td style="padding: 14px; border: none;">ECS component denotes that this worker will lose authority over T imminently. If AcknowledgeAuthorityLoss is set then authority handover will complete before the handover timeout. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/component-added">ComponentAdded</a></td>
<td style="padding: 14px; border: none;">ECS component that denotes that a SpatialOS component has just been added. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/component-removed">ComponentRemoved</a></td>
<td style="padding: 14px; border: none;">ECS component that denotes that a SpatialOS component has just been removed. </td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/not-authoritative">NotAuthoritative</a></td>
<td style="padding: 14px; border: none;">ECS component denotes that this worker is not authoritative over T. </td>
</tr>
</table>



</p>

#### Interfaces

<table>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/i-command-sender-component-manager">ICommandSenderComponentManager</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/i-reactive-command-component-manager">IReactiveCommandComponentManager</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/i-reactive-component-manager">IReactiveComponentManager</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
<tr>
<td style="padding: 14px; border: none; width: 39ch"><a href="{{urlRoot}}/api/reactive-components/i-reactive-component-replication-handler">IReactiveComponentReplicationHandler</a></td>
<td style="padding: 14px; border: none;"></td>
</tr>
</table>



