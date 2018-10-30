using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///    Checks for entities that have acknowledged authority loss during soft-handover and forwards this to SpatialOS.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class AcknowledgeAuthorityLossSystem : ComponentSystem
    {
        [Inject] private WorkerSystem workerSystem;

        private readonly List<ComponentAuthorityLossDetails> authorityLossDetails =
            new List<ComponentAuthorityLossDetails>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(AbstractAcknowledgeAuthorityLossHandler).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var authorityLossHandler = (AbstractAcknowledgeAuthorityLossHandler) Activator.CreateInstance(type);

                    authorityLossDetails.Add(new ComponentAuthorityLossDetails
                    {
                        Handler = authorityLossHandler,
                        AuthorityLossGroup = GetComponentGroup(authorityLossHandler.Query)
                    });
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var details in authorityLossDetails)
            {
                Profiler.BeginSample("AcknowledgingAuthorityLoss");
                details.Handler.AcknowledgeAuthorityLoss(details.AuthorityLossGroup, this, workerSystem.Connection);
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
