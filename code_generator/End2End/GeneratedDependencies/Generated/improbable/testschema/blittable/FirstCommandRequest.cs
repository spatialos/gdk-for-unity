// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema.Blittable
{ 
    
    public struct FirstCommandRequest
    {
        public int Field;
    
        public static FirstCommandRequest ToNative(global::Improbable.TestSchema.Blittable.FirstCommandRequest spatialType)
        {
            var nativeType = new FirstCommandRequest();
            nativeType.Field = spatialType.field;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Blittable.FirstCommandRequest ToSpatial(global::Generated.Improbable.TestSchema.Blittable.FirstCommandRequest nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Blittable.FirstCommandRequest();
            spatialType.field = nativeType.Field;
            return spatialType;
        }
    }
}
