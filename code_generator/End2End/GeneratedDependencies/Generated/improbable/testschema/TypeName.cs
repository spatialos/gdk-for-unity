// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema
{ 
    
    public struct TypeName
    {
        public global::Generated.Improbable.TestSchema.TypeName.Other OtherType;
    
        public static TypeName ToNative(global::Improbable.TestSchema.TypeName spatialType)
        {
            var nativeType = new TypeName();
            nativeType.OtherType = global::Generated.Improbable.TestSchema.TypeName.Other.ToNative(spatialType.otherType);
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.TypeName ToSpatial(global::Generated.Improbable.TestSchema.TypeName nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.TypeName();
            spatialType.otherType = global::Generated.Improbable.TestSchema.TypeName.Other.ToSpatial(nativeType.OtherType);
            return spatialType;
        }
    
        
        public struct Other
        {
            public global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName SameName;
        
            public static Other ToNative(global::Improbable.TestSchema.TypeName.Other spatialType)
            {
                var nativeType = new Other();
                nativeType.SameName = global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.ToNative(spatialType.sameName);
                return nativeType;
            }
        
            public static global::Improbable.TestSchema.TypeName.Other ToSpatial(global::Generated.Improbable.TestSchema.TypeName.Other nativeType)
            {
                var spatialType = new global::Improbable.TestSchema.TypeName.Other();
                spatialType.sameName = global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.ToSpatial(nativeType.SameName);
                return spatialType;
            }
        
            
            public struct NestedTypeName
            {
                public global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0 OtherZero;
                public global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.NestedEnum EnumField;
            
                public static NestedTypeName ToNative(global::Improbable.TestSchema.TypeName.Other.NestedTypeName spatialType)
                {
                    var nativeType = new NestedTypeName();
                    nativeType.OtherZero = global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0.ToNative(spatialType.otherZero);
                    nativeType.EnumField = (global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.NestedEnum) spatialType.enumField;
                    return nativeType;
                }
            
                public static global::Improbable.TestSchema.TypeName.Other.NestedTypeName ToSpatial(global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName nativeType)
                {
                    var spatialType = new global::Improbable.TestSchema.TypeName.Other.NestedTypeName();
                    spatialType.otherZero = global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0.ToSpatial(nativeType.OtherZero);
                    spatialType.enumField = (global::Improbable.TestSchema.TypeName.Other.NestedTypeName.NestedEnum) nativeType.EnumField;
                    return spatialType;
                }
            
                
                public struct Other0
                {
                    public int Foo;
                
                    public static Other0 ToNative(global::Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0 spatialType)
                    {
                        var nativeType = new Other0();
                        nativeType.Foo = spatialType.foo;
                        return nativeType;
                    }
                
                    public static global::Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0 ToSpatial(global::Generated.Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0 nativeType)
                    {
                        var spatialType = new global::Improbable.TestSchema.TypeName.Other.NestedTypeName.Other0();
                        spatialType.foo = nativeType.Foo;
                        return spatialType;
                    }
                }
            
                
                public enum NestedEnum : uint
                {
                    enum_value = 1,
                }
                
            }
        }
    }
}
