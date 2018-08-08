using System.Collections.Generic;
using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;

namespace Playground
{
    public static class ArchetypeConfig
    {
        public const string CharacterArchetype = "Character";
        public const string CubeArchetype = "Cube";

        public static readonly Dictionary<string, Dictionary<string, ComponentType[]>>
            WorkerTypeToArchetypeNameToComponentTypes = new Dictionary<string, Dictionary<string, ComponentType[]>>
            {
                {
                    nameof(UnityGameLogic),
                    new Dictionary<string, ComponentType[]>()
                    {
                        { CharacterArchetype, new ComponentType[] { typeof(BufferedTransform) } },
                        { CubeArchetype, new ComponentType[] { typeof(BufferedTransform) } }
                    }
                },
                {
                    nameof(UnityClient),
                    new Dictionary<string, ComponentType[]>()
                    {
                        { CharacterArchetype, new ComponentType[] { typeof(BufferedTransform) } },
                        { CubeArchetype, new ComponentType[] { typeof(BufferedTransform) } }
                    }
                }
            };
    }
}
