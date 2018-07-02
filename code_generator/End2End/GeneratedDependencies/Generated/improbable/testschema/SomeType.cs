// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema
{ 
    
    public struct SomeType
    {
    
        public static SomeType ToNative(global::Improbable.TestSchema.SomeType spatialType)
        {
            var nativeType = new SomeType();
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.SomeType ToSpatial(global::Generated.Improbable.TestSchema.SomeType nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.SomeType();
            return spatialType;
        }
    }
}
