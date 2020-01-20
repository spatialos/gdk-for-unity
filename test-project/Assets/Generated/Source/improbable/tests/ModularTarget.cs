// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Tests
{
    [global::System.Serializable]
    public struct ModularTarget
    {
        public uint Thing;

        public ModularTarget(uint thing)
        {
            Thing = thing;
        }

        public class InteriorClass
        {
            // I exist!
        }

        public static class Serialization
        {
            public static void Serialize(ModularTarget instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddUint32(1, instance.Thing);
                }
            }

            public static ModularTarget Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new ModularTarget();

                {
                    instance.Thing = obj.GetUint32(1);
                }

                return instance;
            }
        }
    }
}
