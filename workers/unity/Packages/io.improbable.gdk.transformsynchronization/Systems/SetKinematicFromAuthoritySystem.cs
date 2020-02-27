using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(ResetForAuthorityGainedSystem))]
    public class SetKinematicFromAuthoritySystem : ComponentSystem
    {
        private ComponentUpdateSystem updateSystem;

        private ComponentType[] initBaseComponentTypes;
        private ComponentType[] authBaseComponentTypes;

        private readonly Dictionary<Type, Action> initKinematicActions = new Dictionary<Type, Action>();
        private readonly Dictionary<Type, Action> authKinematicActions = new Dictionary<Type, Action>();

        internal delegate void AuthChangeFunc<in T>(ref KinematicStateWhenAuth state, AuthorityChangeReceived auth,
            T component)
            where T : class;

        protected override void OnCreate()
        {
            base.OnCreate();

            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            initBaseComponentTypes = new[]
            {
                ComponentType.ReadWrite<KinematicStateWhenAuth>(),
                ComponentType.ReadOnly<NewlyAddedSpatialOSEntity>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            };

            authBaseComponentTypes = new[]
            {
                ComponentType.ReadWrite<KinematicStateWhenAuth>(),
                ComponentType.ReadOnly<SpatialEntityId>(),
            };

            RegisterTransformSyncType(new RigidbodyTransformSync());
        }

        internal void RegisterTransformSyncType<T>(ITransformSync<T> impl)
            where T : class
        {
            CreateInitAction((EntityQueryBuilder.F_DC<KinematicStateWhenAuth, T>) impl.InitKinematicState);
            CreateAuthChangeAction((AuthChangeFunc<T>) impl.ApplyKinematicStateOnAuthChange);
        }

        private void CreateInitAction<T>(EntityQueryBuilder.F_DC<KinematicStateWhenAuth, T> initFunc)
            where T : class
        {
            var entityQuery = GetEntityQuery(TransformUtils.ConstructEntityQueryDesc<T>(initBaseComponentTypes));
            entityQuery.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            initKinematicActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach(initFunc));
        }

        private void CreateAuthChangeAction<T>(AuthChangeFunc<T> authFunc)
            where T : class
        {
            var componentQueryDesc = TransformUtils.ConstructEntityQueryDesc<T>(authBaseComponentTypes);
            componentQueryDesc.None = new[] { ComponentType.ReadOnly<NewlyAddedSpatialOSEntity>() };

            var entityQuery = GetEntityQuery(componentQueryDesc);

            authKinematicActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach(
                (
                    T component,
                    ref KinematicStateWhenAuth kinematicStateWhenAuth,
                    ref SpatialEntityId spatialEntityId) =>
                {
                    var changes = updateSystem.GetAuthorityChangesReceived(spatialEntityId.EntityId,
                        TransformInternal.ComponentId);
                    if (changes.Count == 0)
                    {
                        return;
                    }

                    var auth = changes[changes.Count - 1];

                    authFunc(ref kinematicStateWhenAuth, auth, component);
                }));
        }

        protected override void OnUpdate()
        {
            UpdateNewEntityGroup();
            UpdateAuthChangeGroup();
        }

        private void UpdateNewEntityGroup()
        {
            foreach (var initAction in initKinematicActions)
            {
                initAction.Value();
            }
        }

        private void UpdateAuthChangeGroup()
        {
            foreach (var authChangeAction in authKinematicActions)
            {
                authChangeAction.Value();
            }
        }
    }
}
