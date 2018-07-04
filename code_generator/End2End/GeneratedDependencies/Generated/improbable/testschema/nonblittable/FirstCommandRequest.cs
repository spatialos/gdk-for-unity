// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    
    public struct FirstCommandRequest
    {
        public int Field;
    
        public static FirstCommandRequest ToNative(global::Improbable.TestSchema.Nonblittable.FirstCommandRequest spatialType)
        {
            var nativeType = new FirstCommandRequest();
            nativeType.Field = spatialType.field;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Nonblittable.FirstCommandRequest ToSpatial(global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandRequest nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Nonblittable.FirstCommandRequest();
            spatialType.field = nativeType.Field;
            return spatialType;
        }
    }
    
}
