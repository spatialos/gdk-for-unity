using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     This class is for when a user wants to Add/Remove a Component (not
    ///     IComponentData) during a system update without invalidating their injected arrays.
    ///     The user must call Flush on this buffer at the end of the Update function to apply
    ///     the buffered changes.
    /// </summary>
    public class ViewCommandBuffer
    {
        private const string LoggerName = nameof(ViewCommandBuffer);

        private const string UnknownErrorEncountered =
            "ViewCommandBuffer encountered unknown command type during buffer flush.";

        private readonly EntityManager EntityManager;
        private readonly ILogDispatcher LogDispatcher;
        private readonly Action<Entity, ComponentType, object> setComponentObjectAction;
        private readonly Queue<BufferedCommand> bufferedCommands = new Queue<BufferedCommand>();

        private static readonly MethodInfo setComponentObjectMethodInfo =
            typeof(EntityManager).GetMethod("SetComponentObject", BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { typeof(Entity), typeof(ComponentType), typeof(object) },
                new ParameterModifier[] { });

        public ViewCommandBuffer(EntityManager entityManager, ILogDispatcher logDispatcher)
        {
            EntityManager = entityManager;
            LogDispatcher = logDispatcher;
            setComponentObjectAction = (Action<Entity, ComponentType, object>) Delegate.CreateDelegate(
                typeof(Action<Entity, ComponentType, object>), entityManager, setComponentObjectMethodInfo);
        }

        /// <summary>
        ///     Adds a GameObject <see cref="Component"/> to an ECS entity.
        /// </summary>
        /// <param name="entity">The ECS entity</param>
        /// <param name="component">The component</param>
        /// <typeparam name="T">The type of the component.</typeparam>
        public void AddComponent<T>(Entity entity, T component) where T : Component
        {
            AddComponent(entity, ComponentType.ReadWrite<T>(), component);
        }

        /// <summary>
        ///     Adds a GameObject <see cref="Component"/> to an ECS entity.
        /// </summary>
        /// <param name="entity">The ECS entity</param>
        /// <param name="componentType">The type of the component</param>
        /// <param name="componentObj">The component</param>
        public void AddComponent(Entity entity, ComponentType componentType, object componentObj)
        {
            bufferedCommands.Enqueue(new BufferedCommand
            {
                Entity = entity,
                CommandType = CommandType.AddComponent,
                ComponentType = componentType,
                ComponentObj = componentObj
            });
        }

        /// <summary>
        ///     Removes a GameObject <see cref="Component"/> from an ECS entity.
        /// </summary>
        /// <param name="entity">The ECS entity.</param>
        /// <param name="componentType">The type of the component to remove.</param>
        public void RemoveComponent(Entity entity, ComponentType componentType)
        {
            bufferedCommands.Enqueue(new BufferedCommand
            {
                Entity = entity,
                CommandType = CommandType.RemoveComponent,
                ComponentType = componentType,
                ComponentObj = null
            });
        }

        /// <summary>
        ///     Plays back and applies all buffered actions in order.
        /// </summary>
        public void FlushBuffer()
        {
            foreach (var bufferedCommand in bufferedCommands)
            {
                switch (bufferedCommand.CommandType)
                {
                    case CommandType.AddComponent:
                        SetComponentObject(bufferedCommand.Entity,
                            bufferedCommand.ComponentType,
                            bufferedCommand.ComponentObj);
                        break;
                    case CommandType.RemoveComponent:
                        EntityManager.RemoveComponent(bufferedCommand.Entity,
                            bufferedCommand.ComponentType);
                        break;
                    default:
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnknownErrorEncountered)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField("CommandType", bufferedCommand.CommandType));
                        break;
                }
            }

            bufferedCommands.Clear();
        }

        private void SetComponentObject(Entity entity, ComponentType componentType, object component)
        {
            if (!EntityManager.HasComponent(entity, componentType))
            {
                EntityManager.AddComponent(entity, componentType);
            }

            setComponentObjectAction(entity, componentType, component);
        }

        private struct BufferedCommand
        {
            public Entity Entity;
            public CommandType CommandType;
            public ComponentType ComponentType;
            public object ComponentObj;
        }

        private enum CommandType
        {
            AddComponent,
            RemoveComponent
        }
    }
}
