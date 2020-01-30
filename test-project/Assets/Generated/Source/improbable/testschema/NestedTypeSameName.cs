// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    [global::System.Serializable]
    public struct NestedTypeSameName
    {
        public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName Other0Field;
        public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName Other1Field;

        public NestedTypeSameName(global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName other0Field, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName other1Field)
        {
            Other0Field = other0Field;
            Other1Field = other1Field;
        }

        public static class Serialization
        {
            public static void Serialize(NestedTypeSameName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(instance.Other0Field, obj.AddObject(1));
                }

                {
                    obj.AddEnum(2, (uint) instance.Other1Field);
                }
            }

            public static NestedTypeSameName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new NestedTypeSameName();

                {
                    instance.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(1));
                }

                {
                    instance.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(2);
                }

                return instance;
            }
        }

        [global::System.Serializable]
        public struct Other
        {
            public static class Serialization
            {
                public static void Serialize(Other instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                }

                public static Other Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new Other();

                    return instance;
                }
            }

            [global::System.Serializable]
            public struct NestedTypeSameName
            {
                public int NestedField;
                public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName Other0Field;
                public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName Other1Field;

                public NestedTypeSameName(int nestedField, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName other0Field, global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName other1Field)
                {
                    NestedField = nestedField;
                    Other0Field = other0Field;
                    Other1Field = other1Field;
                }

                public static class Serialization
                {
                    public static void Serialize(NestedTypeSameName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                    {
                        {
                            obj.AddInt32(1, instance.NestedField);
                        }

                        {
                            global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(instance.Other0Field, obj.AddObject(2));
                        }

                        {
                            obj.AddEnum(3, (uint) instance.Other1Field);
                        }
                    }

                    public static NestedTypeSameName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                    {
                        var instance = new NestedTypeSameName();

                        {
                            instance.NestedField = obj.GetInt32(1);
                        }

                        {
                            instance.Other0Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(2));
                        }

                        {
                            instance.Other1Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(3);
                        }

                        return instance;
                    }
                }

                [global::System.Serializable]
                public struct Other0
                {
                    public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName Field;

                    public Other0(global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName field)
                    {
                        Field = field;
                    }

                    public static class Serialization
                    {
                        public static void Serialize(Other0 instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            {
                                global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Serialize(instance.Field, obj.AddObject(1));
                            }
                        }

                        public static Other0 Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            var instance = new Other0();

                            {
                                instance.Field = global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName.Serialization.Deserialize(obj.GetObject(1));
                            }

                            return instance;
                        }
                    }

                    [global::System.Serializable]
                    public struct NestedTypeSameName
                    {
                        public static class Serialization
                        {
                            public static void Serialize(NestedTypeSameName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                            {
                            }

                            public static NestedTypeSameName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                            {
                                var instance = new NestedTypeSameName();

                                return instance;
                            }
                        }
                    }
                }

                [global::System.Serializable]
                public struct Other1
                {
                    public global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName Field;

                    public Other1(global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName field)
                    {
                        Field = field;
                    }

                    public static class Serialization
                    {
                        public static void Serialize(Other1 instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            {
                                obj.AddEnum(1, (uint) instance.Field);
                            }
                        }

                        public static Other1 Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            var instance = new Other1();

                            {
                                instance.Field = (global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName) obj.GetEnum(1);
                            }

                            return instance;
                        }
                    }

                    [global::System.Serializable]
                    public enum NestedTypeSameName : uint
                    {
                        Other1 = 1
                    }

                    [global::System.Serializable]
                    public enum NestedEnum : uint
                    {
                        NestedTypeSameName = 1
                    }
                }
            }
        }
    }
}
