// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.TestSchema
{
    
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
            
                public NestedTypeSameName(int nestedField)
                {
                    NestedField = nestedField;
                }
                public static class Serialization
                {
                    public static void Serialize(NestedTypeSameName instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                    {
                        {
                            obj.AddInt32(1, instance.NestedField);
                        }
                    }
            
                    public static NestedTypeSameName Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                    {
                        var instance = new NestedTypeSameName();
                        {
                            instance.NestedField = obj.GetInt32(1);
                        }
                        return instance;
                    }
                }
            
                
                [global::System.Serializable]
                public struct Other0
                {
                    public static class Serialization
                    {
                        public static void Serialize(Other0 instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                        }
                
                        public static Other0 Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            var instance = new Other0();
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
                    public static class Serialization
                    {
                        public static void Serialize(Other1 instance, global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                        }
                
                        public static Other1 Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
                        {
                            var instance = new Other1();
                            return instance;
                        }
                    }
                
                    
                    [global::System.Serializable]
                    public enum NestedTypeSameName : uint
                    {
                        Other1 = 1,
                    }
                    
                
                    
                    [global::System.Serializable]
                    public enum NestedEnum : uint
                    {
                        NestedTypeSameName = 1,
                    }
                    
                }
                
            }
            
        }
        
    }
    
}
