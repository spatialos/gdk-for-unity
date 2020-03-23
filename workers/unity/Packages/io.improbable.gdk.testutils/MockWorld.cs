using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using NUnit.Framework;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.TestUtils
{
    public class MockWorld : IDisposable
    {
        public struct Options
        {
            public string WorkerType;
            public Action<World> AdditionalSystems;
            public ILogDispatcher Logger;
        }

        public WorkerInWorld Worker { get; private set; }
        public MockConnectionHandler Connection { get; private set; }

        public EntityGameObjectLinker Linker { get; private set; }

        private readonly HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        public static MockWorld Create(Options options)
        {
            var mockWorld = new MockWorld();

            var connectionBuilder = new MockConnectionHandlerBuilder();
            mockWorld.Connection = connectionBuilder.ConnectionHandler;

            mockWorld.Worker = WorkerInWorld
                .CreateWorkerInWorldAsync(connectionBuilder,
                    options.WorkerType ?? "TestWorkerType",
                    options.Logger ?? new LoggingDispatcher(),
                    Vector3.zero)
                .Result;

            options.AdditionalSystems?.Invoke(mockWorld.Worker.World);

            mockWorld.Linker = new EntityGameObjectLinker(mockWorld.Worker.World);

            PlayerLoopUtils.ResolveSystemGroups(mockWorld.Worker.World);

            return mockWorld;
        }

        public T GetSystem<T>() where T : ComponentSystemBase
        {
            return Worker.World.GetExistingSystem<T>();
        }

        public MockWorldWithContext<T> Step<T>(Func<MockWorld, T> frame)
        {
            var context = frame(this);
            Update();
            return new MockWorldWithContext<T>(this, context);
        }

        public MockWorld Step(Action<MockWorld> frame)
        {
            frame(this);
            Update();
            return this;
        }

        private void Update()
        {
            Worker.World.Update();
        }

        public (GameObject, T) CreateGameObject<T>(long entityId) where T : MonoBehaviour
        {
            var gameObject = new GameObject("TestGameObject");
            gameObjects.Add(gameObject);

            var component = gameObject.AddComponent<T>();
            component.enabled = false;

            Linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            return (gameObject, component);
        }

        public void Dispose()
        {
            foreach (var go in gameObjects)
            {
                UnityEngine.Object.DestroyImmediate(go);
            }

            Worker?.Dispose();
            Connection?.Dispose();
        }

        public readonly struct MockWorldWithContext<T>
        {
            private readonly T context;
            private readonly MockWorld world;

            public MockWorldWithContext(MockWorld world, T context)
            {
                this.world = world;
                this.context = context;
            }

            public MockWorldWithContext<U> Step<U>(Func<MockWorld, T, U> frame)
            {
                var newContext = frame(world, context);
                world.Update();

                return new MockWorldWithContext<U>(world, newContext);
            }

            public MockWorldWithContext<T> Step(Action<MockWorld> frame)
            {
                frame(world);
                world.Update();

                return this;
            }

            public MockWorldWithContext<T> Step(Action<MockWorld, T> frame)
            {
                frame(world, context);
                world.Update();

                return this;
            }
        }
    }
}
