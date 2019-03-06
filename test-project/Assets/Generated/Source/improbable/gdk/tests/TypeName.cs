// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests
{
    
    [System.Serializable]
    public struct TypeName
    {
        public global::Improbable.Gdk.Tests.TypeName.Other OtherType;
    
        public TypeName(global::Improbable.Gdk.Tests.TypeName.Other otherType)
        {
            OtherType = otherType;
        }
        public static class Serialization
        {
            public static void Serialize(TypeName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    global::Improbable.Gdk.Tests.TypeName.Other.Serialization.Serialize(instance.OtherType, obj.AddObject(1));
                }
            }
    
            public static TypeName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new TypeName();
                {
                    instance.OtherType = global::Improbable.Gdk.Tests.TypeName.Other.Serialization.Deserialize(obj.GetObject(1));
                }
                return instance;
            }
        }
    
        
        [System.Serializable]
        public struct Other
        {
            public global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName SameName;
        
            public Other(global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName sameName)
            {
                SameName = sameName;
            }
            public static class Serialization
            {
                public static void Serialize(Other instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    {
                        global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Serialization.Serialize(instance.SameName, obj.AddObject(1));
                    }
                }
        
                public static Other Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                {
                    var instance = new Other();
                    {
                        instance.SameName = global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Serialization.Deserialize(obj.GetObject(1));
                    }
                    return instance;
                }
            }
        
            
            [System.Serializable]
            public struct NestedTypeName
            {
                public global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0 OtherZero;
                public global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.NestedEnum EnumField;
            
                public NestedTypeName(global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0 otherZero, global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.NestedEnum enumField)
                {
                    OtherZero = otherZero;
                    EnumField = enumField;
                }
                public static class Serialization
                {
                    public static void Serialize(NestedTypeName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                    {
                        {
                            global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0.Serialization.Serialize(instance.OtherZero, obj.AddObject(1));
                        }
                        {
                            obj.AddEnum(2, (uint) instance.EnumField);
                        }
                    }
            
                    public static NestedTypeName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                    {
                        var instance = new NestedTypeName();
                        {
                            instance.OtherZero = global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0.Serialization.Deserialize(obj.GetObject(1));
                        }
                        {
                            instance.EnumField = (global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.NestedEnum) obj.GetEnum(2);
                        }
                        return instance;
                    }
                }
            
                
                [System.Serializable]
                public struct Other0
                {
                    public int Foo;
                
                    public Other0(int foo)
                    {
                        Foo = foo;
                    }
                    public static class Serialization
                    {
                        public static void Serialize(Other0 instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            {
                                obj.AddInt32(1, instance.Foo);
                            }
                        }
                
                        public static Other0 Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            var instance = new Other0();
                            {
                                instance.Foo = obj.GetInt32(1);
                            }
                            return instance;
                        }
                    }
                }
                
            
                
                [System.Serializable]
                public enum NestedEnum : uint
                {
                    enum_value = 1,
                }
                
            }
            
        }
        
    }
    
}
