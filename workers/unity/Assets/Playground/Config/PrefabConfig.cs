using System.Collections.Generic;

namespace Playground
{
    public static class PrefabConfig
    {
        public static readonly Dictionary<string, PrefabMapping> PrefabMappings = new Dictionary<string, PrefabMapping>
        {
            {
                ArchetypeConfig.CubeArchetype,
                new PrefabMapping { UnityGameLogic = "Prefabs/Cube", UnityClient = "Prefabs/CubeKinematic" }
            },
            {
                ArchetypeConfig.CharacterArchetype,
                new PrefabMapping { UnityGameLogic = "Prefabs/Character", UnityClient = "Prefabs/Character" }
            }
        };

        public struct PrefabMapping
        {
            public string UnityGameLogic;
            public string UnityClient;
        }
    }
}
