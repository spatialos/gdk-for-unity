using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Packages.io.improbable.gdk.testutils;
using Unity.Entities;
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
            public bool EnableSerialization;
        }

        public WorkerInWorld Worker { get; private set; }
        public MockConnectionHandler Connection { get; private set; }
        public EntityGameObjectLinker Linker { get; private set; }

        private readonly HashSet<GameObject> gameObjects = new HashSet<GameObject>();
        private MockCommandSender CommandSender { get; set; }

        public static MockWorld Create(Options options)
        {
            var mockWorld = new MockWorld();

            var connectionBuilder = new MockConnectionHandlerBuilder(options.EnableSerialization);
            mockWorld.Connection = connectionBuilder.ConnectionHandler;
            mockWorld.CommandSender = new MockCommandSender(mockWorld);
            mockWorld.Worker = WorkerInWorld
                .CreateWorkerInWorldAsync(connectionBuilder,
                    options.WorkerType ?? "TestWorkerType",
                    options.Logger ?? new LoggingDispatcher(),
                    Vector3.zero)
                .Result;

            options.AdditionalSystems?.Invoke(mockWorld.Worker.World);

            mockWorld.Linker = new EntityGameObjectLinker(mockWorld.Worker.World);

            mockWorld.GetSystem<CommandSystem>().SetSender(mockWorld.CommandSender);

            PlayerLoopUtils.ResolveSystemGroups(mockWorld.Worker.World);

            return mockWorld;
        }

        public void Setup<TRequest>(uint componentId)
        {
            CommandSender.Setup<TRequest>(componentId);
        }

        public void GenerateResponses<TRequest, TResponse>(Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            CommandSender.GenerateResponses(creator);
        }

        public void GenerateResponse<TRequest, TResponse>(CommandRequestId id,
            Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            CommandSender.GenerateResponse(id, creator);
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

            public MockWorldWithContext<U> Step<U>(Func<MockWorld, U> frame)
            {
                var newContext = frame(world);
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
