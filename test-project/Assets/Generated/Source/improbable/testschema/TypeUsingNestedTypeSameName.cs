// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    [global::System.Serializable]
    public struct TypeUsingNestedTypeSameName
    {
        public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName Field1;
        public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0 Field2;
        public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName Field3;

        public TypeUsingNestedTypeSameName(global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName field1, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0 field2, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName field3)
        {
            Field1 = field1;
            Field2 = field2;
            Field3 = field3;
        }

        public static class Serialization
        {
            public static void Serialize(TypeUsingNestedTypeSameName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Serialization.Serialize(instance.Field1, obj.AddObject(1));
                }

                {
                    global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.Serialization.Serialize(instance.Field2, obj.AddObject(2));
                }

                {
                    global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(instance.Field3, obj.AddObject(3));
                }
            }

            public static TypeUsingNestedTypeSameName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TypeUsingNestedTypeSameName();

                {
                    instance.Field1 = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(1));
                }

                {
                    instance.Field2 = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.Serialization.Deserialize(obj.GetObject(2));
                }

                {
                    instance.Field3 = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(3));
                }

                return instance;
            }
        }
    }
}
