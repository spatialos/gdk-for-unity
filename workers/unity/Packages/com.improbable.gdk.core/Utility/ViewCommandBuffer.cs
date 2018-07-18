using System.Collections.Generic;
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
        private readonly Queue<BufferedCommand> bufferedCommands = new Queue<BufferedCommand>();

        private const string LoggerName = "ViewCommandBuffer";

        private const string UnknownErrorEncountered =
            "ViewCommandBuffer encountered unknown command type during buffer flush.";

        public void AddComponent<T>(Entity entity, T component) where T : Component
        {
            AddComponent(entity, ComponentType.Create<T>(), component);
        }

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

        public void FlushBuffer(MutableView view)
        {
            foreach (var bufferedCommand in bufferedCommands)
            {
                switch (bufferedCommand.CommandType)
                {
                    case CommandType.AddComponent:
                        view.SetComponentObject(bufferedCommand.Entity,
                            bufferedCommand.ComponentType,
                            bufferedCommand.ComponentObj);
                        break;
                    case CommandType.RemoveComponent:
                        view.RemoveComponent(bufferedCommand.Entity,
                            bufferedCommand.ComponentType);
                        break;
                    default:
                        view.LogDispatcher.HandleLog(LogType.Error, new LogEvent(UnknownErrorEncountered)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField("CommandType", bufferedCommand.CommandType));
                        break;
                }
            }

            bufferedCommands.Clear();
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
