using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///    Checks for entities that have acknowledged authority loss during soft-handover and forwards this to SpatialOS.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class AcknowledgeAuthorityLossSystem : ComponentSystem
    {
        private readonly List<ComponentAuthorityLossDetails> authorityLossDetails =
            new List<ComponentAuthorityLossDetails>();

        private ComponentUpdateSystem updateSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            updateSystem = World.GetExistingManager<ComponentUpdateSystem>();
            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(AbstractAcknowledgeAuthorityLossHandler)))
            {
                var authorityLossHandler = (AbstractAcknowledgeAuthorityLossHandler) Activator.CreateInstance(type);

                authorityLossDetails.Add(new ComponentAuthorityLossDetails
                {
                    Handler = authorityLossHandler,
                    AuthorityLossGroup = GetComponentGroup(authorityLossHandler.Query)
                });
            }
        }

        protected override void OnUpdate()
        {
            foreach (var details in authorityLossDetails)
            {
                Profiler.BeginSample("AcknowledgingAuthorityLoss");
                details.Handler.AcknowledgeAuthorityLoss(details.AuthorityLossGroup, this, updateSystem);
                Profiler.EndSample();
            }
        }

        private struct ComponentAuthorityLossDetails
        {
            public AbstractAcknowledgeAuthorityLossHandler Handler;
            public ComponentGroup AuthorityLossGroup;
        }
    }
}
