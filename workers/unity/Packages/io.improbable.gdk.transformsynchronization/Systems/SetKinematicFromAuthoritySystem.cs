using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

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

        public delegate void AuthChangeFunc<in T>(ref KinematicStateWhenAuth state, AuthorityChangeReceived auth,
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


            RegisterType<Rigidbody>(
                (ref KinematicStateWhenAuth kinematicStateWhenAuth,
                    Rigidbody rigidbody) =>
                {
                    kinematicStateWhenAuth = new KinematicStateWhenAuth
                    {
                        KinematicWhenAuthoritative = rigidbody.isKinematic
                    };

                    rigidbody.isKinematic = true;
                },
                (ref KinematicStateWhenAuth kinematicStateWhenAuth,
                    AuthorityChangeReceived auth,
                    Rigidbody rigidbody) =>
                {
                    switch (auth.Authority)
                    {
                        case Authority.NotAuthoritative:
                            kinematicStateWhenAuth = new KinematicStateWhenAuth
                            {
                                KinematicWhenAuthoritative = rigidbody.isKinematic
                            };
                            rigidbody.isKinematic = true;
                            break;
                        case Authority.Authoritative:
                            rigidbody.isKinematic = kinematicStateWhenAuth.KinematicWhenAuthoritative;
                            break;
                    }
                });
        }

        public void RegisterType<T>(EntityQueryBuilder.F_DC<KinematicStateWhenAuth, T> initFunc,
            AuthChangeFunc<T> authFunc)
            where T : class
        {
            CreateInitAction(initFunc);
            CreateAuthChangeAction(authFunc);
        }

        private void CreateInitAction<T>(EntityQueryBuilder.F_DC<KinematicStateWhenAuth, T> initFunc)
            where T : class
        {
            var componentType = ComponentType.ReadOnly<T>();

            var includedComponentTypes = initBaseComponentTypes
                .Concat(new[] { componentType })
                .ToArray();

            var componentQueryDesc = new EntityQueryDesc()
            {
                All = includedComponentTypes
            };

            var entityQuery = GetEntityQuery(componentQueryDesc);
            entityQuery.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            initKinematicActions.Add(typeof(T), () => Entities.With(entityQuery).ForEach(initFunc));
        }

        private void CreateAuthChangeAction<T>(AuthChangeFunc<T> authFunc)
            where T : class
        {
            var componentType = ComponentType.ReadOnly<T>();

            var includedComponentTypes = authBaseComponentTypes
                .Concat(new[] { componentType })
                .ToArray();

            var componentQueryDesc = new EntityQueryDesc()
            {
                All = includedComponentTypes,
                None = new[] { ComponentType.ReadOnly<NewlyAddedSpatialOSEntity>() }
            };

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
