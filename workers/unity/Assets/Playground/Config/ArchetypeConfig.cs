using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public static class ArchetypeConfig
    {
        public const string CharacterArchetype = "Character";
        public const string CubeArchetype = "Cube";
        public const string SpinnerArchetype = "Spinner";
        
        private const string ArchetypeMappingNotFound = "No corresponding archetype mapping found.";
        private const string LoggerName = "ArchetypeConfig";

        public static void AddComponentDataForArchetype(Worker worker, string archetype, 
            EntityCommandBuffer commandBuffer, Entity entity)
        {
            switch (worker.WorkerType)
            {
                case SystemConfig.UnityClient when archetype == CharacterArchetype:
                case SystemConfig.UnityClient when archetype == CubeArchetype:
                case SystemConfig.UnityClient when archetype == SpinnerArchetype:
                    commandBuffer.AddBuffer<BufferedTransform>(entity);
                    break;
                case SystemConfig.UnityGameLogic when archetype == CharacterArchetype:
                case SystemConfig.UnityGameLogic when archetype == CubeArchetype:
                case SystemConfig.UnityGameLogic when archetype == SpinnerArchetype:
                    commandBuffer.AddBuffer<BufferedTransform>(entity);
                    break;
                default:
                    worker.LogDispatcher.HandleLog(LogType.Error, new LogEvent(ArchetypeMappingNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("ArchetypeName", archetype)
                        .WithField("WorkerType", worker.WorkerType));
                    break;
            }
        }
    }
}
