// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests
{ 
    
    public struct TypeName
    {
        public global::Generated.Improbable.Gdk.Tests.TypeName.Other OtherType;
    
        public static TypeName ToNative(global::Improbable.Gdk.Tests.TypeName spatialType)
        {
            var nativeType = new TypeName();
            nativeType.OtherType = global::Generated.Improbable.Gdk.Tests.TypeName.Other.ToNative(spatialType.otherType);
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.TypeName ToSpatial(global::Generated.Improbable.Gdk.Tests.TypeName nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.TypeName();
            spatialType.otherType = global::Generated.Improbable.Gdk.Tests.TypeName.Other.ToSpatial(nativeType.OtherType);
            return spatialType;
        }
    
        
        public struct Other
        {
            public global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName SameName;
        
            public static Other ToNative(global::Improbable.Gdk.Tests.TypeName.Other spatialType)
            {
                var nativeType = new Other();
                nativeType.SameName = global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.ToNative(spatialType.sameName);
                return nativeType;
            }
        
            public static global::Improbable.Gdk.Tests.TypeName.Other ToSpatial(global::Generated.Improbable.Gdk.Tests.TypeName.Other nativeType)
            {
                var spatialType = new global::Improbable.Gdk.Tests.TypeName.Other();
                spatialType.sameName = global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.ToSpatial(nativeType.SameName);
                return spatialType;
            }
        
            
            public struct NestedTypeName
            {
                public global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0 OtherZero;
                public global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.NestedEnum EnumField;
            
                public static NestedTypeName ToNative(global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName spatialType)
                {
                    var nativeType = new NestedTypeName();
                    nativeType.OtherZero = global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0.ToNative(spatialType.otherZero);
                    nativeType.EnumField = (global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.NestedEnum) spatialType.enumField;
                    return nativeType;
                }
            
                public static global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName ToSpatial(global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName nativeType)
                {
                    var spatialType = new global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName();
                    spatialType.otherZero = global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0.ToSpatial(nativeType.OtherZero);
                    spatialType.enumField = (global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.NestedEnum) nativeType.EnumField;
                    return spatialType;
                }
            
                
                public struct Other0
                {
                    public int Foo;
                
                    public static Other0 ToNative(global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0 spatialType)
                    {
                        var nativeType = new Other0();
                        nativeType.Foo = spatialType.foo;
                        return nativeType;
                    }
                
                    public static global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0 ToSpatial(global::Generated.Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0 nativeType)
                    {
                        var spatialType = new global::Improbable.Gdk.Tests.TypeName.Other.NestedTypeName.Other0();
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
