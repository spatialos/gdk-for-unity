using System.Collections.Generic;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;

namespace Playground
{
    public static class ArchetypeConfig
    {
        public const string CharacterArchetype = "Character";
        public const string CubeArchetype = "Cube";
        public const string RotatingCuboidArchetype = "RotatingCuboid";

        public static readonly Dictionary<string, Dictionary<string, ComponentType[]>>
            WorkerTypeToArchetypeNameToComponentTypes = new Dictionary<string, Dictionary<string, ComponentType[]>>
            {
                {
                    UnityGameLogic.WorkerType,
                    new Dictionary<string, ComponentType[]>()
                    {
                        { CharacterArchetype, new ComponentType[] { typeof(BufferedTransform) } },
                        { CubeArchetype, new ComponentType[] { typeof(BufferedTransform) } },
                        { RotatingCuboidArchetype, new ComponentType[] { typeof(BufferedTransform) } }
                    }
                },
                {
                    UnityClient.WorkerType,
                    new Dictionary<string, ComponentType[]>()
                    {
                        { CharacterArchetype, new ComponentType[] { typeof(BufferedTransform) } },
                        { CubeArchetype, new ComponentType[] { typeof(BufferedTransform) } },
                        { RotatingCuboidArchetype, new ComponentType[] { typeof(BufferedTransform) } }
                    }
                }
            };
    }
}
